using Application.Identity;
using Application.Identity.Dtos;
using Core.Shared.Exceptions;
using Microsoft.AspNetCore.Identity;
using System.Text;

namespace Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
      
        private readonly SignInManager<ApplicationUser> _signInManager;

        public IdentityService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            this._userManager = userManager;
            
            this._signInManager = signInManager;
        }



        public async Task<UserDto> GetUserAsync(string userId)
        {
            ApplicationUser user = await GetUserByIdAndCheckIfNull(userId);
            return ToUserDto(user);

        }

              private static UserDto ToUserDto(ApplicationUser user)
              {
                 return new UserDto
                 {
                    Email = user.Email,
                    FirstName = user.FirstName,
                    Id = Guid.Parse( user.Id),
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    UserName = user.UserName
                 };
              }

              private async Task<ApplicationUser> GetUserByIdAndCheckIfNull(string userId)
              {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null) throw new NotFoundException();
                return user;
              }

        public async Task<string> GetUserNameAsync(string userId)
        {
            var user = await GetUserByIdAndCheckIfNull(userId);
            return await _userManager.GetUserNameAsync(user);
        }

        public async Task<bool> IsInRoleAsync(string userId, string role)
        {
            var user = await GetUserByIdAndCheckIfNull(userId);
            return await _userManager.IsInRoleAsync(user, role);
        }





        public async Task<AuthenticationResponse> RegisterAsync(RegisterUserDto user)
        {
            if (await CheckIfEmailInUse(user.Email))
                return new AuthenticationResponse() { Message = "Email is Already in use" };

            var userEntity = RegisterDtoToApplicationUser(user);
            var result = await _userManager.CreateAsync(userEntity, user.Password);
           


            if (!result.Succeeded)  throw new BadRequestException(HandleErrors(result));
            

            
           var signInResult= await _signInManager.PasswordSignInAsync(userEntity,user.Password, true,false);
           
            
            return PrepareResponseForSuccess(userEntity, signInResult.Succeeded);

        }

        private AuthenticationResponse PrepareResponseForSuccess(ApplicationUser userEntity, bool isAuthenticated)
        {
            return new AuthenticationResponse
            {
                Message = "Registeration Succeeded",
                Email = userEntity.Email,
                UserName = userEntity.UserName,
                IsAuthenticated = isAuthenticated
            };
                }

                private  async Task<bool> CheckIfEmailInUse(string email) 
                {
                    return (await _userManager.FindByEmailAsync(email)) is not null;
                }
                private  string HandleErrors(IdentityResult result)
                {
            
                    var errorsAsText = new StringBuilder();
                    result.Errors.ToList().ForEach(error =>
                    {
                        errorsAsText.Append(error.Description);
                        errorsAsText.Append("\n");
                    });
                    return errorsAsText.ToString();
            
                }
            
                private static ApplicationUser RegisterDtoToApplicationUser(RegisterUserDto user)
                {
                    return new ApplicationUser
                    {
                       Created = DateTime.Now,
                       Email = user.Email,
                       FirstName = user.FirstName,
                       LastName = user.LastName,
                       PhoneNumber = user.PhoneNumber,
                       UserName=user.UserName
                    };
                }

        public async Task<LoginResponse> LoginAsync(LoginDto login)
        {
            var user = await _userManager.FindByNameAsync(login.UserName);

            if (user is null) throw new NotFoundException(nameof(user), login.UserName);

            var result = await _signInManager.PasswordSignInAsync(user, login.Password, true, false);

            if (!result.Succeeded) throw new BadRequestException("User name or password might be wrong");

            return GetLoginResponse(login, result);
        }

                private static LoginResponse GetLoginResponse(LoginDto login, SignInResult result)
                {
                    return new LoginResponse()
                    {
                        IsAuthenticated = result.Succeeded,
                        UserName = login.UserName,
                        Message = "Logged in successfully" 
                    };
                }
    }
}
