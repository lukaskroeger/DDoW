using MAUIApp.Services;
using MAUIApp.Views;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MAUIApp.ViewModels;
public class MainViewModel
{
    public ObservableCollection<WikiCardViewModel> Items { get; set; }
    private DataService _dataService;

    public MainViewModel(DataService dataService)
    {
        _dataService = dataService;
        Items = new ObservableCollection<WikiCardViewModel>();
        Items.CollectionChanged += Items_CollectionChanged;

        _currentRequests = 0;

        LikeCommand = new Command(async (item) =>
        {
            WikiCardViewModel viewModel = (WikiCardViewModel)item;
            Items.Remove(viewModel);
            var currentRequest = dataService.LikeArticle(viewModel.ArticleId);
            List<Models.WikiArticle> articles = await currentRequest;
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
            WikiCardViewModel viewModel = Items.FirstOrDefault();
            await Shell.Current.GoToAsync($"{nameof(WikipediaWebView)}?name={viewModel.ArticleId}");
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

}
