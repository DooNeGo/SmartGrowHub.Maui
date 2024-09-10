using SmartGrowHub.Domain.Requests;
using SmartGrowHub.Domain.Responses;

namespace SmartGrowHub.Maui.Services.Abstractions;

public interface IAuthService
{
    TryOptionAsync<LogInResponse> LogInAsync(LogInRequest request, bool remember, CancellationToken cancellationToken);
    TryOptionAsync<LogInResponse> LogInIfRememberAsync(CancellationToken cancellationToken);
    Either<Exception, RegisterResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken);
    Try<bool> Logout();
}
