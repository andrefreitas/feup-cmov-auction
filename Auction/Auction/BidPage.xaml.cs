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

namespace Auction
{
    //Create a model
    public class Model
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Model(double x, double y)
        {
            X = x;
            Y = y;
        }
    }

    // Create a ViewModel
    public class ViewModel
    {
        public ObservableCollection<Model> Collection { get; set; }
        public ViewModel()
        {
            Collection = new ObservableCollection<Model>();
            GenerateDatas();
        }
        private void GenerateDatas()
        {
            this.Collection.Add(new Model(0, 1));
            this.Collection.Add(new Model(1, 2));
            this.Collection.Add(new Model(2, 3));
            this.Collection.Add(new Model(3, 4));
        }
    }

    public partial class PivotPage1 : PhoneApplicationPage
    {
        public PivotPage1()
        {
            InitializeComponent();
        }
    }


}