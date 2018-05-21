using Blotter.Domain;

namespace Blotter.Services
{
    public interface IStaticData
    {
        string[] Customers { get; }
        CurrencyPair[] CurrencyPairs { get; }
    }
}
