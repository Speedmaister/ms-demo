﻿using FreeJustBelot.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace FreeJustBelot.Pages
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class HomePage : FreeJustBelot.Common.LayoutAwarePage
    {
        public HomePage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var context = this.DataContext as HomeViewModel;
            context.LoadFriends(e.Parameter);
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }

        private void LoadGames(object sender, RoutedEventArgs e)
        {
            var homeVM = this.DataContext as HomeViewModel;
            homeVM.Refresh.Execute(null);
        }

        private void OnCreateGameButtonClick(object sender, RoutedEventArgs e)
        {
            this.AddFriendPanel.Visibility = Visibility.Collapsed;
            this.JoinGamePanel.Visibility = Visibility.Collapsed;
            this.CreateGamePanel.Visibility = Visibility.Visible;
            this.BottomAppBar.IsOpen = false;
            this.TopAppBar.IsOpen = false;
        }

        private void OnJoinGameButtonClick(object sender, RoutedEventArgs e)
        {
            this.AddFriendPanel.Visibility = Visibility.Collapsed;
            this.CreateGamePanel.Visibility = Visibility.Collapsed;
            this.JoinGamePanel.Visibility = Visibility.Visible;
            this.BottomAppBar.IsOpen = false;
            this.TopAppBar.IsOpen = false;
        }

        private void OnAddFriendButtonClick(object sender, RoutedEventArgs e)
        {
            this.AddFriendPanel.Visibility = Visibility.Visible;
            this.CreateGamePanel.Visibility = Visibility.Collapsed;
            this.JoinGamePanel.Visibility = Visibility.Collapsed;
            this.BottomAppBar.IsOpen = false;
            this.TopAppBar.IsOpen = false;
        }

        private void OnSubmitButtonClicked(object sender, RoutedEventArgs e)
        {
            this.AddFriendPanel.Visibility = Visibility.Collapsed;
            this.CreateGamePanel.Visibility = Visibility.Collapsed;
            this.JoinGamePanel.Visibility = Visibility.Collapsed;
        }
    }
}
