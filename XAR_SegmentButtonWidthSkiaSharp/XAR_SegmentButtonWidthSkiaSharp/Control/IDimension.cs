using System;
using Xamarin.Forms;

namespace XAR_SegmentButtonWidthSkiaSharp.Control
{
    public interface IDimension
    {
       float ConvertDpToPx(double point);
    }
}
