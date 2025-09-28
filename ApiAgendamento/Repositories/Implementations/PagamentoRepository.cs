using ApiAgendamento.Data;
using ApiAgendamento.Models.DTO;
using ApiAgendamento.Models;
using ApiAgendamento.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApiAgendamento.Repositories.Implementations
{
    public interface IPagamentoRepository : IRepository<Pagamento>
    {
        
    }

    public class PagamentoRepository : Repository<Pagamento>, IPagamentoRepository
    {
        private readonly AppDbContext _context;

        public PagamentoRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

    }

}
