using Atividade_07_05.Models;
using Microsoft.EntityFrameworkCore;
namespace Atividade_07_05.Data
{
    public class GerenciadorContext : DbContext
    {
        public DbSet<Cliente> Clientes { get; set; }

        public DbSet<Tarefa> Tarefas { get; set; }

        public GerenciadorContext(DbContextOptions<GerenciadorContext> options) : base(options)
        {

        }
    }
}