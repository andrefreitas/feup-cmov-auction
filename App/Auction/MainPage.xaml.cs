using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json.Linq;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace Auction
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("id"))
            {
                Frame.Navigate(typeof(HomePage));
            }
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

        private void registerButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(RegisterPage));
        }

        private async void loginButton_Click(object sender, RoutedEventArgs e)
        {
 
            String email = emailTextBox.Text;
            String password = passwordTextBox.Password;

  
            if (!Helpers.emailIsValid(email))
            {
                MessageDialog msgbox = new MessageDialog("O email é inválido!");
                await msgbox.ShowAsync();
                return;
            }

            if (!Helpers.passwordIsValid(password))
            {
                MessageDialog msgbox = new MessageDialog("A password tem que ter mais de 4 caracteres!");
                await msgbox.ShowAsync();
                return;
            }

            try
            {
                JObject json = await API.login(email, password);
                String id = (string)json["id"];
                ApplicationData.Current.LocalSettings.Values["id"] = id;
                Frame.Navigate(typeof(HomePage));
            }
            catch (Exception ex)
            {
                String msg = ex.Message;
                if (msg == "404")
                {
                    MessageDialog msgbox = new MessageDialog("Login inválido");
                    msgbox.ShowAsync();
                }
                else
                {
                    MessageDialog msgbox = new MessageDialog("Sem ligação à Internet");
                    msgbox.ShowAsync();
                }

            }

        }


  

    
    }
}
