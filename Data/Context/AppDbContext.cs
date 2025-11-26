using Microsoft.EntityFrameworkCore;
using SimuladorCajero.Models;

namespace SimuladorCajero.Data.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Cuenta> Cuentas { get; set; }
        public DbSet<Ahorros> Ahorros { get; set; }
        public DbSet<Corriente> Corrientes { get; set; }
        public DbSet<TarjetaCredito> TarjetasCredito { get; set; }
        public DbSet<Transaccion> Transacciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 🔹 Herencia TPH (Table Per Hierarchy) para cuentas
            modelBuilder.Entity<Cuenta>()
                .HasDiscriminator<string>("TipoCuenta")
                .HasValue<Ahorros>("Ahorros")
                .HasValue<Corriente>("Corriente");

            // 🔹 Relación Usuario 1:1 Cuenta
            modelBuilder.Entity<Usuario>()
                .HasOne<Cuenta>(u => u.Cuenta)
                .WithOne(c => c.Usuario)
                .HasForeignKey<Cuenta>(c => c.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            // 🔹 Relación Usuario 1:1 Tarjeta de Crédito
            modelBuilder.Entity<Usuario>()
                .HasOne<TarjetaCredito>(u => u.TarjetaCredito)
                .WithOne(tc => tc.Usuario)
                .HasForeignKey<TarjetaCredito>(tc => tc.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            // 🔹 Relación Cuenta 1:N Transacciones
            modelBuilder.Entity<Cuenta>()
                .HasMany(c => c.Transacciones)
                .WithOne(t => t.Cuenta)
                .HasForeignKey(t => t.CuentaId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
