using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Auction.Models;

namespace Auction.ViewModels
{
    class ViewModelChart
    {
        public ObservableCollection<ModelChart> Collection { get; set; }
        public ViewModelChart()
        {
            Collection = new ObservableCollection<ModelChart>();
            GenerateDatas();
        }
        private void GenerateDatas()
        {
            this.Collection.Add(new ModelChart(0, 1));
            this.Collection.Add(new ModelChart(1, 2));
            this.Collection.Add(new ModelChart(2, 3));
            this.Collection.Add(new ModelChart(3, 4));
        }
    }
}
