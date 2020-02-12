using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace XAR_SegmentButtonWidthSkiaSharp
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
       
        public MainPage()
        {
            InitializeComponent();

            btn.Texts = new List<string>() { "Tp.HCM", "Ha Noi", "Da Nang" };

           
            OnTapCommand = new Command<int>((positionCurrent) =>
            {
                lblText.Text = btn.Texts[positionCurrent];
            });

            BindingContext = this;
        }
        public ICommand OnTapCommand { get; set; }
    }
}