using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Server.Domain.UserConfiguration.Repositories;

namespace Server.Domain.UserConfiguration
{
    public class AppUserManager
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IMapper _mapper;

        public AppUserManager(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper, IRefreshTokenRepository refreshTokenRepository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task CreateUser(AppUser appUser, string password, List<string> roles)
        {
            appUser.EmailConfirmed = true;
            await _userManager.CreateAsync(appUser, password);
            foreach (var role in roles)
            {
                await _userManager.AddToRoleAsync(appUser, role);
            }
        }

        public async Task CreateRole(string roleName)
        {
            var role = new IdentityRole
            {
                Id = Guid.NewGuid().ToString(),
                Name = roleName
            };
            await _roleManager.CreateAsync(role);
        }

        public IList<AppUser> GetUsers()
        {
            foreach (var user in _userManager.Users)
            {
                var userRoles = _userManager.GetRolesAsync(user).Result;
                user.RoleNames = new List<string>();
                foreach (var role in userRoles)
                {
                    user.RoleNames.Add(role);
                }
            }
            return _userManager.Users.ToList();
        }

        public IList<RefreshToken> GetSessions()
        {
            var tokens = _refreshTokenRepository.GetAll();
            foreach (var token in tokens)
            {
                var user = _userManager.FindByIdAsync(token.ClientId).Result;
                if (user != null)
                {
                    token.ClientName = user.Email;
                }
            }
            return tokens;
        }
    }
}