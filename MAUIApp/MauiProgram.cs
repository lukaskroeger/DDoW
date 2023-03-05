using MAUIApp.Services;
using MAUIApp.ViewModels;
using MAUIApp.Views;
using Microsoft.Extensions.Configuration;
using System.Reflection;

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
            .AddTransient<SettingsPage>()
            .AddTransient<SettingsViewModel>()
            .AddTransient<DataService>()
            .AddSingleton<HttpClient>()
            .AddSingleton<SettingsService>();

        var a = Assembly.GetExecutingAssembly();
        using var stream = a.GetManifestResourceStream("MAUIApp.appsettings.json");

        var config = new ConfigurationBuilder()
                    .AddJsonStream(stream)
                    .Build();


        builder.Configuration.AddConfiguration(config);

        return builder.Build();
    }
}
