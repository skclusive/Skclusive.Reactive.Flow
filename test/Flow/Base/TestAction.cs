using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Skclusive.Reactive.Flow;
using Xunit;

namespace Skclusive.Reactive.Flow.Tests
{
    internal class TestStartAction : IAction
    {
        internal string Error { init; get; }
    }

    internal class TestBeginAction : IAction
    {
    }

    internal class TestFinishAction : IAction
    {
    }

    internal class TestCancelAction : IAction
    {
    }

    internal class TestErrorAction : IAction
    {
        internal string Message { init; get; }
    }

    internal class TestEndAction : IAction
    {
        internal long Value { get; init; }
    }
}
