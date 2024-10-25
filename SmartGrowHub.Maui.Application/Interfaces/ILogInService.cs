using SmartGrowHub.Application.LogIn;

namespace SmartGrowHub.Maui.Application.Interfaces;

public interface ILogInService
{
    Eff<Unit> LogIn(LogInRequest request, bool remember, CancellationToken cancellationToken);
}
