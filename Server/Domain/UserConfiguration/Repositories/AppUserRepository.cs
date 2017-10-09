using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Server.Data;
using Server.Helpers;

namespace Server.Domain.UserConfiguration.Repositories
{
    public class AppUserRepository : IAppUserRepository
    {
        private readonly TpContext _appDbContext;
        public AppUserRepository(TpContext context)
        {
            _appDbContext = context;
        }

        public void Delete(AppUser entity)
        {
            throw new NotImplementedException();
        }

        public AppUser FindByName(string userName)
        {
            try
            {
                var value = _appDbContext.AppUsers.FirstOrDefault(b => b.UserName == userName);
                if (value == null)
                    throw new RepositoryException(StatusCodes.Status404NotFound, $"User {userName} not found");
                return value;
            }
            catch (RepositoryException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new RepositoryException(StatusCodes.Status400BadRequest, $"FindByName {userName} threw an exception: {exception.Message}", exception);
            }
        }

        public AppUser FindById(string userId)
        {
            try
            {
                var value = _appDbContext.AppUsers.FirstOrDefault(b => b.Id == userId);
                if (value == null)
                    throw new RepositoryException(StatusCodes.Status404NotFound, $"User Id {userId} not found");
                return value;
            }
            catch (RepositoryException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new RepositoryException(StatusCodes.Status400BadRequest, $"FindByName {userId} threw an exception: {exception.Message}", exception);
            }
        }

        public System.Collections.Generic.IList<AppUser> GetAll()
        {
            try
            {
                var value = _appDbContext.AppUsers;
                if (value == null)
                    throw new RepositoryException(StatusCodes.Status404NotFound, $"Users not found");
                return value.ToList();
            }
            catch (RepositoryException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new RepositoryException(StatusCodes.Status400BadRequest, $"GetAll threw an exception: {exception.Message}", exception);
            }
        }

        public void Insert(AppUser entity)
        {
            throw new NotImplementedException();
        }

        public void Update(AppUser entity)
        {
            throw new NotImplementedException();
        }
    }
}