using ApiAgendamento.Data;
using ApiAgendamento.Models.DTO;
using ApiAgendamento.Models;
using ApiAgendamento.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApiAgendamento.Repositories.Implementations
{
    public interface IAgendamentoRepository : IRepository<Agendamento>
    {
        Task<bool> ExisteConflitoAsync(AgendamentoDto dto);
        Task<IEnumerable<Agendamento>> GetByUsuarioAsync(int usuarioId);
    }

    public class AgendamentoRepository : Repository<Agendamento>, IAgendamentoRepository
    {
        private readonly AppDbContext _context;

        public AgendamentoRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> ExisteConflitoAsync(AgendamentoDto dto)
        {
            return await _context.Agendamentos.AnyAsync(a =>
                dto.DataHoraInicio < a.DataHoraFim &&
                dto.DataHoraFim > a.DataHoraInicio &&
                a.QuadraId == dto.QuadraId &&
                a.Status != StatusAgendamento.CANCELADO &&
                a.Status != StatusAgendamento.RECUSADO);
        }

        public async Task<IEnumerable<Agendamento>> GetByUsuarioAsync(int usuarioId)
        {
            return await _context.Agendamentos
                .Where(a => a.UsuarioId == usuarioId)
                .ToListAsync();
        }
    }

}
