using Autofac;
using System.Windows;
using Blotter.Client.Infrastructure;
using ReactiveUI.Autofac;

namespace Blotter.Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>Initializes a new instance of the <see cref="T:System.Windows.Application" /> class.</summary>
        /// <exception cref="T:System.InvalidOperationException">More than one instance of the <see cref="T:System.Windows.Application" /> class is created per <see cref="T:System.AppDomain" />.</exception>
        public App()
        {
            ShutdownMode = ShutdownMode.OnLastWindowClose;
        }

        /// <inheritdoc />
        /// <summary>Raises the <see cref="E:System.Windows.Application.Startup" /> event.</summary>
        /// <param name="e">A <see cref="T:System.Windows.StartupEventArgs" /> that contains the event data.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {
                var builder = new ContainerBuilder();
                builder.RegisterModule<AppRegistry>();

                IContainer container = builder.Build();
                RxAppAutofacExtension.UseAutofacDependencyResolver(container);
                
                var factory = container.Resolve<IWindowFactory>();
                var window = factory.Create(true);
                //window.Dispatcher => //System.Windows.Application.Current.Dispatcher

                // Run start up jobs
                // var tradePriceUpdater = container.Resolve<TradePriceUpdateJob>(); // Set to AutoActivate() at registration.

                window.Show();    
            }
            catch (System.Exception ex)
            {
                
            }
        }
    }
}