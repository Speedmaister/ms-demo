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

namespace FreeJustBelot.ViewModels
{
    public class HomeViewModel : BindableBase
    {
        private ICommand refreshCommand;
        private INavigationService navigation;
        private ICommand createGame;
        private ICommand joinGame;
        private SettingsService settings;
        public IEnumerable<GameModel> AllGames { get; set; }
        public ICommand LogOutCommand { get; set; }
        public CreateGameModel NewGame { get; set; }
        public string GamePassword { get; set; }
        public WaitForGameToStartViewModel WaitVM { get; set; }

        public HomeViewModel(INavigationService navigation,SettingsService settings)
        {
            this.settings = settings;
            this.navigation = navigation;
            this.NewGame = new CreateGameModel();
            this.LogOutCommand = new DelegateCommand<object>(this.HandleLogoutCommand);
            this.WaitVM = new WaitForGameToStartViewModel(this.navigation);
        }

        public ICommand JoinGame
        {
            get
            {
                if (this.joinGame == null)
                {
                    this.joinGame = new DelegateCommand<GameModel>(this.HandleJoinGameCommand);
                }

                return this.joinGame;
            }
        }

        public ICommand CreateGame
        {
            get
            {
                if (this.createGame == null)
                {
                    this.createGame = new DelegateCommand<object>(this.HandleCreateCommand);
                }

                return this.createGame;
            }
        }

        public ICommand Refresh
        {
            get
            {
                if (this.refreshCommand == null)
                {
                    this.refreshCommand = new DelegateCommand<object>(this.HandleRefreshCommand);
                }

                return this.refreshCommand;
            }
        }

        private async void HandleJoinGameCommand(GameModel parameter)
        {
            JoinGameModel model = new JoinGameModel();
            model.GameName = parameter.Name;
            model.Host = parameter.Host;
            model.Password = this.GamePassword;
            var response = await DataPersister.JoinGameAsync(model, LoginViewModel.sessionKey);
            if (response.Message == "Joined.")
            {
                this.navigation.Navigate(Views.WaitRoom);
                this.WaitVM.IsVisible = false;
                this.WaitVM.SetAndStartConnection(model.GameName);
            }
        }

        private async void HandleCreateCommand(object parameter)
        {
            string gameName = this.NewGame.Name;
            string password = this.NewGame.Password;

            this.NewGame.Name = "";
            this.NewGame.Password = "";
            this.OnPropertyChanged("NewGame");
            var gameTobeCreated = new CreateGameModel { Name = gameName, Password = password };

            var response = await DataPersister.CreateGameAsync(gameTobeCreated, LoginViewModel.sessionKey);
            if (response.Message == "Created.")
            {
                this.navigation.Navigate(Views.WaitRoom);
                this.WaitVM.IsVisible = true;
                this.WaitVM.SetAndStartConnection(gameName);
                return;
            }
        }

        private void HandleRefreshCommand(object parameter)
        {
            LoadGamesAsync();
        }

        private async void HandleLogoutCommand(object parameter)
        {
            await DataPersister.LogoutUser(LoginViewModel.sessionKey);
            LoginViewModel.sessionKey = null;
            LoginViewModel.nickname = null;
            this.settings.DeleteProfileFromLocalSettings();
            this.navigation.Navigate(Views.LoginRegister);
        }

        private async void LoadGamesAsync()
        {
            var albums = await DataPersister.GetAllGames(LoginViewModel.sessionKey);
            this.AllGames = albums;
            this.OnPropertyChanged("AllGames");
        }

    }
}
