using Server.Data;
using Server.Domain.UserConfiguration.Repositories;
using Server.Helpers;

namespace Server.Domain
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context, ITimeService timeService)
        {
            _context = context;
            AppUsers = new AppUserRepository(_context);
            AppProfiles = new AppProfileRepository(_context);
            RefreshTokens = new RefreshTokenRepository(_context);
        }
        public void Dispose()
        {
            _context.Dispose();
        }

        public ApplicationDbContext ApplicationDataContext => _context;
        public IAppUserRepository AppUsers { get; }
        public IAppProfileRepository AppProfiles { get; }
        public IRefreshTokenRepository RefreshTokens { get; }

        public int Complete()
        {
            return _context.SaveChanges();
        }
    }
}