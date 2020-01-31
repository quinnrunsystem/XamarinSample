using System;
using System.Collections.Generic;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace XAR_SegmentButtonWidthSkiaSharp
{
    public class SegmentButton : SKCanvasView
    {

        #region CornerRadius
        public static readonly BindableProperty CornerRadiusProperty =
            BindableProperty.Create("CornerRadius", typeof(float), typeof(SegmentButton));

        public float CornerRadius
        {
            set => SetValue(CornerRadiusProperty, value);
            get => (float)GetValue(CornerRadiusProperty);
        }
        #endregion

        #region BorderWidth
        public static readonly BindableProperty BorderWidthProperty =
            BindableProperty.Create("BorderWidth", typeof(double), typeof(SegmentButton));

        public double BorderWidth
        {
            set => SetValue(BorderWidthProperty, value);
            get => (double)GetValue(BorderWidthProperty);
        }
        #endregion

        #region BorderColor
        public static readonly BindableProperty BorderColorProperty =
            BindableProperty.Create("BorderColor", typeof(Color), typeof(SegmentButton),defaultValue:Color.Transparent);

        public Color BorderColor
        {
            set => SetValue(BorderColorProperty, value);
            get => (Color)GetValue(BorderColorProperty);
        }
        #endregion


        #region HasBorder
        public static readonly BindableProperty HasBorderProperty =
            BindableProperty.Create("HasBorder", typeof(bool), typeof(SegmentButton), defaultValue: false);

        public bool HasBorder
        {
            set => SetValue(HasBorderProperty, value);
            get => (bool)GetValue(HasBorderProperty);
        }
        #endregion


        #region Texts
        public static readonly BindableProperty TextsProperty =
            BindableProperty.Create("Texts", typeof(List<string>), typeof(SegmentButton));

        public List<string> Texts
        {
            set => SetValue(TextsProperty, value);
            get => (List<string>)GetValue(TextsProperty);
        }
        #endregion

        #region FontSize
        public static readonly BindableProperty FontSizeProperty =
            BindableProperty.Create("FontSize", typeof(float), typeof(SegmentButton));

        public float FontSize
        {
            set => SetValue(FontSizeProperty, value);
            get => (float)GetValue(FontSizeProperty);
        }
        #endregion

        public SegmentButton()
        {
            PaintSurface += OnPaintSuface;
          
           // InvalidateSurface();
        }

        private void OnPaintSuface(object sender, SKPaintSurfaceEventArgs e)
        {
            var info = e.Info;
            var surface = e.Surface;
            var canvas = surface.Canvas;
            SKPaint paint;
            SKRect rect;
            SKRoundRect roundRect;

            canvas.Clear();

            paint = new SKPaint();
            paint.Style = SKPaintStyle.Fill;
            paint.Color = SKColors.Green;
            paint.IsAntialias = true;
            
            rect = new SKRect(0, 0, info.Width, info.Height);
            roundRect = new SKRoundRect(rect, CornerRadius, CornerRadius);
            canvas.DrawRoundRect(roundRect, paint);

            //border
            if(HasBorder)
            {
                float x = (float)((info.Width - (info.Width-BorderWidth)) / 2);
                float y = (float)((info.Height - (info.Height-BorderWidth)) / 2);
                rect = new SKRect(x, y, (float)(x + (info.Width - BorderWidth)), (float)(y + (info.Height - BorderWidth)));


                paint.Style = SKPaintStyle.Fill;
                paint.Color = BorderColor.ToSKColor();
                roundRect = new SKRoundRect(rect, CornerRadius, CornerRadius);
                canvas.DrawRoundRect(roundRect, paint);


                //draw line vertical

                SKPaint paintLine = new SKPaint()
                {
                    Style = SKPaintStyle.Stroke,
                    StrokeWidth = (float)BorderWidth/2,
                    Color = SKColors.Green,
                };
               
                for(int i=1;i<Texts.Count;i++)
                {
                    var rect1 = new SKRect(info.Width / 3 * i, 0, info.Width / 3, info.Height);
                    canvas.DrawRect(rect1, paintLine);
                }
            }

            rect = new SKRect(0, 0, (float)(info.Width / 3 + BorderWidth / 2), info.Height);
            paint.Color = Color.Pink.ToSKColor();
            roundRect = new SKRoundRect(rect, CornerRadius, CornerRadius);


          
            canvas.DrawRoundRect(roundRect, paint);

            rect.Left = rect.Right / 2;
            canvas.DrawRect(rect, paint);
           



        }
    }
}
