using Serilog;
using SmartGrowHub.Maui.Services.Api;
using SmartGrowHub.Maui.Services.Extensions;
using SmartGrowHub.Shared.Tokens;

namespace SmartGrowHub.Maui.Services.App;

public interface IAuthService
{
    Task RequestOtpToEmail(string emailAddress, CancellationToken cancellationToken);
    Task VerifyOtp(string oneTimePassword, CancellationToken cancellationToken);
    Task RefreshTokens(CancellationToken cancellationToken);
}

internal sealed class AuthService : IAuthService
{
    private readonly IAuthApi _authApi;
    private readonly ISecureStorage _secureStorage;
    private readonly ILogger _logger;

    public AuthService(
        IAuthApi authApi,
        ISecureStorage secureStorage,
        ILogger logger)
    {
        _authApi = authApi;
        _secureStorage = secureStorage;
        _logger = logger;
    }

    public async Task RequestOtpToEmail(string emailAddress, CancellationToken cancellationToken)
    {
        try
        {
            await _authApi.RequestOtpToEmailAsync(emailAddress, cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to send otp to email");
            throw;
        }
    }

    public async Task VerifyOtp(string oneTimePassword, CancellationToken cancellationToken)
    {
        try
        {
            AuthTokensDto response = await _authApi.VerifyOtpAsync(oneTimePassword, cancellationToken).ConfigureAwait(false);
            await _secureStorage.SetAuthTokensAsync(response).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to check otp");
            throw;
        }
    }

    public async Task RefreshTokens(CancellationToken cancellationToken)
    {
        try
        {
            string? refreshToken = await _secureStorage.GetRefreshTokenAsync().ConfigureAwait(false);
            if (string.IsNullOrEmpty(refreshToken)) return;
            AuthTokensDto authTokens = await _authApi.RefreshTokensAsync(refreshToken, cancellationToken).ConfigureAwait(false);
            await _secureStorage.SetAuthTokensAsync(authTokens).ConfigureAwait(false);
        }
        catch (Exception ex) when (ex is not InvalidOperationException)
        {
            _logger.Error(ex, "Failed to refresh tokens");
            throw;
        }
    }
}