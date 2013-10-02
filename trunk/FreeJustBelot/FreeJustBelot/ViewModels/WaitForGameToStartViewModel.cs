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
using Newtonsoft.Json;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace FreeJustBelot.ViewModels
{
    public class WaitForGameToStartViewModel : BindableBase
    {
        private IEnumerable<string> Players { get; set; }
        public List<string> TeamA { get; set; }
        public List<string> TeamB { get; set; }
        private IHubProxy hub;
        private HubConnection connection;
        public event EventHandler updatePlayersList;

        public WaitForGameToStartViewModel()
        {
            this.TeamA = new List<string>();
            this.TeamB = new List<string>();
        }

        public async void SetAndStartConnection(string gameName)
        {
            connection = new HubConnection("http://freejustbelot.apphb.com/");
            //connection = new HubConnection(DataPersister.GetBaseUrl() + "signalr");
            hub = connection.CreateHubProxy("JustBelotWaitRoom");
            hub.On("joinGame", data =>
            {
                RoomModel roomModel = JsonConvert.DeserializeObject<RoomModel>(data.ToString());
                this.Players = roomModel.Players;
                this.updatePlayersList(this, null);
               // App.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,() => this.OnPropertyChanged("Players"));
            });

            this.ConnectToHub(LoginViewModel.sessionKey, gameName);
        }

        public void RefreshList()
        {
            this.TeamA.Clear();
            this.TeamB.Clear();
            this.TeamA.Add(this.Players.ElementAt(0));
            this.TeamA.Add(this.Players.ElementAt(2));
            this.TeamB.Add(this.Players.ElementAt(1));
            this.TeamB.Add(this.Players.ElementAt(3));
            this.OnPropertyChanged("TeamA");
            this.OnPropertyChanged("TeamB");
        }

        public async void ConnectToHub(string sessionKey, string gameName)
        {
            await connection.Start();
            await hub.Invoke("JoinGame", sessionKey, gameName);
        }
    }
}
