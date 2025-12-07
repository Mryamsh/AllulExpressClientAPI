
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

    [HttpGet("by-qrcode")]
    public async Task<IActionResult> GetPostByQr([FromQuery] string qr, [FromQuery] int clientId)
    {
        Console.WriteLine(qr);
        var post = await _db.Posts.FirstOrDefaultAsync(p => p.Qrcode == qr);
        Console.WriteLine(post);
        if (post == null)
            return NotFound("Post not found");

        if (post.ClientId != clientId)
            return BadRequest("This post does not belong to this client");

        return Ok(post);
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
