using ApiAgendamento.Data;
using ApiAgendamento.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiAgendamento.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagamentoController : ControllerBase
    {
        private readonly AppDbContext _context;
        public PagamentoController(AppDbContext context) 
        {
            _context = context;
        }

        // GET: api/<PagamentoController>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_context.Pagamentos.ToList());
        }

        // GET api/<PagamentoController>/5
        [HttpGet("{id}")]
        public IActionResult Get([FromRoute] int id)
        {
            return Ok(_context.Pagamentos.Where(p => p.Id == id).FirstOrDefault());
        }

        // POST api/<PagamentoController>
        [HttpPost]
        public IActionResult Post([FromBody] Pagamento novoPagamento)
        {
            _context.Pagamentos.Add(novoPagamento);
            _context.SaveChanges();
            return Created("/pagamento", novoPagamento);
        }

        // PUT api/<PagamentoController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<PagamentoController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
