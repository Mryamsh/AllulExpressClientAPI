using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using AllulExpressClientApi.Data;
using AllulExpressClientApi.Models;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ClientController : ControllerBase
{

    private readonly AppDbContext _db;
    public ClientController(AppDbContext db)
    {
        _db = db;

    }


}
