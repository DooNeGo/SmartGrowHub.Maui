using SmartGrowHub.Shared.Tokens;

namespace SmartGrowHub.Maui.Services.Api.Mocks;

internal sealed class MockAuthService : IAuthService
{
    public IO<AuthTokensDto> VerifyOtp(string otp)
    {
        throw new NotImplementedException();
    }

    public IO<AuthTokensDto> RefreshTokens(string refreshToken)
    {
        throw new NotImplementedException();
    }

    public IO<Unit> RequestOtpToEmail(string emailAddress)
    {
        throw new NotImplementedException();
    }

    public IO<Unit> LogOut(string refreshToken)
    {
        throw new NotImplementedException();
    }
}