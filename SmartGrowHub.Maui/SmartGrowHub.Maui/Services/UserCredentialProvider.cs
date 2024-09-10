using SmartGrowHub.Domain.Common;

namespace SmartGrowHub.Maui.Services;

public interface IUserCredentialProvider
{
    TryAsync<Unit> SetAsync(UserName userName, Password password, CancellationToken cancellationToken);
    TryOptionAsync<(UserName, Password)> GetAsync(CancellationToken cancellationToken);
    Try<bool> Remove();
}

public sealed class UserCredentialProvider(ISecureStorageService secureStorage) : IUserCredentialProvider
{
    private const string UserNameKey = nameof(UserName);
    private const string PasswordKey = nameof(Password);

    public TryAsync<Unit> SetAsync(UserName userName, Password password, CancellationToken cancellationToken) =>
        from _1 in secureStorage.SetAsync(UserNameKey, userName, cancellationToken)
        from _2 in secureStorage.SetAsync(PasswordKey, password, cancellationToken)
        select unit;

    public TryOptionAsync<(UserName, Password)> GetAsync(CancellationToken cancellationToken) =>
        from userName in secureStorage.GetAsync(UserNameKey, cancellationToken)
        from password in secureStorage.GetAsync(PasswordKey, cancellationToken)
        select ((UserName)userName, (Password)password);

    public Try<bool> Remove() =>
        from userName in secureStorage.Remove(UserNameKey)
        from password in secureStorage.Remove(PasswordKey)
        select password;
}