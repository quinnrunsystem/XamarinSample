using System;
using Android.Content.Res;
using Xamarin.Forms;
using XAR_SegmentButtonWidthSkiaSharp.Control;
using XAR_SegmentButtonWidthSkiaSharp.Droid.Renderers;

[assembly: Dependency(typeof(DimensionService))]
namespace XAR_SegmentButtonWidthSkiaSharp.Droid.Renderers
{
    public class DimensionService: IDimension
    {
        public float ConvertDpToPx(double dp)
        {
            var app = Android.App.Application.Context;
            var density = app.Resources.DisplayMetrics.Density;
            var px = density * dp;
            return px <= (double)float.MaxValue ? (float)px : float.MaxValue;
        }

        
    }
}
