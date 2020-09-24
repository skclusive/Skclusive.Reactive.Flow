using System;
using System.Collections.Generic;
using System.Linq;

namespace Skclusive.Reactive.Flow
{
    public class EffectFlow : IEffectFlow
    {
        private IList<IEffect> Effects { get; }

        private IActionObservable ActionObservable { get;  }

        private IDisposable Subscription { set; get; }

        private bool active;

        public EffectFlow(IEnumerable<IEffect> effects, IActionObservable actionObservable)
        {
            Effects = effects.ToList();

            ActionObservable = actionObservable;
        }

        private void OnNext(IAction action)
        {
            if (active)
            {
                foreach (var effect in Effects)
                {
                    effect.Run(action);
                }
            }
        }

        public void Start()
        {
            if (active)
            {
                throw new Exception("already started");
            }

            active = true;

            Subscription = ActionObservable.Actions.Subscribe((action) =>
            {
                OnNext(action);
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
