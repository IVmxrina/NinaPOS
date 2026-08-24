using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NinaPOS.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace NinaPOS.ViewModels;

public partial class TicketViewModel : ObservableObject
{
    private readonly NinaPosDbContext _db;
    private readonly IServiceProvider _services;

    public TicketViewModel(NinaPosDbContext db, IServiceProvider services)
    {
        _db = db;
        _services = services;
    }

    public INavigation? Navigation { get; set; }
    public Page? CurrentPage { get; set; }

    public ObservableCollection<TicketItem> Items { get; } = new();

    public bool CantidadMinima;

    [ObservableProperty]
    private string codigoIngresado = string.Empty;

    [ObservableProperty]
    private int cantidadPrevia = 1;

    [ObservableProperty]
    private decimal total = 0;

    [ObservableProperty]
    private TicketItem? itemSeleccionado;

    [RelayCommand]
    private async Task Cobrar()
    {
        if (Navigation is null || Items.Count == 0) return;

        var cobroVm = ActivatorUtilities.CreateInstance<CobroViewModel>(_services, Items);
        cobroVm.Navigation = Navigation;

        var cobroPage = ActivatorUtilities.CreateInstance<Views.CobroPage>(_services, cobroVm);

        await Navigation.PushModalAsync(cobroPage);
    }

    [RelayCommand]
    private void EscanearProducto()
    {
        if (string.IsNullOrWhiteSpace(CodigoIngresado))
            return;

        int cantidad = 1;
        string codigo = CodigoIngresado.Trim();

        var partes = codigo.Split('x', StringSplitOptions.RemoveEmptyEntries);
        if (partes.Length == 2 && int.TryParse(partes[0], out var cantidadParseada) && cantidadParseada > 0)
        {
            cantidad = cantidadParseada;
            codigo = partes[1];
        }
        else if (partes.Length == 1)
        {
            cantidad = 1;
            codigo = partes[0];
        }

        var producto = _db.Productos.FirstOrDefault(p => p.CodigoBarras == codigo);

        if (producto is null)
        {
            CodigoIngresado = string.Empty;
            return;
        }

        var existente = Items.FirstOrDefault(i => i.ProductoId == producto.Id);
        if (existente is not null)
        {
            existente.Cantidad += cantidad;
        }
        else
        {
            Items.Add(new TicketItem
            {
                ProductoId = producto.Id,
                Nombre = producto.Nombre,
                PrecioUnitario = producto.Precio,
                Cantidad = cantidad
            });
        }

        CodigoIngresado = string.Empty;
        CantidadPrevia = 1;
        RecalcularTotal();
    }

    [RelayCommand]
    private void DisminuirCantidad(TicketItem item)
    {
        if (item.Cantidad == 1)
        {
            Debug.WriteLine("TODO: deshabilitar boton");
        }   
        else
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