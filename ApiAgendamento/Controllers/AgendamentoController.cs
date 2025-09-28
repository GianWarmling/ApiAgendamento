using ApiAgendamento.Data;
using ApiAgendamento.Models;
using ApiAgendamento.Models.DTO;
using ApiAgendamento.Repositories.Implementations;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiAgendamento.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AgendamentoController : GenericController<Agendamento, AgendamentoDto>
    {
        private readonly IAgendamentoRepository _agendamentoRepo;
        private readonly IPagamentoRepository _pagamentoRepo;
        private readonly IMapper _mapper;

        public AgendamentoController(IAgendamentoRepository agendamentoRepo, IMapper mapper, IPagamentoRepository pagamentoRepo)
            : base(agendamentoRepo, mapper)
        {
            _agendamentoRepo = agendamentoRepo;
            _mapper = mapper;
            _pagamentoRepo = pagamentoRepo;
        }

        [HttpPost]
        public override async Task<IActionResult> Post([FromBody] AgendamentoDto dto)
        {
            // 1. Validação de conflito de agendamento
            if (await _agendamentoRepo.ExisteConflitoAsync(dto))
                return BadRequest("Já existe um agendamento nesse horário para essa quadra!");

            // 2. Cálculo do valor
            TimeSpan diferenca = dto.DataHoraFim - dto.DataHoraInicio;
            var valorTotal = (decimal)diferenca.TotalHours * 100;

            // 3. Mapeia DTO -> Entidade
            var agendamento = _mapper.Map<Agendamento>(dto);
            agendamento.ValorTotal = valorTotal;
            agendamento.Status = StatusAgendamento.PENDENTE;

            // 4. Persiste Agendamento
            await _agendamentoRepo.AddAsync(agendamento);
            await _agendamentoRepo.SaveChangesAsync();

            // 5. Cria pagamento vinculado
            var pagamento = new Pagamento
            {
                AgendamentoId = agendamento.Id,
                Status = StatusPagamento.PENDENTE,
                Valor = valorTotal,
                MetodoPagamento = dto.MetodoPagamento
            };

            await _pagamentoRepo.AddAsync(pagamento);
            await _pagamentoRepo.SaveChangesAsync();

            return CreatedAtAction(
                nameof(Get),
                agendamento
            );
        }

        [HttpGet("usuario/{usuarioId}")]
        public async Task<IActionResult> GetByUsuario(int usuarioId)
        {
            var agendamentos = await _agendamentoRepo.GetByUsuarioAsync(usuarioId);
            return Ok(agendamentos);
        }
    }

}
