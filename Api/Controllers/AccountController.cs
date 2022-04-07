using Api.Data;
using Api.DTOs;
using Api.Entities;
using Api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Api.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly ILogger<AccountController> _logger;
        private readonly ApiDataContext _context;
        private readonly ITokenService _tokenService;

        public AccountController(ILogger<AccountController> logger, ApiDataContext context, ITokenService tokenService)
        {
            _logger = logger;
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async ValueTask<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if(await IsUserExists(registerDto.Username!)) return BadRequest("Username is taken.");

            using var hmac = new HMACSHA512();

            var user = new AppUser
            {
                UserName = registerDto.Username,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password!)),
                PasswordSalt = hmac.Key
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }


        [HttpPost("login")]
        public async ValueTask<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == loginDto.Username);

            if (user == null) return Unauthorized("Invalid username.");

            using var hmac = new HMACSHA512(user.PasswordSalt!);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password!));

            for(int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash![i]) return Unauthorized("Invalid password");
            }

            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }


        private async ValueTask<bool> IsUserExists(string username)
        {
            return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
    }
}
