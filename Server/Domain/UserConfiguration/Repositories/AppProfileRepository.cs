using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Helpers;
using Server.Repositories;

namespace Server.Domain.UserConfiguration.Repositories
{
    public class AppProfileRepository : Repository<AppProfile, string>, IAppProfileRepository
    {
        public AppProfileRepository(ApplicationDbContext context) : base(context)
        {
        }
        public ApplicationDbContext ApplicationDataContext => Context as ApplicationDbContext;

        public AppProfile FindById(string id)
        {
            try
            {
                var value = ApplicationDataContext.AppProfiles.Include(p => p.Identity).FirstOrDefault(b => b.Identity.Id == id);
                if (value == null)
                    throw new RepositoryException(StatusCodes.Status404NotFound, $"User {id} not found");
                return value;
            }
            catch (RepositoryException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new RepositoryException(StatusCodes.Status400BadRequest, $"FindById {id} threw an exception: {exception.Message}", exception);
            }
        }
    }}