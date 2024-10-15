using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;

namespace SmartGrowHub.Maui.Application.Interfaces;

public interface IUserService
{
    Eff<User> GetUser(Id<User> id, CancellationToken cancellationToken);
}
