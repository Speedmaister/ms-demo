using FreeJustBelot.Commands;
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
        private INavigationService navigation;
        private ICommand showRegisterForm;
        private ICommand showLoginForm;
        private ICommand register;
        private ICommand login;
        private bool isRegisterVisible = false;
        private bool isLoginVisible = true;
        private string pageTitle = "Login";

        public static string sessionKey;

        public static string nickname;
        private bool isValidData;

        public LoginViewModel(INavigationService navigation)
        {
            this.IsValidData = false;
            this.navigation = navigation;
            this.UserRegisterForm = new UserModel();
        }

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
                    this.login = new DelegateCommand<object>(this.HandleLoginCommand);
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

            var response = await DataPersister.RegisterUser(registrationModel);
            LoginViewModel.sessionKey = response.SessionKey;
            if (LoginViewModel.sessionKey != null)
            {
                LoginViewModel.nickname = response.Nickname;
                this.navigation.Navigate(Views.Home);
                this.UserRegisterForm.Nickname = "";
                this.UserRegisterForm.Password = "";
                this.IsValidData = false;
            }
            else
            {
                this.IsValidData = true;
            }

            this.OnPropertyChanged("UserRegisterForm");
        }

        private async void HandleLoginCommand(object parameter)
        {
            var userName = this.UserRegisterForm.Username;
            var password = this.UserRegisterForm.Password;

            string authCode = GetAuthCode(userName, password);

            UserModel loginModel = new UserModel
            {
                Username = userName,
                AuthCode = authCode
            };

            var response = await DataPersister.LoginUser(loginModel);
            LoginViewModel.sessionKey = response.SessionKey;
            if (LoginViewModel.sessionKey != null)
            {
                LoginViewModel.nickname = response.Nickname;
                this.navigation.Navigate(Views.Home);
                this.UserRegisterForm.Password = "";
                this.IsValidData = false;
            }
            else
            {
                this.UserRegisterForm.Password = "";
                this.IsValidData = true;
            }

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
