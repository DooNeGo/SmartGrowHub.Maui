namespace SmartGrowHub.Maui.Application.Interfaces;

public interface ILogOutService
{
    Eff<Unit> LogOut(CancellationToken cancellationToken);
}
