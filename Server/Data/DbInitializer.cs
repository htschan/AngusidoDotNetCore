using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Server.Domain.UserConfiguration;
using Server.Helpers;
using Server.Controllers;
using Server.ViewModels.Validations;

namespace Server.Data
{
    public class DbInitializer : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly AppUserManager _appUserManager;
        private readonly IMapper _mapper;
        private readonly ITimeService _timeService;
        private readonly Random _random;

        public DbInitializer(ApplicationDbContext context, AppUserManager appUserManager, IMapper mapper, ITimeService timeService)
        {
            _context = context;
            _appUserManager = appUserManager;
            _mapper = mapper;
            _timeService = timeService;
            _random = new Random((int)DateTime.Now.Ticks);
        }

        public async void Initialize()
        {
            _context.Database.EnsureCreated();

            // Look for any users.
            if (_context.AppUsers.Any())
            {
                return;   // DB has been seeded
            }

            var userDict = new Dictionary<int, AppUser>();

            await _appUserManager.CreateRole(Constants.Strings.JwtClaims.ApiAccess);
            await _appUserManager.CreateRole(Constants.Strings.JwtClaims.ApiAccessPower);
            await _appUserManager.CreateRole(Constants.Strings.JwtClaims.ApiAccessAdmin);

            // create some users
            var users = new[]
            {
                new RegisterDto { Firstname = "Hans", Name = "Tschan", Email = "hts@koch-it.ch", Username = "hts" },
            };
            for (var i = 0; i < users.Length; i++)
            {
                var userIdentity = _mapper.Map<AppUser>(users[i]);
                await _appUserManager.CreateUser(userIdentity, "axil311", new List<string> { Constants.Strings.JwtClaims.ApiAccess });
                userDict.Add(i, userIdentity);
            }

            var powerUser = new RegisterDto { Firstname = "Power", Name = "Timepuncher", Email = "pwer@timepuncher.ch" };
            var powerUserIdentity = _mapper.Map<AppUser>(powerUser);
            await _appUserManager.CreateUser(powerUserIdentity, "axil311", new List<string> { Constants.Strings.JwtClaims.ApiAccess, Constants.Strings.JwtClaims.ApiAccessPower });

            var adminUser = new RegisterDto { Firstname = "Admin", Name = "Timepuncher", Email = "admin@timepuncher.ch" };
            var adminUserIdentity = _mapper.Map<AppUser>(adminUser);
            await _appUserManager.CreateUser(adminUserIdentity, "axil311", new List<string> { Constants.Strings.JwtClaims.ApiAccess, Constants.Strings.JwtClaims.ApiAccessAdmin });
            
            _context.SaveChanges();
        }


        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~DbInitializer() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

    }

}