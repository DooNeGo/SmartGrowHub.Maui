namespace SmartGrowHub.Maui.Services.Infrastructure;

public interface ITimeProvider
{
    IO<DateTime> UtcNow { get; }
    IO<DateTime> Now { get; }
}

public sealed class TimeProvider : ITimeProvider
{
    public IO<DateTime> UtcNow { get; } = IO.lift(() => DateTime.UtcNow);
    
    public IO<DateTime> Now { get; } = IO.lift(() => DateTime.Now);
}