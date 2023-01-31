using MAUIApp.Services;
using MAUIApp.Views;
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
        LikeCommand = new Command(async (item) =>
        {
            WikiCardViewModel viewModel = (WikiCardViewModel)item;
            Items.Remove(viewModel);
            List<Models.WikiArticle> articles = await _dataService.LikeArticle(viewModel.ArticleId);
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
        Init();
    }
    private async void Init()
    {
        IEnumerable<WikiCardViewModel> articles = (await _dataService.GetInitial()).Select(a => new WikiCardViewModel(a));
        foreach (WikiCardViewModel initialArticle in articles)
        {
            Items.Add(initialArticle);
        }
    }
    public ICommand LikeCommand { get; set; }
    public ICommand DislikeCommand { get; set; }
    public ICommand ReadMoreCommand { get; set; }

}
