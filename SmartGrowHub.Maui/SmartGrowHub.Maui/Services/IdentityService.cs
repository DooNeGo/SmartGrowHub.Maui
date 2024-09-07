using LanguageExt;
using SmartGrowHub.Maui.Users;

namespace SmartGrowHub.Maui.Services;

public interface IIdentityService
{
    Option<UserDto> CurrentUser { get; }
    Task TryLoadUserAsync(CancellationToken cancellationToken = default);
    Task SaveUserAsync(UserDto user, CancellationToken cancellationToken = default);
}

public sealed class IdentityService : IIdentityService
{
    public Option<UserDto> CurrentUser { get; private set; } = Option<UserDto>.None;

    public Task SaveUserAsync(UserDto user, CancellationToken cancellationToken = default)
    {
        CurrentUser = user;
        return Task.CompletedTask;
    }

    public Task TryLoadUserAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
