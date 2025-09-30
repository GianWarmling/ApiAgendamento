namespace ApiAgendamento.Repositories.Inferfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> AddAsync(T dados);
        Task<T> UpdateAsync(T dados);
        Task DeleteAsync(T dado);
        Task<T?> GetByIdAsync(int id);
    }
}
