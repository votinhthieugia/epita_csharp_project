using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace FoodMaze.Scripts.Game.UI
{
    class UIImagePool
    {
        private static UIImagePool instance;

        public static UIImagePool Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UIImagePool();
                }

                return instance;
            }
            private set { }
        }

        private List<Image> activeImages;
        private Stack<Image> inactiveImages;

        private UIImagePool()
        {
            activeImages = new List<Image>();
            inactiveImages = new Stack<Image>();
        }

        public Image Pop()
        {
            Image image = null;

            if (inactiveImages.Count == 0)
            {
                image = new Image();
                image.HorizontalAlignment = HorizontalAlignment.Left;
                image.VerticalAlignment = VerticalAlignment.Top;
            } else
            {
                image = inactiveImages.Pop();
            }

            activeImages.Add(image);
            return image;
        }

        public void Push(Image image)
        {
            activeImages.Remove(image);
            inactiveImages.Push(image);
        }
    }
}
