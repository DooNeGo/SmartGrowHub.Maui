using SmartGrowHub.Domain.Extensions;

namespace SmartGrowHub.Maui.Services.Extensions;

public static class DispatcherExtensions
{
    public static IO<T> InvokeOnUiThreadIfNeeded<T>(this IDispatcher dispatcher, Func<Task<T>> action) =>
        liftIO(() => dispatcher.IsDispatchRequired
            ? dispatcher.DispatchAsync(action)
            : action());

    public static IO<Unit> InvokeOnUiThreadIfNeeded(this IDispatcher dispatcher, Func<Unit> action) =>
        lift(() => dispatcher.IsDispatchRequired
            ? dispatcher.Dispatch(action)
            : action());

    public static IO<Unit> InvokeOnUiThreadIfNeeded(this IDispatcher dispatcher, Func<IO<Unit>> action) =>
        dispatcher.IsDispatchRequired
            ? dispatcher.DispatchAsync(() => action().RunAsync().AsTask()).ToIO()
            : action();

    private static Unit Dispatch<T>(this IDispatcher dispatcher, Func<T> action)
    {
        dispatcher.Dispatch(() => action());
        return unit;
    }
}