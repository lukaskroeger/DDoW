using MAUIApp.Models;
using MAUIApp.Services;
using Microsoft.Extensions.Configuration;
using System.Windows.Input;

namespace MAUIApp.ViewModels;
public class SettingsViewModel : IQueryAttributable
{
    private readonly DatabaseService _database;
    private readonly SettingsService _settingsService;

    public SettingsViewModel(IConfiguration config, SettingsService settingsService, DatabaseService database)
    {
        _settingsService = settingsService;

        SimilarityServiceUri = _settingsService.SimilarityServiceUri;

        Languages = config.GetSection("Wikipedia:Languages").GetChildren().Select(x => new Language() { DisplayName = x.Value, Key = x.Key }).ToList();
        SelectedLanguage = Languages.First(x => x.Key == _settingsService.LanguageKey);

        SaveSettingsCommand = new Command(async () =>
        {
            if (!Uri.IsWellFormedUriString(SimilarityServiceUri, UriKind.Absolute))
            {
                await Shell.Current.DisplayAlert("Alert", "The url is not valid. Please input a valid url", "OK");
            }

            await Shell.Current.GoToAsync("..");
            await UpdateSettings();
        });
        _database = database;
    }
    public string SimilarityServiceUri { get; set; }

    private async Task UpdateSettings()
    {
        _settingsService.SimilarityServiceUri = SimilarityServiceUri;
        if (_settingsService.LanguageKey != SelectedLanguage.Key)
        {
            _settingsService.LanguageKey = SelectedLanguage.Key;
            await SaveFunction(true);
            return;
        }
        await SaveFunction(false);
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        SaveFunction = (Func<bool, Task>)query["saveFunction"];
    }

    public Language SelectedLanguage { get; set; }
    public IList<Language> Languages { get; set; }
    public ICommand SaveSettingsCommand { get; private set; }
    public Func<bool, Task> SaveFunction { get; set; }
}
