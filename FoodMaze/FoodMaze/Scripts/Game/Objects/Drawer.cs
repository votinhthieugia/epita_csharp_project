using FoodMaze.Scripts.Game.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace FoodMaze.Scripts.Game.Objects
{
    class Drawer : IDrawer
    {
        Panel UI { get; set; }

        public Drawer(Panel ui, float width, float height)
        {
            UI = ui;
            UI.Background = new SolidColorBrush(Colors.Black);
            UI.HorizontalAlignment = HorizontalAlignment.Center;
            UI.VerticalAlignment = VerticalAlignment.Center;
            UI.Width = width;
            UI.Height = height;
            UI.Margin = new Thickness(0, 0, 0, 0);
        }

        public void Register(IDrawable drawable)
        {
            ICollection<object> objects = drawable.GetObjects();
            foreach (object obj in objects)
            {
                UIElement element = (UIElement)obj;
                if (!UI.Children.Contains(element)) UI.Children.Add(element);
            }
        }

        public void Unregister(IDrawable drawable)
        {
            ICollection<object> objects = drawable.GetObjects();
            foreach (object obj in objects)
            {
                UI.Children.Remove((UIElement)obj);
            }
        }

        public object GetRenderer()
        {
            return UI;
        }
    }
}
