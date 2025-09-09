using ApiAgendamento.Data;
using ApiAgendamento.Models.DTO;
using BCrypt.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiAgendamento.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        public LoginController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        [HttpPost]
        public IActionResult Login([FromBody] LoginDTO dadosLogin)
        {
            var usuario = _context.Usuarios.FirstOrDefault(x => x.Email == dadosLogin.Email);
            if (usuario == null)
            {
                return BadRequest("Email ou Senha Inválidos!");
            }
            //validar senha
            var senhaValida = BCrypt.Net.BCrypt.Verify(dadosLogin.Senha, usuario.SenhaHash);
            if (!senhaValida)
            {
                return BadRequest("Email ou Senha Inválidos!");
            }

            //gerar token
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, usuario.Nome)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:ChaveSecreta"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(issuer: null,
                                             audience: null,
                                             claims: claims,
                                             expires: DateTime.UtcNow.AddHours(4),
                                             signingCredentials: creds);

            return Ok(new JwtSecurityTokenHandler().WriteToken(token));
        }
    }
}
