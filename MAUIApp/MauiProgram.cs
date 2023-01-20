using MAUIApp.Services;
using MAUIApp.ViewModels;
using Microsoft.Extensions.Configuration;

namespace MAUIApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        MauiAppBuilder builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });
        builder.Services
            .AddTransient<MainPage>()
            .AddTransient<MainViewModel>()
            .AddTransient<DataService>()
            .AddSingleton<HttpClient>();

        return builder.Build();
    }
}
