using System.Reactive.Concurrency;
using System.Windows.Threading;
using Blotter.Infrastructure;

namespace Blotter.Client.Infrastructure
{
    public class SchedulerProvider : ISchedulerProvider
    {
        // used by Autofac
        public SchedulerProvider()
        {
            MainThread = new DispatcherScheduler(System.Windows.Application.Current.Dispatcher);
        }

        // testability purposes
        public SchedulerProvider(Dispatcher dispatcher)
        {
            MainThread = new DispatcherScheduler(dispatcher);
        }

        public IScheduler MainThread { get; }

        public IScheduler Background => TaskPoolScheduler.Default;
    }
}