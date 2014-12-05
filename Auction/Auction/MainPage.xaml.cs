using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Auction.Resources;
using Newtonsoft.Json.Linq;
using Windows.Storage;

namespace Auction
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}

        private void registerButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/RegisterPage.xaml", UriKind.Relative));
        }

        private async void loginButton_Click(object sender, RoutedEventArgs e)
        {
            String email = emailTextBox.Text;
            String password = passwordTextBox.Password;


            if (!Helpers.emailIsValid(email))
            {
                MessageBox.Show("O email é inválido!");
                return;
            }

            if (!Helpers.passwordIsValid(password))
            {
                MessageBox.Show("A password tem que ter mais de 4 caracteres!");
                return;
            }

            try
            {
                JObject json = await API.login(email, password);
                String id = (string)json["id"];
                ApplicationData.Current.LocalSettings.Values["id"] = id;

                JArray auctions = await API.getAuctions();
                JArray openAuctions = (JArray)auctions.Where(a => (String)a["state"] == "open");
                if (openAuctions.Count != 0)
                {
                    if ((String)json["auction"] != "")
                    {
                        this.NavigationService.Navigate(new Uri("/BidPage.xaml", UriKind.Relative));
                    }
                    else this.NavigationService.Navigate(new Uri("/HomePage.xaml", UriKind.Relative));
                }
                else
                {
                    this.NavigationService.Navigate(new Uri("/LastAuctionPage.xaml", UriKind.Relative));
                }

                
            }
            catch (Exception ex)
            {
                String msg = ex.Message;
                if (msg == "404")
                {
                    MessageBox.Show("Login inválido");
                }
                else
                {
                    MessageBox.Show("Sem ligação à Internet");
                }

            }
        }
    }
}