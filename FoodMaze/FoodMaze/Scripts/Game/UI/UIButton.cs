using FoodMaze.Scripts.Game.Core;
using FoodMaze.Scripts.Game.Objects;
using FoodMaze.Scripts.Game.Objects.Touches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace FoodMaze.Scripts.Game.UI
{
    class UIButton : IDrawable, ITouchable
    {
        public Image image;
        public OnTouchableDown OnTouchDownHandler;
        public OnTouchableUp OnTouchUpHandler;
        public OnTouchableCanceled OnTouchCanceledHandler;
        private bool isDown;
        private Touch currentTouch;

        TappedEventHandler eventHandler;
        
        public UIButton(ImageId imageId, int w, int h, int x, int y)
        {
            image = UIImagePool.Instance.Pop();
            image.Source = ImageManager.GetImageSource(imageId);
            image.Width = w;
            image.Height = h;
            image.Margin = new Thickness(x, y, 0, 0);
            image.IsHoldingEnabled = true;
            image.Tapped += OnTapped;
        }

        public void SetEventHandler(TappedEventHandler eventHandler)
        {
            this.eventHandler = eventHandler;
        }

        void OnTapped(object sender, object e)
        {
            if (eventHandler != null)
            {
                eventHandler?.Invoke(sender, (TappedRoutedEventArgs)e);
            }
        }

        public ICollection<object> GetObjects()
        {
            return new List<object>() { image };
        }

        public void RegisterTo(IDrawer drawer)
        {
            drawer.Register(this);
        }

        public void Unregister(IDrawer drawer)
        {
            drawer.Unregister(this);
        }

        public void UpdateTouch(Touch[] touches, int numTouches)
        {
            if (currentTouch != null)
            {
                if (currentTouch.IsEnded)
                {
                    if (IsTouchInside(currentTouch.EndX, currentTouch.EndY)) OnTouchUpHandler?.Invoke(this, currentTouch);
                    else OnTouchCanceledHandler?.Invoke(this, currentTouch);
                    currentTouch = null;
                    isDown = false;
                }
                // Handle hold.
                //else
                //{
                    
                //}
            }
            else
            {
                foreach (Touch touch in touches)
                {
                    if (IsTouchInside(touch.StartX, touch.StartY) && !isDown)
                    {
                        OnTouchDownHandler?.Invoke(this, touch);
                        isDown = true;
                        currentTouch = touch;
                        break;
                    }
                }
            }
        }

        private bool IsTouchInside(int touchX, int touchY)
        {
            return image.Margin.Left <= touchX && touchX <= image.Margin.Left + image.Width &&
                    image.Margin.Top <= touchY && touchY <= image.Margin.Top + image.Height;
        }

        public void Dispose()
        {
            UIImagePool.Instance.Push(image);
            image = null;
        }
    }
}
