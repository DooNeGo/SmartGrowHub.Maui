using System.ComponentModel;
using Mopups.Interfaces;
using Mopups.Pages;

namespace SmartGrowHub.Maui.Services.Extensions;

public sealed class ViewNotFoundException : Exception;

public static class PopupNavigationExtensions
{
    private static readonly Dictionary<Type, Type> ViewModelToViewMappings = [];

    public static Task ShowPopupAsync<TPopupModel>(this IPopupNavigation popupNavigation, bool animate = true)
        where TPopupModel : INotifyPropertyChanged
    {
        IServiceProvider serviceProvider = Application.Current?.Handler.MauiContext?.Services
            ?? throw new InvalidOperationException("Could not locate IServiceProvider");

        Type viewType = ViewModelToViewMappings.GetValueOrDefault(typeof(TPopupModel))
            ?? throw new ViewNotFoundException();

        PopupPage popup = serviceProvider.GetRequiredService(viewType) as PopupPage
            ?? throw new ViewNotFoundException();

        return popupNavigation.PushAsync(popup, animate);
    }

    public static IServiceCollection AddTransientMopup<TPopupView, TPopupModel>(this IServiceCollection services)
        where TPopupView : PopupPage
        where TPopupModel : class, INotifyPropertyChanged
    {
        ViewModelToViewMappings.Add(typeof(TPopupModel), typeof(TPopupView));

        return services
            .AddTransient<TPopupView>()
            .AddTransient<TPopupModel>();
    }
}