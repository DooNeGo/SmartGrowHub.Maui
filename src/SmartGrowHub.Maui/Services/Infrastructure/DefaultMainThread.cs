namespace SmartGrowHub.Maui.Services.Infrastructure;

public interface IMainThread
{
    bool IsMainThread { get; }

    Task InvokeOnMainThreadAsync(Action action);
    Task<T> InvokeOnMainThreadAsync<T>(Func<T> func);
    Task<T> InvokeOnMainThreadAsync<T>(Func<Task<T>> func);
    Task InvokeOnMainThreadAsync(Func<Task> func);
    Task BeginInvokeOnMainThread(Action action);
    Task<SynchronizationContext> GetMainThreadSynchronizationContext();
}

public sealed class DefaultMainThread : IMainThread
{
    public bool IsMainThread => MainThread.IsMainThread;

    public Task InvokeOnMainThreadAsync(Action action) => MainThread.InvokeOnMainThreadAsync(action);

    public Task<T> InvokeOnMainThreadAsync<T>(Func<T> func) => MainThread.InvokeOnMainThreadAsync(func);

    public Task<T> InvokeOnMainThreadAsync<T>(Func<Task<T>> func) => MainThread.InvokeOnMainThreadAsync(func);

    public Task InvokeOnMainThreadAsync(Func<Task> func) => MainThread.InvokeOnMainThreadAsync(func);

    public Task BeginInvokeOnMainThread(Action action)
    {
        MainThread.BeginInvokeOnMainThread(action);
        return Task.CompletedTask;
    }

    public Task<SynchronizationContext> GetMainThreadSynchronizationContext() =>
        MainThread.GetMainThreadSynchronizationContextAsync();
}