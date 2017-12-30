using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Server.Data;
using Server.Helpers;
using Server.Repositories;

namespace Server.Domain.UserConfiguration.Repositories
{
    public class RefreshTokenRepository : Repository<RefreshToken, string>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(ApplicationDbContext context) : base(context)
        {
        }

        public ApplicationDbContext ApplicationDataContext => Context as ApplicationDbContext;

        public void AddToken(RefreshToken token)
        {
            ApplicationDataContext.RefreshTokens.Add(token);
        }

        public void ExpireToken(RefreshToken token)
        {
            ApplicationDataContext.RefreshTokens.Update(token);
        }

        public RefreshToken GetToken(string refreshToken)
        {
            return ApplicationDataContext.RefreshTokens.FirstOrDefault(x => x.Token == refreshToken);
        }
    }
}