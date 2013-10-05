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
    public class SearchViewModel : BindableBase
    {
        private string queryText;
        private ICommand addSelectedPeopleCommand;
        private SettingsService settings;

        public string FriendName { get; set; }

        public SearchViewModel(SettingsService settings)
        {
            this.settings = settings;
        }

        public IEnumerable<string> Results { get; set; }

        public ICommand AddSelectedPeopleCommand
        {
            get
            {
                if (this.addSelectedPeopleCommand == null)
                {
                    this.addSelectedPeopleCommand = new DelegateCommand<object>(this.HandleAddFriendCommand);
                }

                return addSelectedPeopleCommand;
            }
        }

        private async void HandleAddFriendCommand(object parameter)
        {
                var response = await DataPersister.FindFriend(LoginViewModel.sessionKey, this.FriendName);
                if (response.Message == "Found.")
                {
                    await this.settings.SaveNewFriendToList(new FriendModel { FriendName = this.FriendName });
                }
        }

        public string QueryText
        {
            get
            {
                return this.queryText;
            }
            set
            {
                this.queryText = value;
                this.OnPropertyChanged("QueryText");
            }
        }

        public async void Search()
        {
            var allUsers = await DataPersister.GetAllUsers(LoginViewModel.sessionKey);
            Results = allUsers.Where(x => x.Contains(queryText));
            this.OnPropertyChanged("Results");
        }
    }
}
