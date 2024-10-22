using Mediator;
using SmartGrowHub.Domain.Common;

namespace SmartGrowHub.Maui.Application.Commands;

public sealed record RefreshTokensCommand : ICommand<AuthTokens>
{
    public static readonly RefreshTokensCommand Default = new();
}