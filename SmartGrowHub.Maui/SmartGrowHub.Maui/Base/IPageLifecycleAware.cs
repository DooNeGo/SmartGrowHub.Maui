namespace SmartGrowHub.Maui.Base;

public interface IPageLifecycleAware
{
    void Initialize() { }
    void OnAppearing() { }
    void OnDisappearing() { }
}