using ApiAgendamento.Data;
using ApiAgendamento.Models;
using ApiAgendamento.Models.DTO;
using AutoMapper;
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
        private readonly IMapper _mapper;
        public AgendamentoController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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
            Agendamento agendamento = new Agendamento();
            _mapper.Map(novoAgendamento, agendamento);
            _context.Agendamentos.Add(agendamento);
            _context.SaveChanges();
            return Created("/agendamento", novoAgendamento);
        }

        // PUT api/<AgendamentoController>/5
        [HttpPut("{id}")]
        public IActionResult Put([FromRoute] int id, [FromBody] AgendamentoDto agendamentoAtualizado)
        {
            var agendamento = _context.Agendamentos.FirstOrDefault(a => a.Id == id);
            if (agendamento == null)
            {
                return BadRequest("Agendamento não encontrado!");
            }
            _mapper.Map(agendamentoAtualizado, agendamento);
            _context.Update(agendamento);
            _context.SaveChanges();
            return Ok("Agendamento atualizado com sucesso!");
        }

        // DELETE api/<AgendamentoController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var agendamento = _context.Agendamentos.FirstOrDefault(a => a.Id == id);
            if (agendamento == null)
            {
                return BadRequest("Agendamento não encontrado!");
            }
            _context.Remove(agendamento);
            _context.SaveChanges();
            return Ok("Agendamento removido com sucesso!");
        }
    }
}
