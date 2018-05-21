using System.Reflection;
using Autofac;
using Blotter.Client.Views;
using Blotter.Infrastructure;
using Blotter.Services;
using Dragablz;
using ReactiveUI.Autofac;
using Splat;

namespace Blotter.Client.Infrastructure
{
    internal class AppRegistry : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            //Set up logging ... not yet.

            builder.RegisterType<SchedulerProvider>().As<ISchedulerProvider>().SingleInstance();
            
            builder.RegisterType<ViewContainer>().As<IViewContainer>();
            builder.RegisterType<RxUiHostViewModel>();
            builder.RegisterType<RxUiViewModel>();
            
            builder.RegisterType<MenuBuilder>().AsSelf();
            builder.RegisterType<WindowFactory>().As<IWindowFactory>();
            builder.RegisterType<InterTabClient>().As<IInterTabClient>();
            builder.RegisterType<WindowViewModel>().AsSelf();
            
            builder.RegisterForReactiveUI(Assembly.GetExecutingAssembly());

            // Service Module
            builder.RegisterModule<ServiceModule>();
        }
    }
}