using Microsoft.Extensions.Configuration;
using System.Xml.Linq;

namespace MAUIApp.Views;

[QueryProperty(nameof(Address), "address")]
public partial class WikipediaWebView : ContentPage
{
    public WikipediaWebView()
	{
        InitializeComponent();     
    }

    public Uri Address
    {
        set
        {            
            webView.Source = value;
        }
    }

}