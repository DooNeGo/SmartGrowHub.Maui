using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;
using SmartGrowHub.Maui.Services.Api;
using SmartGrowHub.Shared.Users.Dto;

namespace SmartGrowHub.Maui.Services;

public interface IUserService
{
    TryOptionAsync<UserDto> GetUserAsync(Id<User> id, CancellationToken cancellationToken);
}

public sealed class UserService(IHttpService httpService) : IUserService
{
    public TryOptionAsync<UserDto> GetUserAsync(Id<User> id, CancellationToken cancellationToken) =>
        httpService.GetAsync<UserDto>($"user?id={id}", cancellationToken);
}