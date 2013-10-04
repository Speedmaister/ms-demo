using FreeJustBelot.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Collections;
using Windows.Security.Credentials;
using Windows.Storage;

namespace FreeJustBelot.Services
{
    public class SettingsService
    {
        private StorageFolder roamingStorage = Windows.Storage.ApplicationData.Current.RoamingFolder;
        private PasswordVault vault;
        private const string VaultSource = "userprofile";
        private const string FriendsFileName = "friends.json";
        private string username;

        public SettingsService()
        {
            this.vault = new PasswordVault();
        }

        public void SaveProfileToLocalSettings(string username, string password)
        {
            vault.Add(new PasswordCredential(VaultSource, username, password));
            this.username = username;
        }

        public LoginModel LoadProfileFromLocalSettings()
        {
            try
            {
                var profile = vault.FindAllByResource(VaultSource).FirstOrDefault();
                this.username = profile.UserName;
                LoginModel model = new LoginModel { Username = profile.UserName, Password = vault.Retrieve(VaultSource, profile.UserName).Password };
                return model;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public void DeleteProfileFromLocalSettings()
        {
            vault.Remove(vault.Retrieve(VaultSource, this.username));
        }

        public async Task<List<FriendModel>> GetFriendsFromRoamingFile()
        {
            try
            {
                var file = await this.roamingStorage.GetFileAsync(FriendsFileName);
                var content = await FileIO.ReadTextAsync(file);
                List<FriendModel> friends = await JsonConvert.DeserializeObjectAsync<List<FriendModel>>(content);
                return friends;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async void CreateFriendsListInRoamingFile()
        {
            var file = await this.roamingStorage.CreateFileAsync(FriendsFileName);
        }

        public async Task SaveNewFriendToList(FriendModel newFriend)
        {
            try
            {
                var file = await this.roamingStorage.GetFileAsync(FriendsFileName);
                var content = await FileIO.ReadTextAsync(file);
                List<FriendModel> friends = await JsonConvert.DeserializeObjectAsync<List<FriendModel>>(content);
                if (friends == null)
                {
                    friends = new List<FriendModel>();
                }

                friends.Add(newFriend);
                string friendsListString = await JsonConvert.SerializeObjectAsync(friends);
                await FileIO.WriteTextAsync(file, friendsListString);
            }
            catch (Exception ex)
            {
                return;
            }
        }
    }
}
