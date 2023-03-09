using MAUIApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUIApp.ViewModels;
public class InteractionViewModel
{
    private readonly Interaction _interaction;
    public InteractionViewModel(Interaction interaction)
    {
        _interaction = interaction;
    }

    public string ArticleId { get => _interaction.ArticleId; }
    public string Date { get => _interaction.DateTime.ToShortDateString(); }
    public string Time { get => _interaction.DateTime.ToShortTimeString(); }
    public string PrecedorArticleText { get => $"Source: {_interaction.PredecessorArticleId}"; }

}
