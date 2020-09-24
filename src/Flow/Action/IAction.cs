namespace Skclusive.Reactive.Flow
{
    public interface IActionDispatcher
    {
        void Dispatch(IAction action);
    }
}
