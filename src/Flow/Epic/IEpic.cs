using System;

namespace Skclusive.Reactive.Flow
{
    public interface IEpic
    {
        IObservable<IAction> Configure(IObservable<IAction> actions);
    }
}
