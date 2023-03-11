namespace MAUIApp.Services;
public class SettingsService
{
    private const string SimilarityService = "similarity_service";
    private const string SimilarityServiceDefault = "";

    private const string Language = "language";
    private const string LanguageDefault = "en";

    public string SimilarityServiceUri
    {
        get => Preferences.Get(SimilarityService, SimilarityServiceDefault);
        set => Preferences.Set(SimilarityService, value);
    }

    public string LanguageKey
    {
        get => Preferences.Get(Language, LanguageDefault);
        set => Preferences.Set(Language, value);
    }
}
