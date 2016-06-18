using FoodMaze.Scripts.Game.Objects;
using FoodMaze.Scripts.Game.Objects.Coordinates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace FoodMaze.Scripts.Game.UI
{
    class UIAIPlayer : UITileObject
    {
        private TextBlock text;

        public UIAIPlayer(TileObject tileObject, IWorld world) : base(tileObject, world)
        {
            text = new TextBlock();
            text.Text = "AI";
            text.Foreground = new SolidColorBrush(Colors.Red);
        }

        public override void UpdateHandler()
        {
            base.UpdateHandler();
            text.Margin = new Thickness(image.Margin.Left, image.Margin.Top - Width / 2, 0, 0);
        }

        public override ICollection<object> GetObjects()
        {
            ICollection<object> objs = base.GetObjects();
            objs.Add(text);
            return objs;
        }

        protected override ImageSource Source
        {
            get
            {
                return ImageManager.GetImageSource(ImageId.AI);
            }
        }

        public override int Width
        {
            get
            {
                return world.TileWidth() * 3 / 4;
            }
        }
    }
}
