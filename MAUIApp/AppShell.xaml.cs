using MAUIApp.Views;

namespace MAUIApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(WikipediaWebView), typeof(WikipediaWebView));
    }
}
