
using System;
using System.Linq;
using System.Collections.Generic;

using TimeTracker.Models;
using TimeTracker.Helpers;
using TimeTracker.Service;
using TimeTracker.EventBus;

namespace TimeTracker.ViewModels
{
    public sealed class SettingsViewModel : EditableObject<SettingsViewModel>
    {
        public SettingsViewModel(ApplicationEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;

            SaveSettingsCommand = new RelayCommand(SaveSettingsHandler);
            CancelSaveSettingsCommand = new RelayCommand(CancelSaveSettingsHandler);

            _categories = new List<string>();
            _trackedSites = new List<string>();

            CreateEventHubSubscriptions();
        }

        #region Props and Fields

        private List<string> _categories;
        private List<string> _trackedSites;

        private ApplicationEnvironment _appEnvironment;

        private Boolean _needBeginNotify;
        public Boolean NeedBeginNotify
        {
            get { return _needBeginNotify; }
            set { SetProperty(ref _needBeginNotify, value); }
        }

        private Boolean _enablePopupNotify;
        public Boolean EnablePopupNotify
        {
            get { return _enablePopupNotify; }
            set { SetProperty(ref _enablePopupNotify, value); }
        }

        private Boolean _enableSoundNotify;
        public Boolean EnableSoundNotify
        {
            get { return _enableSoundNotify; }
            set { SetProperty(ref _enableSoundNotify, value); }
        }

        private String _categoriesRawText;
        public String CategoriesRawText
        {
            get { return _categoriesRawText; }
            set
            {
                SetProperty(ref _categoriesRawText, value);
                _categories = CommaSeparatedTextToList(_categoriesRawText);
            }
        }

        private String _sitesRawText;
        public String SitesRawText
        {
            get { return _sitesRawText; }
            set
            {
                SetProperty(ref _sitesRawText, value);
                _trackedSites = CommaSeparatedTextToList(_sitesRawText);
            }
        }

        public RelayCommand SaveSettingsCommand { get; set; }
        public RelayCommand CancelSaveSettingsCommand { get; set; }

        private List<string> CommaSeparatedTextToList(string text)
        {
            if (!String.IsNullOrEmpty(text))
            {
                var arr = text.Split(';');
                return arr.ToList();
            }

            return new List<string>();
        }

        private void CreateEventHubSubscriptions()
        {
            _appEnvironment.EventHub.Subscribe<NeedServerSyncMessage>(OnSyncDataFromServer);
        }
        
        private void OnSyncDataFromServer(NeedServerSyncMessage m)
        {
            GetSettingsHandler();
        }

        private void GetSettingsHandler()
        {
            try
            {
                if (_appEnvironment.ServiceConnector == null)
                    throw new NullReferenceException($"Null reference of { nameof(_appEnvironment.ServiceConnector) }");

                var response = _appEnvironment.ServiceConnector.Get<EmptyRequestBodyDto, SettingDto, ErrorMsgDto>(new Request<EmptyRequestBodyDto>
                {
                    AuthToken = new KeyValuePair<string, string>("session_token", SessionStorage.Instance.SessionId),
                    UrlSegments = new Dictionary<string, string>() { { "id", SessionStorage.Instance.ProfileId } },
                    OpName = OperationType.GetSettings
                });

                if (response.SuccessBody != null)
                {
                    using (UntrackedContext<SettingsViewModel>.Untrack(this))
                    {
                        if (response.SuccessBody.ActivityCategories != null)
                            CategoriesRawText = String.Join(";", response.SuccessBody.ActivityCategories);

                        if (response.SuccessBody.TrackedSites != null)
                            SitesRawText = String.Join(";", response.SuccessBody.TrackedSites);

                        NeedBeginNotify = response.SuccessBody.NotificationNeedStart;
                        EnableSoundNotify = response.SuccessBody.EnableSoundNotify;
                        EnablePopupNotify = response.SuccessBody.EnablePopupNotify;
                    }

                    _appEnvironment.EventHub.Publish<ChangeSettingsMessage>(new ChangeSettingsMessage() {
                        NeedBegin = NeedBeginNotify,
                        NeedPopupNotify = EnablePopupNotify,
                        NeedSoundNotify = EnableSoundNotify,
                        Categories = _categories
                    });
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

        private void SaveSettingsHandler()
        {
            try
            {
                if (_appEnvironment.ServiceConnector == null)
                    throw new NullReferenceException($"Null reference of { nameof(_appEnvironment.ServiceConnector) }");

                var settingDto = new SettingDto()
                {
                    ProfileId = SessionStorage.Instance.ProfileId,
                    NotificationNeedStart = NeedBeginNotify,
                    EnablePopupNotify = EnablePopupNotify,
                    EnableSoundNotify = EnableSoundNotify,
                    ActivityCategories = _categories,
                    TrackedSites = _trackedSites
                };

                var response = _appEnvironment.ServiceConnector.Post<SettingDto, SuccessMsgDto, ErrorMsgDto>(new Request<SettingDto>
                {
                    AuthToken = new KeyValuePair<string, string>("session_token", SessionStorage.Instance.SessionId),
                    UrlSegments = new Dictionary<string, string>() { { "id", SessionStorage.Instance.ProfileId } },
                    Body = settingDto,
                    OpName = OperationType.UpdateSettings,
                });

                if (response.SuccessBody != null)
                {

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
                if (IsDirty)
                {
                    AcceptChanges();
                    _appEnvironment.EventHub.Publish<ChangeSettingsMessage>(new ChangeSettingsMessage() {
                        NeedBegin = NeedBeginNotify,
                        NeedPopupNotify = EnablePopupNotify,
                        NeedSoundNotify = EnableSoundNotify,
                        Categories = _categories
                    });
                }
            }
        }

        private void CancelSaveSettingsHandler()
        {
            if (IsDirty)
                RejectChanges();
        }

        #endregion
    }
}
