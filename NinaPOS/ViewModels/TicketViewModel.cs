using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NinaPOS.Models;

namespace NinaPOS.ViewModels;

public partial class TicketViewModel : ObservableObject
{
    private readonly NinaPosDbContext _db;

    // ObservableCollection act automatica
    public ObservableCollection<TicketItem> Items { get; } = new();

    [ObservableProperty]
    private string codigoIngresado = string.Empty;

    [ObservableProperty]
    private int cantidadPrevia = 1; // RF-03 -> multiplicador antes del escaneo

    [ObservableProperty]
    private decimal total = 0;

    [ObservableProperty]
    private TicketItem? itemSeleccionado; // eliminarlo (RF-10)

    public TicketViewModel(NinaPosDbContext db)
    {
        _db = db;
    }

    // RF-02: entrada de producto por codigo 
    [RelayCommand]
    private void EscanearProducto()
    {
        if (string.IsNullOrWhiteSpace(CodigoIngresado))
            return;

        var producto = _db.Productos
            .FirstOrDefault(p => p.CodigoBarras == CodigoIngresado);

        if (producto is null)
        {
            // En la Fase 4 esto disparara un aviso visual en pantalla;
            CodigoIngresado = string.Empty;
            return;
        }

        // Si el producto ya esta en el ticket, solo suma cantidad
        var existente = Items.FirstOrDefault(i => i.ProductoId == producto.Id);
        if (existente is not null)
        {
            existente.Cantidad += CantidadPrevia;
        }
        else
        {
            Items.Add(new TicketItem
            {
                ProductoId = producto.Id,
                Nombre = producto.Nombre,
                PrecioUnitario = producto.Precio,
                Cantidad = CantidadPrevia
            });
        }

        CodigoIngresado = string.Empty;
        CantidadPrevia = 1; // el multiplicador se resetea tras cada escaneo
        RecalcularTotal();
    }

    [RelayCommand]
    private void DisminuirCantidad(TicketItem item)
    {
        if(item.Cantidad >= 1)
        { 
            item.Cantidad--;
        }

        RecalcularTotal();
    }
    [RelayCommand]
    private void AumentarCantidad(TicketItem item)
    {
        item.Cantidad++;
        
        RecalcularTotal();
    }


    // RF-10: eliminar un articulo ya escaneado
    [RelayCommand]
    private void EliminarItem(TicketItem item)
    {
        Items.Remove(item);
        RecalcularTotal();
    }



    private void RecalcularTotal()
    {
        Total = Items.Sum(i => i.Subtotal);
    }
}