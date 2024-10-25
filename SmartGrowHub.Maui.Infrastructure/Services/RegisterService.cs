using SmartGrowHub.Application.Register;
using SmartGrowHub.Application.Services;
using SmartGrowHub.Maui.Application.Interfaces;

namespace SmartGrowHub.Maui.Infrastructure.Services;

internal sealed class RegisterService(IAuthService authService) : IRegisterService
{
    public Eff<Unit> Register(RegisterRequest request, CancellationToken cancellationToken) =>
        from response in authService.Register(request, cancellationToken)
        select unit;
}
