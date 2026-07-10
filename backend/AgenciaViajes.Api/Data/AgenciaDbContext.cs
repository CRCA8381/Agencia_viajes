using AgenciaViajes.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace AgenciaViajes.Api.Data;

public class AgenciaDbContext(DbContextOptions<AgenciaDbContext> options) : DbContext(options)
{
    public DbSet<Destino> Destinos => Set<Destino>();
    public DbSet<Paquete> Paquetes => Set<Paquete>();
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Reserva> Reservas => Set<Reserva>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>().HasIndex(x => x.Documento).IsUnique();
        modelBuilder.Entity<Cliente>().HasIndex(x => x.Email).IsUnique();
        modelBuilder.Entity<Paquete>().HasOne(x => x.Destino).WithMany(x => x.Paquetes)
            .HasForeignKey(x => x.DestinoId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Reserva>().HasOne(x => x.Cliente).WithMany(x => x.Reservas)
            .HasForeignKey(x => x.ClienteId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Reserva>().HasOne(x => x.Paquete).WithMany(x => x.Reservas)
            .HasForeignKey(x => x.PaqueteId).OnDelete(DeleteBehavior.Restrict);
    }
}
