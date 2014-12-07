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
using Microsoft.Phone.Notification;
using System.Text;

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
            toastNotification();
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
                    lastBidTextBlock.Text = "Última oferta: " + value + "€";

                    this.bidsModel.addBid(this.bidsModel.Count, Convert.ToInt32(value));
                    l.PointsSource = bidsModel;
                }
                
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sem ligação à Internet");
            }
        }

        private async void toastNotification()
        {
            /// Holds the push channel that is created or found.
            HttpNotificationChannel pushChannel;

            // The name of our push channel.
            string channelName = "ToastAuctionChannel";

            // Try to find the push channel.
            pushChannel = HttpNotificationChannel.Find(channelName);

            // If the channel was not found, then create a new connection to the push service.
            if (pushChannel == null)
            {
                pushChannel = new HttpNotificationChannel(channelName);

                // Register for all the events before attempting to open the channel.
                pushChannel.ChannelUriUpdated += new EventHandler<NotificationChannelUriEventArgs>(PushChannel_ChannelUriUpdated);
                pushChannel.ErrorOccurred += new EventHandler<NotificationChannelErrorEventArgs>(PushChannel_ErrorOccurred);

                // Register for this notification only if you need to receive the notifications while your application is running.
                pushChannel.ShellToastNotificationReceived += new EventHandler<NotificationEventArgs>(PushChannel_ShellToastNotificationReceived);

                pushChannel.Open();

                // Bind this new channel for toast events.
                pushChannel.BindToShellToast();
                pushChannel.BindToShellTile();

            }
            else
            {
                // The channel was already open, so just register for all the events.
                pushChannel.ChannelUriUpdated += new EventHandler<NotificationChannelUriEventArgs>(PushChannel_ChannelUriUpdated);
                pushChannel.ErrorOccurred += new EventHandler<NotificationChannelErrorEventArgs>(PushChannel_ErrorOccurred);

                // Register for this notification only if you need to receive the notifications while your application is running.
                pushChannel.ShellToastNotificationReceived += new EventHandler<NotificationEventArgs>(PushChannel_ShellToastNotificationReceived);
            }
        }

        /// <summary>
        /// Event handler for when the push channel Uri is updated.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PushChannel_ChannelUriUpdated(object sender, NotificationChannelUriEventArgs e)
        {

            Dispatcher.BeginInvoke(async () =>
            {

                String customerID = (String)ApplicationData.Current.LocalSettings.Values["id"];

                try
                {
                    JObject json = await API.subscribe(auctionID, customerID, e.ChannelUri.ToString());
                    this.NavigationService.Navigate(new Uri("/BidPage.xaml", UriKind.Relative));
                }
                catch (Exception ex)
                {
                    String msg = ex.Message;
                    if (msg == "404")
                    {
                        MessageBox.Show("Códigos de ID inválidos!");
                    }
                    else
                    {
                        MessageBox.Show("Sem ligação à Internet");
                    }

                }

            });


        }

        /// <summary>
        /// Event handler for when a push notification error occurs.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PushChannel_ErrorOccurred(object sender, NotificationChannelErrorEventArgs e)
        {
            // Error handling logic for your particular application would be here.
            Dispatcher.BeginInvoke(() =>
                MessageBox.Show(String.Format("A push notification {0} error occurred.  {1} ({2}) {3}",
                    e.ErrorType, e.Message, e.ErrorCode, e.ErrorAdditionalData))
                    );
        }

        /// <summary>
        /// Event handler for when a toast notification arrives while your application is running.  
        /// The toast will not display if your application is running so you must add this
        /// event handler if you want to do something with the toast notification.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PushChannel_ShellToastNotificationReceived(object sender, NotificationEventArgs e)
        {
            StringBuilder message = new StringBuilder();
            string relativeUri = string.Empty;

            message.AppendFormat("Às {0}:\n", DateTime.Now.ToShortTimeString());

            

            // Parse out the information that was part of the message.
            foreach (string key in e.Collection.Keys)
            {
                message.AppendFormat(" {0}\n", e.Collection[key]);


                Dispatcher.BeginInvoke(() =>
                {
                    //your operations go here
                    if (key == "wp:Text2")
                    {
                        string value = e.Collection[key].Split(' ')[3];
                        lastBidTextBlock.Text = "Última oferta: " + value + "€";

                        this.bidsModel.addBid(this.bidsModel.Count, Convert.ToInt32(value));
                        l.PointsSource = bidsModel;
                    }
                });
                
                if (string.Compare(
                    key,
                    "wp:Param",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.CompareOptions.IgnoreCase) == 0)
                {
                    relativeUri = e.Collection[key];
                }
            }

            // Display a dialog of all the fields in the toast.
            Dispatcher.BeginInvoke(() => MessageBox.Show(message.ToString()));

        }
    }


}