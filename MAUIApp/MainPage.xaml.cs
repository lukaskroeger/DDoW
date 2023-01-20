using MAUIApp.ViewModels;

namespace MAUIApp;

public partial class MainPage : ContentPage
{
    public MainPage(MainViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
    }

    private async void DislikeButton_Clicked(object sender, EventArgs e)
    {
        await cardView.DislikeTopCard();
    }

    private async void LikeButton_Clicked(object sender, EventArgs e)
    {
        await cardView.LikeTopCard();
    }

    private async void UpButton_Clicked(object sender, EventArgs e)
    {
        await cardView.UpTopCard();
    }
}

