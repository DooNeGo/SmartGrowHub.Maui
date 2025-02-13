using System.ComponentModel;
using Mopups.Pages;
using SmartGrowHub.Maui.Behaviors;

namespace SmartGrowHub.Maui.Base;

public abstract class BasePopupPage<TViewModel> : PopupPage where TViewModel : INotifyPropertyChanged
{
    protected BasePopupPage(TViewModel viewModel)
    {
        base.BindingContext = viewModel;
        Behaviors.Add(new PageLifecycleBehavior());
    }
    
    public new TViewModel BindingContext => (TViewModel)base.BindingContext;

    protected override void OnAppearingAnimationEnd()
    {
        base.OnAppearingAnimationEnd();
        if (BindingContext is IPopupPageLifecycleAware bindingContext)
        {
            bindingContext.OnAppearingAnimationEnd();
        }
    }
    
    protected override void OnDisappearingAnimationEnd()
    {
        base.OnDisappearingAnimationEnd();
        if (BindingContext is IPopupPageLifecycleAware bindingContext)
        {
            bindingContext.OnDisappearingAnimationEnd();
        }
    }
}