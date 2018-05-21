using System.Windows;
using System.Windows.Controls;
using ReactiveUI;

namespace Blotter.Client.Views
{
    /// <summary>
    /// Interaction logic for RxUiView.xaml
    /// </summary>
    public partial class RxUiView : UserControl, IViewFor<RxUiViewModel>
    {
        public RxUiView()
        {
            InitializeComponent();


            /*  https://reactiveui.net/docs/handbook/data-binding/windows-presentation-foundation
                the XAML DependencyProperty system causes memory leaks if you don't use `WhenActivated` 
                there's a few rules, but the number one rule is: if you do a `WhenAny` on anything other than `this`, then you need to put it inside a `WhenActivated`
                this.WhenActivated(d =>
                {
                    d(ViewModel.WhenAnyValue(x => x.Something).Subscribe(...));
                });
            */

            // Setup the bindings
            // Note: We have to use WhenActivated here, since we need to dispose the
            // bindings on XAML-based platforms, or else the bindings leak memory.

            this.WhenActivated(d =>
            {
                d(this.WhenAnyValue(x => x.ViewModel).BindTo(this, x => x.DataContext));
            });

            //this.WhenAnyValue(x => x.ViewModel).BindTo(this, x => x.DataContext);
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel", typeof (RxUiViewModel), typeof (RxUiView), new PropertyMetadata(default(RxUiViewModel)));

        public RxUiViewModel ViewModel
        {
            get { return (RxUiViewModel) GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (RxUiViewModel)value; }
        }
    }
}
