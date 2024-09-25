using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;
using SmartGrowHub.Maui.Services.Api;
using SmartGrowHub.Shared.Users.Dto;
using SmartGrowHub.Shared.Users.Extensions;

namespace SmartGrowHub.Maui.Services;

public interface IUserService
{
    Eff<Option<User>> GetUserAsync(Id<User> id, CancellationToken cancellationToken);
}

public sealed class UserService(IHttpService httpService) : IUserService
{
    public Eff<Option<User>> GetUserAsync(Id<User> id, CancellationToken cancellationToken) =>
        httpService.GetAsync<UserDto>($"user?id={id}", cancellationToken)
            .Bind(option => option.Match(
                Some: user => user.TryToDomain().ToEff().Map(Optional),
                None: () => Pure(Option<User>.None)));
}