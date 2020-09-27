using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Skclusive.Reactive.Flow;
using Xunit;

namespace Skclusive.Reactive.Flow.Tests
{
    internal class TestActionObserver : IObserver<IAction>
    {
        internal List<IAction> Actions = new List<IAction>();

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(IAction action)
        {
            Actions.Add(action);
        }
    }
}
