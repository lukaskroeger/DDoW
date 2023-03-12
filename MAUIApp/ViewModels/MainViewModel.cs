using MAUIApp.Models;
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
    private DatabaseService _database;
    private SettingsService _settingsService;

    public MainViewModel(DataService dataService, SettingsService settingsService, IConfiguration config, DatabaseService database)
    {
        _config = config;
        _dataService = dataService;
        _database = database;
        _settingsService = settingsService;
        Items = new ObservableCollection<WikiCardViewModel>();
        Items.CollectionChanged += Items_CollectionChanged;

        LikeCommand = new Command(async (item) =>
        {
            WikiCardViewModel viewModel = (WikiCardViewModel)item;
            Items.Remove(viewModel);
            await _database.InsertInteraction(Interaction.FromWikiArticleNow(viewModel.Article, InteractionType.Like));
            List<Models.WikiArticle>? articles = null;
            try
            {
                articles = await dataService.LikeArticle(viewModel.ArticleId);
            }
            catch (Exception ex)
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
            await _database.InsertInteraction(Interaction.FromWikiArticleNow(viewModel.Article, InteractionType.Dislike));
            Items.Remove(viewModel);
        });

        ReadMoreCommand = new Command(async () =>
        {
            WikiCardViewModel? viewModel = Items.FirstOrDefault();
            if (viewModel is null)
            {
                return;
            }
            Uri baseAddress = new($"https://{_settingsService.LanguageKey}.{_config.GetSection("Wikipedia")["ContentUrl"]}");
            Uri uri = new(baseAddress, Uri.EscapeDataString(viewModel.ArticleId));
            Dictionary<string, object> navigationParameter = new()
            {
                {"address", uri }
            };
            await Shell.Current.GoToAsync($"{nameof(WikipediaWebView)}", navigationParameter);
        });

        OpenSettingsCommand = new Command(async () =>
        {
            Func<bool, Task> saveFunction = async (languageChange) =>
            {
                if (languageChange)
                {
                    await _database.ClearCardStack();
                    Items.Clear();
                }
            };
            Dictionary<string, object> navigationParameter = new()
            {
                {"saveFunction", saveFunction }
            };
            await Shell.Current.GoToAsync($"{nameof(SettingsPage)}", navigationParameter);
        });

        OpenInteractionsCommand = new Command(async () =>
        {
            await Shell.Current.GoToAsync($"{nameof(InteractionsPage)}");
        });

        InitCards();
    }

    private async void Items_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        IList? list = sender as IList;
        if (list is not null && list.Count == 0)
        {
            //TODO Check if a request is still in process
            IEnumerable<WikiArticle> articles = await _dataService.GetRandom(5);
            AddArticlesToItems(articles);
        }

        switch (e.Action)
        {
            case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                if (e.NewItems?.Cast<WikiCardViewModel>() is IEnumerable<WikiCardViewModel> newItems)
                {
                    await _database.InsertCardStackArticles(newItems.Select(x => x.Article));
                }
                break;
            case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                if (e.OldItems?.Cast<WikiCardViewModel>() is IEnumerable<WikiCardViewModel> oldItems)
                {
                    await _database.DeleteCardStackArticles(oldItems.Select(x => x.ArticleId));
                }
                break;
        }
    }

    private async Task InitCards()
    {
        IEnumerable<WikiArticle> articles = await _database.GetCardStackAsync();
        if (articles.Count() == 0)
        {
            articles = await _dataService.GetRandom(5);
        }
        AddArticlesToItems(articles);
    }

    private async Task<IEnumerable<WikiArticle>> GetRandomArticles(int amount)
    {
        IEnumerable<WikiArticle> randomArticles = await _dataService.GetRandom(amount);
        return randomArticles;
    }
    private void AddArticlesToItems(IEnumerable<WikiArticle> articles)
    {
        foreach (WikiCardViewModel article in articles.Select(x => new WikiCardViewModel(x)).Where(x => !string.IsNullOrWhiteSpace(x.Text)))
        {
            Items.Add(article);
        }
    }

    public ICommand LikeCommand { get; set; }
    public ICommand DislikeCommand { get; set; }
    public ICommand ReadMoreCommand { get; set; }
    public ICommand OpenSettingsCommand { get; private set; }
    public ICommand OpenInteractionsCommand { get; private set; }
}
