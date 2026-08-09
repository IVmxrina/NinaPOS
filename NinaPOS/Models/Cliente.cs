using System;
using System.Collections.Generic;
using System.Text;

namespace NinaPOS.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string DNI { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Residencia { get; set; } = string.Empty;
    }
}
