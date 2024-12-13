namespace SmartGrowHub.Maui.Services.Infrastructure;

public interface IMainThreadService
{
    bool IsMainThread { get; }

    IO<Unit> InvokeOnMainThread(Action action);
    IO<T> InvokeOnMainThread<T>(Func<T> func);
    IO<T> InvokeOnMainThread<T>(Func<Task<T>> func);
    IO<Unit> InvokeOnMainThread(Func<Task> func);
    IO<Unit> BeginInvokeOnMainThread(Action action);
    IO<SynchronizationContext> GetMainThreadSynchronizationContext();
}

public sealed class MainThreadService : IMainThreadService
{
    public bool IsMainThread => MainThread.IsMainThread;

    public IO<Unit> InvokeOnMainThread(Action action) =>
        liftIO(() => MainThread.InvokeOnMainThreadAsync(action));

    public IO<T> InvokeOnMainThread<T>(Func<T> func) =>
        liftIO(() => MainThread.InvokeOnMainThreadAsync(func));

    public IO<T> InvokeOnMainThread<T>(Func<Task<T>> func) =>
        liftIO(() => MainThread.InvokeOnMainThreadAsync(func));

    public IO<Unit> InvokeOnMainThread(Func<Task> func) =>
        liftIO(() => MainThread.InvokeOnMainThreadAsync(func));

    public IO<Unit> BeginInvokeOnMainThread(Action action) =>
        lift(() => MainThread.BeginInvokeOnMainThread(action));

    public IO<SynchronizationContext> GetMainThreadSynchronizationContext() =>
        liftIO(MainThread.GetMainThreadSynchronizationContextAsync);
}