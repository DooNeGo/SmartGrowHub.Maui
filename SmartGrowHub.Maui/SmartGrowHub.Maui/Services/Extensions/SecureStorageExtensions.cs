using SmartGrowHub.Shared.Tokens;

namespace SmartGrowHub.Maui.Services.Extensions;

public static class SecureStorageExtensions
{
    private const string RefreshTokenKey = "refresh_token";
    private const string AccessTokenKey = "access_token";

    public static OptionT<IO, AuthTokensDto> GetAuthTokens(this ISecureStorage secureStorage) =>
        from accessToken in secureStorage.GetAccessToken()
        from refreshToken in secureStorage.GetRefreshToken()
        select new AuthTokensDto(accessToken, refreshToken);

    public static IO<Unit> SetAuthTokens(this ISecureStorage secureStorage, AuthTokensDto authTokens) =>
        from _1 in secureStorage.SetAccessToken(authTokens.AccessToken)
        from _2 in secureStorage.SetRefreshToken(authTokens.RefreshToken)
        select _2;
    
    public static OptionT<IO, string> GetAccessToken(this ISecureStorage secureStorage) =>
        secureStorage.GetValue(AccessTokenKey);
    
    public static IO<Unit> SetAccessToken(this ISecureStorage secureStorage, string accessToken) =>
        secureStorage.SetValue(AccessTokenKey, accessToken);
    
    public static OptionT<IO, string> GetRefreshToken(this ISecureStorage secureStorage) =>
        secureStorage.GetValue(RefreshTokenKey);
    
    public static IO<Unit> SetRefreshToken(this ISecureStorage secureStorage, string refreshToken) =>
        secureStorage.SetValue(RefreshTokenKey, refreshToken);

    public static OptionT<IO, string> GetValue(this ISecureStorage secureStorage, string key) =>
        IO.liftAsync(() => secureStorage.GetAsync(key).Map(Optional));

    public static IO<bool> RemoveValue(this ISecureStorage secureStorage, string key) =>
        IO.lift(() => secureStorage.Remove(key));
    
    public static IO<Unit> RemoveAllValues(this ISecureStorage secureStorage) =>
        IO.lift(secureStorage.RemoveAll);

    public static IO<Unit> SetValue(this ISecureStorage secureStorage, string key, string value) =>
        IO.liftAsync(() => secureStorage.SetAsync(key, value).ToUnit());
}