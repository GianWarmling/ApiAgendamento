using ApiAgendamento.Data;
using ApiAgendamento.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiAgendamento.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AgendamentoController : ControllerBase
    {
        private readonly AppDbContext _context;
        public AgendamentoController(AppDbContext context)
        {
            _context = context;
        }
        // GET: api/<AgendamentoController>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_context.Agendamentos.ToList());
        }

        // GET api/<AgendamentoController>/5
        [HttpGet("{id}")]
        public IActionResult Get([FromRoute] int id)
        {
            return Ok(_context.Agendamentos.Where(a => a.Id == id).FirstOrDefault());
        }

        // POST api/<AgendamentoController>
        [HttpPost]
        public IActionResult Post([FromBody] AgendamentoDto novoAgendamento)
        {
            _context.Agendamentos.Add(new Models.Agendamento()
            {
                DataHoraInicio = novoAgendamento.DataHoraInicio,
                DataHoraFim = novoAgendamento.DataHoraFim,
                ValorTotal = novoAgendamento.ValorTotal,
                Status = novoAgendamento.Status,
                UsuarioId = novoAgendamento.UsuarioId,
                QuadraId = novoAgendamento.QuadraId
            });
            _context.SaveChanges();
            return Created("/agendamento", novoAgendamento);
        }

        // PUT api/<AgendamentoController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AgendamentoController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
