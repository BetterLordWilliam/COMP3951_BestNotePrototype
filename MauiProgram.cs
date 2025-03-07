﻿using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Core.Hosting;
using Syncfusion.Maui.TreeView;

namespace BestNote_3951
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
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
            builder.ConfigureSyncfusionCore();
#endif

            return builder.Build();
        }
    }
}
