using System;
using System.Collections.Generic;
using System.Text;

namespace NinaPOS.Models
{
    public class Producto
    {
        public int Id { get; set; }
        public string CodigoBarras { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; } // IVA incluido
        public string Categoria { get; set; } = string.Empty;
    }
}
