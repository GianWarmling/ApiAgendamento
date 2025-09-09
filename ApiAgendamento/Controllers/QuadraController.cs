using ApiAgendamento.Data;
using ApiAgendamento.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiAgendamento.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuadraController : ControllerBase
    {
        private readonly AppDbContext _context;
        public QuadraController(AppDbContext context) 
        {
            _context = context;
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
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<QuadraController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
