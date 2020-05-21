﻿using gView.Framework.system;
using gView.MapServer;
using gView.Server.AppCode;
using Microsoft.Extensions.Options;
using System;
using gView.Framework.IO;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using System.Threading;
using gView.Server.Services.Logging;

namespace gView.Server.Services.MapServer
{
    public class InternetMapServerService
    {
        private ILogger _logger;
        private IServiceProvider _serviceProvider;

        public InternetMapServerService(
            IServiceProvider serviceProvider,
            IOptionsMonitor<InternetMapServerServiceOptions> optionsMonitor, 
            ILogger<InternetMapServerService> logger = null)
        {
            _serviceProvider = serviceProvider;
            Options = optionsMonitor.CurrentValue;
            _logger = logger ?? new ConsoleLogger<InternetMapServerService>();

            if (Options.IsValid)
            {
                
                

                foreach (string createDirectroy in new string[] {
                    Options.ServicesPath,
                    Options.LoginManagerRootPath,
                    $"{ Options.LoginManagerRootPath }/manage",
                    $"{ Options.LoginManagerRootPath }/token",
                    Options.LoggingRootPath
                })
                {
                    if (!new DirectoryInfo(createDirectroy).Exists)
                    {
                        new DirectoryInfo(createDirectroy).Create();
                    }
                }

                AddServices(String.Empty);

                var pluginMananger = new PlugInManager();
                Interpreters = pluginMananger.GetPlugins(typeof(IServiceRequestInterpreter)).ToArray();

                TaskQueue = new TaskQueue<IServiceRequestContext>(Options.TaskQueue_MaxThreads, Options.TaskQueue_QueueLength);
            }
        }

        private MapServerInstance _instance;
        public MapServerInstance Instance
        {
            get
            {
                if (_instance == null)
                {
                    var msds = (MapServerDeployService)_serviceProvider.GetService(typeof(MapServerDeployService));
                    var logger = (MapServicesEventLogger)_serviceProvider.GetService(typeof(MapServicesEventLogger));

                    _instance = new MapServerInstance(this, msds, logger, Options.Port);
                }

                return _instance;
            }
        }

        public readonly InternetMapServerServiceOptions Options;
        public readonly TaskQueue<IServiceRequestContext> TaskQueue;
        public readonly Type[] Interpreters;
        public ConcurrentBag<IMapService> MapServices = new ConcurrentBag<IMapService>();

        private void AddServices(string folder)
        {

            foreach (var mapFileInfo in new DirectoryInfo((Options.ServicesPath + "/" + folder).ToPlattformPath()).GetFiles("*.mxl"))
            {
                string mapName = String.Empty;
                try
                {
                    if (TryAddService(mapFileInfo, folder) == null)
                    {
                        throw new Exception("unable to load servive: " + mapFileInfo.FullName);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Service { mapName }: loadConfig - { mapFileInfo.Name  }: { ex.Message }");
                }
            }

            #region Add Folders on same level

            foreach (var folderDirectory in new DirectoryInfo((Options.ServicesPath + "/" + folder).ToPlattformPath()).GetDirectories())
            {
                MapService folderService = new MapService(this, folderDirectory.FullName, folder, MapServiceType.Folder);
                if (MapServices.Where(s => s.Fullname == folderService.Fullname && s.Type == folderService.Type).Count() == 0)
                {
                    MapServices.Add(folderService);
                    Console.WriteLine("folder " + folderService.Name + " added");
                }
            }

            #endregion
        }

        public void AddMapService(string mapName, MapServiceType type)
        {
            foreach (IMapService service in MapServices)
            {
                if (service.Fullname == mapName)
                {
                    return;
                }
            }
            string folder = String.Empty;
            if (mapName.Contains("/"))
            {
                if (mapName.Split('/').Length > 2)
                {
                    throw new Exception("Invalid map name: " + mapName);
                }
                folder = mapName.Split('/')[0];
                mapName = mapName.Split('/')[1];
            }
            MapServices.Add(new MapService(this, mapName.Trim(), folder.Trim(), type));
        }

        private object _tryAddServiceLocker = new object();
        private IMapService TryAddService(FileInfo mapFileInfo, string folder)
        {
            lock (_tryAddServiceLocker)
            {
                if (!mapFileInfo.Exists)
                {
                    return null;
                }

                MapService mapService = new MapService(this, mapFileInfo.FullName, folder, MapServiceType.MXL);
                if (MapServices.Where(s => s.Fullname == mapService.Fullname && s.Type == mapService.Type).Count() > 0)
                {
                    // allready exists
                    return MapServices.Where(s => s.Fullname == mapService.Fullname && s.Type == mapService.Type).FirstOrDefault();
                }

                if (!String.IsNullOrWhiteSpace(folder))
                {
                    #region Add Service Parent Folders

                    string folderName = String.Empty, parentFolder = String.Empty;
                    foreach (var subFolder in folder.Split('/'))
                    {
                        folderName += (folderName.Length > 0 ? "/" : "") + subFolder;
                        DirectoryInfo folderDirectory = new DirectoryInfo((Options.ServicesPath + "/" + folder).ToPlattformPath());
                        MapService folderService = new MapService(this, folderDirectory.FullName, parentFolder, MapServiceType.Folder);

                        if (MapServices.Where(s => s.Fullname == folderService.Fullname && s.Type == folderService.Type).Count() == 0)
                        {
                            MapServices.Add(folderService);
                            Console.WriteLine("folder " + folderService.Name + " added");
                        }

                        parentFolder = folderName;
                    }

                    #endregion
                }

                MapServices.Add(mapService);
                Console.WriteLine("service " + mapService.Name + " added");

                return mapService;
            }
        }

        public IMapService TryAddService(string name, string folder)
        {
            var mapFileInfo = new FileInfo(Options.ServicesPath + (String.IsNullOrWhiteSpace(folder) ? "" : "/" + folder) + "/" + name + ".mxl");
            return TryAddService(mapFileInfo, folder);
        }

        private static object _reloadServicesLocker = new object();
        private static object _reloadServicesLockerKey = null;
        public void ReloadServices(string folder, bool forceRefresh = false)
        {
            lock (_reloadServicesLocker)
            {
                while (_reloadServicesLockerKey != null)
                {
                    Thread.Sleep(100);
                }
                _reloadServicesLockerKey = new object();
            }
            try
            {
                if (forceRefresh == true || MapServices.Where(s => s.Type != MapServiceType.Folder && s.Folder == folder).Count() == 0)
                {
                    AddServices(folder);
                }
            }
            finally
            {
                _reloadServicesLockerKey = null;
            }
        }

        public IServiceRequestInterpreter GetInterpreter(Type type)
        {
            var interpreter = new PlugInManager().CreateInstance<IServiceRequestInterpreter>(type);
            if (interpreter == null)
            {
                throw new Exception("Can't intialize interperter");
            }

            interpreter.OnCreate(Instance);
            return interpreter;
        }

        public IServiceRequestInterpreter GetInterpreter(Guid guid)
        {
            var interpreter = new PlugInManager().CreateInstance(guid) as IServiceRequestInterpreter;
            if (interpreter == null)
            {
                throw new Exception("Can't intialize interperter");
            }

            interpreter.OnCreate(Instance);
            return interpreter;
        }

        public IMapService GetMapService(string id)
        {
            var mapService = MapServices
                        .Where(f => f.Type == MapServiceType.MXL && id.Equals(f.Fullname, StringComparison.InvariantCultureIgnoreCase))
                        .FirstOrDefault();

            return mapService;
        }

        public IMapService GetFolderService(string id)
        {
            var folderService = MapServices
                        .Where(f => f.Type == MapServiceType.Folder && String.IsNullOrWhiteSpace(f.Folder) && id.Equals(f.Name, StringComparison.InvariantCultureIgnoreCase))
                        .FirstOrDefault();

            return folderService;
        }
    }
}