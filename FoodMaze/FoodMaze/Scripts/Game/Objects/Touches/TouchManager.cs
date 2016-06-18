using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Input;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace FoodMaze.Scripts.Game.Objects.Touches
{
    class TouchManager
    {
        private static TouchManager instance;
        public static TouchManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TouchManager();
                }

                return instance;
            }
        }

        public List<Touch> touches { get; private set; }
        public Panel renderer;

        public void Init(Panel renderer)
        {
            touches = new List<Touch>();
            this.renderer = renderer;
            renderer.PointerPressed += PointerPressed;
            renderer.PointerReleased += PointerReleased;
            renderer.PointerExited += PointerExited;
            renderer.PointerCanceled += PointerCanceled;
            renderer.PointerMoved += PointerMoved;
        }

        public void RemoveEndedTouches()
        {
            int i = 0;
            while (i < touches.Count)
            {
                if (touches[i].IsEnded)
                {
                    touches.Remove(touches[i]);
                }
                else
                {
                    i++;
                }
            }
        }

        private void PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            PointerPoint pp = e.GetCurrentPoint(renderer);
            if (pp.IsInContact)
            {
                bool found = false;
                foreach (Touch touch in touches)
                {
                    if (touch.Id == pp.PointerId && !touch.IsEnded)
                    {
                        touch.X = (int)pp.Position.X;
                        touch.Y = (int)pp.Position.Y;
                        found = true;
                        break;
                    }
                }

                if (!found) touches.Add(new Touch(pp.PointerId, (int)pp.Position.X, (int)pp.Position.Y));
            }
        }

        private void PointerCanceled(object sender, PointerRoutedEventArgs e)
        {
            PointerPoint pp = e.GetCurrentPoint(renderer);
            RemoveTouch(pp);
        }

        private void PointerExited(object sender, PointerRoutedEventArgs e)
        {
            PointerPoint pp = e.GetCurrentPoint(renderer);
            RemoveTouch(pp);
        }

        private void PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            PointerPoint pp = e.GetCurrentPoint(renderer);
            RemoveTouch(pp);
        }

        private void PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            PointerPoint pp = e.GetCurrentPoint(renderer);
            RemoveTouch(pp);
            touches.Add(new Touch(pp.PointerId, (int)pp.Position.X, (int)pp.Position.Y));
        }

        private void RemoveTouch(PointerPoint pp)
        {
            foreach (Touch touch in touches)
            {
                if (touch.Id == pp.PointerId)
                {
                    touch.EndX = (int)pp.Position.X;
                    touch.EndY = (int)pp.Position.Y;
                    touch.IsEnded = true;
                    break;
                }
            }
        }
    }
}
