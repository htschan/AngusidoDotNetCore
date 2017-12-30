using Server.Repositories;

namespace Server.Domain.UserConfiguration.Repositories
{
    public interface IRefreshTokenRepository : IRepository<RefreshToken, string>
    {
        void AddToken(RefreshToken token);
        void ExpireToken(RefreshToken token);
        RefreshToken GetToken(string refresh_token);
    }
}