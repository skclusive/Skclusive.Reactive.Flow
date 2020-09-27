using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Skclusive.Reactive.Flow;
using Xunit;

namespace Skclusive.Reactive.Flow.Tests
{
    internal class TestEpic : IEpic
    {
        public IObservable<IAction> Configure(IObservable<IAction> actions)
        {
            return actions.OfType<TestStartAction>().SelectMany((_) =>
                    Observable.Return<IAction>(new TestAction())
                .Concat(
                    Observable.Interval(TimeSpan.FromSeconds(1)).Take(2).Select(value => new TestEndAction { Value = value })
                ));
        }
    }
}
