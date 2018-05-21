using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Input;
using Dragablz;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;

namespace Blotter.Client.Infrastructure
{
    public class WindowViewModel : AbstractNotifyPropertyChanged, IDisposable
    {
        private readonly ViewContainer.Factory _viewContainerFactory;
        private readonly MenuBuilder.Factory _menuItemsFactory;
        private readonly ReactiveCommand _showMenuCommand;
        private readonly IDisposable _cleanUp;
        private IViewContainer _selected;
        
        public ICommand MemoryCollectCommand { get; } = ReactiveCommand.Create(() =>
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        });

        public WindowViewModel(
            ViewContainer.Factory viewContainerFactory,
            MenuBuilder.Factory menuItemsFactory, 
            IInterTabClient interTabClient)
        {
            _viewContainerFactory = viewContainerFactory;
            _menuItemsFactory = menuItemsFactory;
            InterTabClient = interTabClient;

            _showMenuCommand = ReactiveCommand.Create(ShowMenu, Observable.Return<bool>(Selected != null && !(Selected.Content is MenuBuilder)));
            ShowInGitHubCommand = ReactiveCommand.Create(() => Process.Start("https://github.com/RolandPheasant"));

            var menuController = Views.ToObservableChangeSet()
                                        .Filter(vc => vc.Content is MenuBuilder)
                                        .Transform(vc => (MenuBuilder)vc.Content)
                                        .MergeMany(menuItems => menuItems.ItemCreated)
                                        .Subscribe(item =>
                                        {
                                            Views.Add(item);
                                            Selected = item;
                                        });
            
            _cleanUp = Disposable.Create(() =>
            {
                menuController.Dispose();
                foreach (var disposable in Views.Select(vc => vc.Content).OfType<IDisposable>())
                    disposable.Dispose();
            });
        }

        public delegate WindowViewModel Factory(IInterTabClient interTabClient);

        public void ShowMenu()
        {
            var existing = Views.FirstOrDefault(vc => vc.Content is MenuBuilder);
            if (existing == null)
            {
                var newItem = _viewContainerFactory("Menu", _menuItemsFactory());
                Views.Add(newItem);
                Selected = newItem;
            }
            else
            {
                Selected = existing;
            }
        }
        
        public ItemActionCallback ClosingTabItemHandler => ClosingTabItemHandlerImpl;

        private void ClosingTabItemHandlerImpl(ItemActionCallbackArgs<TabablzControl> args)
        {
            var container = (ViewContainer)args.DragablzItem.DataContext;
            if (container.Equals(Selected))
            {
                Selected = Views.FirstOrDefault(vc => vc != container);
            }
            var disposable = container.Content as IDisposable;
            disposable?.Dispose();
        }

        public ObservableCollection<IViewContainer> Views { get; } = new ObservableCollection<IViewContainer>();

        public IViewContainer Selected
        {
            get => _selected;
            set => SetAndRaise(ref _selected, value);
        }

        public IInterTabClient InterTabClient { get; }

        public ICommand ShowMenuCommand => _showMenuCommand;

        public ReactiveCommand ShowInGitHubCommand { get; }

        public void Dispose()
        {
            _cleanUp.Dispose();
        }
    }
}