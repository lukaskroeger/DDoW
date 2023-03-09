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
    }

    public async Task Init()
    {
        IEnumerable<Interaction> interactions = await _database.GetLikesAsync();
        foreach (InteractionViewModel interaction in interactions.Select(x => new InteractionViewModel(x)))
        {
            Interactions.Add(interaction);
        }
    }
    public ObservableCollection<InteractionViewModel> Interactions { get; set; }
}
