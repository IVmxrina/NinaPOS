using NinaPOS.ViewModels;

namespace NinaPOS.Views;

public partial class CobroPage : ContentPage
{
    public CobroPage(CobroViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    public void OnTarjetaClicked(object sender, EventArgs e)
    {
        if (sender is not Button boton) return;
    }

    public async void OnEfectivoClicked(object sender, EventArgs e)
    {
        if (BindingContext is CobroViewModel viewModel)
        {
            try
            {
                // Se muestra la alerta
                bool confirmarPago = await this.DisplayAlertAsync(
                    "Simulación de Pago",
                    $"¿Desea simular el pago con efectivo por un total de {viewModel.TotalAPagar:C2}?",
                    "Sí, Confirmar",
                    "No, Cancelar"
                );

                // Si se acepta se procede al cobro
                if (confirmarPago)
                {
                    // CORREGIDO: Validamos y ejecutamos PagoConEfectivoCommand usando ExecuteAsync
                    if (viewModel.PagoConEfectivoCommand.CanExecute(null))
                    {
                        await viewModel.PagoConEfectivoCommand.ExecuteAsync(null);
                    }
                }
            }
            catch (Exception ex)
            {
                // Control de errores por si ocurre algun fallo inesperado en la interfaz
                await this.DisplayAlertAsync("Error", $"No se pudo procesar la acción: {ex.Message}", "OK");
            }

        }
    }

    public void OnCuponClicked(object sender, EventArgs e)
    {
        if (sender is not Button boton) return;
    }

}