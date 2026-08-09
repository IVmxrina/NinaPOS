using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore.Infrastructure;
using NinaPOS.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NinaPOS.ViewModels
{
    public partial class CounterViewModel : ObservableObject
    {
        public readonly NinaPosDbContext _db;

        [ObservableProperty]
        private int count = 0;

        [ObservableProperty]
        private string productosInfo = string.Empty;

        public string CountText => Count == 1 ? "Clicked 1 time" : $"Clicked {Count} times";

        public CounterViewModel(NinaPosDbContext db)
        {
            _db = db;
            var total = _db.Productos.Count();
            ProductosInfo = $"Productos en BD: {total}";
        }

        [RelayCommand]
        private void Increment()
        {
            Count++;
            OnPropertyChanged(nameof(CountText));
        }
    }
}
