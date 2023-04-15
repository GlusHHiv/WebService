using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using webApiMessenger.Domain.Abstractions;
using webApiMessenger.Persistence.Repositories;

namespace webApiMessenger.WebApi.Services;

public class TokenProvider : ITokenProvider
{
    private readonly UserRepository _userRepository;
    private readonly JwtSettings _jwtSettings;

    public TokenProvider(UserRepository userRepository, IOptions<JwtSettings> options)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _jwtSettings = options.Value;
    }

    public async Task<string> GetJwt(string login)
    {
        var user = await _userRepository.GetByLogin(login);
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Nick),
            new("Id", user.Id.ToString()),
            new("Age", user.Age.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var jwt = new JwtSecurityToken(
            claims: claims,
            issuer: _jwtSettings.ValidIssuer,
            audience: _jwtSettings.ValidAudience,
            expires: DateTime.UtcNow.Add(TimeSpan.FromDays(_jwtSettings.ExpiresDay)),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
}