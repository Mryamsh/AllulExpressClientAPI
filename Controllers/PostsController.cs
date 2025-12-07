
using AllulExpressClientApi.Data;
using AllulExpressClientApi.Models;
using AllulExpressClientApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PostsController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly QrCodeService _qrService;


    public PostsController(AppDbContext db, QrCodeService qrService)
    {
        _db = db;
        _qrService = qrService;
    }
    [Authorize]
    [HttpGet("{clientId}/posts")]
    public async Task<IActionResult> GetPostsByClient(int clientId)
    {
        var posts = await _db.Posts
            .Where(p => p.ClientId == clientId)
            .ToListAsync();

        if (posts.Count == 0)
            return NotFound(new { message = "No posts found for this client" });

        return Ok(posts);
    }

    // [HttpGet("by-qr")]
    // [Authorize] // Ensure client is authenticated
    // public async Task<IActionResult> GetPostByQrCode([FromQuery] string qrCode)
    // {
    //     // 1️⃣ Get the ClientId from JWT
    //     var clientIdClaim = User.FindFirst("Id")?.Value;
    //     if (clientIdClaim == null)
    //         return Unauthorized(new { message = "Invalid token" });

    //     int clientId = int.Parse(clientIdClaim);

    //     // 2️⃣ Find the post with this QR code for this client
    //     var post = await _db.Posts
    //         .FirstOrDefaultAsync(p => p.QrCode == qrCode && p.ClientId == clientId);

    //     if (post == null)
    //         return NotFound(new { message = "Post not found for this client" });

    //     return Ok(post);
    // }

    [HttpGet("by-qrcode")]
    public IActionResult GetPostByQr(string qr, int clientId)
    {
        try
        {
            string decrypted = _qrService.Decrypt(qr);

            Console.WriteLine("QR RAW: " + qr);
            Console.WriteLine("QR DECRYPTED: " + decrypted);

            var parts = decrypted.Split('|');
            if (parts.Length != 3)
                return BadRequest("Invalid QR Code");

            int postId = int.Parse(parts[2]);
            int qrClientId = int.Parse(parts[0]); // from QR

            Console.WriteLine($"QR ClientId = {qrClientId}, Request ClientId = {clientId}");

            if (qrClientId != clientId)
                return BadRequest("Client mismatch");

            var post = _db.Posts.FirstOrDefault(i => i.Id == postId);

            if (post == null)
                return NotFound("Post not found");

            return Ok(post);
        }
        catch
        {
            return BadRequest("QR decode failed");
        }
    }


    [HttpGet("{postId}/client/{clientId}")]
    public async Task<IActionResult> GetPostById(int postId, int clientId)
    {
        // Fetch the post including the driver info
        var post = await _db.Posts
            .Include(p => p.driver) // Make sure you have a navigation property "Driver" in Post model
            .FirstOrDefaultAsync(p => p.Id == postId && p.ClientId == clientId);

        if (post == null)
        {
            return NotFound(new { message = "Post not found or not associated with this client." });
        }

        // Optional: return custom object instead of full EF entities
        var result = new
        {
            PostId = post.Id,
            post.Businessname,
            post.City,
            post.Phonenum1,
            post.Phonenum2,
            post.Price,
            post.Shipmentfee,
            post.Postnum,
            post.ChangeOrReturn,
            post.Numberofpieces,
            post.Exactaddress,
            post.Poststatus,
            post.Note,
            post.Savedate,


            Driver = post.driver != null ? new
            {
                post.driver.Id,
                post.driver.Name,
                post.driver.Phonenum1,
                post.driver.Phonenum2,
                post.driver.Email,
                post.driver.Note,



                post.driver.Vehicledetail
            } : null
        };

        return Ok(result);
    }

}
