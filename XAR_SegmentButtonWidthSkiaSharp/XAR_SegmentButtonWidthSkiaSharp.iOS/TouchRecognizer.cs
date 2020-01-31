using System;
using System.Collections.Generic;
using CoreGraphics;
using Foundation;
using TouchTracking;
using UIKit;
using Xamarin.Forms;

namespace XAR_SegmentButtonWidthSkiaSharp.iOS
{
    internal class TouchRecognizer : UIGestureRecognizer
    {
        Element _element;
        UIView _view;
        TouchEffect _touchEffect;
        bool _capture;

        static Dictionary<UIView, TouchRecognizer> _viewDictionary =
            new Dictionary<UIView, TouchRecognizer>();

        static Dictionary<long, TouchRecognizer> _idToTouchDictionary =
            new Dictionary<long, TouchRecognizer>();



        public TouchRecognizer(Element element, UIView view, TouchEffect touchEffect)
        {
            _element = element;
            _view = view;
            _touchEffect = touchEffect;

            _viewDictionary.Add(_view, this);
        }


        #region TouchesBegan
        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);

            foreach (var touch in touches)
            {
                long id = touch.Handle.ToInt64();
                FireEvent(this, id, TouchActionType.Pressed, touch, true);

                if (!_idToTouchDictionary.ContainsKey(id))
                    _idToTouchDictionary.Add(id, this);
            }

            //Save the setting of the Capture property
            _capture = _touchEffect.Capture;

        }
        #endregion
        #region TouchesMoved
        public override void TouchesMoved(NSSet touches, UIEvent evt)
        {
            base.TouchesMoved(touches, evt);

            foreach (var touch in touches)
            {
                long id = touch.Handle.ToInt64();

                if (_capture)
                    FireEvent(this, id, TouchActionType.Moved, touch, true);
                else
                {
                    CheckForBoundaryHop(touch);

                    if (_idToTouchDictionary[id] != null)
                        FireEvent(this, id, TouchActionType.Moved, touch, true);
                }
            }

        }
        #endregion
        #region Touch End
        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            base.TouchesEnded(touches, evt);

            foreach (var touch in touches)
            {
                long id = touch.Handle.ToInt64();
                if (_capture)
                    FireEvent(this, id, TouchActionType.Released, touch, false);
                else
                {
                    CheckForBoundaryHop(touch);
                    if (_idToTouchDictionary[id] != null)
                        FireEvent(this, id, TouchActionType.Released, touch, false);
                }
                _idToTouchDictionary.Remove(id);
            }
        }
        #endregion
        #region TouchCancelled
        public override void TouchesCancelled(NSSet touches, UIEvent evt)
        {
            base.TouchesCancelled(touches, evt);

            foreach (var touch in touches)
            {
                long id = touch.Handle.ToInt64();

                if(_capture)
                    FireEvent(this, id, TouchActionType.Cancelled, touch, false);
                else if (_idToTouchDictionary[id]!=null)
                    FireEvent(_idToTouchDictionary[id], id, TouchActionType.Cancelled, touch, false);
                _idToTouchDictionary.Remove(id);
            }
        }
        #endregion

        private void FireEvent(TouchRecognizer touchRecognizer, long id, TouchActionType pressed, NSObject touch, bool v)
        {
            throw new NotImplementedException();
        }
    }
}