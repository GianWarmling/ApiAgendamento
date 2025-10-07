using ApiAgendamento.Data;
using ApiAgendamento.Models;
using ApiAgendamento.Models.DTO;
using ApiAgendamento.Repositories.Inferfaces;
using Microsoft.EntityFrameworkCore;

namespace ApiAgendamento.Repositories.Implementations
{
    public interface IAgendamentoRepository : IRepository<Agendamento>
    { 
        Task<bool> ExisteConflito(AgendamentoDto agendamentoDto);
    }

    public class AgendamentoRepository : Repository<Agendamento>, IAgendamentoRepository
    {
        private readonly AppDbContext _context;

        public AgendamentoRepository (AppDbContext context) : base (context)
        { 
            _context = context;
        }
        public async Task<bool> ExisteConflito(AgendamentoDto novoAgendamento)
        {
            var existeAgendamento = await _context.Agendamentos
                .AnyAsync(a =>
                // Condições de sobreposição de horário
                novoAgendamento.DataHoraInicio < a.DataHoraFim &&
                novoAgendamento.DataHoraFim > a.DataHoraInicio &&

                // Filtros adicionais
                a.QuadraId == novoAgendamento.QuadraId &&
                a.Status != StatusAgendamento.CANCELADO &&
                a.Status != StatusAgendamento.RECUSADO
            );
            return existeAgendamento;
        }
    }
}
