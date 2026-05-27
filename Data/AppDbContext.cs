using APIWizPedroKarut.Models;
using Microsoft.EntityFrameworkCore;

namespace APIWizPedroKarut.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Pedido> Pedidos { get; set; }

        public DbSet<ItemPedido> ItensPedido { get; set; }  

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
    }
}
