using System.Xml.Linq;

namespace MAUIApp.Views;

[QueryProperty(nameof(Name), "name")]
public partial class WikipediaWebView : ContentPage
{
    
    public WikipediaWebView()
	{
        InitializeComponent();
        
    }

    public string Name
    {
        set
        {
            webView.Source = Uri.EscapeDataString($"https://en.m.wikipedia.org/wiki/{value}");
        }
    }

}