using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace Skclusive.Reactive.Flow
{
    public class ActionStream : IActionStream
    {
        private ISubject<IAction> _subject { get; }

        public ActionStream()
        {
            _subject = new Subject<IAction>();
        }

        public IObservable<IAction> Actions => _subject.AsObservable();

        public void Dispatch(IAction action)
        {
            System.Console.WriteLine($"action: {GetFriendlyName(action.GetType())} at: {DateTimeOffset.UtcNow}");
            _subject.OnNext(action);
        }

        public Task<T> DispatchAsync<T>(IAction action) where T : IAction
        {
            return DispatchAsync<T>(action, _ => true);
        }

        public Task<T> DispatchAsync<T>(IAction action, Func<T, bool> filter) where T : IAction
        {
            var source = new TaskCompletionSource<T>();

            IDisposable disposable = null;

            disposable = _subject.OfType<T>().Where(filter).FirstAsync().Subscribe(matched =>
            {
                disposable.Dispose();

                source.SetResult(matched);
            });

            Dispatch(action);

            return source.Task;
        }

        public static string GetFriendlyName(Type type)
        {
            if (type.IsGenericType)
            {
                var name = type.Name.Substring(0, type.Name.IndexOf('`'));
                var types = string.Join(",", type.GetGenericArguments().Select(GetFriendlyName));
                return $"{name}<{types}>";
            }
            else
            {
                return type.Name;
            }
        }
    }
}
