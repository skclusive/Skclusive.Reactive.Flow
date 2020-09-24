using System;

namespace Skclusive.Reactive.Flow
{
    public interface IEpicFlow : IDisposable
    {
        void Start();

        void Stop();
    }
}
