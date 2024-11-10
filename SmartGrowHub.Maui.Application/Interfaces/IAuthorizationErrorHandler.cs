namespace SmartGrowHub.Maui.Application.Interfaces;

public interface IAuthorizationErrorHandler
{
    Eff<Unit> Handle(CancellationToken cancellationToken);
}
