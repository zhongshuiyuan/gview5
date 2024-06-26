﻿using gView.DataExplorer.Plugins.ExplorerObjects.Base;
using gView.Framework.Common;
using gView.Framework.Core.Common;
using gView.Framework.DataExplorer.Abstraction;
using System.Linq;
using System.Threading.Tasks;

namespace gView.DataExplorer.Plugins.ExplorerObjects.ToolCommands;

[RegisterPlugIn("DCEE848B-F1FC-44AC-B378-6999EEECAF48")]
internal class ToolsObject : ExplorerParentObject,
                             IExplorerGroupObject
{
    public ToolsObject()
        : base(50)
    {
    }

    #region IExplorerObject Member

    public string Name => "Tools";

    public string FullName => "Tools";

    public string? Type => "Command Tools";

    public string Icon => "basic:package";

    public Task<object?> GetInstanceAsync() => Task.FromResult<object?>(null);

    #endregion

    #region ISerializableExplorerObject Member

    public Task<IExplorerObject?> CreateInstanceByFullName(string FullName, ISerializableExplorerObjectCache? cache)
    {
        if (FullName == string.Empty)
        {
            return Task.FromResult<IExplorerObject?>(new ToolsObject());
        }

        return Task.FromResult<IExplorerObject?>(null);
    }

    #endregion

    #region IExplorerParentObject Member

    async public override Task<bool> Refresh()
    {
        await base.Refresh();

        var pluginManager = new PlugInManager();

        foreach (var toolCommand in pluginManager
                                        .GetPluginInstances(typeof(IExplorerToolCommand))
                                        .Where(p => p is IExplorerToolCommand)
                                        .Select(p => (IExplorerToolCommand)p)
                                        .OrderBy(p => p.Name))
        {
            AddChildObject(new ToolObject(this, toolCommand));
        }

        return true;
    }

    #endregion

    #region IExplorerGroupObject

    public void SetParentExplorerObject(IExplorerObject parentExplorerObject)
    {
        Parent = parentExplorerObject;
    }

    #endregion
}
