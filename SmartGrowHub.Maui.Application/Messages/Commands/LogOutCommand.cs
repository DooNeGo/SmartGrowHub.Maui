using Mediator;

namespace SmartGrowHub.Maui.Application.Messages.Commands;

public sealed class LogOutCommand : ICommand<Unit>
{
    public static readonly LogOutCommand Default = new();
}
