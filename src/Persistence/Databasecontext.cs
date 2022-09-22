using Microsoft.EntityFrameworkCore;
using src.Models;

namespace src.Persistence;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext>options) : base(options)
    {
        
    }
    public DbSet<Pessoa> pessoas { get; set; }
    public DbSet<Contrato> contratos { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Pessoa>(tabela => {
            tabela.HasKey(e => e.Id);
            tabela
                .HasMany(e => e.contratos)
                .WithOne()
                .HasForeignKey(c => c.PessoaId);
        });

        builder.Entity<Contrato>(tabela => {
            tabela.HasKey(e => e.Id);
        });
    }
}