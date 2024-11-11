using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Common.Password;
using SmartGrowHub.Domain.Model;
using SmartGrowHub.Maui.Application.Interfaces;

namespace SmartGrowHub.Maui.Infrastructure.Services.Mocks;

internal sealed class MockUserService : IUserService
{
    public Eff<User> GetUser(Id<User> id, CancellationToken cancellationToken)
    {
        return Pure(new User(id,
            (UserName)"HelloDavid",
            Password.Empty,
            (EmailAddress)"matvey@gmail.com",
            (NonEmptyString)"DooNeGo"));
    }
}