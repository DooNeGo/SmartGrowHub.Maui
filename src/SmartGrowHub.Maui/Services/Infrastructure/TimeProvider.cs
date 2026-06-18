namespace SmartGrowHub.Maui.Services.Infrastructure;

public interface ITimeProvider
{
    DateTime UtcNow { get; }
    DateTime Now { get; }
}

public sealed class TimeProvider : ITimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
    public DateTime Now => DateTime.Now;
}