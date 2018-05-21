using Autofac;

namespace Blotter.Services
{
    public class ServiceModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<StaticData>().As<IStaticData>().SingleInstance();
            builder.RegisterType<MarketDataService>().As<IMarketDataService>().SingleInstance();

            builder.RegisterType<TradeService>().As<ITradeService>().SingleInstance();
            builder.RegisterType<TradeGenerator>().AsSelf().SingleInstance();

            builder.RegisterType<TradePriceUpdateJob>().AsSelf().SingleInstance().AutoActivate(); // starts this job when the dependency map is Build()
        }
    }
}