using AsyncAwaitBestPractices;

namespace SmartGrowHub.Maui.Services.Extensions;

public static class AsyncAwaitBestPracticeExtensions
{
    public static Unit SafeFireAndForget(this Task task)
    {
        SafeFireAndForgetExtensions.SafeFireAndForget(task);
        return Unit.Default;
    }

    public static Unit SafeFireAndForget<T>(this ValueTask<T> task)
    {
        SafeFireAndForgetExtensions.SafeFireAndForget(task);
        return Unit.Default;
    }
}
