
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Threading;

using TimeTracker.Models;
using TimeTracker.EventBus;
using TimeTracker.ViewModels;

namespace TimeTracker
{
    public class ActivityDispatcher
    {
        public ActivityDispatcher(TinyMessengerHub eventHub, Action incrementTotal)
        {
            _incrementTotal = incrementTotal;

            _eventHub = eventHub;
            _trackTimer = new DispatcherTimer();
            _trackTimer.Interval = TimeSpan.FromSeconds(_tickIntervalSeconds);
            _trackTimer.Tick += TickEvent;

            CreateEventHubSubscriptions();
        }

        private void TickEvent(object sender, EventArgs e)
        {
            if (_trackedActivity != null)
            {
                _trackedActivity.DurationSeconds += _tickIntervalSeconds;
                _incrementTotal();
            }
        }

        public void RunDispatchLoop(Func<List<String>> checkActivities)
        {
            Dispatcher dispatcher = default(Dispatcher);
            var currentApp = System.Windows.Application.Current;
            dispatcher = currentApp.Dispatcher;

            Task.Factory.StartNew(async () => 
            {
                while (_loopEnabled)
                {
                    if (_needBeginNotify)
                    {
                        dispatcher.Invoke(() =>
                        {
                            foreach (var description in checkActivities())
                            {
                                _eventHub.Publish<NotificationMessage>(new NotificationMessage() { Text = description, TimeStamp = DateTime.Now });
                            }
                        });
                    }

                    await Task.Delay(_waitLoopMs);
                }
            });
        }

        private void StopDispatchLoop()
        {
            _loopEnabled = false;
        }

        private void CreateEventHubSubscriptions()
        {
            _eventHub.Subscribe<ChangeSettingsMessage>(ChangeSettingsHandler);
        }

        private void ChangeSettingsHandler(ChangeSettingsMessage m)
        {
            _needBeginNotify = m.NeedBegin;
        }

        public void StartTracking(ActivityDomainModel trackedActivity)
        {
            if (_trackedActivity != null && _trackedActivity.State == ActivityState.Running)
                _trackedActivity.State = ActivityState.Paused;

            _trackedActivity = trackedActivity;
            _trackedActivity.State = ActivityState.Running;
            _trackedActivity.IsStarted = true;
            _trackTimer.Start();
        }

        public void StopTracking()
        {
            _trackedActivity.State = ActivityState.Paused;
            _trackTimer.Stop();
        }

        #region fields

        public volatile bool _needBeginNotify; //use interlocked

        private Action _incrementTotal;

        private volatile bool _loopEnabled = true; //use interlocked

        private const int _waitLoopMs = 3000;

        private TinyMessengerHub _eventHub;

        private const Int64 _tickIntervalSeconds = 1;
        private ActivityDomainModel _trackedActivity;
        private readonly DispatcherTimer _trackTimer;

        #endregion
    }
}
