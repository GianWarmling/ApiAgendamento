using ApiAgendamento.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiAgendamento.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Agendamento>Agendamentos { get; set; }
        public DbSet<Usuario>Usuarios { get; set; }
        public DbSet<Quadra>Quadras { get; set; }

    }
}
