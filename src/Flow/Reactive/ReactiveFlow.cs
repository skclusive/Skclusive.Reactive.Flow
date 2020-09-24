using System;
using System.Collections.Generic;
using System.Linq;

namespace Skclusive.Reactive.Flow
{
    public class ReactiveFlow : IReactiveFlow
    {
        private IEpicFlow EpicFlow { get; }

        private IEffectFlow EffectFlow { get; }

        public ReactiveFlow(IEpicFlow epicFlow, IEffectFlow effectFlow)
        {
            EpicFlow = epicFlow;

            EffectFlow = effectFlow;
        }

        public void Start()
        {
            EpicFlow.Start();

            EffectFlow.Start();
        }

        public void Stop()
        {
            EpicFlow.Stop();

            EffectFlow.Stop();
        }

        public void Dispose()
        {
            EpicFlow.Dispose();

            EffectFlow.Dispose();
        }
    }
}
