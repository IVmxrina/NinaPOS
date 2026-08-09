using System;
using System.Collections.Generic;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;

namespace NinaPOS.Models;

public partial class TicketItem : ObservableObject
{
    public int ProductoId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public decimal PrecioUnitario { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Subtotal))] //Asegura actualizacion
    private int cantidad = 1;

    // Propiedad calculada: se recalcula sola cada vez que la lee la UI
    public decimal Subtotal => PrecioUnitario * cantidad;
}
