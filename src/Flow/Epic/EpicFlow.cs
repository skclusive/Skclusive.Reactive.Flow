using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace Skclusive.Reactive.Flow
{
    public class EpicFlow : IEpicFlow
    {
        private IList<IEpic> Epics { get; }

        private IActionObservable ActionObservable { get; }

        private IActionDispatcher ActionDispatcher { get;  }

        private ISchedulerProvider SchedulerProvider { get; }

        private IDisposable Subscription { set; get; }

        private bool active;

        public EpicFlow(ISchedulerProvider schedulerProvider, IEnumerable<IEpic> epics, IActionObservable actionObservable, IActionDispatcher actionDispatcher)
        {
            Epics = epics.ToList();

            SchedulerProvider = schedulerProvider;

            ActionObservable = actionObservable;

            ActionDispatcher = actionDispatcher;
        }

        public void Start()
        {
            if (active)
            {
                throw new Exception("already started");
            }

            active = true;

            var actions = Observable.Merge(Epics.Select(epic => epic.Configure(ActionObservable.Actions)));

            Subscription = actions.ObserveOn(SchedulerProvider.Scheduler).Subscribe((action) =>
            {
                ActionDispatcher.Dispatch(action);
            });
        }

        public void Stop()
        {
            active = false;
        }

        public void Dispose()
        {
            Subscription?.Dispose();

            Subscription = null;
        }
    }
}
