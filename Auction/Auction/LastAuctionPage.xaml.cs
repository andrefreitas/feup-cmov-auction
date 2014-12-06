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
using System.Windows.Media.Imaging;

namespace Auction
{
    public partial class LastAuctionPage : PhoneApplicationPage
    {
        public LastAuctionPage()
        {
            InitializeComponent();
            loadLastAuction();
        }

        public async void loadLastAuction() 
        {
            try
            {
                JArray auctions = await API.getAuctions();
                JObject auction = (JObject)auctions.Where(a => (String)a["state"] == "finished").Last();
                String name = (String)auction["name"];
                String photoID = (String)auction["photo_id"];
                int minimumBid = (int)auction["minimum_bid"];
                JArray bids = (JArray)auction["bids"];

                int lastBid;
                if (bids.Count > 0)
                {
                    lastBid = (int)bids[bids.Count - 1]["value"];
                }
                else
                {
                    lastBid = minimumBid;
                }

                nameTextBlock.Text = name;
                minimumBidTextBlock.Text = "Valor inicial: " + minimumBid.ToString() + "€";
                lastBidTextBlock.Text = "Valor final: " + lastBid + "€";


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
    }
}