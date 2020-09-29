using System;
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
            _subject.OnNext(action);
        }

        public Task DispatchAsync<T>(IAction action) where T : IAction
        {
            var source = new TaskCompletionSource();

            IDisposable disposable = null;

            disposable = _subject.OfType<T>().Subscribe(matched =>
            {
                source.SetResult();

                disposable.Dispose();
            });

            Dispatch(action);

            return source.Task;
        }

        public Task DispatchAsync<T>(IAction action, Func<T, bool> filter) where T : IAction
        {
            var source = new TaskCompletionSource();

            IDisposable disposable = null;

            disposable = _subject.OfType<T>().Where(filter).Subscribe(matched =>
            {
                source.SetResult();

                disposable.Dispose();
            });

            Dispatch(action);

            return source.Task;
        }
    }
}
