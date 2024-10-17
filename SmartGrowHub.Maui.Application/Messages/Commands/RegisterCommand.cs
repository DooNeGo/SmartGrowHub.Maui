using Mediator;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Common.Password;

namespace SmartGrowHub.Maui.Application.Messages.Commands;

public sealed record RegisterCommand(
    UserName UserName,
    PlainTextPassword Password,
    EmailAddress EmailAddress,
    NonEmptyString DisplayName)
    : ICommand<Unit>;
