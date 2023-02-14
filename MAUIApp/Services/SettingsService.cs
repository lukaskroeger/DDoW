using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUIApp.Services;
public class SettingsService
{
    private const string SimilarityService = "similarity_service";
    private const string SimilarityServiceDefault = "";

    public string SimilarityServiceUri
    {
        get => Preferences.Get(SimilarityService, SimilarityServiceDefault);
        set => Preferences.Set(SimilarityService, value);
    }
}
