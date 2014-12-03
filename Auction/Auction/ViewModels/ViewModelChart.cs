using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Auction.Models;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace Auction.ViewModels
{
    public class ViewModelChart
    {
        public ObservableCollection<ModelChart> Collection { get; set; }
        public ViewModelChart()
        {
            Collection = new ObservableCollection<ModelChart>();
            GenerateDatas();
        }
        private async void GenerateDatas()
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
                    this.Collection.Add(new ModelChart(i + 1, (int)bids[i]));
                }
                    
            }
            
        }
    }
}
