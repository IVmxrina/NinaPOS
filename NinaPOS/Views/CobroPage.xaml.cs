using NinaPOS.ViewModels;

namespace NinaPOS.Views;

public partial class CobroPage : ContentPage
{
    public CobroPage(CobroViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    private async void OnCancelarClicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }

    private async void OnConfirmarClicked(object sender, EventArgs e)
    {
        var vm = (CobroViewModel)BindingContext;
        vm.ConfirmarPagoCommand.Execute(null); // ejecuta la lógica del ViewModel
        if (vm.VentaConfirmada)
        {
            await Navigation.PopModalAsync(); // el code-behind decide cuándo cerrar
        }
    }
}