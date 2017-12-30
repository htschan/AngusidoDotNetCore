using System;
using Server.Data;
using Server.Domain.UserConfiguration.Repositories;

namespace Server.Domain
{
    public interface IUnitOfWork : IDisposable
    {
        ApplicationDbContext ApplicationDataContext { get; }
        IAppUserRepository AppUsers { get; }
        IAppProfileRepository AppProfiles { get; }
        IRefreshTokenRepository RefreshTokens { get; }
        int Complete();
    }
}