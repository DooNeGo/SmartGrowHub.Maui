namespace SmartGrowHub.Maui.Users;

public sealed record UserDto(
    Ulid Id,
    string Username,
    string Email,
    string DisplayName);