using Application.Identity;
using Application.Identity.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IIdentityService _identityService;

        public AccountController(IIdentityService identityService)
        {
            this._identityService = identityService;
        }

        [HttpPost("Login")]
        public async Task<LoginResponse> Login(LoginDto login)
        {
            return  await _identityService.LoginAsync(login);
        }
        [HttpPost("Register")]
        public Task<AuthenticationResponse> Register(RegisterUserDto user)
        {
            return _identityService.RegisterAsync(user);
        }
    }
}
