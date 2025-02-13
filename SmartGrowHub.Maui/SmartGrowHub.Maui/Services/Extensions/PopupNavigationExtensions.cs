using System.ComponentModel;
using Mopups.Interfaces;
using Mopups.Pages;

namespace SmartGrowHub.Maui.Services.Extensions;

public sealed class ViewNotFoundException : Exception;

public static class PopupNavigationExtensions
{
    private static readonly Dictionary<Type, Type> ViewModelToViewMappings = [];

    public static Task HidePopupAsync<TPopupViewModel>(this IPopupNavigation popupNavigation, bool animate = true)
        where TPopupViewModel : INotifyPropertyChanged
    {
        Type viewType = ViewModelToViewMappings.GetValueOrDefault(typeof(TPopupViewModel))
            ?? throw new ViewNotFoundException();

        PopupPage popupPage = popupNavigation.PopupStack.FirstOrDefault(page => page.GetType() == viewType)
            ?? throw new ViewNotFoundException();

        return popupNavigation.RemovePageAsync(popupPage, animate);
    }

    public static Task ShowPopupAsync<TPopupViewModel>(this IPopupNavigation popupNavigation, bool animate = true)
        where TPopupViewModel : INotifyPropertyChanged
    {
        IServiceProvider serviceProvider = Application.Current?.Handler?.MauiContext?.Services 
            ?? throw new InvalidOperationException("Could not locate IServiceProvider");
        
        Type viewType = ViewModelToViewMappings.GetValueOrDefault(typeof(TPopupViewModel))
            ?? throw new ViewNotFoundException();
        
        PopupPage popup = serviceProvider.GetRequiredService(viewType) as PopupPage
            ?? throw new ViewNotFoundException();

        return popupNavigation.PushAsync(popup, animate);
    }
    
    public static IServiceCollection AddTransientMopup<TPopupView, TPopupViewModel>(this IServiceCollection services)
        where TPopupView : PopupPage
        where TPopupViewModel : class, INotifyPropertyChanged
    {
        ViewModelToViewMappings.Add(typeof(TPopupViewModel), typeof(TPopupView));
        
        return services
            .AddTransient<TPopupView>()
            .AddTransient<TPopupViewModel>();
    }
}