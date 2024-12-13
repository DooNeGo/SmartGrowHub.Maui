using SmartGrowHub.Shared.Tokens;

namespace SmartGrowHub.Maui.Services.Api.Mocks;

internal sealed class MockAuthService : IAuthService
{
    public Eff<AuthTokensDto> CheckOtp(int otp, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Eff<AuthTokensDto> RefreshTokens(string refreshToken, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Eff<Unit> SendOtpToEmail(string emailAddress, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Eff<Unit> LogOut(string refreshToken, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}