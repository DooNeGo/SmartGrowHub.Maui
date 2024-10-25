namespace SmartGrowHub.Maui.Application.Interfaces;

public interface INoAuthorizeService
{
    Eff<Unit> Handle(CancellationToken cancellationToken);
}
