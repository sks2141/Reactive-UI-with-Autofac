using System;
using System.Reactive.Linq;
using Blotter.Domain;
using Blotter.Services;
using DynamicData;
using DynamicData.Binding;
using DynamicData.PLinq;
using DynamicData.ReactiveUI;
using ReactiveUI;

namespace Blotter.Client.Views
{
    public class RxUiViewModel : ReactiveObject, IDisposable
    {
        private readonly IDisposable _cleanUp;

        //This is the target list which we will populate from the dynamic data stream
        private readonly ReactiveList<TradeProxy> _data = new ReactiveList<TradeProxy>();

        private string _searchText;

        public RxUiViewModel(ITradeService tradeService)
        {
            //Change the filter when the user entered search text changes
            var filter = this.WhenAnyValue(x => x.SearchText)
                .Throttle(TimeSpan.FromMilliseconds(250))
                .Select(BuildFilter);

            _cleanUp = tradeService.Live.Connect()
                .Filter(filter) // apply user filter
                                //if targetting Net4 or Net45 platform can use parallelisation for transforms 'cause it's quicker
                .Transform(trade => new TradeProxy(trade), new ParallelisationOptions(ParallelType.Ordered, 5))
                .Sort(SortExpressionComparer<TradeProxy>.Descending(t => t.Timestamp), SortOptimisations.ComparesImmutableValuesOnly, 25)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(_data) //bind the results to the ReactiveList 
                .DisposeMany() //since TradeProxy is disposable dispose when no longer required
                .Subscribe(_ =>
                {
                        
                });
        }

        public delegate RxUiViewModel Factory();

        public string SearchText
        {
            get => _searchText;
            set => this.RaiseAndSetIfChanged(ref _searchText, value);
        }

        public IReadOnlyReactiveList<TradeProxy> Data => _data;

        private Func<Trade, bool> BuildFilter(string searchText)
        {
            if (string.IsNullOrEmpty(SearchText))
                return le => true;

            return t => t.CurrencyPair.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                        t.Customer.Contains(SearchText, StringComparison.OrdinalIgnoreCase);
        }

        public void Dispose()
        {
            _cleanUp.Dispose();
        }
    }
}