using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Collections.ObjectModel;
using Auction.ViewModels;
using Newtonsoft.Json.Linq;
using System.Windows.Media.Imaging;
using Windows.Storage;
using Auction.Models;
using Sparrow.Chart;
using System.Threading.Tasks;

namespace Auction
{
    public partial class BidPage : PhoneApplicationPage
    {
        private String auctionID;
        private ViewModelChart bidsModel;
        private LineSeries l;
        private int lastBid;

        public BidPage()
        {
            InitializeComponent();
            bidsModel = new ViewModelChart();
            l = new LineSeries();
            l.XPath = "X";
            l.YPath = "Y";
            l.PointsSource = bidsModel;
            chart.Series.Add(l);
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
                auctionID = (String)auction["id"];
                JArray bids = (JArray)auction["bids"];


                this.bidsModel.addBid(0, minimumBid);

                if (bids.Count > 0)
                {
                    for (int i = 0; i < bids.Count; i++)
                    {
                        this.bidsModel.addBid(i + 1, (int)bids[i]["value"]);
                        if (i == bids.Count - 1)
                        {
                            lastBid = (int)bids[i]["value"];
                        }
                    }
                }
                else
                {
                    lastBid = minimumBid;
                }

                l.PointsSource = bidsModel;

                nameTextBlock.Text = name;
                minimumBidTextBlock.Text = "Mínimo: " + minimumBid.ToString() + "€";
                lastBidTextBlock.Text = "Última oferta: " + lastBid.ToString() + "€";

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

        public async void bidButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                String customerID = (String)ApplicationData.Current.LocalSettings.Values["id"];
                String value = bidTextBox.Text;

                if (Convert.ToInt32(value) < lastBid)
                {
                    MessageBox.Show("Tem de realizar uma oferta de valor superior à última!");
                } 
                else 
                {
                    JObject bid = await API.bid(auctionID, customerID, value);
                    MessageBox.Show("Oferta realizada");

                    JArray auctions = await API.getAuctions();
                    JObject auction = (JObject)auctions.Where(a => (String)a["state"] == "open").First();

                    int minimumBid = (int)auction["minimum_bid"];
                    JArray bids = (JArray)auction["bids"];

                    this.bidsModel.addBid(this.bidsModel.Count, Convert.ToInt32(value));
                    l.PointsSource = bidsModel;
                }
                
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sem ligação à Internet");
            }
        }
    }


}