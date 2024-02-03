using LookupAPI.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LookupAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration
            )
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {

                var authClaims = new List<Claim>
                {
                    new Claim("firstName", user.FirstName),
                    new Claim("lastName", user.LastName),
                    new Claim("id",user.Id),
                    new Claim("email",user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                var token = GetToken(authClaims);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Email);

            if (userExists != null)
                return StatusCode(StatusCodes.Status400BadRequest);

            ApplicationUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError);

            return Ok();
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }
    }
}
