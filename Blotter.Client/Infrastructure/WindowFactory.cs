using System;
using Dragablz;

namespace Blotter.Client.Infrastructure
{
    public class WindowFactory : IWindowFactory
    {
        private readonly WindowViewModel.Factory _windowViewModelFactory;

        public WindowFactory(WindowViewModel.Factory windowViewModelFactory)
        {
            _windowViewModelFactory = windowViewModelFactory;
        }

        public delegate WindowFactory Factory();

        public MainWindow Create(bool showMenu = false)
        {
            var window = new MainWindow();
            var model = _windowViewModelFactory(new InterTabClient(this));

            if (showMenu)
            {
                model.ShowMenu();
            }

            window.DataContext = model;

            window.Closing += (sender, e) =>
            {
                if (TabablzControl.GetIsClosingAsPartOfDragOperation(window)) return;

                var todispose = ((MainWindow)sender).DataContext as IDisposable;
                todispose?.Dispose();
            };

            return window;
        }
    }
}