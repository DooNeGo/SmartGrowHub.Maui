using Mediator;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Common.Password;

namespace SmartGrowHub.Maui.Application.Commands;

public sealed record LogInCommand(
    UserName UserName,
    PlainTextPassword Password,
    bool Remember)
    : ICommand<Unit>;
