using FreeJustBelot.Commands;
using FreeJustBelot.Common;
using FreeJustBelot.Data;
using FreeJustBelot.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FreeJustBelot.ViewModels
{
    public class AppViewModel : BindableBase
    {
        private INavigationService navigation;
        private SettingsService settings;

        public AppViewModel()
            : base()
        {
            this.settings = new SettingsService();
            this.navigation = new NavigationService();
            this.LoginVM = new LoginViewModel(this.navigation,this.settings);
            this.HomeVM = new HomeViewModel(this.navigation,this.settings);
            this.SearchVM = new SearchViewModel(this.settings);
            this.GameVM = new GameViewModel();
        }

        public LoginViewModel LoginVM { get; set; }
        public HomeViewModel HomeVM { get; set; }
        public SearchViewModel SearchVM { get; set; }
        public GameViewModel GameVM { get; set; }

        public bool IsLogout()
        {
            if (LoginViewModel.sessionKey == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
