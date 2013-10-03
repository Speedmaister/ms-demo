using FreeJustBelot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Collections;
using Windows.Security.Credentials;

namespace FreeJustBelot.Services
{
    public class SettingsService
    {
        private IPropertySet roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings.Values;
        private PasswordVault vault;
        private const string VaultSource = "userprofile";
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
    }
}
