using MAUIApp.ViewModels;

namespace MAUIApp;

public partial class App : Application
{
	public App(MainPage mainPage)
	{
		InitializeComponent();
        MainPage = mainPage;
	}
}
