using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CoreGraphics;
using Foundation;
using SkiaSharp.Views.Forms;
using TouchTracking;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XAR_SegmentButtonWidthSkiaSharp.Control;
using XAR_SegmentButtonWidthSkiaSharp.iOS.Renderers;
using XAR_SegmentButtonWidthSkiaSharp.TouchTracking;

[assembly: ExportRenderer(typeof(MySKCanvasView), typeof(MySKCanvasViewRenderer))]
namespace XAR_SegmentButtonWidthSkiaSharp.iOS.Renderers
{
    public class MySKCanvasViewRenderer : SKCanvasViewRenderer
    {
        MySKCanvasView _element;
        CustomTouchRecognizer _uIGesture;
        protected override void OnElementChanged(ElementChangedEventArgs<SKCanvasView> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null)
            {
                // Cleanup resources and remove event handlers for this element.
                if (Control != null)
                {
                    if (_uIGesture != null)
                        Control.RemoveGestureRecognizer(_uIGesture);
                }
            }

            if (e.NewElement != null)
            {
                // Use the properties of this element to assign to the native control, which is assigned to the base.Control property
                _element = e.NewElement as MySKCanvasView;
                if (_element != null)
                {
                    Control.UserInteractionEnabled = true;
                    Control.AddGestureRecognizer(new CustomTouchRecognizer(_element, Control));

                }
            }
        }
    }

    public class CustomTouchRecognizer : UIGestureRecognizer
    {
        MySKCanvasView _element;
        SkiaSharp.Views.iOS.SKCanvasView _control;

        static Dictionary<UIView, CustomTouchRecognizer> _viewDictionary =
           new Dictionary<UIView, CustomTouchRecognizer>();

        private static Dictionary<long, CustomTouchRecognizer> _idToTouchDictionary =
            new Dictionary<long, CustomTouchRecognizer>();

        public CustomTouchRecognizer(MySKCanvasView canvasView, SkiaSharp.Views.iOS.SKCanvasView control)
        {
            _element = canvasView;
            _control = control;
            _viewDictionary.Add(_control, this);
        }

        #region TouchesBegan
        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);

            foreach (var touch in touches.Cast<UITouch>())
            {
                var id = touch.Handle.ToInt64();
                FireEvent(this, id, TouchActionType.Pressed, touch, true);

                if (!_idToTouchDictionary.ContainsKey(id))
                    _idToTouchDictionary.Add(id, this);
            }

        }
        #endregion

        #region TouchesMoved
        public override void TouchesMoved(NSSet touches, UIEvent evt)
        {
            base.TouchesMoved(touches, evt);

            foreach (var touch in touches.Cast<UITouch>())
            {
                long id = touch.Handle.ToInt64();
                CheckForBoundaryHop(touch);
                if (_idToTouchDictionary[id] != null)
                    FireEvent(_idToTouchDictionary[id], id, TouchActionType.Moved, touch, true);
            }
        }
        #endregion

        #region TouchEnd
        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            base.TouchesEnded(touches, evt);

            foreach (var touch in touches.Cast<UITouch>())
            {
                long id = touch.Handle.ToInt64();

                CheckForBoundaryHop(touch);
                if (_idToTouchDictionary[id] != null)
                    FireEvent(_idToTouchDictionary[id], id, TouchActionType.Released, touch, false);

                _idToTouchDictionary.Remove(id);
            }
        }
        #endregion

        #region TouchCancelled
        public override void TouchesCancelled(NSSet touches, UIEvent evt)
        {
            base.TouchesCancelled(touches, evt);

            foreach (var touch in touches.Cast<UITouch>())
            {
                long id = touch.Handle.ToInt64();
                FireEvent(_idToTouchDictionary[id], id, TouchActionType.Cancelled, touch, false);
                _idToTouchDictionary.Remove(id);
            }
        }
        #endregion

        private void FireEvent(CustomTouchRecognizer recognizer, long id, TouchActionType actionType, UITouch touch, bool isInContact)
        {
            //Convert touch location to Xamarin.Forms Point value
            CGPoint cGPoint = touch.LocationInView(recognizer.View);
            Point point = new Point(cGPoint.X, cGPoint.Y);


            var screenScale = UIScreen.MainScreen.Scale;
            point = new Point(point.X * screenScale, point.Y * screenScale);

            _element.InvokeOnTouch(new TouchActionEventArgs(id, actionType, point, isInContact));
        }

        private void CheckForBoundaryHop(UITouch touch)
        {
            var id = touch.Handle.ToInt64();

            CustomTouchRecognizer recognizerHit = null;

            foreach (var view in _viewDictionary.Keys)
            {
                CGPoint location = touch.LocationInView(view);

                CGRect rect = new CGRect(new CGPoint(), view.Frame.Size);

                if (rect.Contains(location))
                    recognizerHit = _viewDictionary[view];
            }

            if (recognizerHit != _idToTouchDictionary[id])
            {
                if (_idToTouchDictionary[id] != null)
                    FireEvent(_idToTouchDictionary[id], id, TouchActionType.Exited, touch, true);
                if (recognizerHit != null)
                    FireEvent(recognizerHit, id, TouchActionType.Entered, touch, true);
                _idToTouchDictionary[id] = recognizerHit;

            }
        }
    }
}
