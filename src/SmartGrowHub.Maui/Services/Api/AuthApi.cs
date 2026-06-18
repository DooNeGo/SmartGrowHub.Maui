using SmartGrowHub.Maui.Services.Infrastructure;
using SmartGrowHub.Shared.Auth;
using SmartGrowHub.Shared.Results;
using SmartGrowHub.Shared.Tokens;

namespace SmartGrowHub.Maui.Services.Api;

public interface IAuthApi
{
    Task<AuthTokensDto> VerifyOtpAsync(string otp, CancellationToken cancellationToken);
    Task<AuthTokensDto> RefreshTokensAsync(string refreshToken, CancellationToken cancellationToken);
    Task RequestOtpToEmailAsync(string emailAddress, CancellationToken cancellationToken);
}

public sealed class AuthApi : IAuthApi
{
    private readonly IHttpService _httpService;

    public AuthApi(IHttpService httpService)
    {
        _httpService = httpService;
        _httpService.ClientName = nameof(IAuthApi);
    }

    public async Task RequestOtpToEmailAsync(string emailAddress, CancellationToken cancellationToken)
    {
        Result? result = await _httpService
            .SendAsJsonAsync<Result, RequestOtpToEmailRequest>(
                HttpMethod.Post, new Uri("/api/auth/otp/email", UriKind.Relative),
                new RequestOtpToEmailRequest(emailAddress), cancellationToken)
            .ConfigureAwait(false);

        if (result is null)
            throw new InvalidOperationException("Response was null");
        if (!result.IsSuccess)
            throw new InvalidOperationException(result.ErrorMessage ?? "Request failed");
    }

    public async Task<AuthTokensDto> VerifyOtpAsync(string otp, CancellationToken cancellationToken)
    {
        Result<AuthTokensDto>? result = await _httpService
            .SendAsJsonAsync<Result<AuthTokensDto>, VerifyOtpRequest>(
                HttpMethod.Post, new Uri("/api/auth/otp/verify", UriKind.Relative),
                new VerifyOtpRequest(otp), cancellationToken)
            .ConfigureAwait(false);

        if (result is null)
            throw new InvalidOperationException("Response was null");
        if (!result.IsSuccess || result.Data is null)
            throw new InvalidOperationException(result.ErrorMessage ?? "OTP verification failed");

        return result.Data;
    }

    public async Task<AuthTokensDto> RefreshTokensAsync(string refreshToken, CancellationToken cancellationToken)
    {
        Result<AuthTokensDto>? result = await _httpService
            .SendAsJsonAsync<Result<AuthTokensDto>, RefreshTokensRequest>(
                HttpMethod.Post, new Uri("/api/auth/tokens/refresh", UriKind.Relative),
                new RefreshTokensRequest(refreshToken), cancellationToken)
            .ConfigureAwait(false);

        if (result is null)
            throw new InvalidOperationException("Response was null");
        if (!result.IsSuccess || result.Data is null)
            throw new InvalidOperationException(result.ErrorMessage ?? "Token refresh failed");

        return result.Data;
    }
}
