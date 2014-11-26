using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using Newtonsoft.Json.Linq;
using Windows.Storage;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Auction
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RegisterPage : Page
    {
        public RegisterPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
        }

        private async void registerButton_Click(object sender, RoutedEventArgs e)
        {
            String name = nameTextBox.Text;
            String email = emailTextBox.Text;
            String password = passwordTextBox.Password;

            if(!Helpers.nameIsValid(name)){
                MessageDialog msgbox = new MessageDialog("O nome é inválido!");
                await msgbox.ShowAsync();
                return;
            }

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

            try  {
                JObject json = await API.register(name, email, password);
                String id = (string) json["id"];
                ApplicationData.Current.LocalSettings.Values["id"] = id;
                MessageDialog msgbox = new MessageDialog("Registo efetuado com sucesso!");
                await msgbox.ShowAsync();
                Frame.Navigate(typeof(HomePage));
            }
            catch (Exception ex){
                String msg = ex.Message;
                if(msg == "404"){
                    MessageDialog msgbox = new MessageDialog("O Email já existe!");
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
