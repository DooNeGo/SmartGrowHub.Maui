using SmartGrowHub.Maui.Base;

namespace SmartGrowHub.Maui.Behaviors;

public sealed class PageLifecycleBehavior : Behavior<Page>
{
    private IPageLifecycleAware? _bindingContext;
    
    protected override void OnAttachedTo(Page bindable)
    {
        base.OnAttachedTo(bindable);
        
        if (bindable.BindingContext is not IPageLifecycleAware bindingContext) return;
        
        _bindingContext = bindingContext;
        
        bindingContext.Initialize();
        
        bindable.Appearing += OnAppearing;
        bindable.Disappearing += OnDisappearing;
    }

    protected override void OnDetachingFrom(Page bindable)
    {
        base.OnDetachingFrom(bindable);
        
        bindable.Appearing -= OnAppearing;
        bindable.Disappearing -= OnDisappearing;
    }

    private void OnAppearing(object? sender, EventArgs e) => _bindingContext?.OnAppearing();

    private void OnDisappearing(object? sender, EventArgs e) => _bindingContext?.OnDisappearing();
}