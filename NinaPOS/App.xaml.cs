using Microsoft.Extensions.DependencyInjection;
using NinaPOS.Views;

namespace NinaPOS
{
    public partial class App : Application
    {

        private readonly IServiceProvider _services;

        public App(IServiceProvider services)
        {
            InitializeComponent();
            _services = services;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(_services.GetRequiredService<Views.LoginPage>());
        }
    }
}