﻿#pragma checksum "C:\Users\Ana  Gomes\Documents\feup-cmov-auction\Auction\Auction\BidPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "98109A46F10D55BE18A4F7A30B5D7FDA"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using Sparrow.Chart;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace Auction {
    
    
    public partial class BidPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Image pictureImage;
        
        internal System.Windows.Controls.TextBlock nameTextBlock;
        
        internal System.Windows.Controls.TextBlock minimumBidTextBlock;
        
        internal System.Windows.Controls.TextBlock lastBidTextBlock;
        
        internal System.Windows.Controls.TextBox bidTextBox;
        
        internal System.Windows.Controls.Button bidButton;
        
        internal Sparrow.Chart.SparrowChart chart;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/Auction;component/BidPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.pictureImage = ((System.Windows.Controls.Image)(this.FindName("pictureImage")));
            this.nameTextBlock = ((System.Windows.Controls.TextBlock)(this.FindName("nameTextBlock")));
            this.minimumBidTextBlock = ((System.Windows.Controls.TextBlock)(this.FindName("minimumBidTextBlock")));
            this.lastBidTextBlock = ((System.Windows.Controls.TextBlock)(this.FindName("lastBidTextBlock")));
            this.bidTextBox = ((System.Windows.Controls.TextBox)(this.FindName("bidTextBox")));
            this.bidButton = ((System.Windows.Controls.Button)(this.FindName("bidButton")));
            this.chart = ((Sparrow.Chart.SparrowChart)(this.FindName("chart")));
        }
    }
}

