using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace Skclusive.Reactive.Flow
{
    public class ActionStream: IActionStream
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
    }
}
