using MAUIApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MAUIApp.ViewModels;
public class SettingsViewModel
{

    private readonly SettingsService _settingsService;

    public SettingsViewModel(SettingsService settingsService)
    {
        _settingsService = settingsService;

        SimilarityServiceUri = _settingsService.SimilarityServiceUri;

        SaveSettingsCommand = new Command(async () =>
        {
            if (Uri.IsWellFormedUriString(SimilarityServiceUri, UriKind.Absolute))
            {
                UpdateSimilarityServiceEndpoint();
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await Shell.Current.DisplayAlert("Alert", "The url is not valid. Please input a valid url", "OK");
            }
        });

    }
    public string SimilarityServiceUri { get; set; }

    private void UpdateSimilarityServiceEndpoint()
    {
        _settingsService.SimilarityServiceUri = SimilarityServiceUri;
    }

    public ICommand SaveSettingsCommand { get; private set; }
}
