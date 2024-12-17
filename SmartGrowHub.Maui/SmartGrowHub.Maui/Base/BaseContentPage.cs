using System.ComponentModel;

namespace SmartGrowHub.Maui.Base;

public abstract class BaseContentPage : ContentPage
{
    private readonly INotifyPropertyChanged _pageModel;
    
    public BaseContentPage(INotifyPropertyChanged pageModel)
    {
        BindingContext = _pageModel = pageModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await Task.Delay(500);
        
        if (_pageModel is IOnAppearedAware supportAppearing)
        {
            supportAppearing.OnAppeared();
        }
    }

    protected override async void OnDisappearing()
    {
        base.OnDisappearing();

        await Task.Delay(500);
        
        if (_pageModel is IOnDisappearedAware supportDisappearing)
        {
            supportDisappearing.OnDisappeared();
        }
    }
}