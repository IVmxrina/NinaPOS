using System;
using System.Collections.Generic;
using System.Text;

namespace NinaPOS.Models
{

    public enum RolUsuario { Cajero, Gerente, Administrador }
    public class Usuario
    {
        public int Id { get; set; }
        public string CodigoEmpleado { get; set; } = string.Empty;
        public string Contrasena { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public RolUsuario Rol { get; set; }
    }
}
