using AllulExpressClientApi;
using AllulExpressClientApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class AuthController : Microsoft.AspNetCore.Mvc.ControllerBase
{
    private readonly AppDbContext _db;
    private readonly WhatsAppService _whatsAppService;

    public AuthController(AppDbContext db, WhatsAppService whatsAppService)
    {
        _db = db;
        _whatsAppService = whatsAppService;
    }

    [HttpPost("request-password-reset")]
    public async Task<IActionResult> RequestPasswordReset([FromBody] string phoneNumber)
    {
        var client = await _db.Clients.FirstOrDefaultAsync(c => c.Phonenum1 == phoneNumber);
        if (client == null)
            return NotFound(new { message = "Client not found" });

        var otp = new Random().Next(100000, 999999).ToString();

        _db.PasswordResetRequests.Add(new PasswordResetRequest
        {
            ClientId = client.Id,
            OTP = otp,
            ExpiresAt = DateTime.UtcNow.AddMinutes(10)
        });

        await _db.SaveChangesAsync();

        await _whatsAppService.SendOtpAsync(client.Phonenum1, otp);

        return Ok(new { message = "OTP sent to WhatsApp" });
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        var resetRequest = await _db.PasswordResetRequests
            .Where(r => r.ClientId == request.ClientId && r.OTP == request.OTP && !r.IsUsed)
            .FirstOrDefaultAsync();

        if (resetRequest == null || resetRequest.ExpiresAt < DateTime.UtcNow)
            return BadRequest(new { message = "Invalid or expired OTP" });

        var client = await _db.Clients.FindAsync(request.ClientId);
        if (client == null)
            return NotFound(new { message = "Client not found" });

        client.Password = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);

        resetRequest.IsUsed = true;

        await _db.SaveChangesAsync();

        return Ok(new { message = "Password changed successfully" });
    }
}

public class ResetPasswordRequest
{
    public int ClientId { get; set; }
    public string OTP { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}
