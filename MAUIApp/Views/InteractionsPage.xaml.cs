using MAUIApp.ViewModels;

namespace MAUIApp.Views;

public partial class InteractionsPage : ContentPage
{
    private readonly InteractionsOverviewViewModel _viewModel;
	public InteractionsPage(InteractionsOverviewViewModel viewModel)
	{
		InitializeComponent();
        _viewModel= viewModel;
        BindingContext = _viewModel;
	}
}