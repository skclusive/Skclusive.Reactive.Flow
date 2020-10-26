using System;
using System.Threading.Tasks;

namespace Skclusive.Reactive.Flow
{
    public interface IActionDispatcher
    {
        void Dispatch(IAction action);

        Task<T> DispatchAsync<T>(IAction action) where T : IAction;

        Task<T> DispatchAsync<T>(IAction action, Func<T, bool> filter) where T : IAction;
    }
}
