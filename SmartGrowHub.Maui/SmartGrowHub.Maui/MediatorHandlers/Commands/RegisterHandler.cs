using Mediator;
using SmartGrowHub.Application.Register;
using SmartGrowHub.Application.Services;
using SmartGrowHub.Maui.Application.Commands;

namespace SmartGrowHub.Maui.MediatorHandlers.Commands;

internal sealed class RegisterHandler(IAuthService authService)
    : ICommandHandler<RegisterCommand, Unit>
{
    public ValueTask<Unit> Handle(RegisterCommand command, CancellationToken cancellationToken) =>
        Register(command, cancellationToken).RunUnsafeAsync();

    private Eff<Unit> Register(RegisterCommand command, CancellationToken cancellationToken) =>
        from request in Pure(new RegisterRequest(
            command.UserName, command.Password,
            command.EmailAddress, command.DisplayName))
        from response in authService.Register(request, cancellationToken)
        select unit;
}
