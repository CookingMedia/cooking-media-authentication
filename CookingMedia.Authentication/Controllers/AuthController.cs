using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CookingMedia.Authentication.Entities;
using CookingMedia.Authentication.Models;
using Isopoh.Cryptography.Argon2;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace CookingMedia.Authentication.Controllers;

[Route("v1/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AuthenticationDBContext _context;
    private readonly IConfiguration _configuration;

    public AuthController(AuthenticationDBContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseModel>> Login(LoginRequestModel req)
    {
        User? user;
        //req.Email = req.Email.ToLower();
        if ((user = await _context.Users.FirstOrDefaultAsync(e => e.Email == req.Email)) == null)
        {
            return Unauthorized(new { ErrorCode = "WRONG_EMAIL" });
        }
        if (!Argon2.Verify(user.Password, req.Password))
        {
            return Unauthorized(new { ErrorCode = "WRONG_PASSWORD" });
        }
        return GenerateLoginResponse(user);
    }

    [HttpPost("signup")]
    public async Task<ActionResult<LoginResponseModel>> Signup(SignupRequestModel req)
    {
        // Check if email is already used
        req.Email = req.Email.ToLower();
        if (await _context.Users.AnyAsync(e => e.Email == req.Email))
        {
            return BadRequest(new { ErrorCode = "EMAIL_ALREADY_USED" });
        }

        // Create user
        var user = new User
        {
            Email = req.Email,
            Password = Argon2.Hash(req.Password, type: Argon2Type.DataIndependentAddressing),
            Name = req.Name,
            Role = Entities.User.Roles.Member
        };
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return GenerateLoginResponse(user);
    }

    [HttpGet("verify")]
    public IActionResult VerifyToken(string token)
    {
        try
        {
            JwtUtils.Verify(token, _configuration["Jwt:Key"]);
            return NoContent();
        }
        catch (SecurityTokenExpiredException)
        {
            return Unauthorized(new { ErrorCode = "EXPIRED_TOKEN" });
        }
        catch (SecurityTokenInvalidSignatureException)
        {
            return Unauthorized(new { ErrorCode = "INVALID_SIGNING_KEY" });
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }

    private LoginResponseModel GenerateLoginResponse(User user)
    {
        return new LoginResponseModel
        {
            Token = JwtUtils.GenerateTokenString(
                user.Id,
                TimeSpan.FromMinutes(double.Parse(_configuration["Jwt:Duration"])),
                _configuration["Jwt:Key"],
                new Claim(ClaimTypes.Role, user.Role)
            ),
            User = new UserResponseModel
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                Role = user.Role
            }
        };
    }
}
