using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Identity.Dtos; 

namespace Application.Identity
{
    public interface IIdentityService
    {
        Task<UserDto> Register(RegisterUserDto user);
        Task<string> GetUserNameAsync(string userId);
        Task<bool> IsInRoleAsync(string userId, string role);
        Task<UserDto> GetUser(string userId);
            
    }
}
