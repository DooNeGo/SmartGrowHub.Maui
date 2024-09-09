using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Requests;
using SmartGrowHub.Domain.Responses;
using SmartGrowHub.Maui.Services.Abstractions;
using SmartGrowHub.Shared.Auth.Dto.LogIn;
using SmartGrowHub.Shared.Auth.Extensions;

namespace SmartGrowHub.Maui.Services.Api;

public sealed class AuthService(
    IHttpService httpService,
    ITokenProvider tokenProvider)
    : IAuthService
{
    private const string UserNameKey = nameof(UserName);
    private const string PasswordKey = nameof(Password);

    public TryOptionAsync<LogInResponse> LogInAsync(LogInRequest request, CancellationToken cancellationToken)
        => httpService
            .PostAsync<LogInRequestDto, LogInResponseDto>("auth/login", request.ToDto(), cancellationToken)
            .Bind(response => tokenProvider
                .SetAsync(response.JwtToken, cancellationToken)
                .Map(_ => response.ToDomain())
                .ToTryOption());

    public Try<bool> Logout() => tokenProvider.Remove();

    public Either<Exception, RegisterResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
