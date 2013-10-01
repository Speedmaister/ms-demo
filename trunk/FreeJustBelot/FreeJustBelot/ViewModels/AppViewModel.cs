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

        public AppViewModel()
            : base()
        {
            this.navigation = new NavigationService();
            this.LoginVM = new LoginViewModel(this.navigation);
            this.HomeVM = new HomeViewModel(this.navigation);
        }

        public LoginViewModel LoginVM { get; set; }
        public HomeViewModel HomeVM { get; set; }

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
