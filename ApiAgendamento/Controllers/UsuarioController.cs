using ApiAgendamento.Data;
using ApiAgendamento.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiAgendamento.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly AppDbContext _context; 
        public UsuarioController(AppDbContext context) 
        {
            _context = context;
        }
        // GET: api/<UsuarioController>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_context.Usuarios.ToList().Select(u =>
            {
                return new
                {
                    u.Id,
                    u.Nome,
                    u.Email,
                    u.Telefone,
                    u.IsAdmin
                };
            }));
        }

        // GET api/<UsuarioController>/5
        [HttpGet("{id}")]
        public IActionResult Get([FromRoute] int id)
        {
            return Ok(_context.Usuarios.Where(u => u.Id == id).ToList().Select(u =>
            {
                return new
                {
                    u.Id,
                    u.Nome,
                    u.Email,
                    u.Telefone,
                    u.IsAdmin
                };
            }));
        }

        // POST api/<UsuarioController>
        [HttpPost]
        public IActionResult Post([FromBody] Usuario novoUsuario)
        {
            novoUsuario.SenhaHash = BCrypt.Net.BCrypt.HashPassword(novoUsuario.SenhaHash);
            _context.Usuarios.Add(novoUsuario);
            _context.SaveChanges();
            return Created("/usuario", novoUsuario);
        }

        // PUT api/<UsuarioController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UsuarioController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var usuarioExiste = _context.Usuarios.FirstOrDefault(u => u.Id == id);
            if (usuarioExiste is null)
            {
                return BadRequest("Usuário não existe!");
            }
            _context.Usuarios.Remove(usuarioExiste);
            _context.SaveChanges();
            return Ok("Usuário removido com sucesso!");
        }
    }
}
