using System.Collections.ObjectModel;
using System.Windows.Input;
using NinaPOS.Models;

namespace NinaPOS.Views.Controls;

public partial class ListaTicket : ContentView
{
    public static readonly BindableProperty ItemsProperty =
        BindableProperty.Create(nameof(Items), typeof(ObservableCollection<TicketItem>), typeof(ListaTicket));

    public static readonly BindableProperty EditableProperty =
        BindableProperty.Create(nameof(Editable), typeof(bool), typeof(ListaTicket), defaultValue: true);

    public static readonly BindableProperty AumentarCantidadCommandProperty =
        BindableProperty.Create(nameof(AumentarCantidadCommand), typeof(ICommand), typeof(ListaTicket));

    public static readonly BindableProperty DisminuirCantidadCommandProperty =
        BindableProperty.Create(nameof(DisminuirCantidadCommand), typeof(ICommand), typeof(ListaTicket));

    public static readonly BindableProperty EliminarItemCommandProperty =
        BindableProperty.Create(nameof(EliminarItemCommand), typeof(ICommand), typeof(ListaTicket));

    public ObservableCollection<TicketItem> Items
    {
        get => (ObservableCollection<TicketItem>)GetValue(ItemsProperty);
        set => SetValue(ItemsProperty, value);
    }

    public bool Editable
    {
        get => (bool)GetValue(EditableProperty);
        set => SetValue(EditableProperty, value);
    }

    public ICommand AumentarCantidadCommand
    {
        get => (ICommand)GetValue(AumentarCantidadCommandProperty);
        set => SetValue(AumentarCantidadCommandProperty, value);
    }

    public ICommand DisminuirCantidadCommand
    {
        get => (ICommand)GetValue(DisminuirCantidadCommandProperty);
        set => SetValue(DisminuirCantidadCommandProperty, value);
    }

    public ICommand EliminarItemCommand
    {
        get => (ICommand)GetValue(EliminarItemCommandProperty);
        set => SetValue(EliminarItemCommandProperty, value);
    }

    public ListaTicket()
    {
        InitializeComponent();
    }
}