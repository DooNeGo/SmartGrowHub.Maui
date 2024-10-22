using Mediator;

namespace SmartGrowHub.Maui.Application.Notifications;

public sealed class NoAuthorizeNotification : INotification
{
    public static readonly NoAuthorizeNotification Default = new();
}
