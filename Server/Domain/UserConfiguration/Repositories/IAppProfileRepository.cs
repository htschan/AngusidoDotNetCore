using Server.Repositories;

namespace Server.Domain.UserConfiguration.Repositories
{
    public interface IAppProfileRepository : IRepository<AppProfile, string>
    {
        AppProfile FindById(string id);
    }
}