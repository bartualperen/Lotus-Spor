using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Lotus_Spor.Services;

using Microsoft.Maui.Hosting;
#if ANDROID
using Lotus_Spor.Platforms.Android;
//#elif IOS
//using Lotus_Spor.Platforms.iOS;
//#elif WINDOWS
//using Lotus_Spor.Platforms.Windows;
#endif



namespace Lotus_Spor
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
                })
                .UseMauiCommunityToolkit();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddTransient<LessonManagementPage>();
            builder.Services.AddTransient<AdminPanelPage>();

            return builder.Build();
        }
    }
}
