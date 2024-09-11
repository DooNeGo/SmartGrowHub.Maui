using AsyncAwaitBestPractices;

namespace SmartGrowHub.Maui.Services.Extensions;

public static class AsyncAwaitBestPracticeExtensions
{
    public static Unit SafeFireAndForget(this Task task)
    {
        SafeFireAndForgetExtensions.SafeFireAndForget(task);
        return unit;
    }
}
