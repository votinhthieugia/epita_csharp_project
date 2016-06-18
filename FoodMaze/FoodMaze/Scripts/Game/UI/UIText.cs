using FoodMaze.Scripts.Game.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace FoodMaze.Scripts.Game.UI
{
    class UIText : IDrawable
    {
        private TextBlock textBlock;

        public object FontSizes { get; private set; }

        public UIText(string text, int x, int y)
        {
            textBlock = new TextBlock();
            textBlock.Foreground = new SolidColorBrush(Colors.White);
            textBlock.Margin = new Thickness(x, y, 0, 0);
            textBlock.FontWeight = FontWeights.Bold;
            textBlock.FontSize = 30;
            SetText(text);
        }

        public void SetText(string text)
        {
            textBlock.Text = text;
        }

        public virtual ICollection<object> GetObjects()
        {
            return new List<object>() { textBlock };
        }

        public void RegisterTo(IDrawer drawer)
        {
            drawer.Register(this);
        }

        public virtual void Dispose()
        {

        }

        public void Unregister(IDrawer drawer)
        {
            drawer.Unregister(this);
        }
    }
}
