﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CollectionViewSample.Views.Layout;
using Xamarin.Forms;

namespace CollectionViewSample
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public ICommand NavigateCommand { get; private set; }

        public MainPage()
        {
            InitializeComponent();
          
            NavigateCommand = new Command<Type>(async (Type pageType) =>
            {
                var page = (Page)Activator.CreateInstance(pageType);
                await Navigation.PushAsync(page);
            });

            BindingContext = this;
        }
    }
}
