using System;

namespace Skclusive.Reactive.Flow
{
    public interface IEffectFlow : IDisposable
    {
        void Start();

        void Stop();
    }
}
