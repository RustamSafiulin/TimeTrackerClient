
using System;
using System.Diagnostics;

using TimeTracker.Models;
using TimeTracker.Helpers;
using TimeTracker.Service;
using TimeTracker.EventBus;

namespace TimeTracker.ViewModels
{
    public class RegisrationInfoDomainModel : DomainModelBase
    {
        public RegisrationInfoDomainModel()
        { }

        private string _email;
        public string Email
        {
            get { return _email; }
            set { SetProperty(ref _email, value); }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

        private string _username;
        public string UserName
        {
            get { return _username; }
            set { SetProperty(ref _username, value); }
        }
    }

    public class LoginDomainModel : DomainModelBase
    {
        public LoginDomainModel()
        { }

        #region props and fields
        
        private string _email;
        public string Email
        {
            get { return _email; }
            set { SetProperty(ref _email, value); }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

        private bool _rememberMe;
        public bool RememberMe
        {
            get { return _rememberMe; }
            set { SetProperty(ref _rememberMe, value); }
        }

        #endregion
    }


    public class EntryViewModel : DomainModelBase
    {
        public EntryViewModel(ApplicationEnvironment appApplicationEnvironment)
        {
            LoginModel = new LoginDomainModel();
            RegInfoModel = new RegisrationInfoDomainModel();

            LoginCommand = new RelayCommand(Login);
            SignupCommand = new RelayCommand(Signup);

            _appApplicationEnvironment = appApplicationEnvironment;
            CreateEventHubSubscriptions();

            LoginModel.RememberMe = _appApplicationEnvironment.AppConfiguration.StoredParameters.RememberMe;
            LoginModel.Email = _appApplicationEnvironment.AppConfiguration.StoredParameters.Email;
            LoginModel.Password = _appApplicationEnvironment.AppConfiguration.StoredParameters.Password;

        }

        #region Props and Fields

        private ApplicationEnvironment _appApplicationEnvironment;

        public LoginDomainModel LoginModel { get; private set; }
        public RegisrationInfoDomainModel RegInfoModel { get; private set; }

        public RelayCommand LoginCommand { get; set; }
        public RelayCommand SignupCommand { get; set; }

        private bool _isViewVisible = true;
        public bool IsViewVisible
        {
            get { return _isViewVisible; }
            set { SetProperty(ref _isViewVisible, value); }
        }

        #endregion

        private void CreateEventHubSubscriptions()
        {
            _appApplicationEnvironment.EventHub.Subscribe<LogoutMessage>(OnLogoutHandler);
        }

        public void OnLogoutHandler(LogoutMessage m)
        {
            IsViewVisible = true;
        }

        public void OnAppNeedExitHandler()
        {
            if (LoginModel.RememberMe)
            {
                _appApplicationEnvironment.AppConfiguration.StoredParameters.RememberMe = true;
                _appApplicationEnvironment.AppConfiguration.StoredParameters.Email = LoginModel.Email;
                _appApplicationEnvironment.AppConfiguration.StoredParameters.Password = LoginModel.Password;
            }

            _appApplicationEnvironment.EventHub.Publish(new AppExitMessage());
        }

        private void Login()
        {
            try
            {
                if (_appApplicationEnvironment.ServiceConnector == null)
                    throw new NullReferenceException($"Null reference of { nameof(_appApplicationEnvironment.ServiceConnector) }");

                var profileDto = new ProfileDto { Email = LoginModel.Email, Password = LoginModel.Password };
                var response = _appApplicationEnvironment.ServiceConnector.Post<ProfileDto, SessionInfoDto, ErrorMsgDto>(new Request<ProfileDto>
                {
                    Body = profileDto,
                    OpName = OperationType.Signin
                });

                if (response.SuccessBody != null)
                {
                    Debug.WriteLine($"Success login at { DateTime.Now.ToString("HH:mm:ss tt") }");
                    IsViewVisible = false;
                    _appApplicationEnvironment.EventHub.Publish(new LoginMessage() { SessionInfo = response.SuccessBody });
                }
            }
            catch (ApplicationException e)
            {
                Debug.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        private void Signup()
        {
            try
            {
                if (_appApplicationEnvironment.ServiceConnector == null)
                    throw new NullReferenceException($"Null reference of { nameof(_appApplicationEnvironment.ServiceConnector) }");

                var profileDto = new ProfileDto { Email = RegInfoModel.Email, Password = RegInfoModel.Password, UserName = RegInfoModel.UserName };
                var response = _appApplicationEnvironment.ServiceConnector.Post<ProfileDto, SuccessMsgDto, ErrorMsgDto>(new Request<ProfileDto>
                {
                    Body = profileDto,
                    OpName = OperationType.Signup
                });

                if (response.SuccessBody != null)
                {
                    Debug.WriteLine($"Success registration at { DateTime.Now.ToString("HH:mm:ss tt") }");
                    _appApplicationEnvironment.EventHub.Publish(new SignupMessage() { SuccessMsg = response.SuccessBody });
                }
            }
            catch (ApplicationException e)
            {
                Debug.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }
    }
}