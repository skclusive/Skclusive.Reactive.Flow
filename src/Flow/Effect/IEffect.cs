namespace Skclusive.Reactive.Flow
{
    public interface IEffect
    {
        void Run(IAction action);
    }
}
