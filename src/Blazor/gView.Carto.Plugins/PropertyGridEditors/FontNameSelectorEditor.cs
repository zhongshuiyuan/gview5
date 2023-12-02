﻿using gView.Carto.Plugins.Extensions;
using gView.Carto.Plugins.PropertyGridEditors.Models;
using gView.Carto.Razor.Components.Dialogs.Models;
using gView.Framework.Blazor.Services.Abstraction;
using gView.Razor.Abstractions;
using System.Reflection;

namespace gView.Carto.Plugins.PropertyGridEditors;

internal class FontNameSelectorEditor : IPropertyGridEditAsync
{
    public Type PropertyType => typeof(FontName);

    async public Task<object?> EditAsync(IApplicationScope scope, object instance, PropertyInfo propertyInfo)
    {
        var scopeService = scope.ToCartoScopeService();

        var fontName = propertyInfo.GetValue(instance) as FontName;
        if (fontName == null)
        {
            return null;
        }

        var model = await scopeService.ShowModalDialog(
            typeof(gView.Carto.Razor.Components.Dialogs.FontNameSelectorDialog),
            "Select Font",
            new FontNameSelectorModel()
            {
                FontName = fontName.Value,
            });

        if (model is null)
        {
            return null;
        }

        fontName.Value = model.FontName;

        return fontName;
    }
}
