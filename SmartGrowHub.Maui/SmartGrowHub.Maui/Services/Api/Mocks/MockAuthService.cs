using SmartGrowHub.Shared.Tokens;

namespace SmartGrowHub.Maui.Services.Api.Mocks;

internal sealed class MockAuthService : IAuthService
{
    public OptionT<IO, AuthTokensDto> CheckOtp(string otp, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public OptionT<IO, AuthTokensDto> RefreshTokens(string refreshToken, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public IO<Unit> SendOtpToEmail(string emailAddress, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public IO<Unit> LogOut(string refreshToken, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}