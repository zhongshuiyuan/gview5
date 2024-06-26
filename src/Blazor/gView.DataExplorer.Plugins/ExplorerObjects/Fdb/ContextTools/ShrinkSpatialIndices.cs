﻿using gView.Cmd.Core.Abstraction;
using gView.Cmd.Fdb.Lib;
using gView.DataExplorer.Plugins.Extensions;
using gView.DataExplorer.Razor.Components.Dialogs.Models;
using gView.Framework.Blazor;
using gView.Framework.Blazor.Services.Abstraction;
using gView.Framework.Core.Data;
using gView.Framework.DataExplorer.Abstraction;
using gView.Framework.Common;
using System.Collections.Generic;
using System.Threading.Tasks;
using gView.Framework.DataExplorer.Services.Abstraction;

namespace gView.DataExplorer.Plugins.ExplorerObjects.Fdb.ContextTools;

internal class ShrinkSpatialIndices : IExplorerObjectContextTool
{
    public string Name => "Shrink spatial indices...";

    public string Icon => "basic:warning_yellow";

    public bool IsEnabled(IExplorerApplicationScopeService scope, IExplorerObject exObject)
    {
        return true;
    }

    async public Task<bool> OnEvent(IExplorerApplicationScopeService scope, IExplorerObject exObject)
    {
        var instance = await exObject.GetInstanceAsync();

        IDictionary<string, object>? parameters = null;
        ICommand? command = null;

        if (instance is IFeatureDataset)
        {
            var featureDataset = (IFeatureDataset)instance;
            var featureDatasetGuid = PlugInManager.PlugInID(featureDataset);

            command = new ShrinkDatasetSpatialIndexCommand();
            parameters = new Dictionary<string, object>()
            {
                { "dataset_connstr", featureDataset.ConnectionString },
                { "dataset_guid", featureDatasetGuid.ToString() }
            };
        }
        else if (instance is IFeatureClass)
        {
            var featureClass = (IFeatureClass)instance;
            var featureDataset = featureClass.Dataset;
            var featureDatasetGuid = PlugInManager.PlugInID(featureDataset);

            command = new ShrinkFeatureClassSpatialIndexCommand();
            parameters = new Dictionary<string, object>()
            {
                { "dataset_connstr", featureDataset.ConnectionString },
                { "dataset_guid", featureDatasetGuid.ToString() },
                { "dataset_fc", featureClass.Name },
            };
        }
        else
        {
            return false;
        }

        await scope.ShowKnownDialog(
                    KnownDialogs.ExecuteCommand,
                    $"Shrink spatial index",
                    new ExecuteCommandModel()
                    {
                        CommandItems = new[]
                        {
                            new CommandItem()
                            {
                                Command = command,
                                Parameters = parameters
                            }
                        }
                    });

        return true;
    }
}
