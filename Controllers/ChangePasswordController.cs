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
    private readonly ILogger<AuthController> _logger;

    public AuthController(AppDbContext db, WhatsAppService whatsAppService, ILogger<AuthController> logger)
    {
        _db = db;
        _whatsAppService = whatsAppService;
        _logger = logger;
    }
    [HttpPost("request-otp")]
    public async Task<IActionResult> RequestOtp([FromBody] string phone)
    {
        Console.WriteLine("phone " + phone);


        var user = await _db.Clients.FirstOrDefaultAsync(c => c.Phonenum1 == phone);
        if (user == null)
            return NotFound(new { message = "Phone not registered" });

        string otp = OtpService.GenerateOtp();
        Console.WriteLine("otp " + otp);


        //  return Ok(new { message = "OTP sent via WhatsApp" + otp });
        user.OtpCode = otp;
        user.OtpExpiry = DateTime.UtcNow.AddMinutes(5);  // Valid 5 minutes
        await _db.SaveChangesAsync();

        // var wa = new WhatsAppService();
        _whatsAppService.SendOtp(phone, otp);

        return Ok(new { message = "OTP sent via WhatsApp" });
    }




    [HttpPost("verify-otp")]
    public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequest req)
    {
        var user = await _db.Clients.FirstOrDefaultAsync(c => c.Phonenum1 == req.Phone);

        if (user == null)
            return NotFound();

        if (user.OtpCode != req.Otp)
            return BadRequest(new { message = "Invalid OTP" });

        if (user.OtpExpiry < DateTime.UtcNow)
            return BadRequest(new { message = "OTP expired" });

        return Ok(new { message = "OTP verified" });
    }



    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest req)
    {
        var user = await _db.Clients.FirstOrDefaultAsync(c => c.Phonenum1 == req.Phone);

        if (user == null)
            return NotFound();


        user.Password = BCrypt.Net.BCrypt.HashPassword(req.NewPassword);

        // user.Password = req.NewPassword; // You should hash it using BCrypt
        user.OtpCode = null;
        user.OtpExpiry = null;

        await _db.SaveChangesAsync();

        return Ok(new { message = "Password updated" });
    }


}