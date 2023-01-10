using MAUIApp.ViewModels;

namespace MAUIApp;

public partial class MainPage : ContentPage
{
	public MainPage(MainViewModel viewModel)
	{
        BindingContext = viewModel;
        InitializeComponent();				
	}	
}

