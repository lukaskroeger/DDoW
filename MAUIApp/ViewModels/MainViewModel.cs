using MAUIApp.Services;
using MAUIApp.Views;
using Microsoft.Extensions.Configuration;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MAUIApp.ViewModels;
public class MainViewModel
{
    public ObservableCollection<WikiCardViewModel> Items { get; set; }

    private IConfiguration _config;
    private DataService _dataService;

    public MainViewModel(DataService dataService, IConfiguration config)
    {
        _config = config;
        _dataService = dataService;
        Items = new ObservableCollection<WikiCardViewModel>();
        Items.CollectionChanged += Items_CollectionChanged;

        LikeCommand = new Command(async (item) =>
        {
            WikiCardViewModel viewModel = (WikiCardViewModel)item;
            Items.Remove(viewModel);
            List<Models.WikiArticle>? articles = null;
            try
            {
                articles = await dataService.LikeArticle(viewModel.ArticleId);
            }
            catch(Exception ex)
            {
                await Shell.Current.DisplayAlert("Error during request", ex.Message, "Ok");
                return;
            }
            if (articles is null)
            {
                return;
            }
            IEnumerable<WikiCardViewModel> newArticles = await Task.Run(() => articles.Select(a => new WikiCardViewModel(a)));
            foreach (WikiCardViewModel newArticle in newArticles)
            {
                Items.Add(newArticle);
            }           

        });
        DislikeCommand = new Command(async (item) =>
        {
            WikiCardViewModel viewModel = (WikiCardViewModel)item;
            Items.Remove(viewModel);
        });

        ReadMoreCommand = new Command(async () =>
        {
            WikiCardViewModel? viewModel = Items.FirstOrDefault();
            if(viewModel is null)
            {
                return;
            }
            Uri baseAddress = new Uri(_config.GetSection("Wikipedia")["ContentUrl"]);
            Uri uri = new Uri(baseAddress, Uri.EscapeDataString(viewModel.ArticleId));
            var navigationParameter = new Dictionary<string, object>
            {
                {"address", uri }
            };
            await Shell.Current.GoToAsync($"{nameof(WikipediaWebView)}", navigationParameter);
        });

        OpenSettingsCommand = new Command(async () =>
        {
            await Shell.Current.GoToAsync($"{nameof(SettingsPage)}");
        });

        InitCards();
    }

    private async void Items_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        IList list = (IList)sender;
        if (list.Count == 0) 
        {   
            //TODO Check if a request is still in process
            await InitCards();
        }
    }

    private async Task InitCards()
    {
        IEnumerable<WikiCardViewModel> articles = (await _dataService.GetRandom(5)).Select(a => new WikiCardViewModel(a));
        foreach (WikiCardViewModel initialArticle in articles)
        {
            Items.Add(initialArticle);
        }
    }
    public ICommand LikeCommand { get; set; }
    public ICommand DislikeCommand { get; set; }
    public ICommand ReadMoreCommand { get; set; }
    public ICommand OpenSettingsCommand { get; private set; }
}
