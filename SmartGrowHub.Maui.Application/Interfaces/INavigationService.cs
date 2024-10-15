namespace SmartGrowHub.Maui.Application.Interfaces;

public interface INavigationService
{
    IO<Unit> GoBackAsync(CancellationToken cancellationToken = default);
    IO<Unit> GoToAsync(string path, CancellationToken cancellationToken = default);
    IO<Unit> GoToLogIn();
    IO<Unit> GoToMain();
}
