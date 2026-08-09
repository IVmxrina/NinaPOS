using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NinaPOS.Models;
using NinaPOS.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace NinaPOS.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        public readonly NinaPosDbContext _db;
        public readonly SesionActual _sesion;
        public readonly IServiceProvider _services;

        [ObservableProperty]
        private string codigoEmpleado = string.Empty;

        [ObservableProperty]
        private string contrasena = string.Empty;

        [ObservableProperty]
        private string mensajeError = string.Empty;

        public LoginViewModel(NinaPosDbContext db, SesionActual sesion, IServiceProvider services)
        {
            _db = db;
            _sesion = sesion;
            _services = services;
        }

        [RelayCommand]
        private void IniciarSesion()
        {
            var usuario = _db.Usuarios.FirstOrDefault(u => u.CodigoEmpleado == CodigoEmpleado && u.Contrasena == Contrasena);

            if (usuario is null)
            {
                MensajeError = "Credenciales incorrectas :p.";
                Contrasena = string.Empty;
                return;
            }

            _sesion.UsuarioLogueado = usuario;

            if (Application.Current?.Windows.Count > 0)
            {
                Application.Current.Windows[0].Page = _services.GetRequiredService<MainPage>();
            }
        }
    }
}
