using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NinaPOS.Models;
using NinaPOS.Services;

namespace NinaPOS.ViewModels;

public partial class CobroViewModel : ObservableObject
{
    private readonly NinaPosDbContext _db;
    private readonly SesionActual _sesion;

    public decimal TotalAPagar { get; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Cambio))]
    [NotifyPropertyChangedFor(nameof(FaltaPorCubrir))]
    [NotifyPropertyChangedFor(nameof(PuedeConfirmar))]
    private decimal cantidadEfectivo;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Cambio))]
    [NotifyPropertyChangedFor(nameof(FaltaPorCubrir))]
    [NotifyPropertyChangedFor(nameof(PuedeConfirmar))]
    private decimal cantidadTarjeta;

    // RF-08: cambio a devolver — nunca negativo, se calcula solo
    public decimal Cambio => Math.Max(0, (CantidadEfectivo + CantidadTarjeta) - TotalAPagar);

    // Cuánto falta para cubrir el total (0 si ya está cubierto o superado)
    public decimal FaltaPorCubrir => Math.Max(0, TotalAPagar - (CantidadEfectivo + CantidadTarjeta));

    // El botón de confirmar solo se habilita cuando el pago cubre el total
    public bool PuedeConfirmar => (CantidadEfectivo + CantidadTarjeta) >= TotalAPagar;

    public bool VentaConfirmada { get; private set; } = false;

    public CobroViewModel(NinaPosDbContext db, SesionActual sesion, decimal totalAPagar)
    {
        _db = db;
        _sesion = sesion;
        TotalAPagar = totalAPagar;
    }

    [RelayCommand]
    private void ConfirmarPago()
    {
        if (!PuedeConfirmar || _sesion.UsuarioLogueado is null)
            return;

        var transaccion = new Transaccion
        {
            Fecha = DateTime.Now,
            Total = TotalAPagar,
            CantidadEfectivo = CantidadEfectivo,
            CantidadTarjeta = CantidadTarjeta,
            UsuarioId = _sesion.UsuarioLogueado.Id
        };

        _db.Transacciones.Add(transaccion);
        _db.SaveChanges();

        VentaConfirmada = true;
    }
}