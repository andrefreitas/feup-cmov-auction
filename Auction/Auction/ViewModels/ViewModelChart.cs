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
        public void addBid(int item, int value)
        {
            this.Add(new ModelChart(item, value));
        }
    }
}
