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

            actionDispatcher.Dispatch(new TestStartAction());

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

            actionDispatcher.Dispatch(new TestStartAction());

            Assert.Single(actionObserver1.Actions);

            Assert.True(actionObserver1.Actions[0] is TestStartAction);

            await Task.Delay(6000);

            Assert.Equal(4, actionObserver1.Actions.Count);

            Assert.True(actionObserver1.Actions[1] is TestBeginAction);
            Assert.True(actionObserver1.Actions[2] is TestEndAction);
            Assert.True(actionObserver1.Actions[3] is TestEndAction);

            var endAction = await actionDispatcher.DispatchAsync<TestEndAction>(new TestStartAction());

            Assert.Equal(0, endAction.Value);

            Assert.Equal(7, actionObserver1.Actions.Count);

            Assert.True(actionObserver1.Actions[4] is TestStartAction);
            Assert.True(actionObserver1.Actions[5] is TestBeginAction);
            Assert.True(actionObserver1.Actions[6] is TestEndAction);
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

            actionDispatcher.Dispatch(new TestStartAction());

            Assert.Single(actionObserver1.Actions);

            await Task.Delay(6100);

            Assert.Equal(4, actionObserver1.Actions.Count);

            var endAction = await actionDispatcher.DispatchAsync<TestEndAction>(new TestStartAction(), end => end.Value == 1);

            Assert.Equal(1, endAction.Value);

            Assert.Equal(8, actionObserver1.Actions.Count);
        }

        [Fact]
        public async Task TestCancellationAsync()
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

            actionDispatcher.Dispatch(new TestStartAction());

            Assert.Single(actionObserver1.Actions);

            await Task.Delay(3100);

            actionDispatcher.Dispatch(new TestCancelAction());

            Assert.Equal(4, actionObserver1.Actions.Count);

            Assert.True(actionObserver1.Actions[0] is TestStartAction);
            Assert.True(actionObserver1.Actions[1] is TestBeginAction);
            Assert.True(actionObserver1.Actions[2] is TestEndAction);
            Assert.True(actionObserver1.Actions[3] is TestCancelAction);
        }

        [Fact]
        public async Task TestEpic2SuccessAsync()
        {
            var services = new ServiceCollection();

            services.TryAddReactiveFlowServices();

            services.TryAddEnumerable(ServiceDescriptor.Scoped<IEpic, TestEpic2>());

            var serviceProvider = services.BuildServiceProvider();

            var reactiveFlow = serviceProvider.GetRequiredService<IReactiveFlow>();

            var actionObservable = serviceProvider.GetRequiredService<IActionObservable>();

            var actionDispatcher = serviceProvider.GetRequiredService<IActionDispatcher>();

            reactiveFlow.Start();

            var actionObserver1 = new TestActionObserver();

            actionObservable.Actions.Subscribe(actionObserver1);

            Assert.Empty(actionObserver1.Actions);

            actionDispatcher.Dispatch(new TestStartAction());

            Assert.Single(actionObserver1.Actions);

            await Task.Delay(3100);

            Assert.Equal(4, actionObserver1.Actions.Count);

            Assert.True(actionObserver1.Actions[0] is TestStartAction);
            Assert.True(actionObserver1.Actions[1] is TestBeginAction);
            Assert.True(actionObserver1.Actions[2] is TestEndAction);
            Assert.True(actionObserver1.Actions[3] is TestFinishAction);
        }

        [Fact]
        public async Task TestEpic2CancellationAsync()
        {
            var services = new ServiceCollection();

            services.TryAddReactiveFlowServices();

            services.TryAddEnumerable(ServiceDescriptor.Scoped<IEpic, TestEpic2>());

            var serviceProvider = services.BuildServiceProvider();

            var reactiveFlow = serviceProvider.GetRequiredService<IReactiveFlow>();

            var actionObservable = serviceProvider.GetRequiredService<IActionObservable>();

            var actionDispatcher = serviceProvider.GetRequiredService<IActionDispatcher>();

            reactiveFlow.Start();

            var actionObserver1 = new TestActionObserver();

            actionObservable.Actions.Subscribe(actionObserver1);

            Assert.Empty(actionObserver1.Actions);

            actionDispatcher.Dispatch(new TestStartAction());

            Assert.Single(actionObserver1.Actions);

            await Task.Delay(2000);

            actionDispatcher.Dispatch(new TestCancelAction());

            Assert.Equal(4, actionObserver1.Actions.Count);

            Assert.True(actionObserver1.Actions[0] is TestStartAction);
            Assert.True(actionObserver1.Actions[1] is TestBeginAction);
            Assert.True(actionObserver1.Actions[2] is TestCancelAction);
            Assert.True(actionObserver1.Actions[3] is TestFinishAction);
        }

        [Fact]
        public async Task TestEpic2ErrorAsync()
        {
            var services = new ServiceCollection();

            services.TryAddReactiveFlowServices();

            services.TryAddEnumerable(ServiceDescriptor.Scoped<IEpic, TestEpic2>());

            var serviceProvider = services.BuildServiceProvider();

            var reactiveFlow = serviceProvider.GetRequiredService<IReactiveFlow>();

            var actionObservable = serviceProvider.GetRequiredService<IActionObservable>();

            var actionDispatcher = serviceProvider.GetRequiredService<IActionDispatcher>();

            reactiveFlow.Start();

            var actionObserver1 = new TestActionObserver();

            actionObservable.Actions.Subscribe(actionObserver1);

            Assert.Empty(actionObserver1.Actions);

            actionDispatcher.Dispatch(new TestStartAction { Error = "error action" });

            await Task.Delay(3100);

            Assert.Equal(4, actionObserver1.Actions.Count);

            Assert.True(actionObserver1.Actions[0] is TestStartAction);
            Assert.True(actionObserver1.Actions[1] is TestBeginAction);
            Assert.True(actionObserver1.Actions[2] is TestErrorAction);
            Assert.True(actionObserver1.Actions[3] is TestFinishAction);

            Assert.Equal("error action-captured", (actionObserver1.Actions[2] as TestErrorAction).Message);
        }
    }
}       
