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
            var usuario = _db.Usuarios.FirstOrDefault(u => u.CodigoEmpleado == codigoEmpleado && u.Contrasena == contrasena);

            if (usuario is null)
            {
                mensajeError = "Credenciales incorrectas :p.";
                contrasena = string.Empty;
                return;
            }

            _sesion.UsuarioLogueado = usuario;

            Application.Current!.MainPage = _services.GetService<MainPage>();
        }
    }
}
