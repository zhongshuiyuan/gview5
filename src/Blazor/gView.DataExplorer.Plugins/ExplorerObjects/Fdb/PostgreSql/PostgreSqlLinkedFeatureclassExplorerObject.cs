﻿using gView.DataExplorer.Plugins.Extensions;
using gView.DataExplorer.Razor.Components.Dialogs.Filters;
using gView.DataExplorer.Razor.Components.Dialogs.Models;
using gView.DataSources.Fdb.MSAccess;
using gView.Framework.Blazor.Services.Abstraction;
using gView.Framework.Core.Data;
using gView.Framework.Core.Common;
using gView.Framework.DataExplorer.Abstraction;
using gView.Framework.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using gView.Framework.DataExplorer.Services.Abstraction;

namespace gView.DataExplorer.Plugins.ExplorerObjects.Fdb.PostgreSql;

//[RegisterPlugIn("18830A3E-FFEA-477c-B3E1-E4624F828034")]
//public class NetworkClassExplorerObject : ExplorerObjectCls, IExplorerSimpleObject, IExplorerObjectCreatable
//{
//    public NetworkClassExplorerObject() : base(null, typeof(pgFeatureClass), 1) { }

//    #region IExplorerObject Member

//    public string Name
//    {
//        get { return String.Empty; }
//    }

//    public string FullName
//    {
//        get { return String.Empty; }
//    }

//    public string Type
//    {
//        get { return "Network Class"; }
//    }

//    public IExplorerIcon Icon
//    {
//        get
//        {
//            return new gView.DataSources.Fdb.UI.MSAccess.AccessFDBNetworkIcon();
//        }
//    }

//    public Task<object> GetInstanceAsync()
//    {
//        return Task.FromResult<object>(null);
//    }

//    #endregion

//    #region IDisposable Member

//    public void Dispose()
//    {

//    }

//    #endregion

//    #region ISerializableExplorerObject Member

//    public Task<IExplorerObject> CreateInstanceByFullName(string FullName, ISerializableExplorerObjectCache cache)
//    {
//        return Task.FromResult<IExplorerObject>(null);
//    }

//    #endregion

//    #region IExplorerObjectCreatable Member

//    public bool CanCreate(IExplorerObject parentExObject)
//    {
//        if (parentExObject is DatasetExplorerObject)
//        {
//            return true;
//        }

//        return false;
//    }

//    async public Task<IExplorerObject> CreateExplorerObject(IExplorerObject parentExObject)
//    {
//        if (!(parentExObject is DatasetExplorerObject))
//        {
//            return null;
//        }

//        IFeatureDataset dataset = await ((DatasetExplorerObject)parentExObject).GetInstanceAsync() as IFeatureDataset;
//        if (dataset == null || !(dataset.Database is pgFDB))
//        {
//            return null;
//        }

//        FormNewNetworkclass dlg = new FormNewNetworkclass(dataset, typeof(CreateFDBNetworkFeatureclass));
//        if (dlg.ShowDialog() != DialogResult.OK)
//        {
//            return null;
//        }

//        CreateFDBNetworkFeatureclass creator = new CreateFDBNetworkFeatureclass(
//            dataset, dlg.NetworkName,
//            dlg.EdgeFeatureclasses,
//            dlg.NodeFeatureclasses);
//        creator.SnapTolerance = dlg.SnapTolerance;
//        creator.ComplexEdgeFcIds = await dlg.ComplexEdgeFcIds();
//        creator.GraphWeights = dlg.GraphWeights;
//        creator.SwitchNodeFcIdAndFieldnames = dlg.SwitchNodeFcIds;
//        creator.NodeTypeFcIds = dlg.NetworkNodeTypeFcIds;

//        FormTaskProgress progress = new FormTaskProgress();
//        progress.ShowProgressDialog(creator, creator.Run());

//        IDatasetElement element = await dataset.Element(dlg.NetworkName);
//        return new FeatureClassExplorerObject(
//            parentExObject as DatasetExplorerObject,
//            parentExObject.Name,
//            element);
//    }

//    #endregion
//}

//[RegisterPlugIn("af5f4d50-745b-11e1-b0c4-0800200c9a66")]
//public class GeographicViewExplorerObject : IExplorerSimpleObject, IExplorerObjectCreatable
//{
//    #region IExplorerObjectCreatable Member

//    public bool CanCreate(IExplorerObject parentExObject)
//    {
//        return parentExObject is DatasetExplorerObject;
//    }

//    async public Task<IExplorerObject> CreateExplorerObject(IExplorerObject parentExObject)
//    {
//        DatasetExplorerObject parent = (DatasetExplorerObject)parentExObject;

//        IFeatureDataset dataset = await parent.GetInstanceAsync() as IFeatureDataset;
//        if (dataset == null)
//        {
//            return null;
//        }

//        AccessFDB fdb = dataset.Database as AccessFDB;
//        if (fdb == null)
//        {
//            return null;
//        }

//        FormRegisterGeographicView dlg = await FormRegisterGeographicView.CreateAsync(dataset);
//        if (dlg.ShowDialog() == DialogResult.OK)
//        {
//            int fc_id = await fdb.CreateSpatialView(dataset.DatasetName, dlg.SpatialViewAlias);

//            IDatasetElement element = await dataset.Element(dlg.SpatialViewAlias);
//            return new FeatureClassExplorerObject(
//                parentExObject as DatasetExplorerObject,
//                parentExObject.Name,
//                element);
//        }
//        return null;
//    }

//    #endregion

//    #region IExplorerObject Member

//    public string Name
//    {
//        get { return "Geographic View"; }
//    }

//    public string FullName
//    {
//        get { return "Geographic View"; }
//    }

//    public string Type
//    {
//        get { return String.Empty; }
//    }

//    public IExplorerIcon Icon
//    {
//        get { return new AccessFDBGeographicViewIcon(); }
//    }

//    public IExplorerObject ParentExplorerObject
//    {
//        get { return null; }
//    }

//    public Task<object> GetInstanceAsync()
//    {
//        return Task.FromResult<object>(null);
//    }

//    public Type ObjectType
//    {
//        get { return null; }
//    }

//    public int Priority { get { return 1; } }

//    #endregion

//    #region IDisposable Member

//    public void Dispose()
//    {

//    }

//    #endregion

//    #region ISerializableExplorerObject Member

//    public Task<IExplorerObject> CreateInstanceByFullName(string FullName, ISerializableExplorerObjectCache cache)
//    {
//        return Task.FromResult<IExplorerObject>(null);
//    }

//    #endregion
//}

[RegisterPlugIn("D320CA4D-E63F-4DD4-96F4-CA11DC95A39E")]
public class PostgreSqlLinkedFeatureclassExplorerObject : IExplorerSimpleObject, 
                                                IExplorerObjectCreatable
{
    #region IExplorerObjectCreatable Member

    public bool CanCreate(IExplorerObject parentExObject)
    {
        return parentExObject is PostgreSqlDatasetExplorerObject;
    }

    async public Task<IExplorerObject?> CreateExplorerObjectAsync(IExplorerApplicationScopeService scope, IExplorerObject parentExObject)
    {
        PostgreSqlDatasetExplorerObject parent = (PostgreSqlDatasetExplorerObject)parentExObject;

        IFeatureDataset? dataset = await parent.GetInstanceAsync() as IFeatureDataset;
        if (dataset == null)
        {
            return null;
        }

        AccessFDB? fdb = dataset.Database as AccessFDB;
        if (fdb == null)
        {
            return null;
        }

        var model = await scope.ShowKnownDialog(
           Framework.Blazor.KnownDialogs.ExplorerDialog,
           model: new ExplorerDialogModel()
           {
               Filters = new List<ExplorerDialogFilter> {
                    new OpenFeatureclassFilter()
               },
               Mode = ExploerDialogMode.Open
           });

        if (model?.Result?.ExplorerObjects != null)
        {
            foreach (var exObject in model.Result.ExplorerObjects)
            {
                var exObjectInstance = await exObject.GetInstanceAsync();
                if (exObjectInstance is IFeatureClass)
                {
                    int fcid = await fdb.CreateLinkedFeatureClass(dataset.DatasetName, (IFeatureClass)exObjectInstance);
                }
            }

            await scope.ForceContentRefresh();
        }

        return null;
    }

    #endregion

    #region IExplorerObject Member

    public string Name
    {
        get { return "Linked Featureclass"; }
    }

    public string FullName
    {
        get { return "Linked Featureclass"; }
    }

    public string Type
    {
        get { return "Linked Featureclass"; }
    }

    public string Icon => "basic:open-in-window";

    public IExplorerObject? ParentExplorerObject
    {
        get { return null; }
    }

    public Task<object?> GetInstanceAsync()
    {
        return Task.FromResult<object?>(null);
    }

    public Type? ObjectType
    {
        get { return null; }
    }

    public int Priority { get { return 1; } }

    #endregion

    #region IDisposable Member

    public void Dispose()
    {

    }

    #endregion

    #region ISerializableExplorerObject Member

    public Task<IExplorerObject?> CreateInstanceByFullName(string FullName, ISerializableExplorerObjectCache? cache)
    {
        return Task.FromResult<IExplorerObject?>(null);
    }

    #endregion
}

////[RegisterPlugIn("9B5B718C-2ECA-47ee-851F-9D33E3D82C55")]
//public class SqlFDBTileGridClassExplorerObject : ExplorerObjectCls, IExplorerSimpleObject, IExplorerObjectCreatable
//{
//    public SqlFDBTileGridClassExplorerObject() : base(null, null, 1) { }

//    #region IExplorerObject Member

//    public string Name
//    {
//        get { return String.Empty; }
//    }

//    public string FullName
//    {
//        get { return String.Empty; }
//    }

//    public string Type
//    {
//        get { return "Tile Grid Class"; }
//    }

//    public IExplorerIcon Icon
//    {
//        get
//        {
//            return new gView.DataSources.Fdb.UI.MSAccess.AccessFDBRasterIcon();
//        }
//    }

//    public Task<object> GetInstanceAsync()
//    {
//        return Task.FromResult<object>(null);
//    }

//    public new int Priority { get { return 1; } }

//    #endregion

//    #region IDisposable Member

//    public void Dispose()
//    {

//    }

//    #endregion

//    #region ISerializableExplorerObject Member

//    public Task<IExplorerObject> CreateInstanceByFullName(string FullName, ISerializableExplorerObjectCache cache)
//    {
//        return Task.FromResult<IExplorerObject>(null);
//    }

//    #endregion

//    #region IExplorerObjectCreatable Member

//    public bool CanCreate(IExplorerObject parentExObject)
//    {
//        if (parentExObject is DatasetExplorerObject)
//        {
//            return true;
//        }

//        return false;
//    }

//    async public Task<IExplorerObject> CreateExplorerObject(IExplorerObject parentExObject)
//    {
//        if (!(parentExObject is DatasetExplorerObject))
//        {
//            return null;
//        }

//        IFeatureDataset dataset = await ((DatasetExplorerObject)parentExObject).GetInstanceAsync() as IFeatureDataset;
//        if (dataset == null || !(dataset.Database is SqlFDB))
//        {
//            return null;
//        }

//        FormNewTileGridClass dlg = new FormNewTileGridClass();
//        if (dlg.ShowDialog() != DialogResult.OK)
//        {
//            return null;
//        }

//        CreateTileGridClass creator = new CreateTileGridClass(
//            dlg.GridName,
//            dataset, dlg.SpatialIndexDefinition,
//            dlg.RasterDataset,
//            dlg.TileSizeX, dlg.TileSizeY, dlg.ResolutionX, dlg.ResolutionY, dlg.Levels,
//            dlg.TileCacheDirectory,
//            dlg.TileGridType);
//        creator.CreateTiles = dlg.GenerateTileCache;
//        creator.TileLevelType = dlg.TileLevelType;
//        creator.CreateLevels = dlg.CreateLevels;

//        FormTaskProgress progress = new FormTaskProgress();
//        progress.ShowProgressDialog(creator, creator.RunTask());

//        IDatasetElement element = await dataset.Element(dlg.GridName);
//        return new FeatureClassExplorerObject(
//            parentExObject as DatasetExplorerObject,
//            parentExObject.Name,
//            element);
//    }

//    #endregion
//}
