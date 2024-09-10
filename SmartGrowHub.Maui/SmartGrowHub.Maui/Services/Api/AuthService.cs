using SmartGrowHub.Domain.Requests;
using SmartGrowHub.Domain.Responses;
using SmartGrowHub.Maui.Services.Abstractions;
using SmartGrowHub.Shared.Auth.Dto.LogIn;
using SmartGrowHub.Shared.Auth.Extensions;

namespace SmartGrowHub.Maui.Services.Api;

public sealed class AuthService(
    IHttpService httpService,
    ITokenProvider tokenProvider,
    IUserCredentialProvider credentialProvider)
    : IAuthService
{
    public event Func<Unit>? LoggedOut;

    public TryOptionAsync<LogInResponse> LogInAsync(LogInRequest request, bool remember, CancellationToken cancellationToken)
        => httpService
            .PostAsync<LogInRequestDto, LogInResponseDto>("auth/login", request.ToDto(), cancellationToken)
            .Bind(response => SaveTokenAsync(response, cancellationToken))
            .Bind(response => remember
                ? SaveCredentialAsync(request, response, cancellationToken)
                : TryOptionAsync(response))
            .Map(response => response.ToDomain());

    private TryOptionAsync<LogInResponseDto> SaveTokenAsync(LogInResponseDto response,
        CancellationToken cancellationToken) =>
        tokenProvider
            .SetAsync(response.JwtToken, cancellationToken)
            .Map(_ => response)
            .ToTryOption();

    private TryOptionAsync<LogInResponseDto> SaveCredentialAsync(LogInRequest request, LogInResponseDto response,
        CancellationToken cancellationToken) =>
        credentialProvider
            .SetAsync(request.UserName, request.Password, cancellationToken)
            .Map(_ => response)
            .ToTryOption();

    public TryOptionAsync<LogInResponse> LogInIfRememberAsync(CancellationToken cancellationToken) =>
        credentialProvider
            .GetAsync(cancellationToken)
            .Map(tuple => new LogInRequest(tuple.Item1, tuple.Item2))
            .Bind(request => LogInAsync(request, remember: false, cancellationToken));

    public Try<bool> Logout() =>
        from token in tokenProvider.Remove()
        from credential in credentialProvider.Remove()
        let _ = LoggedOut?.Invoke()
        select credential;

    public Either<Exception, RegisterResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
