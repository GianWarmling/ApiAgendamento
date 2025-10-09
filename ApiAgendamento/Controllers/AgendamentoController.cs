using ApiAgendamento.Data;
using ApiAgendamento.Models;
using ApiAgendamento.Models.DTO;
using ApiAgendamento.Repositories.Implementations;
using ApiAgendamento.Repositories.Inferfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiAgendamento.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AgendamentoController : GenericController<Agendamento, AgendamentoDto>
    {
        private readonly IRepository<Pagamento> _pagamentoRepository;
        private readonly IAgendamentoRepository _agendamentoRepository;
        private readonly IMapper _mapper;
        public AgendamentoController(IAgendamentoRepository agendamento, IMapper mapper, IRepository<Pagamento> pagamentoRepository) : base(agendamento, mapper)
        {
            _agendamentoRepository = agendamento;
            _mapper = mapper;
            _pagamentoRepository = pagamentoRepository;
        }

        // POST api/<AgendamentoController>
        [HttpPost]
        public override async Task<IActionResult> Post([FromBody] AgendamentoDto novoAgendamento)
        {
            
            if (await _agendamentoRepository.ExisteConflito(novoAgendamento))
            {
                return BadRequest("Já existe um agendamento nesse horário para essa quadra!");
            }
            //FAZER CALCULO DE TOTAL DE HORAS
            TimeSpan diferenca = novoAgendamento.DataHoraFim - novoAgendamento.DataHoraInicio;
            var valorTotal = (decimal)diferenca.TotalHours * 100;

            Agendamento agendamento = new Agendamento();
            _mapper.Map(novoAgendamento, agendamento);
            agendamento.ValorTotal = valorTotal;
            agendamento.Status = StatusAgendamento.PENDENTE;
            await _agendamentoRepository.AddAsync(agendamento);
            await _agendamentoRepository.SaveChanges();
            Pagamento pagamento = new Pagamento()
            {
                AgendamentoId = agendamento.Id,
                Status = StatusPagamento.PENDENTE,
                Valor = valorTotal,
                MetodoPagamento = novoAgendamento.MetodoPagamento
            };
            await _pagamentoRepository.AddAsync(pagamento);
            await _pagamentoRepository.SaveChanges();
            return CreatedAtAction("/agendamento", agendamento);
        }
    }
}
