using Microsoft.EntityFrameworkCore;
using MotoApi.Models;

namespace MotoApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Moto> Motos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Manutencao> Manutencoes { get; set; }
    }
}
