using CommunityToolkit.Mvvm.Messaging;

namespace SmartGrowHub.Maui.Services.Extensions;

public static class MessengerExtensions
{
    public static IO<Unit> SendIO<T>(this IMessenger messenger, T message) where T : class =>
        lift(() => messenger.Send(message)).Map(_ => unit);
}