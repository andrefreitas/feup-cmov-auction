using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using Auction.Models;


namespace Auction.ViewModels
{
    public class ViewModelChart : ObservableCollection<ModelChart>
    {
        public void addBid(int item, int value) {
            this.Add(new ModelChart(item, value));
        }
        /*
        {
            JArray auctions = await API.getAuctions();
            JObject auction = (JObject)auctions.Where(a => (String)a["state"] == "open").First();
            int minimumBid = (int)auction["minimum_bid"];
            JArray bids = (JArray)auction["bids"];

            this.Collection.Add(new ModelChart(0, minimumBid));

            if (bids.Count > 0)
            {
                for (int i = 0; i < bids.Count; i++)
                {
                    this.Collection.Add(new ModelChart(i + 1, (int)bids[i]["value"]));
                }
            }

            //<sparrow:LineSeries PointsSource="{Binding Collection, Mode=OneWay}" XPath="X" YPath="Y"/>
        }*/
    }
}
