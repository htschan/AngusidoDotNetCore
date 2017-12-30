using Server.Repositories;

namespace Server.Domain.UserConfiguration.Repositories
{
    public interface IAppUserRepository : IRepository<AppUser, string>
    {
        AppUser FindByName(string name);
        AppUser FindById(string userId);
    }
}