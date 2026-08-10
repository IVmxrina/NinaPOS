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
}