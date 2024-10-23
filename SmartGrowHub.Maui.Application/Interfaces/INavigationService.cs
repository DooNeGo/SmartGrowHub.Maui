namespace SmartGrowHub.Maui.Application.Interfaces;

public interface INavigationService
{
    IO<Unit> GoBackAsync(CancellationToken cancellationToken = default);
    IO<Unit> GoToAsync(string path, CancellationToken cancellationToken = default);
    IO<Unit> SetLogInAsRootAsync(bool animate = true, CancellationToken cancellationToken = default);
    IO<Unit> SetMainPageAsRootAsync(bool animate = true, CancellationToken cancellationToken = default);
    IO<Unit> SetLogInAsRoot(bool animate = true, CancellationToken cancellationToken = default);
    IO<Unit> SetMainPageAsRoot(bool animate = true, CancellationToken cancellationToken = default);
}
