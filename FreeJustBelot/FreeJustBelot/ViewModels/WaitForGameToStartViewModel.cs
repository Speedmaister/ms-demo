﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client.Hubs;
using FreeJustBelot.Data;
using FreeJustBelot.Models;
using FreeJustBelot.Common;
using Newtonsoft.Json;
using Windows.UI.Core;
using Windows.UI.Xaml;
using System.Windows.Input;
using FreeJustBelot.Commands;
using FreeJustBelot.Services;

namespace FreeJustBelot.ViewModels
{
    public class WaitForGameToStartViewModel : BindableBase
    {
        private bool isGameReadyToStart;
        public List<string> TeamA { get; set; }
        public List<string> TeamB { get; set; }
        private List<string> Players { get; set; }
        public bool IsVisible { get; set; }
        private ICommand startGameCommand;
        public ICommand StartGameCommand
        {
            get
            {
                if (this.startGameCommand == null)
                {
                    this.startGameCommand = new DelegateCommand<object>(this.HandleStartGameCommand);
                }

                return this.startGameCommand;
            }
        }

        private void HandleStartGameCommand(object parameter)
        {
            this.navigation.Navigate(Views.Game);
        }

        private string gameName;

        private IHubProxy hub;
        private HubConnection connection;
        public event EventHandler updatePlayersList;
        private INavigationService navigation;

        public bool IsGameReadyToStart
        {
            get
            {
                return this.isGameReadyToStart;
            }
            set
            {
                this.isGameReadyToStart = value;
                this.OnPropertyChanged("IsGameReadyToStart");
            }
        }

        public WaitForGameToStartViewModel(INavigationService navigation)
        {
            this.navigation = navigation;
            this.IsGameReadyToStart = false;
        }

        public void SetAndStartConnection(string gameName)
        {
            this.OnPropertyChanged("IsVisible");
            this.gameName = gameName;
            connection = new HubConnection("http://freejustbelot.apphb.com/");
            //connection = new HubConnection(DataPersister.GetBaseUrl() + "signalr");
            hub = connection.CreateHubProxy("JustBelotWaitRoom");
            hub.On("PlayerJoinedRoom", data =>
            {
                RoomModel roomModel = JsonConvert.DeserializeObject<RoomModel>(data.ToString());
                this.Players = roomModel.Players;
                this.TeamA = new List<string>() { this.Players[0], this.Players[2] };
                this.TeamB = new List<string>() { this.Players[1], this.Players[3] };
                this.updatePlayersList(this, null);
            });

            hub.On("PlayerLeftRoom", data =>
            {
                string player = data.ToString();
                this.Players.Remove(player);
                this.TeamA.Remove(player);
                this.TeamB.Remove(player);
                this.updatePlayersList(this, null);
            });

            this.ConnectToHub(LoginViewModel.sessionKey, gameName);
        }

        public void RefreshList()
        {
            this.OnPropertyChanged("TeamB");
            this.OnPropertyChanged("TeamA");
            if (this.Players.Count == 4)
            {
                this.IsGameReadyToStart = true;
            }
        }

        public async void ConnectToHub(string sessionKey, string gameName)
        {
            await connection.Start();
            await hub.Invoke("JoinRoom", gameName);
        }

        public async Task<bool> LeaveGame()
        {
            var response = await DataPersister.LeaveGameAsync(this.gameName, LoginViewModel.sessionKey);
            if (response.Message != "Left.")
            {
                throw new InvalidOperationException("Something went wrong.");
            }

            await hub.Invoke("LeaveRoom", this.gameName,LoginViewModel.nickname);

            this.connection.Stop();
            this.hub = null;
            this.connection = null;
            this.Players = null;
            this.TeamA = null;
            this.TeamB = null;
            this.gameName = null;

            return true;
        }
    }
}
