using Mediator;

namespace SmartGrowHub.Maui.Application.Messages.Notifications;

public sealed class NoAuthorizeNotification : INotification
{
    public static readonly NoAuthorizeNotification Default = new();
}
