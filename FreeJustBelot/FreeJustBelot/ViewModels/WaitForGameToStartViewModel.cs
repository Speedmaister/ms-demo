using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client.Hubs;
using FreeJustBelot.Data;
using FreeJustBelot.Models;
using FreeJustBelot.Common;

namespace FreeJustBelot.ViewModels
{
    public class WaitForGameToStartViewModel : BindableBase
    {
        public IEnumerable<string> Players { get; set; }
        private IHubProxy hub;
        private HubConnection connection;

        public WaitForGameToStartViewModel(string gameName)
        {
            connection = new HubConnection(DataPersister.GetBaseUrl() + "signalr", useDefaultUrl: false);
            //connection = new HubConnection(DataPersister.GetBaseUrl() + "signalr");
            hub = connection.CreateHubProxy("JustBelotWaitRoom");
            hub.On("joinGame", data =>
                {
                    var roomModel = data as RoomModel;
                    this.Players = roomModel.Players;
                    this.OnPropertyChanged("Players");
                });

            this.ConnectToHub(LoginViewModel.sessionKey, gameName);
        }

        public async void ConnectToHub(string sessionKey, string gameName)
        {
            await connection.Start();
            await hub.Invoke("JoinGame", sessionKey, gameName);
        }
    }
}
