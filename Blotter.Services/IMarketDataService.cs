using System;
using Blotter.Domain;

namespace Blotter.Services
{
    public interface IMarketDataService
    {
        IObservable<MarketData> Watch(string currencyPair);
    }
}
