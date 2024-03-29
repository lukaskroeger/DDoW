﻿using MAUIApp.Models;
using MAUIApp.Services;
using MAUIApp.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MAUIApp.ViewModels;
public class InteractionViewModel
{
    private readonly Interaction _interaction;
    private readonly IConfiguration _config;
    private readonly SettingsService _settings;

    public InteractionViewModel(IConfiguration config, Interaction interaction, SettingsService settings)
    {
        _interaction = interaction;
        _config = config;
        _settings = settings; 
        OpenArticle = new Command(async () =>
        {
            Uri baseAddress = new Uri($"https://{_settings.LanguageKey}.{_config.GetSection("Wikipedia")["ContentUrl"]}");
            Uri uri = new Uri(baseAddress, Uri.EscapeDataString(interaction.ArticleId));
            var navigationParameter = new Dictionary<string, object>
            {
                {"address", uri }
            };
            await Shell.Current.GoToAsync($"{nameof(WikipediaWebView)}", navigationParameter);

        });
    }

    public string ArticleId { get => _interaction.ArticleId; }
    public string Date { get => _interaction.DateTime.ToShortDateString(); }
    public string Time { get => _interaction.DateTime.ToShortTimeString(); }
    public string PrecedorArticleText { get => $"Source: {_interaction.PredecessorArticleId}"; }

    public ICommand OpenArticle{ get; private set; }

}
