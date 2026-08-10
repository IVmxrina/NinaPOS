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
        public decimal CantidadEfectivo { get; set; }
        public decimal CantidadTarjeta { get; set; }
        public int UsuarioId { get; set; }
    }
}
