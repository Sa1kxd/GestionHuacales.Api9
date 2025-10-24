using Microsoft.EntityFrameworkCore;
using GestionHuacales.Api9.Models;
namespace GestionHuacales.Api9.DATA;

public class Contexto : DbContext
{
    public Contexto(DbContextOptions<Contexto> options) : base(options)
    {
    }
    public DbSet<EntradasHuacales> EntradasHuacales { get; set; }
    public DbSet<TiposHuacales> TiposHuacales { get; set; }
    public DbSet<EntradasHuacalesDetalle> EntradasHuacalesDetalle { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TiposHuacales>().HasData(
            new List<TiposHuacales>
            {
                new()
                {
                    TipoId = 1,
                    Descripcion = "Verde",
                    Existencia = 0
                },
                new()
                {
                    TipoId = 2,
                    Descripcion = "Rojo",
                    Existencia = 0
                }

            }
        );
        base.OnModelCreating(modelBuilder);
    }
}
