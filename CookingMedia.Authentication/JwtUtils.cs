using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CookingMedia.Authentication;

public static class JwtUtils
{
    public static string GenerateTokenString(int memberId, TimeSpan expiration,
        string signingKey, params Claim[] additionalClaims)
    {
        var claims = additionalClaims.Append(new Claim(JwtRegisteredClaimNames.Sub, memberId.ToString()));
        var token = new JwtSecurityToken(
            expires: DateTime.UtcNow.Add(expiration),
            claims: claims,
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)),
                SecurityAlgorithms.HmacSha512)
            );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public static void Verify(string token, string signingKey)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        tokenHandler.ValidateToken(token, new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)),
            ValidateIssuer = false,
            ValidateAudience = false
        }, out _);
    }

    public static int? GetIdFromTokenString(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var id = tokenHandler.ReadJwtToken(token).Subject;
        return int.Parse(id);
    }

    public static string GetBearerTokenFromRequest(HttpRequest request)
    {
        return request.Headers["Authorization"].ToString().Replace("Bearer ", "");
    }

    public static int? GetUserIdFromRequest(HttpRequest request)
    {
        return GetIdFromTokenString(GetBearerTokenFromRequest(request));
    }
}
