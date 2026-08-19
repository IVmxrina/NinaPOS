using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NinaPOS.Models;
using Microsoft.Extensions.DependencyInjection;


namespace NinaPOS.ViewModels;

public partial class TicketViewModel : ObservableObject
{
    private readonly NinaPosDbContext _db;
    private readonly IServiceProvider _services;

    public INavigation? Navigation { get; set; }
    public Page? CurrentPage { get; set; }

    public TicketViewModel(NinaPosDbContext db, IServiceProvider services)
    {
        _db = db;
        _services = services;
    }

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

    [RelayCommand]
    private async Task Cobrar()
    {
        if (Navigation is null || Items.Count == 0) return;

        var cobroVm = ActivatorUtilities.CreateInstance<CobroViewModel>(_services, Total);
        var cobroPage = ActivatorUtilities.CreateInstance<Views.CobroPage>(_services, cobroVm);

        await Navigation.PushModalAsync(cobroPage);

        if (cobroVm.VentaConfirmada)
        {
            Items.Clear();
            RecalcularTotal();
        }
    }

    // RF-02: entrada de producto por codigo 
    [RelayCommand]
    private void EscanearProducto()
    {
        // Si viene vacio no hace nada
        if (string.IsNullOrWhiteSpace(CodigoIngresado))
            return;

        int cantidad = 1;
        string codigo = CodigoIngresado.Trim();

        // Si se introduce NxCodigo, separa el numero y el codigo y los multiplica.
        var partes = codigo.Split('x', StringSplitOptions.RemoveEmptyEntries);
        if(partes.Length == 2 && int.TryParse(partes[0], out var cantidadParseada) && cantidadParseada > 0)
        {
            cantidad = cantidadParseada;
            codigo = partes[1];
        }  
        else if (partes.Length == 1) //Si se introduce xN, tomará ese codigo como correcto y añadira el producto en cantidad 1
        {
            cantidad = 1;
            codigo = partes[0];
        }

        // Sacamos de la base de datos el codigo del producto si este coincide
        var producto = _db.Productos
            .FirstOrDefault(p => p.CodigoBarras == CodigoIngresado);

        // Si el producto es null acaba la accion
        if (producto is null)
        {
            // TODO: esto disparara un aviso visual en pantalla
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
            // Se añade un item mas al ticket
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