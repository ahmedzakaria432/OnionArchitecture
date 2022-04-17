using Application.Identity;
using Application.Identity.Dtos;
using Core.Shared.Exceptions;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public IdentityService(UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
        }
        public async Task<UserDto> GetUser(string userId)
        {
            ApplicationUser user = await GetUserAndCheckIfNull(userId);
            return ToUserDto(user);

        }

        private static UserDto ToUserDto(ApplicationUser user)
        {
            return new UserDto
            {
                Email = user.Email,
                FirstName = user.FirstName,
                Id = user.Id,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                UserName = user.UserName
            };
        }

        private async Task<ApplicationUser> GetUserAndCheckIfNull(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) throw new NotFoundException();
            return user;
        }

        public async Task<string> GetUserNameAsync(string userId)
        {
            var user = await GetUserAndCheckIfNull(userId);
            return await _userManager.GetUserNameAsync(user);
        }

        public async Task<bool> IsInRoleAsync(string userId, string role)
        {
            var user = await GetUserAndCheckIfNull(userId);
            return await _userManager.IsInRoleAsync(user, role);
        }

        public async Task<UserDto> Register(RegisterUserDto user)
        {
            var userEntity = RegisterDtoToApplicationUser(user);
            var result = await _userManager.CreateAsync(userEntity, user.Password);
            HandleErrors(result);
            return ToUserDto(userEntity);

        }

        private static void HandleErrors(IdentityResult result)
        {
            if (!result.Succeeded)
            {
                var errorsAsText = new StringBuilder();
                result.Errors.ToList().ForEach(error =>
                {
                    errorsAsText.Append(error.ToString());
                    errorsAsText.Append("\n");
                });
                throw new Exception(errorsAsText.ToString());
            }
        }

        private static ApplicationUser RegisterDtoToApplicationUser(RegisterUserDto user)
        {
            return new ApplicationUser
            {
                Created = DateTime.Now,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber
            };
        }
    }
}
