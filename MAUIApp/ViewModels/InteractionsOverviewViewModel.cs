using MAUIApp.Models;
using MAUIApp.Services;
using System.Collections.ObjectModel;

namespace MAUIApp.ViewModels;
public class InteractionsOverviewViewModel
{
    private readonly DatabaseService _database;
    public InteractionsOverviewViewModel(DatabaseService database)
    {
        _database = database;
        Interactions = new ObservableCollection<InteractionViewModel>();
        Task.Run(Init);
        ShowLikes = true;
    }

    public async Task Init()
    {
        await RefreshView();
    }
    private async Task RefreshView()
    {
        IEnumerable<Interaction> interactions = ShowLikes ? await _database.GetLikesAsync() : await _database.GetDislikesAsync();
        Interactions.Clear();
        AddAllToInteractionsList(interactions);
    }
    private void AddAllToInteractionsList(IEnumerable<Interaction> all)
    {
        foreach (InteractionViewModel interaction in all.Select(x => new InteractionViewModel(x)))
        {
            Interactions.Add(interaction);
        }

    }

    private bool _showLikes;
    public bool ShowLikes
    {
        get => _showLikes;
        set
        {
            _showLikes = value;
            Task.Run(RefreshView);
        }
    }
    public ObservableCollection<InteractionViewModel> Interactions { get; set; }
}
