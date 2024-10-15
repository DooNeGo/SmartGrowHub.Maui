﻿using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;

namespace SmartGrowHub.Maui.Application.Interfaces;

public interface IUserSessionProvider
{
    event Func<Unit>? SessionSet;
    event Func<Unit>? SessionRemoved;

    Eff<UserSession> GetUserSession(CancellationToken cancellationToken);
    Eff<Id<UserSession>> GetUserSessionId(CancellationToken cancellationToken);
    Eff<AccessToken> GetAccessTokenIfNotExpired(CancellationToken cancellationToken);
    Eff<RefreshToken> GetRefreshToken(CancellationToken cancellationToken);
    IO<Unit> SetSession(UserSession session);
    Eff<Unit> SaveAndSetSession(UserSession session, CancellationToken cancellationToken);
    Eff<Unit> RemoveSession();
    Eff<Unit> UpdateTokens(AuthTokens authTokens, CancellationToken cancellationToken);
}