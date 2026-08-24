using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NinaPOS.Models;
using NinaPOS.Services;
using NinaPOS.ViewModels;
namespace NinaPOS
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddDbContext<NinaPosDbContext>();
            builder.Services.AddTransient<CounterViewModel>();
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<TicketViewModel>();
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<Views.LoginPage>();
            builder.Services.AddSingleton<SesionActual>();

            var app = builder.Build();

            // Crear la BD y sembrar datos de prueba en el primer arranque
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<NinaPosDbContext>();
                //System.Diagnostics.Debug.WriteLine($"[NinaPOS] Ruta de la BD: {NinaPosDbContext.DbPath}");
                db.Database.Migrate();

                
                  /*  db.Productos.AddRange(
                       // new Producto { CodigoBarras = "7501234567890", Nombre = "Leche Entera 1L", Precio = 1.25m, Categoria = "Lácteos" },
                      //  new Producto { CodigoBarras = "7501234567891", Nombre = "Pan de Molde", Precio = 2.10m, Categoria = "Panadería" },
                       // new Producto { CodigoBarras = "7501234567892", Nombre = "Manzanas (kg)", Precio = 1.80m, Categoria = "Frutas" },
                       // new Producto { CodigoBarras = "7501234567893", Nombre = "Bolsa", Precio = 0.15m, Categoria = "Otros" }
                    );
                db.Usuarios.AddRange(
                    // new Usuario { CodigoEmpleado = "C001", Contrasena = "1234", Nombre = "Cajero Demo", Rol = RolUsuario.Cajero },
                   // new Usuario { CodigoEmpleado = "M2003", Contrasena = "9999", Nombre = "Marina Cue", Rol = RolUsuario.Gerente }
                      //  new Usuario { CodigoEmpleado = "G001", Contrasena = "9999", Nombre = "Gerente Demo", Rol = RolUsuario.Gerente }
                );*/
                    db.SaveChanges();
                    
                
            }

            return app;

        }
    }
}
