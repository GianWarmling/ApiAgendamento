using ApiAgendamento.Data;
using ApiAgendamento.Repositories.Inferfaces;
using Microsoft.EntityFrameworkCore;

namespace ApiAgendamento.Repositories.Implementations
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbset;
        public Repository(AppDbContext context) 
        {
            _context = context;
            _dbset = _context.Set<T>();
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbset.ToListAsync();
        }
        public async Task<T> AddAsync(T dados)
        {
            await _dbset.AddAsync(dados);
            await _context.SaveChangesAsync();
            return dados;
        }
        public async Task<T> UpdateAsync(T dados)
        {
            _dbset.Update(dados);
            await _context.SaveChangesAsync();
            return dados;
        }

        public async Task DeleteAsync(T dado)
        {
            _dbset.Remove(dado);
            await _context.SaveChangesAsync();
        }
        public async Task<T?> GetByIdAsync(int id)
        {
            var dado = await _dbset.FindAsync(id);
            return dado;
        }
    }
}
