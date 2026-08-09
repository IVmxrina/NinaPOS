using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using NinaPOS.Models;

namespace NinaPOS;

// Usada SOLO por "dotnet ef" al generar migraciones desde la terminal.
// La app real nunca llama a esta clase — sigue usando NinaPosDbContext.DbPath
// (FileSystem.AppDataDirectory) normalmente al ejecutarse de verdad.
public class NinaPosDbContextFactory : IDesignTimeDbContextFactory<NinaPosDbContext>
{
    public NinaPosDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<NinaPosDbContext>();

        // Ruta fija cualquiera, solo para que EF Core pueda inspeccionar
        // el esquema y generar el archivo de migración — no se usa en runtime.
        optionsBuilder.UseSqlite("Data Source=design-time.db");

        return new NinaPosDbContext(optionsBuilder.Options);
    }
}