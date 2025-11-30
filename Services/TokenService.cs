
using System.Security.Claims;
using System.Text;
using AllulExpressClientApi.Data;
using AllulExpressClientApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;


public class TokenService
{
    private readonly IConfiguration _config;
    private readonly AppDbContext _db;
    public TokenService(AppDbContext db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    public string GenerateToken(Clients client)
    {
        var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
#pragma warning disable CS8604 // Possible null reference argument.
        var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);
#pragma warning restore CS8604 // Possible null reference argument.

#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8604 // Possible null reference argument.
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, client.Id.ToString()),
                new Claim(ClaimTypes.Email,client.Email),
               // new Claim(ClaimTypes.Role, emplyee.Role.ToString())
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8602 // Dereference of a possibly null reference.

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public async Task<bool> IsTokenValidAsync(string token)
    {
        var tokenRecord = await _db.ValidTokenClients
            .FirstOrDefaultAsync(t => t.Token == token);

        if (tokenRecord == null)
            return false; // not in DB → revoked or never issued

        if (tokenRecord.ExpiresAt < DateTime.UtcNow)
            return false; // expired in DB

        return true; // ✅ valid
    }

}
