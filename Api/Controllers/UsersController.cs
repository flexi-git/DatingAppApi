using Api.Data;
using Api.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    public class UsersController : BaseApiController
    {        

        private readonly ILogger<UsersController> _logger;
        private readonly ApiDataContext _context;

        public UsersController(ILogger<UsersController> logger, ApiDataContext context)
        {
            _logger = logger;
            _context = context;
        }

        [AllowAnonymous]
        [HttpGet]
        public async ValueTask<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();

            return users;
        }

        [Authorize]
        [HttpGet("{id}")]
        public async ValueTask<ActionResult<AppUser?>> GetUsers(int id)
        {
            var user = await _context.Users.FindAsync(id);

            return user;
        }
    }
}