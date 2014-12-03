using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Collections.ObjectModel;
using Auction.ViewModels;
using Newtonsoft.Json.Linq;
using System.Windows.Media.Imaging;

namespace Auction
{
    public partial class BidPage : PhoneApplicationPage
    {
        public BidPage()
        {
            InitializeComponent();
            loadActiveAuction();
        }

        private void Bid_TextChanged(object sender, TextChangedEventArgs e)
        {

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

        public void bidButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }


}