using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NinaPOS.Models;
using NinaPOS.Services;

namespace NinaPOS.ViewModels;

public partial class CobroViewModel : ObservableObject
{
    private readonly NinaPosDbContext _db;
    private readonly SesionActual _sesion;

//CONTRUCTOR
    public CobroViewModel(NinaPosDbContext db, SesionActual sesion, ObservableCollection<TicketItem> items)
    {
        _db = db;
        _sesion = sesion;
        Items = items;
    }

//VARIABLES
    //Conjunto de items del ticket
    public ObservableCollection<TicketItem> Items { get; }

    //Navegacion entre paginas
    public INavigation? Navigation { get; set; }

    //Cantidad de dinero total del ticket
    public decimal TotalAPagar => Items.Sum(i => i.Subtotal);

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Cambio))]
    [NotifyPropertyChangedFor(nameof(PuedeConfirmar))]
    private decimal cantidadEfectivo;
    //TODO: arreglar este warning

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Cambio))]
    [NotifyPropertyChangedFor(nameof(PuedeConfirmar))]
    private decimal cantidadTarjeta;
    //TODO: arreglar este warning

    //Una vez se ha cobrado con efectivo se indica el cambio a dar
    public decimal Cambio => Math.Max(0, (CantidadEfectivo + CantidadTarjeta) - TotalAPagar);

    //Variable que obliga a introducir una cantidad de dinero antes de poder cobrar (evitar cobrar 0)
    public bool PuedeConfirmar => (CantidadEfectivo + CantidadTarjeta) >= TotalAPagar && Items.Count > 0;


    //METODOS

    [RelayCommand]
    private async Task PagoConTarjeta()
    {
        // CORREGIDO: Eliminamos 'PuedeConfirmar' porque con tarjeta cobramos directamente el total del ticket si hay items
        if (Items.Count == 0 || _sesion.UsuarioLogueado is null || Navigation is null)
        {
            return;
        }

        try
        {
            // Añadimos la transacción directamente con el TotalAPagar completo
            _db.Transacciones.Add(new Transaccion
            {
                Fecha = DateTime.Now,
                Total = TotalAPagar,
                CantidadEfectivo = 0,
                CantidadTarjeta = TotalAPagar,
                UsuarioId = _sesion.UsuarioLogueado.Id
            });

            // CORREGIDO: Guardamos los cambios en la base de datos (faltaba el SaveChanges en tu método original)
            _db.SaveChanges();
            Debug.WriteLine("Todo kul con la tarjeta (Alerta Nativa)");

            // Finalizamos la transacción limpiando el carrito y cerrando la página
            Items.Clear();
            await Navigation.PopModalAsync();
        }
        catch (Exception e)
        {
            Debug.WriteLine("Error al introducir la transacción de tarjeta a la BD: " + e.Message);
        }
    }


    [RelayCommand]
    private async Task GenerarTransaccion(string metodoPago)
    {
        //Comprueba que los componentes necesarios para el cobro (cantidad, usuario y navegacion) estan disponibles y funcionales
        if (!PuedeConfirmar || _sesion.UsuarioLogueado is null || Navigation is null)
        {
            return;
        }
            
        //Genera una nueva transaccion para meterlo en la BD
        try
        {
            //TODO: si el pago es con tarjeta no debe hacer falta poner el importe en el input (solo clic en el boton)
            if (metodoPago == "tarjeta")
            {
                _db.Transacciones.Add(new Transaccion
                {
                    Fecha = DateTime.Now,
                    Total = TotalAPagar,
                    CantidadEfectivo = 0,
                    CantidadTarjeta = CantidadTarjeta,
                    UsuarioId = _sesion.UsuarioLogueado.Id
                });
                _db.SaveChanges();
                Debug.WriteLine("Todo kul con la tarjeta");
            }
            else if (metodoPago == "efectivo")
            {
                _db.Transacciones.Add(new Transaccion
                {
                    Fecha = DateTime.Now,
                    Total = TotalAPagar,
                    CantidadEfectivo = CantidadEfectivo,
                    CantidadTarjeta = 0,
                    UsuarioId = _sesion.UsuarioLogueado.Id
                });
                _db.SaveChanges();
                Debug.WriteLine("Todo kul con el efectivo");
            }
        } 
        catch(Exception e)
        {
            Debug.WriteLine("Error al introducir la transaccion a la BD: " + e.Message);
        }


        Items.Clear();
        await Navigation.PopModalAsync();
    }

    //Metodo de navegacion (pagina de cobro a pagina de escaneo)
    [RelayCommand]
    private async Task VolverATicket()
    {
        if (Navigation is null) return;
        await Navigation.PopModalAsync();
    }
}