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
    class UIPlayer : UITileObject
    {
        private TextBlock text;

        public UIPlayer(TileObject tileObject, IWorld world) : base(tileObject, world)
        {
            text = new TextBlock();
            text.Text = "You";
            text.Foreground = new SolidColorBrush(Colors.White);
        }

        public override void UpdateHandler()
        {
            base.UpdateHandler();
            text.Margin = new Thickness(image.Margin.Left, image.Margin.Top - Width/2, 0, 0);
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
                return ImageManager.GetImageSource(ImageId.PLAYER);
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
