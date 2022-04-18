using Application.Identity;
using Application.Identity.Dtos;
using Core.Shared.Exceptions;
using Infrastructure.Infrastructure.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Identity
{
    public class IdentityServiceWithJwt : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly Jwt _jwt;
     

        public IdentityServiceWithJwt(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
             IOptions<Jwt> jwt, SignInManager<ApplicationUser> signInManager)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._jwt = jwt.Value;
            
        }



        public async Task<UserDto> GetUserAsync(string userId)
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
                    Id = Guid.Parse( user.Id),
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





        public async Task<AuthenticationResponse> RegisterAsync(RegisterUserDto user)
        {
            if (await CheckIfEmailInUse(user.Email))
                return new AuthenticationResponse() { Message = "Email is Already in use" };

            var userEntity = RegisterDtoToApplicationUser(user);
            var result = await _userManager.CreateAsync(userEntity, user.Password);
            var response = new AuthenticationResponse();
            if (!result.Succeeded)
            {
                response.Message = HandleErrors(result);
                return response;
            }
             
            var token = await CreateToken(userEntity);
            PrepareResponseForSuccess(userEntity, response, token);
            return response ;

        }

                private static void PrepareResponseForSuccess(ApplicationUser userEntity, AuthenticationResponse response, JwtSecurityToken token)
                {
                    response.Message = "Registeration Succeeded";
                    response.Email = userEntity.Email;
                    response.IsAuthenticated = true;
                    response.AdditionalInfo = new { Token= new JwtSecurityTokenHandler().WriteToken(token) };
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
                       UserName=user.Email
                    };
                }

        private async Task<JwtSecurityToken> CreateToken(ApplicationUser user)
        {
            IEnumerable<Claim> claims = await GetClaims(user);

            var symetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symetricSecurityKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken jwtSecurityToket = GetSecurityToken(claims, signingCredentials);

            return jwtSecurityToket;



        }

                private JwtSecurityToken GetSecurityToken(IEnumerable<Claim> claims, SigningCredentials signingCredentials)
                {
                    return new JwtSecurityToken(
                        issuer: _jwt.Issuer,
                        audience: _jwt.Audience,
                        claims: claims,
                        expires: DateTime.Now.AddDays(_jwt.DurationInDays),
                        signingCredentials: signingCredentials);
                }

                private async Task<IEnumerable<Claim>> GetClaims(ApplicationUser user)
                {
                    var userClaims = await _userManager.GetClaimsAsync(user);
                    var roles = await _userManager.GetRolesAsync(user);
                    var roleClaims = new List<Claim>();

                    foreach (var role in roles)
                        roleClaims.Add(new Claim("roles", role));

                    var claims = new[] {
                       new Claim(JwtRegisteredClaimNames.Sub, user.UserName),

                       new Claim(JwtRegisteredClaimNames.Email, user.Email),
                       new Claim("uid", user.Email),
                    }
                    .Union(userClaims)
                    .Union(roleClaims);
                    return claims;
                }

        public Task<LoginResponse> LoginAsync(LoginDto login)
        {
            throw new NotImplementedException();
        }
    }
}
