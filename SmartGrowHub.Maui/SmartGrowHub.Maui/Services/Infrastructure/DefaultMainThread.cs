namespace SmartGrowHub.Maui.Services.Infrastructure;

public interface IMainThread
{
    bool IsMainThread { get; }

    IO<Unit> InvokeOnMainThread(Action action);
    IO<T> InvokeOnMainThread<T>(Func<T> func);
    IO<T> InvokeOnMainThread<T>(Func<Task<T>> func);
    IO<Unit> InvokeOnMainThread(Func<Task> func);
    IO<Unit> BeginInvokeOnMainThread(Action action);
    IO<SynchronizationContext> GetMainThreadSynchronizationContext();
}

public sealed class DefaultMainThread : IMainThread
{
    public bool IsMainThread => MainThread.IsMainThread;

    public IO<Unit> InvokeOnMainThread(Action action) =>
        IO.liftAsync(() => MainThread.InvokeOnMainThreadAsync(action).ToUnit());

    public IO<T> InvokeOnMainThread<T>(Func<T> func) =>
        IO.liftAsync(() => MainThread.InvokeOnMainThreadAsync(func));

    public IO<T> InvokeOnMainThread<T>(Func<Task<T>> func) =>
        IO.liftAsync(() => MainThread.InvokeOnMainThreadAsync(func));

    public IO<Unit> InvokeOnMainThread(Func<Task> func) =>
        IO.liftAsync(() => MainThread.InvokeOnMainThreadAsync(func).ToUnit());

    public IO<Unit> BeginInvokeOnMainThread(Action action) =>
        IO.lift(() => MainThread.BeginInvokeOnMainThread(action));

    public IO<SynchronizationContext> GetMainThreadSynchronizationContext() =>
        IO.liftAsync(MainThread.GetMainThreadSynchronizationContextAsync);
}