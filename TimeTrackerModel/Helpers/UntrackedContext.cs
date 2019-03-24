
using System;

namespace TimeTracker.Helpers
{
    public class UntrackedContext<T> : IDisposable
                    where T : ITrackable
    {
        public T Subject { get; private set; }

        private UntrackedContext(T subject)
        {
            this.Subject = subject;
        }

        public static UntrackedContext<T> Untrack(T subject)
        {
            var handler = new UntrackedContext<T>(subject);
            handler.Untrack();
            return handler;
        }

        private void Untrack()
        {
            this.Subject.IsTrackable = false;
        }

        private void Track()
        {
            this.Subject.IsTrackable = true;
        }

        public void Dispose()
        {
            this.Track();
        }
    }
}