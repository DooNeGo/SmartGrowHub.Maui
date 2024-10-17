using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;
using SmartGrowHub.Maui.Application.Interfaces;
using SmartGrowHub.Maui.Infrastructure.Services.Extensions;
using SmartGrowHub.Shared.Errors;
using SmartGrowHub.Shared.Errors.Extensions;
using SmartGrowHub.Shared.Users.Dto;
using SmartGrowHub.Shared.Users.Extensions;

namespace SmartGrowHub.Maui.Infrastructure.Services;

internal sealed class UserService(HttpClient httpClient) : IUserService
{
    public Eff<User> GetUser(Id<User> id, CancellationToken cancellationToken) =>
        httpClient.GetAsync<UserDto, ErrorDto>($"user?id={id}", cancellationToken)
            .Bind(either => either.Match(
                Left: error => error.ToDomain(),
                Right: user => user.TryToDomain().ToEff()));
}