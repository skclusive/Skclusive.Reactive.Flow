using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Skclusive.Reactive.Flow;
using Xunit;

namespace Skclusive.Reactive.Flow.Tests
{
    internal class TestAction : IAction
    {
    }

    internal class TestStartAction : IAction
    {
    }

    internal class TestEndAction : IAction
    {
        public long Value { get; init; }
    }
}
