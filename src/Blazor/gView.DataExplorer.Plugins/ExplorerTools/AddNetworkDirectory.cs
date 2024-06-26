﻿using gView.DataExplorer.Plugins.ExplorerObjects.FileSystem;
using gView.DataExplorer.Razor.Components.Dialogs.Models;
using gView.Framework.Core.Common;
using gView.Framework.DataExplorer;
using gView.Framework.DataExplorer.Abstraction;
using gView.Framework.DataExplorer.Services.Abstraction;
using gView.Framework.IO;
using System.IO;
using System.Threading.Tasks;

namespace gView.DataExplorer.Plugins.ExplorerTools;

[RegisterPlugIn("7cbbed5e-c071-46de-b30a-9c6140dafd75")]
[AuthorizedPlugin(RequireAdminRole = true)]
internal class AddNetworkDirectory : IExplorerTool
{
    #region IExplorerTool

    public string Name => "Map (network) folder...";

    public bool IsEnabled(IExplorerApplicationScopeService scope) => true;

    public string ToolTip => "";

    public string Icon => "basic:open-in-window";

    public ExplorerToolTarget Target => ExplorerToolTarget.General;

    public async Task<bool> OnEvent(IExplorerApplicationScopeService scope)
    {
        MapNetworkFolderModel? model = null;

        if (scope.CurrentExplorerObject is DirectoryObject)
        {
            model = new MapNetworkFolderModel()
            {
                FolderPath = ((DirectoryObject)scope.CurrentExplorerObject).FullName
            };
        }

        model = await scope.ShowModalDialog(
            typeof(Razor.Components.Dialogs.MapNetworkFolderDialog),
            this.Name,
            model);

        if (!string.IsNullOrWhiteSpace(model?.FolderPath) &&
            Directory.Exists(model.FolderPath))
        {
            ConfigConnections connStream = ConfigConnections.Create(scope.ConfigConnectionStorage, "directories");
            connStream.Add(model.FolderPath.Trim(), model.FolderPath.Trim());

            await scope.ForceContentRefresh();
        }

        return true;
    }

    #endregion

    #region IOrder

    public int SortOrder => 25;

    #endregion

    #region IDisposable

    public void Dispose()
    {

    }

    #endregion
}
