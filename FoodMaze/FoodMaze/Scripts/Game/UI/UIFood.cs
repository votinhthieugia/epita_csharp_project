using FoodMaze.Scripts.Game.Objects;
using FoodMaze.Scripts.Game.Objects.Coordinates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace FoodMaze.Scripts.Game.UI
{
    class UIFood : UITileObject
    {
        public UIFood(TileObject tileObject, IWorld world) : base(tileObject, world)
        {
        }

        protected override ImageSource Source
        {
            get
            {
                return ImageManager.GetImageSource(ImageId.FOOD);
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
