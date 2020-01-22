using System;
using System.Collections.Generic;
using CollectionViewSample.ViewModels;
using Xamarin.Forms;

namespace CollectionViewSample.Views.Layout
{
    public partial class VerticalGridPage : ContentPage
    {
        public VerticalGridPage()
        {
            InitializeComponent();
            BindingContext = new PremierLeagueViewModel();
        }
    }
}
