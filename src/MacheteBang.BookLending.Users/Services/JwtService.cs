using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace MacheteBang.BookLending.Users.Services;

public interface IJwtService
{
    string CreateToken(Guid userId, string userName, string? email, IEnumerable<string> roles);
    bool IsConfigured { get; }
    DateTimeOffset GetExpiration();
}

public class JwtService : IJwtService
{
    private readonly string? _key;
    private readonly string? _issuer;
    private readonly string? _audience;
    private readonly int _expiresMinutes;
    private readonly bool _isConfigured;
    // Removed _lastExpiration to make the service stateless

    public JwtService(IConfiguration config)
    {
        var jwtSection = config.GetSection("Jwt");
        _key = jwtSection["Key"];
        _issuer = jwtSection["Issuer"];
        _audience = jwtSection["Audience"];
        _expiresMinutes = int.TryParse(jwtSection["ExpiresMinutes"], out var m) ? m : 60;
        _isConfigured = !string.IsNullOrWhiteSpace(_key) && !string.IsNullOrWhiteSpace(_issuer) && !string.IsNullOrWhiteSpace(_audience);
    }

    public bool IsConfigured => _isConfigured;

    public string CreateToken(Guid userId, string userName, string? email, IEnumerable<string> roles)
    {
        if (!_isConfigured)
            throw new InvalidOperationException("JWT configuration is missing or invalid.");

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, userName ?? string.Empty),
            new(JwtRegisteredClaimNames.Email, email ?? string.Empty)
        };
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var keyBytes = Encoding.UTF8.GetBytes(_key!);
        var creds = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256);
        // The expiration is calculated per token and not stored in the service
        var expires = DateTimeOffset.UtcNow.AddMinutes(_expiresMinutes);

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: expires.UtcDateTime,
            signingCredentials: creds
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    // Since the service is now stateless, GetExpiration can return the calculated expiration for a given token
    public DateTimeOffset GetExpiration()
    {
        // Returns the expiration as if a token were created now
        return DateTimeOffset.UtcNow.AddMinutes(_expiresMinutes);
    }
}
