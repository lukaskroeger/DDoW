using System.Collections;
using System.Collections.Specialized;
using System.Windows.Input;

namespace MAUIApp.Controls;

public partial class CardStackView : ContentView
{
    public static readonly BindableProperty ItemSourceProperty =
        BindableProperty.Create(
            nameof(ItemSource), 
            typeof(IList),
            typeof(CardStackView),
            propertyChanged: OnItemSourcePropertyChanged);

    public static readonly BindableProperty ItemTemplateProperty =
        BindableProperty.Create(
            nameof(ItemTemplate),
            typeof(DataTemplate),
            typeof(CardStackView),
            propertyChanged: OnItemTemplatePropertyChanged);

    
    public static readonly BindableProperty LikeCommandProperty =
        BindableProperty.Create(
            nameof(LikeCommand),
            typeof(ICommand),
            typeof(CardStackView));

    
    public static readonly BindableProperty DislikeCommandProperty = 
        BindableProperty.Create(
            nameof(DislikeCommand),
            typeof(ICommand),
            typeof(CardStackView));
    
    public static readonly BindableProperty UpCommandProperty= 
        BindableProperty.Create(
            nameof(UpCommand),
            typeof(ICommand),
            typeof(CardStackView));
    public CardStackView()
    {
        InitializeComponent();
    }

    public IList ItemSource
    {
        get { return (IList)GetValue(ItemSourceProperty); }
        set { SetValue(ItemSourceProperty, value); }
    }

    public DataTemplate ItemTemplate
    {
        get { return (DataTemplate)GetValue(ItemTemplateProperty); }
        set { SetValue(ItemTemplateProperty, value); }
    }

    public ICommand LikeCommand 
    {
        get { return (ICommand)GetValue(LikeCommandProperty); }
        set { SetValue(LikeCommandProperty, value); }
    }

    public ICommand DislikeCommand
    {
        get { return (ICommand)GetValue(DislikeCommandProperty); }
        set
        {
            SetValue(DislikeCommandProperty, value);
        }
    }
    public ICommand UpCommand
    {
        get { return (ICommand)GetValue(UpCommandProperty); }
        set { SetValue(UpCommandProperty, value); }
    }
    private static void OnItemSourcePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        ((CardStackView)bindable).Setup();        
    }

    private static void OnItemTemplatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        ((CardStackView)bindable).Setup();
    }

    public void Setup()
    {
        if (ItemSource is null || ItemTemplate is null)
            return;
        AddCollectionChanged(ItemSource);
        for (int i = ItemSource.Count-1; i>=0; i--)
        {
            AddCard(ItemSource[i]);
        }
    }

    private PanGestureRecognizer GetGestureRecognizer()
    {
        PanGestureRecognizer gestureRecognizer = new PanGestureRecognizer();
        gestureRecognizer.PanUpdated += PanGestureRecognizer_PanUpdated;
        return gestureRecognizer;
    }

    private void AddCollectionChanged(IEnumerable list)
    {
        if (list is INotifyCollectionChanged collection)
        {
            collection.CollectionChanged -= ItemsSourceCollectionChanged;
            collection.CollectionChanged += ItemsSourceCollectionChanged;
        }
    }

    private void ItemsSourceCollectionChanged(object s, NotifyCollectionChangedEventArgs e)
    {
        if (e.NewItems is not null)
        {
            foreach (var newItem in e.NewItems)
            {
                AddCard(newItem);
            }
        }
    }

    private void AddCard(object newCard)
    {
        View cardView = ItemTemplate.CreateContent() as View;
        cardView.BindingContext = newCard;
        cardView.GestureRecognizers.Add(GetGestureRecognizer());
        cardView.ZIndex = int.MaxValue - CardStack.Children.Count;
        CardStack.Add(cardView);
    }

    private async void PanGestureRecognizer_PanUpdated(object sender, PanUpdatedEventArgs e)
    {
        View card = (View)CardStack.Children.First();
        switch (e.StatusType)
        {            
            case GestureStatus.Running:                
                card.TranslationX = e.TotalX;
                card.TranslationY = e.TotalY;
                break;
            case GestureStatus.Canceled:
            case GestureStatus.Completed:               
                if (Math.Abs(card.TranslationX) > 250 || Math.Abs(card.TranslationY) > 250)
                {
                    if (Math.Abs(card.TranslationX) > Math.Abs(card.TranslationY))
                    {
                        if(card.TranslationX < 0)
                        {
                            if(DislikeCommand is null)
                            {
                                await card.TranslateTo(0, 0, 250, Easing.SinIn);
                                return;
                            }
                            await card.TranslateTo(-this.Width * 2,0,250, Easing.SinIn);
                            DislikeCommand.Execute(card.BindingContext);
                        }
                        else if(card.TranslationX > 0)
                        {
                            if(LikeCommand is null)
                            {
                                await card.TranslateTo(0, 0, 250, Easing.SinIn);
                                return ;
                            }
                            await card.TranslateTo(this.Width * 2, 0, 250, Easing.SinIn);
                            LikeCommand.Execute(card.BindingContext);
                        }
                    }
                    else
                    {
                        if(card.TranslationY < 0)
                        {
                            if(UpCommand is null)
                            {
                                await card.TranslateTo(0, 0, 250, Easing.SinIn);
                                return;
                            }
                            await card.TranslateTo(0, this.Height * 2, 250, Easing.SinIn);
                            UpCommand.Execute(card.BindingContext);
                        }
                        else
                        {
                            await card.TranslateTo(0, 0, 250, Easing.SinIn);
                            return;
                        }
                    }
                    card.IsVisible = false;
                    CardStack.Children.Remove(card);
                }
                else
                {
                    await card.TranslateTo(0, 0, 250, Easing.SinIn);
                }
                break;
        }
    }
}