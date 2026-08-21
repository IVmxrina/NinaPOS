using NinaPOS.ViewModels;

namespace NinaPOS.Views.Controls;

public partial class TecladoNumerico : ContentView
{
    public TecladoNumerico()
    {
        InitializeComponent();
    }
    public void OnDigitClicked(object sender, EventArgs e)
    {
        if (sender is not Button boton) return;
        var vm = (TicketViewModel)BindingContext;
        vm.CodigoIngresado += boton.Text;
    }

    private void OnBorrarClicked(object sender, EventArgs e)
    {
        var vm = (TicketViewModel)BindingContext;
        vm.CodigoIngresado = string.Empty;
    }

    private void OnBorrarUltimoClicked(object sender, EventArgs e)
    {
        var vm = (TicketViewModel)BindingContext;
        if (vm.CodigoIngresado.Length > 0)
            vm.CodigoIngresado = vm.CodigoIngresado[..^1];
    }

    private void MultiplicarCantidad(object sender, EventArgs e)
    {
        var vm = (TicketViewModel)BindingContext;

        //If para evitar que se añada el multiplicador mas de una vez
        if (vm.CodigoIngresado.Contains('x') || vm.CodigoIngresado.Length == 0)
            return;

        vm.CodigoIngresado += "x";
    }

}