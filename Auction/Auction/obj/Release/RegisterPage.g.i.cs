﻿#pragma checksum "C:\Users\Ana  Gomes\Documents\Visual Studio 2013\Projects\Auction\Auction\RegisterPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "816A711E76102875498D5E9E5EBE1B62"
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
    
    
    public partial class RegisterPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Button registerButton;
        
        internal System.Windows.Controls.TextBox emailTextBox;
        
        internal System.Windows.Controls.PasswordBox passwordTextBox;
        
        internal System.Windows.Controls.TextBox nameTextBox;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Auction;component/RegisterPage.xaml", System.UriKind.Relative));
            this.registerButton = ((System.Windows.Controls.Button)(this.FindName("registerButton")));
            this.emailTextBox = ((System.Windows.Controls.TextBox)(this.FindName("emailTextBox")));
            this.passwordTextBox = ((System.Windows.Controls.PasswordBox)(this.FindName("passwordTextBox")));
            this.nameTextBox = ((System.Windows.Controls.TextBox)(this.FindName("nameTextBox")));
        }
    }
}

