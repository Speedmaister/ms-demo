﻿using FreeJustBelot.Commands;
using FreeJustBelot.Common;
using FreeJustBelot.Data;
using FreeJustBelot.Models;
using FreeJustBelot.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;

namespace FreeJustBelot.ViewModels
{
    public class LoginViewModel : BindableBase
    {
        private SettingsService settings;
        private INavigationService navigation;
        private ICommand showRegisterForm;
        private ICommand showLoginForm;
        private ICommand register;
        private ICommand login;
        private bool isRegisterVisible = false;
        private bool isLoginVisible = true;
        private string pageTitle = "Login";
        private bool isValidData;
        private bool isProgressBarVisible;

        public static string sessionKey;

        public static string nickname;

        public LoginViewModel(INavigationService navigation, SettingsService settings)
        {
            this.settings = settings;
            this.IsValidData = false;
            this.navigation = navigation;
            this.UserRegisterForm = new UserModel();
        }

        public string RegisterErrorMessage { get; set; }

        public string LoginErrorMessage { get; set; }

        public string PageTitle
        {
            get
            {
                return this.pageTitle;
            }

            set
            {
                this.pageTitle = value;
                this.OnPropertyChanged("PageTitle");
            }
        }

        public bool IsValidData
        {
            get
            {
                return this.isValidData;
            }

            set
            {
                this.isValidData = value;
                this.OnPropertyChanged("IsValidData");
            }
        }

        public bool IsLoginVisible
        {
            get
            {
                return this.isLoginVisible;
            }

            set
            {
                this.isLoginVisible = value;
                this.OnPropertyChanged("IsLoginVisible");
            }
        }

        public bool IsProgressBarVisible
        {
            get
            {
                return this.isProgressBarVisible;
            }

            set
            {
                this.isProgressBarVisible = value;
                this.OnPropertyChanged("IsProgressBarVisible");
            }
        }

        public bool IsRegisterVisible
        {
            get
            {
                return this.isRegisterVisible;
            }

            set
            {
                this.isRegisterVisible = value;
                this.OnPropertyChanged("IsRegisterVisible");
            }
        }

        public UserModel UserRegisterForm { get; set; }

        public ICommand ShowLoginForm
        {
            get
            {
                if (this.showLoginForm == null)
                {
                    this.showLoginForm = new DelegateCommand<object>(this.HandleShowLoginFormCommand);
                }

                return this.showLoginForm;
            }
        }

        public ICommand ShowRegisterForm
        {
            get
            {
                if (this.showRegisterForm == null)
                {
                    this.showRegisterForm = new DelegateCommand<object>(this.HandleShowRegisterFormCommand);
                }

                return this.showRegisterForm;
            }
        }

        public ICommand Login
        {
            get
            {
                if (this.login == null)
                {
                    this.login = new DelegateCommand<LoginModel>(this.HandleLoginCommand);
                }

                return this.login;
            }
        }

        public ICommand Register
        {
            get
            {
                if (this.register == null)
                {
                    this.register = new DelegateCommand<object>(this.HandleRegisterCommand);
                }

                return this.register;
            }
        }

        public SettingsService Settings
        {
            get
            {
                return this.settings;
            }
        }

        private async void HandleRegisterCommand(object parameter)
        {
            var userName = this.UserRegisterForm.Username;
            var nickName = this.UserRegisterForm.Nickname;
            var password = this.UserRegisterForm.Password;

            this.OnPropertyChanged("UserRegisterForm");
            string authCode = GetAuthCode(userName, password);

            UserModel registrationModel = new UserModel
            {
                Username = userName,
                Nickname = nickName,
                AuthCode = authCode
            };

            try
            {
                var response = await DataPersister.RegisterUser(registrationModel);
                LoginViewModel.sessionKey = response.SessionKey;
                LoginViewModel.nickname = response.Nickname;
                this.navigation.Navigate(Views.Home);
                this.UserRegisterForm.Nickname = "";
                this.UserRegisterForm.Password = "";
                this.IsValidData = false;
                this.settings.SaveProfileToLocalSettings(registrationModel.Username, registrationModel.AuthCode);
                this.settings.CreateFriendsListInRoamingFile();
            }
            catch (FormatException ex)
            {
                this.IsValidData = true;
                RegisterErrorMessage = ex.Message;
                this.OnPropertyChanged("RegisterErrorMessage");
            }

            this.IsProgressBarVisible = false;

            this.OnPropertyChanged("UserRegisterForm");
        }

        private async void HandleLoginCommand(LoginModel parameter)
        {
            string userName;
            string password;
            string authCode;
            if (parameter == null)
            {
                userName = this.UserRegisterForm.Username;
                password = this.UserRegisterForm.Password;
                authCode = GetAuthCode(userName, password);
            }
            else
            {
                userName = parameter.Username;
                authCode = parameter.Password;
            }

            UserModel loginModel = new UserModel
            {
                Username = userName,
                AuthCode = authCode
            };

            try
            {
                var response = await DataPersister.LoginUser(loginModel);
                LoginViewModel.sessionKey = response.SessionKey;
                LoginViewModel.nickname = response.Nickname;
                this.navigation.Navigate(Views.Home, userName);
                this.UserRegisterForm.Password = "";
                this.IsValidData = false;
                this.settings.SaveProfileToLocalSettings(loginModel.Username, loginModel.AuthCode);
            }
            catch (FormatException ex)
            {
                this.LoginErrorMessage = ex.Message;
                this.OnPropertyChanged("LoginErrorMessage");
                this.IsValidData = true;
            }

            this.IsProgressBarVisible = false;
            this.UserRegisterForm.Password = "";

            this.OnPropertyChanged("UserRegisterForm");
        }

        private void HandleShowLoginFormCommand(object parameter)
        {
            this.PageTitle = "Login";
            this.IsRegisterVisible = false;
            this.IsLoginVisible = true;
        }

        private void HandleShowRegisterFormCommand(object parameter)
        {
            this.PageTitle = "Register";
            this.IsRegisterVisible = true;
            this.IsLoginVisible = false;
        }

        private string GetAuthCode(string userName, string password)
        {
            string authCode = null;
            var bufferForPassword = CryptographicBuffer.ConvertStringToBinary(password, BinaryStringEncoding.Utf8);
            var bufferForUsername = CryptographicBuffer.ConvertStringToBinary(userName, BinaryStringEncoding.Utf8);
            var provider = SymmetricKeyAlgorithmProvider.OpenAlgorithm("RC4");
            var key = provider.CreateSymmetricKey(bufferForPassword);
            var encrypted = CryptographicEngine.Encrypt(key, bufferForUsername, null);
            authCode = CryptographicBuffer.EncodeToBase64String(encrypted);
            return authCode;
        }

    }
}
