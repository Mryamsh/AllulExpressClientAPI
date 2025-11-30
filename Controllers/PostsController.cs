
using AllulExpressClientApi.Data;
using AllulExpressClientApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PostsController : ControllerBase
{
    private readonly AppDbContext _db;

    public PostsController(AppDbContext db)
    {
        _db = db;
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


}
