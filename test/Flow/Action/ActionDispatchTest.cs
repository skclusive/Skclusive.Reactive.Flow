using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Skclusive.Reactive.Flow;
using Xunit;

namespace Skclusive.Reactive.Flow.Tests
{
    public class ActionDispatchTest
    {
        [Fact]
        public void TestDispatch()
        {
            var services = new ServiceCollection();

            services.TryAddReactiveFlowServices();

            var serviceProvider = services.BuildServiceProvider();

            var actionObservable = serviceProvider.GetRequiredService<IActionObservable>();

            var actionDispatcher = serviceProvider.GetRequiredService<IActionDispatcher>();

            var actionObserver1 = new TestActionObserver();

            actionObservable.Actions.Subscribe(actionObserver1);

            var actionObserver2 = new TestActionObserver();

            actionObservable.Actions.Subscribe(actionObserver2);

            Assert.Empty(actionObserver1.Actions);

            Assert.Empty(actionObserver2.Actions);

            actionDispatcher.Dispatch(new TestAction());

            Assert.Single(actionObserver1.Actions);

            Assert.Single(actionObserver2.Actions);

            var actionObserver3 = new TestActionObserver();

            actionObservable.Actions.Subscribe(actionObserver3);

            Assert.Empty(actionObserver3.Actions);
        }

        [Fact]
        public async Task TestDispatchAsync()
        {
            var services = new ServiceCollection();

            services.TryAddReactiveFlowServices();

            services.TryAddEnumerable(ServiceDescriptor.Scoped<IEpic, TestEpic>());

            var serviceProvider = services.BuildServiceProvider();

            var reactiveFlow = serviceProvider.GetRequiredService<IReactiveFlow>();

            var actionObservable = serviceProvider.GetRequiredService<IActionObservable>();

            var actionDispatcher = serviceProvider.GetRequiredService<IActionDispatcher>();

            reactiveFlow.Start();

            var actionObserver1 = new TestActionObserver();

            actionObservable.Actions.Subscribe(actionObserver1);

            Assert.Empty(actionObserver1.Actions);

            actionDispatcher.Dispatch(new TestAction());

            Assert.Single(actionObserver1.Actions);

            await actionDispatcher.DispatchAsync<TestEndAction>(new TestStartAction());

            Assert.Equal(4, actionObserver1.Actions.Count);
        }

        [Fact]
        public async Task TestDispatchFilterAsync()
        {
            var services = new ServiceCollection();

            services.TryAddReactiveFlowServices();

            services.TryAddEnumerable(ServiceDescriptor.Scoped<IEpic, TestEpic>());

            var serviceProvider = services.BuildServiceProvider();

            var reactiveFlow = serviceProvider.GetRequiredService<IReactiveFlow>();

            var actionObservable = serviceProvider.GetRequiredService<IActionObservable>();

            var actionDispatcher = serviceProvider.GetRequiredService<IActionDispatcher>();

            reactiveFlow.Start();

            var actionObserver1 = new TestActionObserver();

            actionObservable.Actions.Subscribe(actionObserver1);

            Assert.Empty(actionObserver1.Actions);

            actionDispatcher.Dispatch(new TestAction());

            Assert.Single(actionObserver1.Actions);

            await actionDispatcher.DispatchAsync<TestEndAction>(new TestStartAction(), end => end.Value == 1);

            Assert.Equal(5, actionObserver1.Actions.Count);
        }
    }
}
