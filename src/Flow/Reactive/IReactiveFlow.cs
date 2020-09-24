using System;

namespace Skclusive.Reactive.Flow
{
    public interface IReactiveFlow : IDisposable
    {
        void Start();

        void Stop();
    }
}
