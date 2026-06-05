namespace SmartGrowHub.Maui.Behaviors;

public static class ViewExtensions
{
    public static T GetOrAddBehavior<T>(this VisualElement element) where T : Behavior, new()
    {
        T? behavior = element.Behaviors.OfType<T>().FirstOrDefault();
        
        if (behavior is null)
        {
            behavior = new T();
            element.Behaviors.Add(behavior);
        }
        
        return behavior;
    }
}