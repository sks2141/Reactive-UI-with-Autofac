using ReactiveUI;

namespace Blotter.Client.Views
{
    public class RxUiHostViewModel
    {
        public RxUiHostViewModel(ReactiveObject content)
        {
            Content = content;
        }

        public delegate RxUiHostViewModel Factory(ReactiveObject content);

        public ReactiveObject Content { get; }
    }
}