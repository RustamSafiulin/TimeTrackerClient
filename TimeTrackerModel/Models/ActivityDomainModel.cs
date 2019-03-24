
using System;
using System.Collections.Generic;

using TimeTracker.Helpers;

namespace TimeTracker.Models
{
    public enum ActivityState
    {
        Paused = 0,
        Running = 1
    }

    public class WorkInterval
    {
        #region Props and Fields
        
        public DateTime Begin { get; set; }
        public DateTime End { get; set; }

        #endregion

        public WorkInterval()
        { }
    }

    public class ActivityDomainModel : EditableObject<ActivityDomainModel>
    {
        public ActivityDomainModel()
        {
            _workIntervals = new List<WorkInterval>();
        }

        #region Props and Fields
        
        public bool AlreadyNotified { get; set; }

        private ActivityState _state;
        public ActivityState State
        {
            get { return _state; }
            set { SetProperty(ref _state, value); }
        }

        private Boolean _isStarted;
        public Boolean IsStarted
        {
            get { return _isStarted; }
            set { SetProperty(ref _isStarted, value); }
        }

        private string _id;
        public string Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        private string _profileId;
        public string ProfileId
        {
            get { return _profileId; }
            set { SetProperty(ref _profileId, value); }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        private string _category;
        public string Category
        {
            get { return _category; }
            set { SetProperty(ref _category, value); }
        }

        /*time of activity created*/
        private DateTime _createdAt;
        public DateTime CreatedAt
        {
            get { return _createdAt; }
            set { SetProperty(ref _createdAt, value); }
        }

        /*actual duration*/
        private Int64 _durationSeconds;
        public Int64 DurationSeconds
        {
            get { return _durationSeconds; }
            set
            {
                SetProperty(ref _durationSeconds, value);
                var time = TimeSpan.FromSeconds(_durationSeconds);
                DurationFriendly = time.ToString(@"hh\:mm\:ss");
            }
        }

        private String _durationFriendly = "00:00:00";
        public String DurationFriendly
        {
            get { return _durationFriendly; }
            set { SetProperty(ref _durationFriendly, value); }
        }

        /*Planned begin time. Compare with ActualBeginTime and notify*/
        private DateTime _plannedBeginTime;
        public DateTime PlannedBeginTime
        {
            get { return _plannedBeginTime; }
            set {
                SetProperty(ref _plannedBeginTime, value);
            }
        }
        
        /*Real begin time*/
        private DateTime _beginTime;
        public DateTime BeginTime
        {
            get { return _beginTime; }
            set { SetProperty(ref _beginTime, value); }
        }

        private List<WorkInterval> _workIntervals;
        public List<WorkInterval> WorkIntervals
        {
            get { return _workIntervals; }
            set { SetProperty(ref _workIntervals, value); }
        }

        #endregion
    }
}
