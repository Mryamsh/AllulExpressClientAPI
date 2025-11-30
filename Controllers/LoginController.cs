
using AllulExpressClientApi;
using AllulExpressClientApi.Data;
using AllulExpressClientApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


[ApiController]
[Route("api/[controller]")]
public class LoginController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly TokenService _tokenService;

    public LoginController(AppDbContext db, TokenService tokenService)
    {
        _db = db;
        _tokenService = tokenService;
    }

    [AllowAnonymous]
    [HttpPost("loginclient")]
    public async Task<IActionResult> LoginClient([FromBody] LoginRequest request)
    {


        // 1 fetch user from DB
        var client = await _db.Clients
     .FirstOrDefaultAsync(u => u.Phonenum1 == request.Phonenum1);

        if (client == null)
        {
            // user not found
            return Unauthorized(new { message = "Invalid phone or password" });
        }

        //  Verify the password (hashed)
        bool isValidPassword = BCrypt.Net.BCrypt.Verify(request.Password, client.Password);

        if (!isValidPassword)
        {
            // password incorrect
            return Unauthorized(new { message = "Invalid phone or password" });
        }


        //  generate JWT
        var token = _tokenService.GenerateToken(client);

        // 3️⃣ save in ValidTokens
        _db.ValidTokenClients.Add(new ValidTokenClients
        {
            clientid = client.Id,
            Token = token,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        });
        await _db.SaveChangesAsync();

        // 4️ return token
        return Ok(new { Token = token, client, client.Language });
    }

    [HttpGet("validate-token")]
    public async Task<IActionResult> ValidateToken([FromQuery] string token)
    {
        var tokenRecord = await _db.ValidTokenClients
            .FirstOrDefaultAsync(t => t.Token == token);

        if (tokenRecord == null)
            return Unauthorized(new { valid = false, reason = "Token not found" });

        if (tokenRecord.ExpiresAt < DateTime.UtcNow)
            return Unauthorized(new { valid = false, reason = "Token expired" });

        return Ok(new { valid = true });
    }

}
