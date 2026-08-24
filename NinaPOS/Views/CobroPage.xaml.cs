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

    public void OnEfectivoClicked(object sender, EventArgs e)
    {
        if (sender is not Button boton) return;
    }

    public void OnCuponClicked(object sender, EventArgs e)
    {
        if (sender is not Button boton) return;
    }

}