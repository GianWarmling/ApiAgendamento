using ApiAgendamento.Data;
using ApiAgendamento.Models;
using ApiAgendamento.Models.DTO;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiAgendamento.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class QuadraController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public QuadraController(AppDbContext context, IMapper mapper) 
        {
            _context = context;
            _mapper = mapper;
        }
        // GET: api/<QuadraController>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_context.Quadras.ToList());
        }

        // GET api/<QuadraController>/5
        [HttpGet("{id}")]
        public IActionResult Get([FromRoute] int id)
        {
            return Ok(_context.Quadras.Where(q => q.Id == id).FirstOrDefault());
        }

        // POST api/<QuadraController>
        [HttpPost]
        public IActionResult Post([FromBody] Quadra novaQuadra)
        {
            _context.Quadras.Add(novaQuadra);
            _context.SaveChanges();
            return Created("/quadra", novaQuadra);
        }

        // PUT api/<QuadraController>/5
        [HttpPut("{id}")]
        public IActionResult Put([FromRoute] int id, [FromBody] QuadraDTO quadraAtualizada)
        {
            var quadra = _context.Quadras.FirstOrDefault(q => q.Id == id);
            if (quadra == null)
            {
                return BadRequest("Quadra não existe!");
            }
            _mapper.Map(quadraAtualizada, quadra);
            _context.Update(quadra);
            _context.SaveChanges();
            return Ok("Quadra atualizada com sucesso!");
        }

        // DELETE api/<QuadraController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute]int id)
        {
            var quadra = _context.Quadras.FirstOrDefault(q => q.Id == id);
            if (quadra is null)
            {
                return BadRequest("Quadra não existe!");
            }
            _context.Remove(quadra);
            _context.SaveChanges();
            return Ok("Quadra removida com sucesso!");
        }
    }
}
