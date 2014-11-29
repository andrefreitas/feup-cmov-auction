using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Windows.UI.Popups;
using Newtonsoft.Json.Linq;
using Windows.Storage;

namespace Auction
{
    public partial class RegisterPage : PhoneApplicationPage
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private async void registerButton_Click(object sender, RoutedEventArgs e)
        {
            String name = nameTextBox.Text;
            String email = emailTextBox.Text;
            String password = passwordTextBox.Password;

            if (!Helpers.nameIsValid(name))
            {
                MessageBox.Show("O nome é inválido!");
                return;
            }

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
                JObject json = await API.register(name, email, password);
                String id = (string)json["id"];
                ApplicationData.Current.LocalSettings.Values["id"] = id;
                MessageBox.Show("Registo efetuado com sucesso!");
                this.NavigationService.Navigate(new Uri("/HomePage.xaml", UriKind.Relative));
            }
            catch (Exception ex)
            {
                String msg = ex.Message;
                if (msg == "404")
                {
                    MessageBox.Show("O Email já existe!");
                }
                else
                {
                    MessageBox.Show("Sem ligação à Internet");
                }

            }
        }
    }
}