﻿using gView.Blazor.Core.Extensions.DependencyInjection;
using gView.DataExplorer.Plugins.Extensions.DependencyInjection;
using gView.DataExplorer.Core.Extensions.DependencyInjeftion;
using Microsoft.Extensions.Logging;
using MudBlazor.Services;

namespace gView.DataExplorer
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            builder.Services.AddMudServices();

            builder.Services.AddExplorerDesktopApplicationService();
            builder.Services.AddPluginMangerService();
            builder.Services.AddIconService();
            builder.Services.AddEventBus();

            return builder.Build();
        }
    }
}