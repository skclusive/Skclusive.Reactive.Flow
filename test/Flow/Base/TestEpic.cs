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
                Observable.Timer(TimeSpan.FromMilliseconds(1)).Select<long, IAction>(_ => new TestBeginAction())
                .Concat(
                    Observable.Interval(TimeSpan.FromSeconds(3)).Take(2).Select(value => new TestEndAction { Value = value })
                ));
        }
    }

    internal class TestEpic2 : IEpic
    {
        public IObservable<IAction> Configure(IObservable<IAction> actions)
        {
            return actions.OfType<TestStartAction>().SelectMany((action) =>
                Observable.Concat
                (
                    Observable.Timer(TimeSpan.FromMilliseconds(1)).Select<long, IAction>(_ => new TestBeginAction()),

                    Observable.Timer(TimeSpan.FromSeconds(3))
                    .TakeUntil(actions.OfType<TestCancelAction>())
                    .Select(value => string.IsNullOrWhiteSpace(action.Error) ? new TestEndAction { Value = value } : throw new Exception(action.Error))
                    .Catch((Exception ex) => Observable.Return<IAction>(new TestErrorAction { Message = $"{ex.Message}-captured" })),

                    Observable.Return<IAction>(new TestFinishAction()))
                );
        }
    }
}
