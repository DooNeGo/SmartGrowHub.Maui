using System.ComponentModel;
using SmartGrowHub.Maui.Behaviors;

namespace SmartGrowHub.Maui.Base;

public abstract class BasePage<TViewModel> : ContentPage where TViewModel : INotifyPropertyChanged
{
    protected BasePage(TViewModel pageModel)
    {
        base.BindingContext = pageModel;
        Behaviors.Add(new PageLifecycleBehavior());
    }
    
    public new TViewModel BindingContext => (TViewModel)base.BindingContext;
}