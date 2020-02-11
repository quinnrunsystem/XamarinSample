using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Input;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using TouchTracking;
using Xamarin.Forms;
using XAR_SegmentButtonWidthSkiaSharp.TouchTracking;

namespace XAR_SegmentButtonWidthSkiaSharp.Control
{
    public class MySKCanvasView : SKCanvasView
    {
        public SKImageInfo Info { get; private set; }
        public (SKPoint min, SKPoint max)[] CoordinateButtons { get; private set; }
        private IDimension _dimension;
        private State _state = State.HoverOut;
        private SKPaint paint;

        #region CornerRadius
        public static readonly BindableProperty CornerRadiusProperty =
            BindableProperty.Create("CornerRadius", typeof(float), typeof(MySKCanvasView));

        public float CornerRadius
        {
            set => SetValue(CornerRadiusProperty, value);
            get => (float)GetValue(CornerRadiusProperty);
        }
        #endregion

        #region BorderWidth
        public static readonly BindableProperty BorderWidthProperty =
            BindableProperty.Create("BorderWidth", typeof(float), typeof(MySKCanvasView));

        public float BorderWidth
        {
            set => SetValue(BorderWidthProperty, value);
            get => (float)GetValue(BorderWidthProperty);
        }
        #endregion

        #region SelectedTabColor
        public static readonly BindableProperty SelectedTabColorProperty =
            BindableProperty.Create("SelectedTabColor", typeof(Color), typeof(MySKCanvasView), Color.White);

        public Color SelectedTabColor
        {
            set => SetValue(SelectedTabColorProperty, value);
            get => (Color)GetValue(SelectedTabColorProperty);
        }
        #endregion

        #region HoverColor
        public static readonly BindableProperty HoverColorProperty =
            BindableProperty.Create("HoverColor", typeof(Color), typeof(MySKCanvasView), Color.Gray);

        public Color HoverColor
        {
            set => SetValue(HoverColorProperty, value);
            get => (Color)GetValue(HoverColorProperty);
        }
        #endregion

        #region TextColor
        public static readonly BindableProperty TextColorProperty =
            BindableProperty.Create("TextColor", typeof(Color), typeof(MySKCanvasView), Color.White);

        public Color TextColor
        {
            set => SetValue(TextColorProperty, value);
            get => (Color)GetValue(TextColorProperty);
        }
        #endregion

        #region SelectedTextColor
        public static readonly BindableProperty SelectedTextColorProperty =
            BindableProperty.Create("SelectedTextColor", typeof(Color), typeof(MySKCanvasView), Color.Black);

        public Color SelectedTextColor
        {
            set => SetValue(TextColorProperty, value);
            get => (Color)GetValue(SelectedTextColorProperty);
        }
        #endregion

        #region BorderColor
        public static readonly BindableProperty BorderColorProperty =
            BindableProperty.Create("BorderColor", typeof(Color), typeof(MySKCanvasView), defaultValue: Color.Black);

        public Color BorderColor
        {
            set => SetValue(BorderColorProperty, value);
            get => (Color)GetValue(BorderColorProperty);
        }
        #endregion

        #region Texts
        public static readonly BindableProperty TextsProperty =
            BindableProperty.Create("Texts", typeof(List<string>), typeof(MySKCanvasView));

        public List<string> Texts
        {
            set => SetValue(TextsProperty, value);
            get => (List<string>)GetValue(TextsProperty);
        }
        #endregion

        #region FontSize
        public static readonly BindableProperty FontSizeProperty =
            BindableProperty.Create("FontSize", typeof(float), typeof(MySKCanvasView), defaultValue: 20f);

        public float FontSize
        {
            set => SetValue(FontSizeProperty, value);
            get => (float)GetValue(FontSizeProperty);
        }
        #endregion

        #region BackgroundContentColor
        public static readonly BindableProperty BackgroundContentColorProperty =
            BindableProperty.Create("BackgroundContentColor", typeof(Color), typeof(MySKCanvasView), Color.Blue);

        public Color BackgroundContentColor
        {
            set => SetValue(BackgroundContentColorProperty, value);
            get => (Color)GetValue(BackgroundContentColorProperty);
        }
        #endregion

        #region ButtonCurrent
        public static readonly BindableProperty ButtonCurrentProperty =
            BindableProperty.Create("FontSize", typeof(int), typeof(MySKCanvasView), defaultValue: 0);

        public int ButtonCurrent
        {
            set => SetValue(ButtonCurrentProperty, value);
            get => (int)GetValue(ButtonCurrentProperty);
        }
        #endregion

        #region TapButtonCommand
        public static readonly BindableProperty TapButtonCommandProperty =
            BindableProperty.Create("TapButtonCommand", typeof(ICommand), typeof(MySKCanvasView));

        public ICommand TapButtonCommand
        {
            set => SetValue(TapButtonCommandProperty, value);
            get => (ICommand)GetValue(TapButtonCommandProperty);
        }
        #endregion

        public MySKCanvasView()
        {
            EnableTouchEvents = true;
            PaintSurface += OnPaintSuface;
            _dimension = DependencyService.Get<IDimension>();
        }

        public void InvokeOnTouch(TouchActionEventArgs args)
        {
            Debug.WriteLine(args.Type.ToString());
            UpdateControl(args.Location, Info, args.Type);
        }

        private void ExecuteTapCommnad(ICommand command, int indexButton)
        {
            if (command == null) return;
            if (command.CanExecute(null))
            {
                command.Execute(indexButton);
            }
        }

        private void SaveCoordinateButtons(SKImageInfo info)
        {
            float buttonWidth = info.Width / Texts.Count;
            CoordinateButtons = new (SKPoint, SKPoint)[Texts.Count];
            for (int i = 0; i < Texts.Count; i++)
            {
                CoordinateButtons[i].min.X = buttonWidth * i;
                CoordinateButtons[i].max.X = CoordinateButtons[i].min.X + buttonWidth;

                CoordinateButtons[i].min.Y = 0;
                CoordinateButtons[i].max.Y = info.Height;
            }
        }

        # region OnPaintSuface
        private void OnPaintSuface(object sender, SKPaintSurfaceEventArgs e)
        {
            var countButton = Texts.Count;
            if (countButton == 0)
                return;

            //get info skcanvas
            var info = Info = e.Info;
            var surface = e.Surface;
            var canvas = surface.Canvas;

            canvas.Clear();

            SaveCoordinateButtons(info); //Update CoordinateTabs

            var borderWidth =  _dimension.ConvertDpToPx(BorderWidth);
            var cornerRadius = _dimension.ConvertDpToPx(CornerRadius);
            var fontSize = _dimension.ConvertDpToPx(FontSize);

            if (paint == null)
                paint = new SKPaint();

            paint.Reset();
            paint.IsAntialias = true;

            //create SKRect full size of canvasview 
            DrawRectBorder(info, canvas, paint, cornerRadius, borderWidth);
            //draw rect content
            DrawRectContent(info, canvas, paint, cornerRadius, borderWidth);
            //draw edges
            DrawEdges(info, canvas, paint, countButton, borderWidth);
            //draw text
            DrawTexts(info, canvas, paint, countButton, fontSize,_state);
            //draw rect hightlight
            DrawRectHightlight(canvas, paint, cornerRadius);

            DrawTextHightLight(info, canvas, paint);
        }
        #endregion

        #region Draw
        void DrawRectBorder(SKImageInfo info, SKCanvas canvas, SKPaint paint, float radius, float bortherWidth)
        {
            if (bortherWidth > 0)
            {
                GetPaintDrawRectBoder(ref paint);

                //create SKRect full size of canvasview 
                var roundRect = new SKRoundRect(new SKRect(0, 0, info.Width, info.Height), radius, radius);
                canvas.DrawRoundRect(roundRect, paint);
            }

        }
        void DrawRectContent(SKImageInfo info, SKCanvas canvas, SKPaint paint, float radius, float borderWidth)
        {
            
            GetPaintDrawRect(ref paint, State.HoverOut);

            //caculate radius of rect content
            radius = (info.Width - borderWidth) / borderWidth * radius;

            //draw rect content
            var roundRectContent = new SKRoundRect(new SKRect(borderWidth, borderWidth, info.Width - borderWidth, info.Height - borderWidth), radius, radius);
            canvas.DrawRoundRect(roundRectContent, paint);
        }
        void DrawTexts(SKImageInfo info, SKCanvas canvas, SKPaint paint, int countButton, float fontSize, State state)
        {
            GetPaintDrawText(ref paint, fontSize, state, isHightLight: false);
            for (int i = 0; i < countButton; i++)
            {
                if (ButtonCurrent == i)
                    continue;
                var widthBound = CoordinateButtons[i].max.X - CoordinateButtons[i].min.X;

                //find the text bounds
                var textBounds = new SKRect();
                paint.MeasureText(Texts[i], ref textBounds);

                //calculate center
                float xText = CoordinateButtons[i].min.X + (widthBound / 2 - textBounds.MidX);
                float yText = info.Height / 2 - textBounds.MidY;

                canvas.DrawText(Texts[i], xText, yText, paint);
            }
        }
        void DrawEdges(SKImageInfo info, SKCanvas canvas, SKPaint paint, int countButton, float borderWith)
        {
            GetPaintDrawEdge(ref paint, borderWith);
            //get coordinate
            var spaceLine = info.Width / countButton;
            using (var path = new SKPath())
            {
                for (int i = 0; i < countButton - 1; i++)
                {
                    //frist
                    if (ButtonCurrent == i || (i + 1 == ButtonCurrent))
                        continue;
 
                    var positionDrawX = spaceLine * (i + 1);
                    path.MoveTo(positionDrawX, 0);
                    path.LineTo(new SKPoint(positionDrawX, info.Height));

                }
                canvas.DrawPath(path, paint);
            }
        }
        void DrawRectHightlight(SKCanvas canvas, SKPaint paint, float radius)
        {
            if (_state == State.HoverIn)
                paint.Color = HoverColor.ToSKColor();
            else if (_state == State.Released || _state == State.HoverOut)
                paint.Color = SelectedTabColor.ToSKColor();

            SKRect rect;
            SKRoundRect roundRect;

            if (ButtonCurrent == 0)
            {
                rect = new SKRect(CoordinateButtons[0].max.X / 2, 0, CoordinateButtons[0].max.X, CoordinateButtons[0].max.Y);
                roundRect = new SKRoundRect(new SKRect(CoordinateButtons[ButtonCurrent].min.X, CoordinateButtons[ButtonCurrent].min.Y, CoordinateButtons[ButtonCurrent].max.X, CoordinateButtons[ButtonCurrent].max.Y), radius, radius);
                canvas.DrawRoundRect(roundRect, paint);
            }
            else if (ButtonCurrent == Texts.Count - 1)
            {
                rect = new SKRect(CoordinateButtons[ButtonCurrent].min.X, 0, (CoordinateButtons[ButtonCurrent].max.X - CoordinateButtons[ButtonCurrent].min.X) / 2 + CoordinateButtons[ButtonCurrent].min.X, CoordinateButtons[ButtonCurrent].max.Y);
                roundRect = new SKRoundRect(new SKRect(CoordinateButtons[ButtonCurrent].min.X, CoordinateButtons[ButtonCurrent].min.Y, CoordinateButtons[ButtonCurrent].max.X, CoordinateButtons[ButtonCurrent].max.Y), radius, radius);
                canvas.DrawRoundRect(roundRect, paint);
            }
            else
                rect = new SKRect(CoordinateButtons[ButtonCurrent].min.X, 0, CoordinateButtons[ButtonCurrent].max.X, CoordinateButtons[ButtonCurrent].max.Y);

            canvas.DrawRect(rect, paint);
        }
        void DrawTextHightLight(SKImageInfo info, SKCanvas canvas, SKPaint paint)
        {
            
            if (_state == State.Released || _state == State.HoverOut)
                paint.Color = SelectedTextColor.ToSKColor();
            else if (_state == State.HoverIn)
                paint.Color = TextColor.ToSKColor();
            else return;

            paint.Style = SKPaintStyle.Fill;
            var widthBound = CoordinateButtons[ButtonCurrent].max.X - CoordinateButtons[ButtonCurrent].min.X;

            //find the text bounds
            var textBounds = new SKRect();
            paint.MeasureText(Texts[ButtonCurrent], ref textBounds);

            //calculate center
            float xText = CoordinateButtons[ButtonCurrent].min.X + (widthBound / 2 - textBounds.MidX);
            float yText = info.Height / 2 - textBounds.MidY;

            canvas.DrawText(Texts[ButtonCurrent], xText, yText, paint);
        }
        #endregion

        #region Get Paint
        void GetPaintDrawText(ref SKPaint paint, float fontSize, State state, bool isHightLight)
        {
            if(!isHightLight)
                paint.Color = TextColor.ToSKColor();
            else if (state == State.HoverIn)
                paint.Color = HoverColor.ToSKColor();
            else if (state == State.Released || state == State.HoverOut)
                paint.Color = SelectedTabColor.ToSKColor();
            paint.Style = SKPaintStyle.Fill;
            paint.TextSize = fontSize;
            paint.Color = TextColor.ToSKColor();
        }
        void GetPaintDrawRect(ref SKPaint paint, State state)
        {
            if (state == State.HoverOut)
                paint.Color = BackgroundContentColor.ToSKColor();
            //if (state == State.HoverIn)
            //    paint.Color = HoverColor.ToSKColor();
            //if (state == State.Released)
            //    paint.Color = SelectedTabColor.ToSKColor();
            paint.Style = SKPaintStyle.Fill;
        }
        void GetPaintDrawRectBoder(ref SKPaint paint)
        {
            paint.Style = SKPaintStyle.Fill;
            paint.Color = BorderColor.ToSKColor();
        }
        void GetPaintDrawEdge(ref SKPaint paint, float borderWidth)
        {
            //draw line down
            paint.Style = SKPaintStyle.Stroke;
            paint.Color = BorderColor.ToSKColor();
            paint.StrokeWidth = borderWidth;
        }
        public void UpdateControl(Point point, SKImageInfo info, TouchActionType touchActionType)
        {

            switch (touchActionType)
            {
                case TouchActionType.Entered:
                case TouchActionType.Pressed:

                    var tabWidth = info.Width / Texts.Count;
                    for (int i = 0; i < Texts.Count; i++)
                    {
                        var min = new Point(tabWidth * i, info.Height);
                        var max = new Point(tabWidth * (i + 1), info.Height);

                        if (point.X >= min.X && point.X <= max.X)
                        {
                            if (CheckCoordinatesOutSide(point, Info, i, Texts.Count))
                                return;
                            _state = State.HoverIn;
                            ButtonCurrent = i;
                            this.InvalidateSurface();

                        }
                    }
                    break;
                case TouchActionType.Moved:

                    break;
                case TouchActionType.Released:
                    if (_state != State.HoverIn)
                        return;
                    if (ButtonCurrent >= 0)
                    {
                        _state = State.Released;
                        this.InvalidateSurface();
                        ExecuteTapCommnad(TapButtonCommand, ButtonCurrent);
                        //reset 
                    }
                    break;
                case TouchActionType.Cancelled:
                case TouchActionType.Exited:
                    _state = State.HoverOut;
                    this.InvalidateSurface();
                    break;
                default:
                    break;
            }
        }
        private bool CheckCoordinatesOutSide(Point pCurrent, SKImageInfo info, int indexTabChild, int countButtonChild)
        {
            if (indexTabChild == 0 || indexTabChild == countButtonChild - 1 && indexTabChild >= 0)
            {
                var radius = info.Height / 2;
                var isDirectionLeft = indexTabChild == 0;
                var pCenter = isDirectionLeft ? new Point(radius, radius) : new Point(info.Width - radius, radius);
                var distance = Math.Sqrt(Math.Pow(pCurrent.X - pCenter.X, 2) + Math.Pow(pCurrent.Y - pCenter.Y, 2));

                if (distance > radius)
                {
                    if (isDirectionLeft)
                    {
                        if (pCurrent.X - pCenter.X < 0)
                            return true;
                    }
                    else
                         if (pCurrent.X - pCenter.X > 0)
                        return true;
                }
            }
            return false;
        }
        #endregion
    }
    public enum State
    {
        HoverIn,
        HoverOut,
        Released,
    }

}
