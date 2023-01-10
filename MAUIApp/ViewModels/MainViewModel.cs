using MAUIApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            var articles = await _dataService.LikeArticle(viewModel.ArticleId);
            var newArticles = articles.Select(a => new WikiCardViewModel(a));
            foreach (var newArticle in newArticles)
            {
                Items.Add(newArticle);
            }
        });
        DislikeCommand = new Command(async (item) =>
        {
            WikiCardViewModel viewModel = (WikiCardViewModel)item;
            Items.Remove(viewModel);            
        });
        Init();
    }
    private async void Init()
    {        
        var articles = (await _dataService.GetInitial()).Select(a => new WikiCardViewModel(a));
        foreach (var initialArticle in articles)
        {
            Items.Add(initialArticle);
        }        
    }
    public ICommand LikeCommand { get; set; }
    public ICommand DislikeCommand { get; set; }
    public ICommand UpCommand { get; set; }

}
