using CarMSApp.Helpers;
using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace CarMSApp.Controllers
{
    public class RegisterModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Username { get; set; }
    }

    public class AuthResponseModel
    {
        public bool Succeeded { get; set; }
        public string Token { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly JwtService _jwtService;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public AuthController(
            JwtService jwtService,
            UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager)
        {
            _jwtService = jwtService;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var response = new AuthResponseModel();

            var userExists = await _userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
            {
                response.Errors.Add("User with this email already exists");
                return BadRequest(response);
            }

            userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
            {
                response.Errors.Add("Username is already taken");
                return BadRequest(response);
            }

            var user = new AppUser
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                response.Errors.AddRange(result.Errors.Select(x => x.Description));
                return BadRequest(response);
            }

            if (!await _roleManager.RoleExistsAsync("User"))
            {
                await _roleManager.CreateAsync(new AppRole { Name = "User" });
            }

            await _userManager.AddToRoleAsync(user, "User");
            var roles = await _userManager.GetRolesAsync(user);

            var token = _jwtService.GenerateToken(user.Id.ToString(), user.UserName, roles.ToList());
            response.Succeeded = true;
            response.Token = token;
            return Ok(response);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var response = new AuthResponseModel();

            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                response.Errors.Add("Invalid username or password");
                return Unauthorized(response);
            }

            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtService.GenerateToken(user.Id.ToString(), user.UserName, roles.ToList());
            response.Succeeded = true;
            response.Token = token;
            return Ok(response);
        }

        
    }
}
