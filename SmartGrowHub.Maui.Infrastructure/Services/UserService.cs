using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;
using SmartGrowHub.Maui.Application.Interfaces;
using SmartGrowHub.Shared.HttpErrors;
using SmartGrowHub.Shared.Users.Dto;
using SmartGrowHub.Shared.Users.Extensions;

namespace SmartGrowHub.Maui.Infrastructure.Services;

internal sealed class UserService(IHttpService httpService) : IUserService
{
    public Eff<User> GetUser(Id<User> id, CancellationToken cancellationToken) =>
        httpService.GetAsync<UserDto, HttpError>($"user?id={id}", cancellationToken)
            .Bind(either => either.Match(
                Left: httpError => httpError.ToError(),
                Right: user => user.TryToDomain().ToEff()));
}