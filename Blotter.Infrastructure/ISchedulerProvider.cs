using System.Reactive.Concurrency;

namespace Blotter.Infrastructure
{
    public interface ISchedulerProvider
    {
        IScheduler MainThread { get; }
        IScheduler Background { get; }
    }
}