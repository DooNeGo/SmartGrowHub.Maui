﻿using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Extensions;
using SmartGrowHub.Maui.Application.Interfaces;
using System.Net.Http.Headers;

namespace SmartGrowHub.Maui.Infrastructure.Services.DelegatingHandlers;

internal sealed class TokenDelegatingHandler(
    IUserSessionService sessionService,
    IRefreshTokensService refreshTokensService)
    : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) =>
        sessionService
            .GetAccessTokenIfNotExpired(cancellationToken)
            .IfFailEff(_ => GetAndRefreshTokens(cancellationToken))
            .Bind(accessToken => SetAuthorization(request, accessToken))
            .Bind(_ => base.SendAsync(request, cancellationToken).ToEff())
            .RunUnsafeAsync().AsTask();

    private static IO<Unit> SetAuthorization(HttpRequestMessage request, AccessToken accessToken) =>
        lift(() =>
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        });

    private Eff<AccessToken> GetAndRefreshTokens(CancellationToken cancellationToken) =>
        from tokens in refreshTokensService.RefreshTokens(cancellationToken)
        select tokens.AccessToken;
}
