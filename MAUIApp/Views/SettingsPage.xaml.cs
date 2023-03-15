using MAUIApp.ViewModels;
using System.Net;

namespace MAUIApp.Views;

public partial class SettingsPage : ContentPage
{
	public SettingsPage(SettingsViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
	}
}