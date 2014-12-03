using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Models
{
    public class ModelChart
    {
        public double X { get; set; }
        public double Y { get; set; }

        public ModelChart(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}
