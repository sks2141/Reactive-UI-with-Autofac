using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Blotter.Client.Views;
using DynamicData.Binding;
using ReactiveUI;

namespace Blotter.Client.Infrastructure
{
    public enum MenuCategory
    {
        ReactiveUi,
        DynamicData
    }

    public class MenuBuilder : AbstractNotifyPropertyChanged, IDisposable
    {
        //private readonly ILogger _logger;
        private readonly ISubject<IViewContainer> _viewCreatedSubject = new Subject<IViewContainer>();

        private readonly IEnumerable<MenuItem> _menuItems;
        private readonly IDisposable _cleanUp;
        private readonly ViewContainer.Factory _viewContainerFactory;
        private readonly RxUiHostViewModel.Factory _rxUiHostViewModelFactory;

        private bool _showLinks = false;
        private MenuCategory _category = MenuCategory.ReactiveUi;
        private IEnumerable<MenuItem> _items;

        public MenuBuilder(//ILogger logger,
            ViewContainer.Factory viewContainerFactory,
            RxUiHostViewModel.Factory rxUiHostViewModelFactory,
            RxUiViewModel.Factory rxUiViewModelFactory)
        {
            //_logger = logger;
            
            _viewContainerFactory = viewContainerFactory;
            _rxUiHostViewModelFactory = rxUiHostViewModelFactory;

            _menuItems = new List<MenuItem>
            {
                new MenuItem("Live Trades (RxUI)", "A basic example, illustrating where reactive-ui and dynamic data can work together",
                             () => OpenRxUi("Live Trades (RxUI)", rxUiViewModelFactory()),
                             MenuCategory.ReactiveUi,
                             new [] 
                             {
                                 new Link("View Model","RxUiViewModel.cs", ""),
                                 new Link("Blog","Integration with reactive ui", ""),
                             }),
            };

            var filterApplier = this.WhenValueChanged(t => t.Category)
                .Subscribe(value =>
                {
                    Items = _menuItems.Where(menu => menu.Category == value).ToArray();
                });

            _cleanUp = Disposable.Create(() =>
            {
                _viewCreatedSubject.OnCompleted();
                filterApplier.Dispose();
            });
        }

        public delegate MenuBuilder Factory();
        
        private void OpenRxUi(string title, ReactiveObject content)
        {
            //_logger.Debug("Opening '{0}'", title);
            _viewCreatedSubject.OnNext(_viewContainerFactory(title, _rxUiHostViewModelFactory(content)));
            //_logger.Debug("--Opened '{0}'", title);
        }

        public MenuCategory Category
        {
            get => _category;
            set => SetAndRaise(ref _category, value);
        }

        public IEnumerable<MenuItem> Items
        {
            get => _items;
            set => SetAndRaise(ref _items, value);
        }

        public bool ShowLinks
        {
            get => _showLinks;
            set => SetAndRaise(ref _showLinks, value);
        }

        public IObservable<IViewContainer> ItemCreated => _viewCreatedSubject.AsObservable();

        public void Dispose()
        {
            _cleanUp.Dispose();
        }
    }
}