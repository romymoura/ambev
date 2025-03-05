using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.Metrics;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime;
using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;
using System.Text;

namespace Ambev.DeveloperEvaluation.Common.Security;

/// <summary>
/// Implementation of JWT (JSON Web Token) generator.
/// </summary>
public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly IConfiguration _configuration;
    private readonly SecuritySettings _securitySettions;

    /// <summary>
    /// Initializes a new instance of the JWT token generator.
    /// </summary>
    /// <param name="configuration">Application configuration containing the necessary keys for token generation.</param>
    public JwtTokenGenerator(IConfiguration configuration, IOptions<SecuritySettings> securitySettions)
    {
        _configuration = configuration;
        _securitySettions = securitySettions.Value;
    }

    /// <summary>
    /// Generates a JWT token for a specific user.
    /// </summary>
    /// <param name="user">User for whom the token will be generated.</param>
    /// <returns>Valid JWT token as string.</returns>
    /// <remarks>
    /// The generated token includes the following claims:
    /// - NameIdentifier (User ID)
    /// - Name (Username)
    /// - Role (User role)
    /// 
    /// The token is valid for 8 hours from the moment of generation.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Thrown when user or secret key is not provided.</exception>
    public string GenerateToken(IUser user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_securitySettions?.IssuerKey ?? string.Empty));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
           new Claim(ClaimTypes.NameIdentifier, user.Id),
           new Claim(ClaimTypes.Name, user.Username),
           new Claim(ClaimTypes.Role, user.Role)
       };

        var dtExpire = DateTime.Now.AddMinutes(_securitySettions?.TokenExp ?? 10);
        var tokenSecurity = new JwtSecurityToken(
            issuer: _securitySettions?.Issuer,
            audience: _securitySettions?.TokenAudirence,
            claims: claims,
            expires: dtExpire,
            signingCredentials: creds
        );

        var token = tokenHandler.WriteToken(tokenSecurity);
        return token;
    }
}