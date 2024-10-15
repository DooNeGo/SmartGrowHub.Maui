using SmartGrowHub.Application.LogIn;
using SmartGrowHub.Application.LogOut;
using SmartGrowHub.Application.RefreshTokens;
using SmartGrowHub.Application.Register;
using SmartGrowHub.Application.Services;
using SmartGrowHub.Maui.Application.Interfaces;

namespace SmartGrowHub.Maui.Infrastructure.Services.Mock;

public sealed class MockAuthService(IUserSessionProvider sessionProvider) : IAuthService
{
    //public Eff<LogInResponse> LogInAsync(LogInRequest request, bool remember, CancellationToken cancellationToken)
    //{
    //    return new LogInResponseDto(new UserSessionDto(
    //        Ulid.NewUlid(),
    //        Ulid.NewUlid(),
    //        "asdasdasdasdsadasdadasdas",
    //        "asdasdsadadadasdasdada"))
    //        .TryToDomain().ToEff()
    //        .Bind(response => IO.yield(1000).Map(_ => response))
    //        .Bind(response => sessionProvider
    //            .SetSession(response.UserSession)
    //            .Map(_ => response));
    //}

    //public Eff<LogOutResponse> LogOutAsync(CancellationToken cancellationToken)
    //{
    //    throw new NotImplementedException();
    //}

    //public Eff<RefreshTokensResponse> RefreshTokensAsync(RefreshTokensRequest request, CancellationToken cancellationToken)
    //{
    //    throw new NotImplementedException();
    //}

    //public Eff<RegisterResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken)
    //{
    //    throw new NotImplementedException();
    //}
    public Eff<LogInResponse> LogIn(LogInRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Eff<LogOutResponse> LogOut(LogOutRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Eff<RefreshTokensResponse> RefreshTokens(RefreshTokensRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Eff<RegisterResponse> Register(RegisterRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}