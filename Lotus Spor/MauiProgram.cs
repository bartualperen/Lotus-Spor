using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Lotus_Spor.Services;

#if ANDROID
using Lotus_Spor.Platforms.Android;
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

#if ANDROID
            builder.Services.AddSingleton<IFontScaleProvider, FontScaleProvider>();
#endif

            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddTransient<LessonManagementPage>();
            builder.Services.AddTransient<AdminPanelPage>();

            return builder.Build();
        }
    }
}
