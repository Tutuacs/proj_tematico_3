using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using api.Model.Dtos.Auth;
using api.Model.Entities;
using Microsoft.IdentityModel.Tokens;

namespace api.Helpers;

public class JwtHelper
{
    private readonly IConfiguration _configuration;

    public JwtHelper(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public (string token, DateTime expiration) GenerateToken(ProfileDb profile)
    {
        var secretKey = _configuration["Jwt:SecretKey"] 
            ?? throw new InvalidOperationException("Jwt:SecretKey não configurado");
        var issuer = _configuration["Jwt:Issuer"];
        var audience = _configuration["Jwt:Audience"];
        var expirationMinutes = int.Parse(_configuration["Jwt:ExpirationInMinutes"] ?? "60");

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var expiration = DateTime.UtcNow.AddMinutes(expirationMinutes);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, profile.Id),
            new Claim(JwtRegisteredClaimNames.Email, profile.Email),
            new Claim(JwtRegisteredClaimNames.Name, profile.Name),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("userId", profile.Id)
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expiration,
            signingCredentials: credentials
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return (tokenString, expiration);
    }

    /// <summary>
    /// Extrai todas as informações do usuário autenticado do JWT
    /// </summary>
    /// <param name="user">ClaimsPrincipal do HttpContext.User</param>
    /// <returns>ProfileResponse completo ou null se não autenticado</returns>
    public static ProfileResponse? GetCurrentUser(ClaimsPrincipal user)
    {
        if (user?.Identity?.IsAuthenticated != true)
            return null;

        var userId = user.FindFirst(JwtRegisteredClaimNames.Sub)?.Value 
                  ?? user.FindFirst("userId")?.Value
                  ?? user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                  
        var email = user.FindFirst(JwtRegisteredClaimNames.Email)?.Value 
                 ?? user.FindFirst(ClaimTypes.Email)?.Value;
                 
        var name = user.FindFirst(JwtRegisteredClaimNames.Name)?.Value 
                ?? user.FindFirst(ClaimTypes.Name)?.Value;

        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(email))
            return null;

        return new ProfileResponse
        {
            Id = userId,
            Email = email,
            Name = name ?? string.Empty,
            CreatedAt = DateOnly.MinValue // Não armazenamos no token
        };
    }
}

