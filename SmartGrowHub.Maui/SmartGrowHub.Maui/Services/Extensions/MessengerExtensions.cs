using CommunityToolkit.Mvvm.Messaging;

namespace SmartGrowHub.Maui.Services.Extensions;

public static class MessengerExtensions
{
    // ReSharper disable once InconsistentNaming
    public static IO<Unit> SendIO<T>(this IMessenger messenger, T message) where T : class =>
        IO.lift(() => messenger.Send(message)).Map(_ => unit);
}