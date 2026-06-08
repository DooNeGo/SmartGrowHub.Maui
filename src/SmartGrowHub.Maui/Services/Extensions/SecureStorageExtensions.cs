using SmartGrowHub.Shared.Tokens;

namespace SmartGrowHub.Maui.Services.Extensions;

public static class SecureStorageExtensions
{
    private const string RefreshTokenKey = "refresh_token";
    private const string AccessTokenKey = "access_token";

    extension(ISecureStorage secureStorage)
    {
        public OptionT<IO, AuthTokensDto> GetAuthTokens() =>
            from accessToken in secureStorage.GetAccessToken()
            from refreshToken in secureStorage.GetRefreshToken()
            select new AuthTokensDto(accessToken, refreshToken);

        public IO<Unit> SetAuthTokens(AuthTokensDto authTokens) =>
            from _1 in secureStorage.SetAccessToken(authTokens.AccessToken)
            from _2 in secureStorage.SetRefreshToken(authTokens.RefreshToken)
            select _2;

        public OptionT<IO, string> GetAccessToken() => secureStorage.GetValue(AccessTokenKey);

        public IO<Unit> SetAccessToken(string accessToken) => secureStorage.SetValue(AccessTokenKey, accessToken);

        public OptionT<IO, string> GetRefreshToken() => secureStorage.GetValue(RefreshTokenKey);

        public IO<Unit> SetRefreshToken(string refreshToken) => secureStorage.SetValue(RefreshTokenKey, refreshToken);

        public OptionT<IO, string> GetValue(string key) =>
            IO.liftAsync(() => secureStorage.GetAsync(key).Map(Prelude.Optional));

        public IO<bool> RemoveValue(string key) => IO.lift(() => secureStorage.Remove(key));

        public IO<Unit> RemoveAllValues() => IO.lift(secureStorage.RemoveAll);

        public IO<Unit> SetValue(string key, string value) =>
            IO.liftAsync(() => secureStorage.SetAsync(key, value).ToUnit());
    }
}