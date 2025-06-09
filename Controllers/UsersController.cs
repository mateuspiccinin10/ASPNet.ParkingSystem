using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ParkingSystem.Data;
using ParkingSystem.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ParkingSystem.Dtos;


namespace ParkingSystem.Controllers;


[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;
    
    private static string GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes("kT93!xwL0sJfPZ@qD8GvHeRu93mLpWxY");

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role.ToLower())
            }),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            )
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public UsersController(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }
    
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserDto dto)
    {
        var user = new User
        {
            Username = dto.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = dto.Role.ToLower(),
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok("Usuário criado");
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);
        if (user == null || !user.IsActive)
            return Unauthorized("Usuário inválido ou desativado.");

        if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            return Unauthorized("Senha inválida.");

        var token = GenerateToken(user);
        return Ok(new { token });
    }
    
    [Authorize(Roles = "admin")]
    [HttpPut("disable/{id}")]
    public async Task<IActionResult> DisableUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return NotFound("Usuário não encontrado.");

        user.IsActive = false;
        await _context.SaveChangesAsync();

        return Ok("Usuário desativado");
    }
    
    [Authorize(Roles = "admin")]
    [HttpPut("enable/{id}")]
    public async Task<IActionResult> EnableUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return NotFound("Usuário não encontrado.");
    
        user.IsActive = true;
        await _context.SaveChangesAsync();
    
        return Ok("Usuário ativado");
    }
}