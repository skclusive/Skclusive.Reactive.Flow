using System.Reactive.Concurrency;

namespace Skclusive.Reactive.Flow
{
    public interface ISchedulerProvider
    {
        IScheduler Scheduler { get; }
    }
}
