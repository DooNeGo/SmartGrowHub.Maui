using SmartGrowHub.Shared.Tokens;

namespace SmartGrowHub.Maui.Services.Extensions;

public static class SecureStorageExtensions
{
    private const string RefreshTokenKey = "refresh_token";
    private const string AccessTokenKey = "access_token";

    extension(ISecureStorage secureStorage)
    {
        public async Task<AuthTokensDto?> GetAuthTokensAsync()
        {
            string? accessToken = await secureStorage.GetAccessTokenAsync().ConfigureAwait(false);
            string? refreshToken = await secureStorage.GetRefreshTokenAsync().ConfigureAwait(false);
        
            if (accessToken is null || refreshToken is null) return null;
        
            return new AuthTokensDto(accessToken, refreshToken);
        }

        public async Task SetAuthTokensAsync(AuthTokensDto authTokens)
        {
            await secureStorage.SetAccessTokenAsync(authTokens.AccessToken).ConfigureAwait(false);
            await secureStorage.SetRefreshTokenAsync(authTokens.RefreshToken).ConfigureAwait(false);
        }

        public Task<string?> GetAccessTokenAsync() => 
            secureStorage.GetAsync(AccessTokenKey);

        public Task SetAccessTokenAsync(string accessToken) => 
            secureStorage.SetAsync(AccessTokenKey, accessToken);

        public Task<string?> GetRefreshTokenAsync() => 
            secureStorage.GetAsync(RefreshTokenKey);

        public Task SetRefreshTokenAsync(string refreshToken) => 
            secureStorage.SetAsync(RefreshTokenKey, refreshToken);

        public Task RemoveAllValuesAsync()
        {
            secureStorage.RemoveAll();
            return Task.CompletedTask;
        }
    }
}