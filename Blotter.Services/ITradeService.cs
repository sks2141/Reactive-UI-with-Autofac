using Blotter.Domain;
using DynamicData;

namespace Blotter.Services
{
    public interface ITradeService
    {
        IObservableCache<Trade, long> All { get; }
        IObservableCache<Trade, long> Live { get; }
    }
}
