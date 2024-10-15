namespace SmartGrowHub.Maui.Services.Extensions;

public static class DispatcherExtensions
{
    public static IO<T> InvokeOnUiThreadIfNeeded<T>(this IDispatcher dispatcher, Func<Task<T>> action) =>
        liftIO(() => dispatcher.IsDispatchRequired
            ? dispatcher.DispatchAsync(action)
            : action());

    public static Unit InvokeOnUiThreadIfNeeded(this IDispatcher dispatcher, Func<Unit> action) =>
        dispatcher.IsDispatchRequired
            ? dispatcher.Dispatch(action)
            : action();
    
    private static Unit Dispatch<T>(this IDispatcher dispatcher, Func<T> action)
    {
        dispatcher.Dispatch(() => action());
        return unit;
    }
}