using System.ComponentModel;
using MPowerKit.Navigation.Utilities;
using MPowerKit.Popups;
using SmartGrowHub.Maui.Services.Infrastructure;

namespace SmartGrowHub.Maui.Services.Extensions;

public sealed class ViewNotFoundException : Exception;

public static class PopupNavigationExtensions
{
    private static readonly Dictionary<Type, Type> ViewModelToViewMappings = [];

    public static IO<Unit> HidePopupAsync<TPopupViewModel>(this IPopupNavigation popupNavigation, bool animate = true)
        where TPopupViewModel : INotifyPropertyChanged
    {
        Type viewType = ViewModelToViewMappings.GetValueOrDefault(typeof(TPopupViewModel))
            ?? throw new ViewNotFoundException();

        PopupPage popupPage = popupNavigation.PopupStack.FirstOrDefault(page => page.GetType() == viewType)
            ?? throw new ViewNotFoundException();

        return popupNavigation.HidePopupAsync(popupPage, animate);
    }

    public static IO<Unit> ShowPopupAsync<TPopupViewModel>(this IPopupNavigation popupNavigation, bool animate = true)
        where TPopupViewModel : INotifyPropertyChanged
    {
        IServiceProvider serviceProvider = Application.Current?.Handler?.GetServiceProvider()
            ?? throw new InvalidOperationException("Could not locate IServiceProvider");
        
        Type viewType = ViewModelToViewMappings.GetValueOrDefault(typeof(TPopupViewModel))
            ?? throw new ViewNotFoundException();
        
        PopupPage popup = serviceProvider.GetRequiredService(viewType) as PopupPage
            ?? throw new ViewNotFoundException();

        return popupNavigation.ShowPopupAsync(popup, animate);
    }
    
    public static IServiceCollection AddTransientPopup<TPopupView, TPopupViewModel>(this IServiceCollection services)
        where TPopupView : PopupPage
        where TPopupViewModel : class, INotifyPropertyChanged
    {
        ViewModelToViewMappings.Add(typeof(TPopupViewModel), typeof(TPopupView));
        return services.RegisterForNavigation<TPopupView, TPopupViewModel>();
    }
}