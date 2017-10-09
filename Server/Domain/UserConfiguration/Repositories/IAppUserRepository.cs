using Server.Repositories;

namespace Server.Domain.UserConfiguration.Repositories
{
    public interface IAppUserRepository : IRepository<AppUser>
    {
         AppUser FindByName(string name);
         AppUser FindById(string userId);
    }
}