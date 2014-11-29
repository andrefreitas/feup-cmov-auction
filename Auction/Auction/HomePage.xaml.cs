using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Newtonsoft.Json.Linq;
using Microsoft.Phone.Notification;
using System.Windows.Media.Imaging;

namespace Auction
{
    public partial class HomePage : PhoneApplicationPage
    {
        public HomePage()
        {
            InitializeComponent();
            loadActiveAuction();
        }

        public async void loadActiveAuction()
        {
            try
            {
                JArray auctions = await API.getAuctions();
                JObject auction = (JObject)auctions.Where(a => (String)a["state"] == "open").First();
                String name = (String)auction["name"];
                String date = (String)auction["date"];
                String photoID = (String)auction["photo_id"];
                int minimumBid = (int)auction["minimum_bid"];
                JArray bids = (JArray)auction["bids"];

                nameTextBlock.Text = name;
                minimumBidTextBlock.Text = "Mínimo: " + minimumBid.ToString() + "€";

                Uri myUri = new Uri("http://neo.andrefreitas.pt:8083/api/photos/" + photoID, UriKind.Absolute);
                BitmapImage bmi = new BitmapImage();
                bmi.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                bmi.UriSource = myUri;
                pictureImage.Source = bmi;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sem ligação à Internet");
            }

        }


        private void subscribeButton_Click(object sender, RoutedEventArgs e)
        {
           
        }
    }
}