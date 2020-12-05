using System.Reactive.Concurrency;

namespace Skclusive.Reactive.Flow
{
    public class SchedulerProvider : ISchedulerProvider
    {
        public IScheduler Scheduler { get; set; }
    }
}
