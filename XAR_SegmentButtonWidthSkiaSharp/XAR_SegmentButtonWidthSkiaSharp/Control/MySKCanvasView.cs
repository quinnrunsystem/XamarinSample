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
        private SKImageInfo _info;
        private (SKPoint min, SKPoint max)[] _coordinateCells;
        private IDimension _dimension;
        private StateCell _state = StateCell.HoverOut;
        private SKPaint paint;

        #region CornerRadius
        public static readonly BindableProperty CornerRadiusProperty =
            BindableProperty.Create(nameof(CornerRadius), typeof(float), typeof(MySKCanvasView));

        public float CornerRadius
        {
            set => SetValue(CornerRadiusProperty, value);
            get => (float)GetValue(CornerRadiusProperty);
        }
        #endregion
        #region BorderWidth
        public static readonly BindableProperty BorderWidthProperty =
            BindableProperty.Create(nameof(BorderWidth), typeof(float), typeof(MySKCanvasView));

        public float BorderWidth
        {
            set => SetValue(BorderWidthProperty, value);
            get => (float)GetValue(BorderWidthProperty);
        }
        #endregion
        #region SelectedCellColor
        public static readonly BindableProperty SelectedCellColorProperty =
            BindableProperty.Create(nameof(SelectedCellColor), typeof(Color), typeof(MySKCanvasView), Color.White);

        public Color SelectedCellColor
        {
            set => SetValue(SelectedCellColorProperty, value);
            get => (Color)GetValue(SelectedCellColorProperty);
        }
        #endregion
        #region HoverCellColor
        public static readonly BindableProperty HoverCellColorProperty =
            BindableProperty.Create(nameof(HoverCellColor), typeof(Color), typeof(MySKCanvasView), Color.Gray);

        public Color HoverCellColor
        {
            set => SetValue(HoverCellColorProperty, value);
            get => (Color)GetValue(HoverCellColorProperty);
        }
        #endregion
        #region TextColor
        public static readonly BindableProperty TextColorProperty =
            BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(MySKCanvasView), Color.White);

        public Color TextColor
        {
            set => SetValue(TextColorProperty, value);
            get => (Color)GetValue(TextColorProperty);
        }
        #endregion
        #region SelectedTextColor
        public static readonly BindableProperty SelectedTextColorProperty =
            BindableProperty.Create(nameof(SelectedTextColor), typeof(Color), typeof(MySKCanvasView), Color.Black);

        public Color SelectedTextColor
        {
            set => SetValue(TextColorProperty, value);
            get => (Color)GetValue(SelectedTextColorProperty);
        }
        #endregion
        #region BorderColor
        public static readonly BindableProperty BorderColorProperty =
            BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(MySKCanvasView), defaultValue: Color.Black);

        public Color BorderColor
        {
            set => SetValue(BorderColorProperty, value);
            get => (Color)GetValue(BorderColorProperty);
        }
        #endregion
        #region Texts
        public static readonly BindableProperty TextsProperty =
            BindableProperty.Create(nameof(Texts), typeof(List<string>), typeof(MySKCanvasView));

        public List<string> Texts
        {
            set => SetValue(TextsProperty, value);
            get => (List<string>)GetValue(TextsProperty);
        }
        #endregion
        #region FontSize
        public static readonly BindableProperty FontSizeProperty =
            BindableProperty.Create(nameof(FontSize), typeof(float), typeof(MySKCanvasView), defaultValue: 20f);

        public float FontSize
        {
            set => SetValue(FontSizeProperty, value);
            get => (float)GetValue(FontSizeProperty);
        }
        #endregion
        #region BackgroundContentColor
        public static readonly BindableProperty BackgroundContentColorProperty =
            BindableProperty.Create(nameof(BackgroundContentColor), typeof(Color), typeof(MySKCanvasView), Color.Blue);

        public Color BackgroundContentColor
        {
            set => SetValue(BackgroundContentColorProperty, value);
            get => (Color)GetValue(BackgroundContentColorProperty);
        }
        #endregion
        #region PositionCurrent
        public static readonly BindableProperty PositionCurrentProperty =
            BindableProperty.Create(nameof(PositionCurrent), typeof(int), typeof(MySKCanvasView));

        public int PositionCurrent
        {
            set => SetValue(PositionCurrentProperty, value);
            get => (int)GetValue(PositionCurrentProperty);
        }
        #endregion
        #region TapButtonCommand
        public static readonly BindableProperty TapButtonCommandProperty =
            BindableProperty.Create(nameof(TapButtonCommand), typeof(ICommand), typeof(MySKCanvasView));

        public ICommand TapButtonCommand
        {
            set => SetValue(TapButtonCommandProperty, value);
            get => (ICommand)GetValue(TapButtonCommandProperty);
        }
        #endregion

        public MySKCanvasView()
        {
            EnableTouchEvents = true;
            _dimension = DependencyService.Get<IDimension>();
        }

        public void InvokeOnTouch(TouchActionEventArgs args)
        {
            Debug.WriteLine(args.Type.ToString());
            UpdateControl(args.Location, _info, args.Type);

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
            _coordinateCells = new (SKPoint, SKPoint)[Texts.Count];
            for (int i = 0; i < Texts.Count; i++)
            {
                _coordinateCells[i].min.X = buttonWidth * i;
                _coordinateCells[i].max.X = _coordinateCells[i].min.X + buttonWidth;

                _coordinateCells[i].min.Y = 0;
                _coordinateCells[i].max.Y = info.Height;
            }
        }

        #region Draw
        void DrawRectBorder(SKImageInfo info, SKCanvas canvas, SKPaint paint, float radius, float bortherWidth)
        {
            if (bortherWidth > 0)  // view not has border
            {
                GetPaintDrawRectBoder(ref paint);

                //create SKRect full size of canvasview 
                var roundRect = new SKRoundRect(new SKRect(0, 0, info.Width, info.Height), radius, radius);
                canvas.DrawRoundRect(roundRect, paint);
            }

        }
        private void DrawRectContent(SKImageInfo info, SKCanvas canvas, SKPaint paint, float radius, float borderWidth,StateCell state)
        {

            GetPaintDrawRect(ref paint,state,isHightLight:false);

            //caculate ratio radius of rect content compare to rect border
            radius = (info.Width - borderWidth) / borderWidth * radius;

            //draw rect content
            var roundRectContent = new SKRoundRect(new SKRect(borderWidth, borderWidth, info.Width - borderWidth, info.Height - borderWidth), radius, radius);
            canvas.DrawRoundRect(roundRectContent, paint);
        }
        private void DrawTexts(SKImageInfo info, SKCanvas canvas, SKPaint paint, int countButton, float fontSize, StateCell state)
        {
            GetPaintDrawText(ref paint, fontSize, state, isHightLight: false);

            for (int i = 0; i < countButton; i++)
            {
                if (PositionCurrent == i) //not draw this text , it is hightlight
                    continue;

                var widthBound = _coordinateCells[i].max.X - _coordinateCells[i].min.X;

                //find the text bounds
                var textBounds = new SKRect();
                paint.MeasureText(Texts[i], ref textBounds);

                //calculate position center
                float xText = _coordinateCells[i].min.X + (widthBound / 2 - textBounds.MidX);
                float yText = info.Height / 2 - textBounds.MidY;

                canvas.DrawText(Texts[i], xText, yText, paint);
            }
        }
        private void DrawEdges(SKImageInfo info, SKCanvas canvas, SKPaint paint, int countButton, float borderWith)
        {
            GetPaintDrawEdge(ref paint, borderWith);
            //get distance between lines
            var spaceLine = info.Width / countButton;
            using (var path = new SKPath())
            {
                for (int i = 0; i < countButton - 1; i++)
                {
                    if (PositionCurrent == i || PositionCurrent ==i + 1)  //cell is highlight, not draw edge
                            continue;
                    var positionDrawX = spaceLine * (i + 1);
                    path.MoveTo(positionDrawX, 0);
                    path.LineTo(new SKPoint(positionDrawX, info.Height));

                }
                canvas.DrawPath(path, paint);
            }
        }
        private void DrawRectHightlight(SKCanvas canvas, SKPaint paint, float radius,StateCell state)
        {
            GetPaintDrawRect(ref paint, state, isHightLight: true);
            SKRect rect;
            SKRoundRect roundRect;

            if (PositionCurrent == 0)
            {
                rect = new SKRect(_coordinateCells[0].max.X / 2, 0, _coordinateCells[0].max.X, _coordinateCells[0].max.Y);
                roundRect = new SKRoundRect(new SKRect(_coordinateCells[PositionCurrent].min.X, _coordinateCells[PositionCurrent].min.Y, _coordinateCells[PositionCurrent].max.X, _coordinateCells[PositionCurrent].max.Y), radius, radius);
                canvas.DrawRoundRect(roundRect, paint);
            }
            else if (PositionCurrent == Texts.Count - 1)
            {
                rect = new SKRect(_coordinateCells[PositionCurrent].min.X, 0, (_coordinateCells[PositionCurrent].max.X - _coordinateCells[PositionCurrent].min.X) / 2 + _coordinateCells[PositionCurrent].min.X, _coordinateCells[PositionCurrent].max.Y);
                roundRect = new SKRoundRect(new SKRect(_coordinateCells[PositionCurrent].min.X, _coordinateCells[PositionCurrent].min.Y, _coordinateCells[PositionCurrent].max.X, _coordinateCells[PositionCurrent].max.Y), radius, radius);
                canvas.DrawRoundRect(roundRect, paint);
            }
            else
                rect = new SKRect(_coordinateCells[PositionCurrent].min.X, 0, _coordinateCells[PositionCurrent].max.X, _coordinateCells[PositionCurrent].max.Y);

            canvas.DrawRect(rect, paint);
        }
        private void DrawTextHightLight(SKImageInfo info, SKCanvas canvas, SKPaint paint, float fontSize, StateCell state)
        {
            GetPaintDrawText(ref paint, fontSize, state, isHightLight: true);

            var widthBound = _coordinateCells[PositionCurrent].max.X - _coordinateCells[PositionCurrent].min.X;

            //find the text bounds
            var textBounds = new SKRect();
            paint.MeasureText(Texts[PositionCurrent], ref textBounds);

            //calculate center
            float xText = _coordinateCells[PositionCurrent].min.X + (widthBound / 2 - textBounds.MidX);
            float yText = info.Height / 2 - textBounds.MidY;

            canvas.DrawText(Texts[PositionCurrent], xText, yText, paint);
        }
        #endregion
        #region GetPaint
        private void GetPaintDrawText(ref SKPaint paint, float fontSize, StateCell state, bool isHightLight)
        {
            if (!isHightLight)
                paint.Color = TextColor.ToSKColor();
            else if (state == StateCell.HoverIn)
                paint.Color = TextColor.ToSKColor();
            else if (state == StateCell.Released || state == StateCell.HoverOut)
                paint.Color = SelectedTextColor.ToSKColor();
            paint.Style = SKPaintStyle.Fill;
            paint.TextSize = fontSize;
        }
        private void GetPaintDrawRect(ref SKPaint paint, StateCell state, bool isHightLight)
        {
            if (!isHightLight)
               paint.Color = BackgroundContentColor.ToSKColor();
            else if (state == StateCell.HoverIn)
                paint.Color = HoverCellColor.ToSKColor();
            else if (state == StateCell.Released || _state == StateCell.HoverOut)
                paint.Color = SelectedCellColor.ToSKColor();
        }
        private void GetPaintDrawRectBoder(ref SKPaint paint)
        {
            paint.Style = SKPaintStyle.Fill;
            paint.Color = BorderColor.ToSKColor();
        }
        private void GetPaintDrawEdge(ref SKPaint paint, float borderWidth)
        {
            //draw line down
            paint.Style = SKPaintStyle.Stroke;
            paint.Color = BorderColor.ToSKColor();
            paint.StrokeWidth = borderWidth;
        }
        #endregion

        int oldPostion= 0;
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
                            if (CheckCoordinatesOutSide(point, info, i, Texts.Count))
                                return;
                            _state = StateCell.HoverIn;
                            oldPostion = PositionCurrent;
                            PositionCurrent = i;
                            this.InvalidateSurface();

                        }
                    }
                    break;
                case TouchActionType.Moved:
                    {
                        var coordinates = _coordinateCells[PositionCurrent];
                        if (point.X < coordinates.min.X || point.X > coordinates.max.X || point.Y < coordinates.min.Y || point.Y > coordinates.max.Y)
                        {
                            UpdateControl(point, info, TouchActionType.Cancelled);
                        }
                    }
                    break;
                case TouchActionType.Released:
                    if (_state != StateCell.HoverIn)
                        return;
                        _state = StateCell.Released;
                        this.InvalidateSurface();
                        ExecuteTapCommnad(TapButtonCommand, PositionCurrent);
                        //reset 
                    break;
                case TouchActionType.Cancelled:
                case TouchActionType.Exited:
                    _state = StateCell.HoverOut;
                    PositionCurrent = oldPostion;
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
        protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            base.OnPaintSurface(e);
            var countButton = Texts.Count;
            if (countButton == 0)
                return;

            //get info skcanvas
            var info = _info = e.Info;
            var surface = e.Surface;
            var canvas = surface.Canvas;

            canvas.Clear();

            SaveCoordinateButtons(info); //Update CoordinateTabs

            var borderWidth = _dimension.ConvertDpToPx(BorderWidth);
            var cornerRadius = _dimension.ConvertDpToPx(CornerRadius);
            var fontSize = _dimension.ConvertDpToPx(FontSize);

            if (paint == null)
                paint = new SKPaint();

            paint.Reset();
            paint.IsAntialias = true;

            //create SKRect full size of canvasview 
            DrawRectBorder(info, canvas, paint, cornerRadius, borderWidth);
            //draw rect content
            DrawRectContent(info, canvas, paint, cornerRadius, borderWidth,_state);
            //draw edges
            DrawEdges(info, canvas, paint, countButton, borderWidth);
            //draw text
            DrawTexts(info, canvas, paint, countButton, fontSize, _state);
            //draw rect when button hover in hightlight 
            DrawRectHightlight(canvas, paint, cornerRadius,_state);
            //draw text when button hover in hightlight 
            DrawTextHightLight(info, canvas, paint,fontSize,_state);
        }
    }
}
