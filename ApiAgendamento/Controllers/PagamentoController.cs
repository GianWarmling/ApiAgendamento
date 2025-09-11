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
    public class PagamentoController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public PagamentoController(AppDbContext context, IMapper mapper) 
        {
            _context = context;
            _mapper = mapper;
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
        public IActionResult Post([FromBody] PagamentoDTO novoPagamento)
        {
            Pagamento pagamento = new Pagamento();
            _mapper.Map(novoPagamento, pagamento);
            _context.Pagamentos.Add(pagamento);
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
