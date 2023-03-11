using MAUIApp.Models;
using MAUIApp.Services;
using Microsoft.Extensions.Configuration;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MAUIApp.ViewModels;
public class InteractionsOverviewViewModel
{
    private readonly DatabaseService _database;
    private readonly IConfiguration _config;
    public InteractionsOverviewViewModel(IConfiguration config, DatabaseService database)
    {
        _database = database;
        _config = config;
        Interactions = new ObservableCollection<InteractionViewModel>();
        ShowLikes = true;
        ClearInteractions = new Command(async () =>
        {
            await _database.ClearInteractions();
            await RefreshView();
        });
    }

    private async Task RefreshView()
    {
        IEnumerable<Interaction> interactions = ShowLikes ? await _database.GetLikesAsync() : await _database.GetDislikesAsync();
        Interactions.Clear();
        AddAllToInteractionsList(interactions);
    }
    private void AddAllToInteractionsList(IEnumerable<Interaction> all)
    {
        foreach (InteractionViewModel interaction in all.Select(x => new InteractionViewModel(_config, x)))
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
            if (value != _showLikes)
            {
                _showLikes = value;
                Task.Run(RefreshView);
            }
        }
    }
    public ObservableCollection<InteractionViewModel> Interactions { get; set; }
    public ICommand ClearInteractions { get; private set; }
}
