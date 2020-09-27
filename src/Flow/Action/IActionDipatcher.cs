using System;
using System.Threading.Tasks;

namespace Skclusive.Reactive.Flow
{
    public interface IActionDispatcher
    {
        void Dispatch(IAction action);

        Task DispatchAsync<T>(IAction action) where T : IAction;

        Task DispatchAsync<T>(IAction action, Func<T, bool> filter) where T : IAction;
    }
}
