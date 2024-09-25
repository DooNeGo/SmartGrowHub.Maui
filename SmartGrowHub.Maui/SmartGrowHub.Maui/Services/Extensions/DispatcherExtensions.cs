namespace SmartGrowHub.Maui.Services.Extensions;

public static class DispatcherExtensions
{
    public static Task<T> InvokeOnUiThreadIfNeeded<T>(this IDispatcher dispatcher, Func<Task<T>> action) =>
        dispatcher.IsDispatchRequired
            ? dispatcher.DispatchAsync(action)
            : action();
    
    public static Unit InvokeOnUiThreadIfNeeded(this IDispatcher dispatcher, Func<Unit> action) =>
        dispatcher.IsDispatchRequired
            ? dispatcher.Dispatch(action)
            : action();

    private static Unit Dispatch(this IDispatcher dispatcher, Func<Unit> action)
    {
        dispatcher.Dispatch(() => action());
        return unit;
    }
    
    private static Unit Dispatch<T>(this IDispatcher dispatcher, Func<T> action)
    {
        dispatcher.Dispatch(() => action());
        return unit;
    }
}