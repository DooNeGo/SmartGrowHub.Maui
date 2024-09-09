using SmartGrowHub.Domain.Requests;
using SmartGrowHub.Domain.Responses;
using SmartGrowHub.Maui.Services.Abstractions;

namespace SmartGrowHub.Maui.Services.Mock;

public sealed class MockAuthService : IAuthService
{
    public TryOptionAsync<LogInResponse> LogInAsync(LogInRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Try<bool> Logout()
    {
        throw new NotImplementedException();
    }

    public Either<Exception, RegisterResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}