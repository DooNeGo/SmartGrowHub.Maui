using SmartGrowHub.Application.Register;

namespace SmartGrowHub.Maui.Application.Interfaces;

public interface IRegisterService
{
    Eff<Unit> Register(RegisterRequest request, CancellationToken cancellationToken);
}
