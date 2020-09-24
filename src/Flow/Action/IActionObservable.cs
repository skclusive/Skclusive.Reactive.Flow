using System;

namespace Skclusive.Reactive.Flow
{
    public interface IActionObservable
    {
        IObservable<IAction> Actions { get; }
    }
}
