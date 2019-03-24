
using System;
using System.IO;
using System.Collections.Generic;

using Microsoft.Win32;

using TimeTracker.Models;
using TimeTracker.Helpers;
using TimeTracker.Service;
using TimeTracker.EventBus;

namespace TimeTracker.ViewModels
{
    public class ProfileSettingsViewModel : DomainModelBase
    {
        public ProfileSettingsViewModel(ApplicationEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
            CreateEventHubSubscriptions();

            ChangeAvatarCommand = new RelayCommand(ChangeAvatarHandler);
            ChangeNameCommand = new RelayCommand(ChangeNameHandler);
            ChangePasswordCommand = new RelayCommand(ChangePasswordHandler);
        }

        #region Props and Fields

        public RelayCommand ChangeNameCommand { get; set; }
        public RelayCommand ChangePasswordCommand { get; set; }
        public RelayCommand ChangeAvatarCommand { get; set; }
        private ApplicationEnvironment _appEnvironment;

        private String _email;
        public String Email
        {
            get { return _email; }
            set { SetProperty(ref _email, value); }
        }

        private String _name;
        public String Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private byte[] _avatarBytes = ImageGenerator.GenerateEmptyAvatarImage("RS");
        public byte[] AvatarBytes
        {
            get { return _avatarBytes; }
            set { SetProperty(ref _avatarBytes, value); }
        }

        private bool _isViewVisible;
        public bool IsViewVisible
        {
            get { return _isViewVisible; }
            set { SetProperty(ref _isViewVisible, value); }
        }

        #endregion

        private void OnSyncDataFromServer(NeedServerSyncMessage m)
        {
            GetAvatarHandler();
            GetProfileInfoHandler();
        }

        private void ChangeNameHandler()
        {
        }

        private void ChangePasswordHandler()
        {
        }

        private void GetProfileInfoHandler()
        {
            try
            {
                if (_appEnvironment.ServiceConnector == null)
                    throw new NullReferenceException($"Null reference of { nameof(_appEnvironment.ServiceConnector) }");

                var response = _appEnvironment.ServiceConnector.Get<EmptyRequestBodyDto, ProfileDto, ErrorMsgDto>(new Request<EmptyRequestBodyDto>
                {
                    AuthToken = new KeyValuePair<string, string>("session_token", SessionStorage.Instance.SessionId),
                    UrlSegments = new Dictionary<string, string>() { { "id", SessionStorage.Instance.ProfileId } },
                    OpName = OperationType.GetProfileInfo
                });

                if (response.SuccessBody != null)
                {
                    if (!String.IsNullOrEmpty(response.SuccessBody.UserName))
                        Name = response.SuccessBody.UserName;
                    else
                        Name = "Имя";

                    if (!String.IsNullOrEmpty(response.SuccessBody.Email))
                        Email = response.SuccessBody.Email;
                    else
                        Email = "Email";
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

        private String GetAvatarLocalCopyPath()
        {
            var avatarFileName = "avatar_" + SessionStorage.Instance.ProfileId;
            var avatarFilePath = String.Empty;
            var configsDir = DefaultRegistryValues.GetConfigsDir();
            if (configsDir != null)
            {
                avatarFilePath = Path.Combine(configsDir, avatarFileName);
            }
            else
            {
                avatarFilePath = avatarFileName;
            }

            return avatarFilePath;
        }

        private void GetAvatarHandler()
        {
            try
            {
                if (_appEnvironment.ServiceConnector == null)
                    throw new NullReferenceException($"Null reference of { nameof(_appEnvironment.ServiceConnector) }");

                var avatarFilePath = GetAvatarLocalCopyPath();

                if (!File.Exists(avatarFilePath))
                {
                    var response = _appEnvironment.ServiceConnector.DownloadFile<EmptyRequestBodyDto, ErrorMsgDto>(new Request<EmptyRequestBodyDto>
                    {
                        AuthToken = new KeyValuePair<string, string>("session_token", SessionStorage.Instance.SessionId),
                        UrlSegments = new Dictionary<string, string>() { { "id", SessionStorage.Instance.ProfileId } },
                        OpName = OperationType.GetProfileAvatar
                    });

                    if (response.SuccessBody != null)
                    {
                        File.WriteAllBytes(avatarFilePath, response.SuccessBody);
                        AvatarBytes = response.SuccessBody;
                    }
                    else
                    {

                    }
                }
                else
                {
                    AvatarBytes = File.ReadAllBytes(avatarFilePath);
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

        private void ChangeAvatarHandler()
        {
            try
            {
                if (_appEnvironment.ServiceConnector == null)
                    throw new NullReferenceException($"Null reference of { nameof(_appEnvironment.ServiceConnector) }");

                var openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == true)
                {
                    var rawBytes = File.ReadAllBytes(openFileDialog.FileName);
                    AvatarBytes = rawBytes;

                    var avatarLocalCopyFilePath = GetAvatarLocalCopyPath();
                    File.WriteAllBytes(avatarLocalCopyFilePath, rawBytes);

                    var response = _appEnvironment.ServiceConnector.UploadFile<EmptyRequestBodyDto, SuccessMsgDto, ErrorMsgDto>(new Request<EmptyRequestBodyDto>
                    {
                        AuthToken = new KeyValuePair<string, string>("session_token", SessionStorage.Instance.SessionId),
                        UrlSegments = new Dictionary<string, string>() { { "id", SessionStorage.Instance.ProfileId } },
                        OpName = OperationType.UpdateProfileAvatar
                    }, 
                    new KeyValuePair<string, string>("avatar", openFileDialog.FileName));

                    if (response.SuccessBody != null)
                    {

                    }
                    else
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
        }

        private void CreateEventHubSubscriptions()
        {
            _appEnvironment.EventHub.Subscribe<ShowProfileSettingsWindowMessage>(OnShowProfileSettingsWindowHandler);
            _appEnvironment.EventHub.Subscribe<NeedServerSyncMessage>(OnSyncDataFromServer);
        }

        public void OnCloseProfileSettingsWindowHandler()
        {
            IsViewVisible = false;
        }

        public void OnShowProfileSettingsWindowHandler(ShowProfileSettingsWindowMessage m)
        {
            IsViewVisible = true;
        }
    }
}
