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
    public partial decimal CantidadIntroducida { get; set; }

    [ObservableProperty]
    public partial decimal CantidadTarjeta { get; set; }

    [ObservableProperty]
    public partial decimal CantidadEfectivo { get; set; }

    [ObservableProperty]
    public partial decimal CantidadRestante {  get; set; }

    [ObservableProperty]
    private bool anularVisible;



    //METODOS

    [RelayCommand]
    private async Task Pago(string metodoPago)
    {
        if (Items.Count == 0 || _sesion.UsuarioLogueado is null || Navigation is null)
        {
            return;
        }

        //Si la cantidad introducida es menor que el total a pagar o es menor o igual a la cantidad restante se mete para proceder al pago dividido
        if (CantidadIntroducida < TotalAPagar || CantidadIntroducida <= CantidadRestante)
        {
            // if que determina como retirar del monto total
            if (CantidadRestante == 0)
            {
                CantidadRestante = TotalAPagar - CantidadIntroducida;
            }
            else
            {
                CantidadRestante = CantidadRestante - CantidadIntroducida;
            }

            //if que determina donde se almacena ese monto retirado
            if (metodoPago == "tarjeta")
            {
                CantidadTarjeta = CantidadIntroducida + CantidadTarjeta;
            }
            else if (metodoPago == "efectivo")
            {
                CantidadEfectivo = CantidadIntroducida + CantidadEfectivo;
            } 
            else
            {
                Debug.WriteLine("Problema con el parametro de tipo de pago");
            }

            AnularVisible = true;
        }

        Debug.WriteLine("Cantidad con tarjeta: " + CantidadTarjeta + "\n" + "Cantidad con efectivo: " + CantidadEfectivo);

        if (CantidadIntroducida == TotalAPagar || CantidadRestante == 0)
        {
            try
            {
                // Añadimos la transaccion directamente con el TotalAPagar completo
                _db.Transacciones.Add(new Transaccion
                {
                    Fecha = DateTime.Now,
                    Total = TotalAPagar,
                    CantidadEfectivo = CantidadEfectivo,
                    CantidadTarjeta = CantidadTarjeta,
                    UsuarioId = _sesion.UsuarioLogueado.Id
                });

                // CORREGIDO: Guardamos los cambios en la base de datos (faltaba el SaveChanges en tu método original)
                _db.SaveChanges();
                Debug.WriteLine("Todo kul con la tarjeta (Alerta Nativa)");

                // Finalizamos la transacción limpiando el carrito y cerrando la página
                AnularVisible = false;
                Items.Clear();
                await Navigation.PopModalAsync();
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error al introducir la transacción de tarjeta a la BD: " + e.Message);
            }
        }
    }

    [RelayCommand]
    private async Task PagoConTarjeta()
    {
        if (Items.Count == 0 || _sesion.UsuarioLogueado is null || Navigation is null)
        {
            return;
        }


        //Si la cantidad introducida es menor que el total a pagar o es menor o igual a la cantidad restante se mete para proceder al pago dividido
        if (CantidadIntroducida < TotalAPagar || CantidadIntroducida <= CantidadRestante)
        {
            if (CantidadRestante == 0)
            {
                CantidadRestante = TotalAPagar - CantidadIntroducida;
            }
            else
            {
                CantidadRestante = CantidadRestante - CantidadIntroducida;
            }
            AnularVisible = true;
        }

        //Si no queda una cantidad restante se procede al fin del pago
        if (CantidadIntroducida == TotalAPagar || CantidadRestante == 0)
        {
            try
                    {
                        // Añadimos la transacción directamente con el TotalAPagar completo
                        _db.Transacciones.Add(new Transaccion
                        {
                            Fecha = DateTime.Now,
                            Total = TotalAPagar,
                            CantidadEfectivo = 0,
                            CantidadTarjeta = CantidadIntroducida,
                            UsuarioId = _sesion.UsuarioLogueado.Id
                        });

                        // CORREGIDO: Guardamos los cambios en la base de datos (faltaba el SaveChanges en tu método original)
                        _db.SaveChanges();
                        Debug.WriteLine("Todo kul con la tarjeta (Alerta Nativa)");

                // Finalizamos la transacción limpiando el carrito y cerrando la página
                AnularVisible = false;
                        Items.Clear();
                        await Navigation.PopModalAsync();
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("Error al introducir la transacción de tarjeta a la BD: " + e.Message);
                    }
        }

        
    }


    [RelayCommand]
    private async Task PagoConEfectivo(string metodoPago)
    {
        //Comprueba que los componentes necesarios para el cobro (cantidad, usuario y navegacion) estan disponibles y funcionales
        if ( _sesion.UsuarioLogueado is null || Navigation is null)
        {
            return;
        }

        Debug.WriteLine("Cantidad introducida: " + CantidadIntroducida);
        Debug.WriteLine("Total a pagar: " + TotalAPagar);

        if (CantidadIntroducida < TotalAPagar)
        {
            //TODO: añadir cobro dividido
            Debug.WriteLine("Queda por añadir el cobro dividido");
            await Navigation.PopModalAsync();
        }

        if (CantidadIntroducida == TotalAPagar)
        {
            //Genera una nueva transaccion para meterlo en la BD
                    try
                    {
                            _db.Transacciones.Add(new Transaccion
                            {
                                Fecha = DateTime.Now,
                                Total = TotalAPagar,
                                CantidadEfectivo = CantidadIntroducida,
                                CantidadTarjeta = 0,
                                UsuarioId = _sesion.UsuarioLogueado.Id
                            });
                            _db.SaveChanges();
                            Debug.WriteLine("Todo kul con el efectivo");
            
                    } 
                    catch(Exception e)
                    {
                        Debug.WriteLine("Error al introducir la transaccion a la BD: " + e.Message);
                    }


                    Items.Clear();
                    await Navigation.PopModalAsync();
        } 
        else
        {
            return;
        }

        
    }


    [RelayCommand]
    private async Task AnularTransaccion()
    {
        CantidadRestante = 0;
        CantidadTarjeta = 0;
        CantidadEfectivo = 0;
        AnularVisible = false;
    }

    //Metodo de navegacion (pagina de cobro a pagina de escaneo)
    [RelayCommand]
    private async Task VolverATicket()
    {
        if (Navigation is null) return;
        await Navigation.PopModalAsync();
    }
}