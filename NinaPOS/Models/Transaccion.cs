using System;
using System.Collections.Generic;
using System.Text;

namespace NinaPOS.Models
{
    public class Transaccion
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public string MetodoPago { get; set; } = string.Empty;
        public int UsuarioId { get; set; }
    }
}
