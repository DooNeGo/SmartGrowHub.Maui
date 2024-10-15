using SmartGrowHub.Application.RefreshTokens;
using SmartGrowHub.Application.Services;
using SmartGrowHub.Maui.Application.Interfaces;

namespace SmartGrowHub.Maui.Infrastructure.Services.DelegatingHandlers;

internal sealed class TokenDelegatingHandler(
    IUserSessionProvider sessionProvider,
    IAuthService authService,
    IDialogService dialogService,
    INavigationService navigationService)
    : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) =>
        sessionProvider
            .GetAccessTokenIfNotExpired(cancellationToken)
            .IfFailEff(_ =>
                from refreshToken in sessionProvider.GetRefreshToken(cancellationToken)
                from response in authService.RefreshTokens(new RefreshTokensRequest(refreshToken), cancellationToken)
                from _1 in sessionProvider.UpdateTokens(response.AuthTokens, cancellationToken)
                select response.AuthTokens.AccessToken)
            .RunAsync()
            .Bind(fin => fin.Match(
                Succ: _ => Task.FromResult(unit),
                Fail: error =>
                    (dialogService.PopAsync() >>
                    dialogService.DisplayAlert("Unathorized", error.Message, "Ok") >>
                    navigationService.GoToLogIn())
                    .RunAsync().AsTask()))
            .Bind(_ => base.SendAsync(request, cancellationToken));
}
