using ApiAuthentication.Data;
using ApiAuthentication.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(ApplicationDbContext dbContext, IConfiguration configuration) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel entity)
        {
            try
            {
                var user = await dbContext.User.FirstOrDefaultAsync(user => user.Email == entity.Email && user.Password == entity.Password);



                if (user is not null)
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!);

                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity([new Claim(ClaimTypes.Email, entity.Email), new Claim(ClaimTypes.Role, user.Role)]),
                        Expires = DateTime.Now.AddMinutes(60),
                        Issuer = configuration["Jwt:Issuer"]!,
                        Audience = configuration["Jwt:Audience"]!,
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };

                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    return Ok(tokenHandler.WriteToken(token));
                }
                return Unauthorized();
            }
            catch
            {
                return Conflict(new { ErrorMessage = "Internal Server Error" });
            }
        }
    }
}
