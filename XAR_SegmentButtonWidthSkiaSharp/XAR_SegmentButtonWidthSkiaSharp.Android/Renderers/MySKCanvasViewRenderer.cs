using System;
using System.Collections.Generic;
using System.ComponentModel;
using Android.Content;
using Android.Views;
using SkiaSharp.Views.Forms;
using TouchTracking;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XAR_SegmentButtonWidthSkiaSharp.Control;
using XAR_SegmentButtonWidthSkiaSharp.TouchTracking;

[assembly: ExportRenderer(typeof(MySKCanvasView), typeof(XAR_SegmentButtonWidthSkiaSharp.Droid.Renderers.MySKCanvasViewRenderer))]
namespace XAR_SegmentButtonWidthSkiaSharp.Droid.Renderers
{
    public class MySKCanvasViewRenderer : SKCanvasViewRenderer
    {
        MySKCanvasView _element;
        List<int> _idTouchs = new List<int>();
        Rectangle _rectView = new Rectangle();
        Point _locationView = new Point();

        public MySKCanvasViewRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<SKCanvasView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
               Control.Touch -= OnTouch;
            }

            if (e.NewElement!=null)
            {
                if (Control != null)
                {
                    _element = Element as MySKCanvasView ;
                    //set event handler on view
                    Control.Touch += OnTouch;
                }
            }          
        }
        
        private void OnTouch(object sender, Android.Views.View.TouchEventArgs e)
        {
            // two object common to all the events
            var view = sender as Android.Views.View;
            MotionEvent motionEvent = e.Event;

          
            //Get the pointer index
            int pointerIndex = motionEvent.ActionIndex;

            var arrLocation = new int[2];
            view.GetLocationOnScreen(arrLocation);
            _locationView.X = arrLocation[0];
            _locationView.Y = arrLocation[1];

            //get the id that identtifies a finger over the course of its progress
            int id = motionEvent.GetPointerId(pointerIndex);

            Point screenPointerCoords = new Point(_locationView.X + motionEvent.GetX(pointerIndex),
                _locationView.Y + motionEvent.GetY(pointerIndex));
            var t = e.Event.GetX(pointerIndex);

            //Use actionMasked here rahter than Action to reduce the number of possibilities
            switch (e.Event.ActionMasked)
            {
                case MotionEventActions.Down:
                case MotionEventActions.PointerDown:
                    {
                        FireEvent(id, TouchActionType.Pressed, screenPointerCoords);
                        _idTouchs.Add(id);
                    }
                    break;
                case MotionEventActions.Move:
                    {
                        //Multiple Move events are bundled so handle them in a loop
                        for (pointerIndex = 0; pointerIndex < motionEvent.PointerCount; pointerIndex++)
                        {
                            if (_idTouchs.Contains(id))
                                FireEvent(id, TouchActionType.Moved, screenPointerCoords);
                        }
                    }
                    break;
                case MotionEventActions.Up:
                case MotionEventActions.Pointer1Up:
                    {
                        if (_idTouchs.Contains(id))
                            FireEvent(id, TouchActionType.Released, screenPointerCoords);
                        _idTouchs.Remove(id);
                    }
                    break;
                case MotionEventActions.Cancel:
                    {
                        if (_idTouchs.Contains(id))
                            FireEvent(id, TouchActionType.Cancelled, screenPointerCoords);
                        _idTouchs.Remove(id);
                    }
                    break;
                default:
                    break;
            }
            e.Handled = true;
        }


        private void FireEvent(int id, TouchActionType actionType, Point pointerLocation)
        {
            double x = pointerLocation.X - _locationView.X;
            double y = pointerLocation.Y - _locationView.Y;
            _element.InvokeOnTouch(new TouchActionEventArgs(id, actionType, new Point(x,y),true));
        }   
    }
}
