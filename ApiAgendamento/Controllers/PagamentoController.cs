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
    public class PagamentoController : GenericController<Pagamento, PagamentoDTO>
    {
        private readonly IRepository<Pagamento> _pagamentoRepository;
        private readonly IAgendamentoRepository _agendamentoRepository;
        public PagamentoController(IMapper mapper, IRepository<Pagamento> pagamentoRepository, IAgendamentoRepository agendamentoRepository) : base (pagamentoRepository, mapper)
        {
            _pagamentoRepository = pagamentoRepository;
            _agendamentoRepository = agendamentoRepository;
        }

        [HttpPatch("{id}/alterarStatus/{acao}")]
        public async Task<IActionResult> ConfirmarPagamento([FromRoute] int id, [FromRoute] StatusPagamento acao)
        {
            var pagamento = await _pagamentoRepository.GetByIdAsync(id);
            if (pagamento == null)
            {
                return BadRequest("Pagamento não encontrado!");
            }
            var agendamento = await _agendamentoRepository.GetByIdAsync(pagamento.AgendamentoId);
            if (agendamento == null)
            {
                return BadRequest("Pagamento não encontrado!");
            }
            pagamento.Status = acao;
            if (acao == StatusPagamento.APROVADO)
            {
                pagamento.DataPagamento = DateTime.Now;
                agendamento.Status = StatusAgendamento.APROVADO;
            } else if (acao == StatusPagamento.ESTORNADO)
            {
                agendamento.Status = StatusAgendamento.CANCELADO;
            } else
            {
                agendamento.Status = StatusAgendamento.RECUSADO;
            }
            await _pagamentoRepository.UpdateAsync(pagamento);
            await _agendamentoRepository.UpdateAsync(agendamento);
            await _pagamentoRepository.SaveChanges();
            return Ok("Pagamento confirmado com sucesso!");
        }
    }
}
