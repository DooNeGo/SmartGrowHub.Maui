using SmartGrowHub.Shared.Tokens;

namespace SmartGrowHub.Maui.Services.Api.Mocks;

internal sealed class MockAuthService : IAuthService
{
    public OptionT<Eff, AuthTokensDto> CheckOtp(int otp, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public OptionT<Eff, AuthTokensDto> RefreshTokens(string refreshToken, CancellationToken cancellationToken)
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