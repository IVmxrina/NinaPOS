using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace NinaPOS.Models
{
    public class NinaPosDbContext : DbContext
    {
        public NinaPosDbContext(DbContextOptions options) : base(options)
        {
        }

        public static string DbPath =>
        Path.Combine(FileSystem.AppDataDirectory, "ninapos.db");

        public DbSet<Producto> Productos => Set<Producto>();
        public DbSet<Cliente> Clientes => Set<Cliente>();
        public DbSet<Transaccion> Transacciones => Set<Transaccion>();
        public DbSet<Usuario> Usuarios => Set<Usuario>();

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
                options.UseSqlite($"Data Source={DbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Producto>()
                .HasIndex(p => p.CodigoBarras)
                .IsUnique();

            modelBuilder.Entity<Cliente>()
                .HasIndex(c => c.DNI)
                .IsUnique();
        }
    }
}
