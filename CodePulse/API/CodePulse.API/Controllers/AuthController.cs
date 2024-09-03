using CodePulse.API.Data.Repositories;
using CodePulse.API.Models.DTO.Login;
using CodePulse.API.Models.DTO.Register;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(UserManager<IdentityUser> userManager, ITokenRepo tokenRepo) : ControllerBase
{
    // POST: {Base api}/api/auth/register
    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto requestDto)
    {
        // Create IdentityUser Object
        var user = new IdentityUser
        {
            UserName = requestDto.Email?.Trim(),
            Email = requestDto.Email?.Trim()
        };
        // Create user
        if (requestDto.Password == null)
        {
            ModelState.AddModelError("Password", "Password is required.");
            return ValidationProblem(ModelState);
        }

        var identityResult = await userManager.CreateAsync(user, requestDto.Password);
        if (identityResult.Succeeded)
        {
            // Add role reader to user
            identityResult = await userManager.AddToRoleAsync(user, "Reader");
            if (identityResult.Succeeded) return Ok();
        }
        else
        {
            if (!identityResult.Errors.Any()) return ValidationProblem(ModelState);
            foreach (var error in identityResult.Errors) ModelState.AddModelError(string.Empty, error.Description);
        }

        return ValidationProblem(ModelState);
    }

    // HTTP POST: {base api}/api/auth/login
    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto requestDto)
    {
        if (requestDto.Email == null || requestDto.Password == null)
        {
            ModelState.AddModelError("", "Email or password is required.");
            return ValidationProblem(ModelState);
        }

        var identityUser = await userManager.FindByEmailAsync(requestDto.Email);
        if (identityUser is not null)
        {
            // Check password
            var checkPasswordAsync = await userManager.CheckPasswordAsync(identityUser, requestDto.Password);
            if (checkPasswordAsync)
            {
                var roles = await userManager.GetRolesAsync(identityUser);
                var jwtToken = tokenRepo.GenerateToken(identityUser, roles.ToList());
                //Create token and response
                var response = new LoginResponseDto
                {
                    Email = requestDto.Email,
                    Roles = roles.ToList(),
                    Token = jwtToken
                };
                return Ok(response);
            }

            ModelState.AddModelError("", "Invalid username or password.");
            return ValidationProblem(ModelState);
        }

        ModelState.AddModelError("", "Invalid username or password.");
        return ValidationProblem(ModelState);
    }
}