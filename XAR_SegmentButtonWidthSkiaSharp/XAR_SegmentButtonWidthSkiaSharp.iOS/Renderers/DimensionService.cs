using System;
using UIKit;
using Xamarin.Forms;
using XAR_SegmentButtonWidthSkiaSharp.Control;
using XAR_SegmentButtonWidthSkiaSharp.iOS.Renderers;

[assembly: Dependency(typeof(DimensionService))]
namespace XAR_SegmentButtonWidthSkiaSharp.iOS.Renderers
{
    public class DimensionService : IDimension
    {
        public float ConvertDpToPx(double point)
        {
            var screenScale = UIScreen.MainScreen.Scale;
            var px = point * screenScale;
            return px <= (double)float.MaxValue ? (float)px : float.MaxValue;
        }
    }
}
