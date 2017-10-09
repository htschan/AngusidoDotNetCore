using Server.Repositories;

namespace Server.Domain.UserConfiguration.Repositories
{
    public interface IRefreshTokenRepository : IRepository<RefreshToken>
    {
        bool AddToken(RefreshToken token);

        bool ExpireToken(RefreshToken token);

        RefreshToken GetToken(string refresh_token);

    }
}