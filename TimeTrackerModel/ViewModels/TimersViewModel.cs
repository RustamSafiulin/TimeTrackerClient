
using System;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;

using System.Collections.ObjectModel;

using TimeTracker.Models;
using TimeTracker.Helpers;
using TimeTracker.Service;
using TimeTracker.EventBus;

namespace TimeTracker.ViewModels
{
    public sealed class TimersViewModel : DomainModelBase
    {
        public TimersViewModel(ApplicationEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;

            _activities = new ObservableCollection<ActivityDomainModel>();
            _activityDispatcher = new ActivityDispatcher(_appEnvironment.EventHub, UpdateTotalTimer);

            _categories = new List<String>();

            ActivityStartStopCommand = new RelayCommand(RunOrStopActivity, () => { return SelectedActivity != null; });
            DeleteActivityCommand = new RelayCommand(DeleteActivity, () => { return SelectedActivity != null; });
            ViewActivityDetailsCommand = new RelayCommand(ViewActivityDetails, () => { return SelectedActivity != null; });
            AddActivityCommand = new RelayCommand(AddActivity, () => { return !String.IsNullOrEmpty(NewActivityDescription) && !String.IsNullOrWhiteSpace(NewActivityDescription); });

            SaveActivityDetailsCommand = new RelayCommand(UpdateActivity);
            CancelSaveActivityDetailsCommand = new RelayCommand(CancelUpdateActivity);
            GetActivitiesCommand = new RelayCommand(GetAllActivities);

            CreateEventHubSubscriptions();
        }

        #region Props and Fields

        private ApplicationEnvironment _appEnvironment;

        public RelayCommand ActivityStartStopCommand { get; set; }
        public RelayCommand DeleteActivityCommand { get; set; }
        public RelayCommand ViewActivityDetailsCommand { get; set; }
        public RelayCommand AddActivityCommand { get; set; }

        public RelayCommand GetActivitiesCommand { get; set; }

        public RelayCommand SaveActivityDetailsCommand { get; set; }
        public RelayCommand CancelSaveActivityDetailsCommand { get; set; }

        private List<String> _categories;
        public List<String> Categories
        {
            get { return _categories; }
            set { SetProperty(ref _categories, value); }
        }

        private ObservableCollection<ActivityDomainModel> _activities;
        public ObservableCollection<ActivityDomainModel> Activities
        {
            get { return _activities; }
            set { SetProperty(ref _activities, value); }
        }

        private ActivityDomainModel _selectedActivity;
        public ActivityDomainModel SelectedActivity
        {
            get { return _selectedActivity; }
            set { SetProperty(ref _selectedActivity, value); }
        }

        private Boolean _detailsDialogEnabled = false;
        public Boolean DetailsDialogEnabled
        {
            get { return _detailsDialogEnabled; }
            set { SetProperty(ref _detailsDialogEnabled, value); }
        }

        private String _newActivityDescription;
        public String NewActivityDescription
        {
            get { return _newActivityDescription; }
            set { SetProperty(ref _newActivityDescription, value); }
        }

        private Double _totalDurationSeconds;
        public Double TotalDurationSeconds
        {
            get { return _totalDurationSeconds; }
            set
            {
                SetProperty(ref _totalDurationSeconds, value);
                var time = TimeSpan.FromSeconds(_totalDurationSeconds);
                TotalDurationFriendly = time.ToString(@"hh\:mm\:ss");
            }
        }

        private String _totalDurationFriendly = "00:00:00";
        public String TotalDurationFriendly
        {
            get { return _totalDurationFriendly; }
            set { SetProperty(ref _totalDurationFriendly, value); }
        }

        private ActivityDispatcher _activityDispatcher;

        #endregion
        
        private void CreateEventHubSubscriptions()
        {
            _appEnvironment.EventHub.Subscribe<NeedServerSyncMessage>(OnSyncDataFromServer);
            _appEnvironment.EventHub.Subscribe<ChangeSettingsMessage>(OnSettingsChange);
        }

        private void OnSyncDataFromServer(NeedServerSyncMessage m)
        {
            GetAllActivities();

            _activityDispatcher.RunDispatchLoop(CheckNeedStartActivityDescriptions);
        }

        private void OnSettingsChange(ChangeSettingsMessage m)
        {
            Categories = m.Categories;
        }

        //used from Dispatcher.Invoke() context that why without lock
        private List<String> CheckNeedStartActivityDescriptions()
        {
            var result = new List<String>();

            foreach (var activity in Activities)
            {
                if (activity.PlannedBeginTime != default(DateTime))
                {
                    if (DateTime.Now >= activity.PlannedBeginTime && !activity.IsStarted)
                    {
                        if (!activity.AlreadyNotified)
                        {
                            activity.AlreadyNotified = true;
                            result.Add(activity.Description);
                        }
                    }
                }
            }

            return result;
        }

        public void UpdateTotalTimer()
        {
            ++TotalDurationSeconds;
        }

        public void OnCloseActivityDetailsWindow()
        {
            DetailsDialogEnabled = false;
        }

        private void ViewActivityDetails()
        {
            DetailsDialogEnabled = true;
        }

        public void CancelUpdateActivity()
        {
            if (SelectedActivity != null && SelectedActivity.IsDirty)
                SelectedActivity.RejectChanges();

            DetailsDialogEnabled = false;
        }

        public void UpdateActivity()
        {
            try
            {
                if (_appEnvironment.ServiceConnector == null)
                    throw new NullReferenceException($"Null reference of { nameof(_appEnvironment.ServiceConnector) }");

                if (SelectedActivity != null)
                {
                    var activityToUpdate = new ActivityDto()
                    {
                        Id = SelectedActivity.Id,
                        ProfileId = SelectedActivity.ProfileId,
                        IsStarted = SelectedActivity.IsStarted,
                        BeginTime = (Int64)DateTimeHelpers.ConvertToUnixTimestamp(SelectedActivity.BeginTime.ToUniversalTime()),
                        CreatedAt = (Int64)DateTimeHelpers.ConvertToUnixTimestamp(SelectedActivity.CreatedAt.ToUniversalTime()),
                        PlannedBeginTime = (Int64)DateTimeHelpers.ConvertToUnixTimestamp(SelectedActivity.PlannedBeginTime.ToUniversalTime()),
                        Description = SelectedActivity.Description,
                        Category = SelectedActivity.Category,
                        ActualDuration = SelectedActivity.DurationSeconds,
                    };

                    foreach (var interval in SelectedActivity.WorkIntervals)
                    {
                        activityToUpdate.WorkIntervals.Add(new WorkIntervalDto()
                        {
                            Begin = (Int64)DateTimeHelpers.ConvertToUnixTimestamp(interval.Begin.ToUniversalTime()),
                            End = (Int64)DateTimeHelpers.ConvertToUnixTimestamp(interval.End.ToUniversalTime())
                        });
                    }

                    var response = _appEnvironment.ServiceConnector.Post<ActivityDto, SuccessMsgDto, ErrorMsgDto>(new Request<ActivityDto>
                    {
                        AuthToken = new KeyValuePair<string, string>("session_token", SessionStorage.Instance.SessionId),
                        UrlSegments = new Dictionary<string, string>() { { "id", SelectedActivity.Id } },
                        Body = activityToUpdate,
                        OpName = OperationType.UpdateActvity
                    });

                    if (response.SuccessBody != null)
                    {

                    }
                }
            }
            catch (ApplicationException e)
            {
                Logger.Log.Debug(e.Message);
            }
            catch (Exception e)
            {
                Logger.Log.Debug(e.Message);
            }
            finally
            {
                if (SelectedActivity != null && SelectedActivity.IsDirty)
                    SelectedActivity.AcceptChanges();

                DetailsDialogEnabled = false;
            }
        }

        private void GetAllActivities()
        {
            try
            {
                if (_appEnvironment.ServiceConnector == null)
                    throw new NullReferenceException($"Null reference of { nameof(_appEnvironment.ServiceConnector) }");

                Double totalSeconds = default(Double);

                var userInfo = new UserInfoDto() { ProfileId = SessionStorage.Instance.ProfileId };
                var response = _appEnvironment.ServiceConnector.Get<UserInfoDto, List<ActivityDto>, ErrorMsgDto>(new Request<UserInfoDto>
                {
                    AuthToken = new KeyValuePair<string, string>("session_token", SessionStorage.Instance.SessionId),
                    Body = userInfo,
                    OpName = OperationType.GetActivities
                });

                if (response.SuccessBody != null)
                {
                    foreach (var activityDto in response.SuccessBody)
                    {
                        var existedActivity = Activities.Where(a => { return a.Id == activityDto.Id; }).FirstOrDefault();

                        if (existedActivity == null)
                        {
                            var newActivity = new ActivityDomainModel();
                            using (UntrackedContext<ActivityDomainModel>.Untrack(newActivity))
                            {
                                newActivity.Id = activityDto.Id;
                                newActivity.ProfileId = activityDto.ProfileId;
                                newActivity.IsStarted = activityDto.IsStarted;
                                newActivity.Description = activityDto.Description;
                                newActivity.Category = activityDto.Category;
                                newActivity.DurationSeconds = activityDto.ActualDuration;
                                newActivity.CreatedAt = DateTimeHelpers.ConvertFromUnixTimestamp(activityDto.CreatedAt).ToLocalTime();
                                newActivity.BeginTime = DateTimeHelpers.ConvertFromUnixTimestamp(activityDto.BeginTime).ToLocalTime();
                                newActivity.PlannedBeginTime = DateTimeHelpers.ConvertFromUnixTimestamp(activityDto.PlannedBeginTime).ToLocalTime();

                                if (activityDto.WorkIntervals != null)
                                {
                                    foreach (var interval in activityDto.WorkIntervals)
                                    {
                                        newActivity.WorkIntervals.Add(new WorkInterval()
                                        {
                                            Begin = DateTimeHelpers.ConvertFromUnixTimestamp(interval.Begin).ToLocalTime(),
                                            End = DateTimeHelpers.ConvertFromUnixTimestamp(interval.End).ToLocalTime()
                                        });
                                    }
                                }
                                
                                Activities.Add(newActivity);
                            };
                            
                            totalSeconds += activityDto.ActualDuration;
                            TotalDurationSeconds = totalSeconds;
                        }
                    }
                }
            }
            catch (ApplicationException e)
            {
                Logger.Log.Debug(e.Message);
            }
            catch (Exception e)
            {
                Logger.Log.Debug(e.Message);
            }
        }

        private void AddActivity()
        {
            try
            {
                if (_appEnvironment.ServiceConnector == null)
                    throw new NullReferenceException($"Null reference of { nameof(_appEnvironment.ServiceConnector) }");

                var activityDto = new ActivityDto {
                    ProfileId = SessionStorage.Instance.ProfileId,
                    IsStarted = false,
                    Category = "",
                    Description = NewActivityDescription,
                    PlannedBeginTime = (Int64)DateTimeHelpers.ConvertToUnixTimestamp(DateTime.Now.AddHours(2).ToUniversalTime())
                };

                var response = _appEnvironment.ServiceConnector.Post<ActivityDto, ActivityDto, ErrorMsgDto>(new Request<ActivityDto>
                {
                    AuthToken = new KeyValuePair<string, string>("session_token", SessionStorage.Instance.SessionId),
                    Body = activityDto,
                    OpName = OperationType.CreateActivity
                });
                
                if (response.SuccessBody != null)
                {
                    var newActivity = new ActivityDomainModel();
                    newActivity.Id = response.SuccessBody.Id;
                    newActivity.IsStarted = response.SuccessBody.IsStarted;
                    newActivity.ProfileId = response.SuccessBody.ProfileId;
                    newActivity.Description = response.SuccessBody.Description;
                    newActivity.Category = response.SuccessBody.Category;
                    newActivity.PlannedBeginTime = DateTimeHelpers.ConvertFromUnixTimestamp(response.SuccessBody.PlannedBeginTime).ToLocalTime();
                    newActivity.CreatedAt = DateTimeHelpers.ConvertFromUnixTimestamp(response.SuccessBody.CreatedAt).ToLocalTime();
                    newActivity.BeginTime = DateTimeHelpers.ConvertFromUnixTimestamp(response.SuccessBody.BeginTime).ToLocalTime();
                    newActivity.DurationSeconds = response.SuccessBody.ActualDuration;

                    if (response.SuccessBody.WorkIntervals != null)
                    {
                        foreach (var interval in activityDto.WorkIntervals)
                        {
                            newActivity.WorkIntervals.Add(new WorkInterval()
                            {
                                Begin = DateTimeHelpers.ConvertFromUnixTimestamp(interval.Begin).ToLocalTime(),
                                End = DateTimeHelpers.ConvertFromUnixTimestamp(interval.End).ToLocalTime()
                            });
                        }
                    }

                    Activities.Add(newActivity);
                }
            }
            catch (ApplicationException e)
            {
                Logger.Log.Debug(e.Message);
            }
            catch (Exception e)
            {
                Logger.Log.Debug(e.Message);
            }
        }

        private void DeleteActivity()
        {
            try
            {
                if (_appEnvironment.ServiceConnector == null)
                    throw new NullReferenceException($"Null reference of { nameof(_appEnvironment.ServiceConnector) }");

                var response = _appEnvironment.ServiceConnector.Delete<ActivityDto, SuccessMsgDto, ErrorMsgDto>(new Request<ActivityDto>
                {
                    AuthToken = new KeyValuePair<string, string>("session_token", SessionStorage.Instance.SessionId),
                    OpName = OperationType.DeleteActivity,
                    UrlSegments = new Dictionary<string, string>() { { "id", SelectedActivity.Id } }
                });

                if (response.SuccessBody != null)
                {
                    Activities.Remove(SelectedActivity);
                }
            }
            catch (ApplicationException e)
            {
                Logger.Log.Debug(e.Message);
            }
            catch (Exception e)
            {
                Logger.Log.Debug(e.Message);
            }
        }

        private void RunOrStopActivity()
        {
            if (SelectedActivity == null)
                return;

            if (SelectedActivity.State == ActivityState.Paused)
            {
                RunActivity();
            }
            else if (SelectedActivity.State == ActivityState.Running)
            {
                StopActivity();
            }
        }

        private void StopActivity()
        {
            try
            {
                _activityDispatcher.StopTracking();
                UpdateActivity();
            }
            catch (ApplicationException e)
            {
                Logger.Log.Debug(e.Message);
            }
            catch (Exception e)
            {
                Logger.Log.Debug(e.Message);
            }
        }

        private void RunActivity()
        {
            try
            {
                _activityDispatcher.StartTracking(SelectedActivity);
                UpdateActivity();
            }
            catch (ApplicationException e)
            {
                Logger.Log.Debug(e.Message);
            }
            catch (Exception e)
            {
                Logger.Log.Debug(e.Message);
            }
        }
    }

}