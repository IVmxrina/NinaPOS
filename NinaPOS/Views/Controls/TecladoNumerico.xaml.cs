using NinaPOS.ViewModels;

namespace NinaPOS.Views.Controls;

public partial class TecladoNumerico : ContentView
{
    

    public static readonly BindableProperty TextoProperty =
        BindableProperty.Create(nameof(TextoProperty), typeof(string), typeof(TecladoNumerico), string.Empty, BindingMode.TwoWay);

    public string Texto
    {
        get => (string)GetValue(TextoProperty);
        set => SetValue(TextoProperty, value);
    }
    
    public TecladoNumerico()
    {
        InitializeComponent();
    }

    public void OnDigitClicked(object sender, EventArgs e)
    {
        if (sender is not Button boton) return;
        Texto += boton.Text;
    }

    private void OnBorrarClicked(object sender, EventArgs e)
    {
        Texto = string.Empty;
    }

    private void OnBorrarUltimoClicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Texto))
        { 
            Texto = Texto[..^1];
        }
    }

    private void MultiplicarCantidad(object sender, EventArgs e)
    {
        var vm = (TicketViewModel)BindingContext;

        //If para evitar que se añada el multiplicador mas de una vez
        if (Texto.Contains('x') || Texto.Length == 0)
            return;

        Texto += "x";
    }

    private void CobroTarjetaClicked(object sender, EventArgs e)
    {
        
    }
    


}