using NinaPOS.ViewModels;

namespace NinaPOS;

public partial class MainPage : ContentPage
{
    public MainPage(TicketViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        vm.Navigation = Navigation;
        vm.CurrentPage = this;
    }
}