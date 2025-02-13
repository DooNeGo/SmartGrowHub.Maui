namespace SmartGrowHub.Maui.Base;

public interface IPopupPageLifecycleAware : IPageLifecycleAware
{
    void OnAppearingAnimationEnd() { }
    void OnDisappearingAnimationEnd() { }
}