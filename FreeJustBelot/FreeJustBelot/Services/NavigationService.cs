using FreeJustBelot.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace FreeJustBelot.Services
{
    public class NavigationService : INavigationService
    {
        private Type GetViewType(Views view)
        {
            switch (view)
            {
                case Views.LoginRegister:
                    return typeof(LoginRegisterPage);
                case Views.Home:
                    return typeof(HomePage);
                case Views.WaitRoom:
                    return typeof(WaitForGameToStart);
                default:
                    break;
            }

            return null;
        }

        public void Navigate(Views sourcePageType)
        {
            var pageType = this.GetViewType(sourcePageType);

            if (pageType != null)
            {

                ((Frame)Window.Current.Content).Navigate(pageType);
            }
        }

        public void Navigate(Views sourcePageType, object parameter)
        {
            var pageType = this.GetViewType(sourcePageType);

            if (pageType != null)
            {
                ((Frame)Window.Current.Content).Navigate(pageType, parameter);
            }
        }

        public void GoBack()
        {
            ((Frame)Window.Current.Content).GoBack();
        }
    }
}
