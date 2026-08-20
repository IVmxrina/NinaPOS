using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NinaPOS.Models;
using NinaPOS.Services;

namespace NinaPOS.ViewModels;

public partial class CobroViewModel : ObservableObject
{
    private readonly NinaPosDbContext _db;
    private readonly SesionActual _sesion;

    public ObservableCollection<TicketItem> Items { get; }
    public INavigation? Navigation { get; set; }

    public decimal TotalAPagar => Items.Sum(i => i.Subtotal);

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Cambio))]
    [NotifyPropertyChangedFor(nameof(PuedeConfirmar))]
    private decimal cantidadEfectivo;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Cambio))]
    [NotifyPropertyChangedFor(nameof(PuedeConfirmar))]
    private decimal cantidadTarjeta;

    public decimal Cambio => Math.Max(0, (CantidadEfectivo + CantidadTarjeta) - TotalAPagar);

    public bool PuedeConfirmar => (CantidadEfectivo + CantidadTarjeta) >= TotalAPagar && Items.Count > 0;

    public CobroViewModel(NinaPosDbContext db, SesionActual sesion, ObservableCollection<TicketItem> items)
    {
        _db = db;
        _sesion = sesion;
        Items = items;
    }

    [RelayCommand]
    private async Task ConfirmarPago()
    {
        if (!PuedeConfirmar || _sesion.UsuarioLogueado is null || Navigation is null)
            return;

        _db.Transacciones.Add(new Transaccion
        {
            Fecha = DateTime.Now,
            Total = TotalAPagar,
            CantidadEfectivo = CantidadEfectivo,  // ver nota abajo
            CantidadTarjeta = CantidadTarjeta,     // ver nota abajo
            UsuarioId = _sesion.UsuarioLogueado.Id
        });
        _db.SaveChanges();

        Items.Clear();
        await Navigation.PopModalAsync();
    }

    [RelayCommand]
    private async Task VolverATicket()
    {
        if (Navigation is null) return;
        await Navigation.PopModalAsync();
    }
}