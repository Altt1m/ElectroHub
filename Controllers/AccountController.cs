using ElectroHub.DTOs.Account;
using ElectroHub.Interfaces;
using ElectroHub.Models;
using ElectroHub.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ElectroHub.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController(
        UserManager<AppUser> userManager,
        ITokenService tokenService,
        SignInManager<AppUser> signInManager,
        IConfiguration configuration)
        : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ValidationProblemDetails(ModelState));

            var user = await userManager.FindByNameAsync(loginDto.Username);
            if (user == null)
            {
                return Unauthorized(new ValidationProblemDetails(new Dictionary<string, string[]>
            {
                { "Login", new[] { "Invalid username or password." } }
            }));
            }

            var result = await signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded)
            {
                return Unauthorized(new ValidationProblemDetails(new Dictionary<string, string[]>
            {
                { "Login", new[] { "Invalid username or password." } }
            }));
            }

            var roles = await userManager.GetRolesAsync(user);
            return Ok(new
            {
                user.UserName,
                user.Email,
                AccessToken = tokenService.CreateAccessToken(user, roles.ToList())
            });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ValidationProblemDetails(ModelState));

            var appUser = new AppUser
            {
                UserName = registerDto.Username,
                Email = registerDto.Email
            };

            var createdUser = await userManager.CreateAsync(appUser, registerDto.Password);
            if (!createdUser.Succeeded)
            {
                var errors = createdUser.Errors.ToDictionary(e => e.Code, e => new[] { e.Description });
                return BadRequest(new ValidationProblemDetails(errors));
            }

            var roleResult = await userManager.AddToRoleAsync(appUser, "User");
            if (!roleResult.Succeeded)
            {
                var errors = roleResult.Errors.ToDictionary(e => e.Code, e => new[] { e.Description });
                return BadRequest(new ValidationProblemDetails(errors));
            }

            var roles = await userManager.GetRolesAsync(appUser);
            return Ok(new NewUserDto
            {
                UserName = appUser.UserName,
                Email = appUser.Email,
                AccessToken = tokenService.CreateAccessToken(appUser, roles.ToList())
            });
        }

    }
}
