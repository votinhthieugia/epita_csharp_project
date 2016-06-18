using FoodMaze.Scripts.Game.Core;
using FoodMaze.Scripts.Game.Objects;
using FoodMaze.Scripts.Game.Objects.Coordinates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace FoodMaze.Scripts.Game.UI
{
    class UITileObject : IDrawable
    {
        public Image image;
        protected TileObject tileObject;
        protected IWorld world;
        protected Position originalPosition;

        protected virtual ImageSource Source
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public virtual int Width
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public UITileObject(TileObject tileObject, IWorld world)
        {
            this.tileObject = tileObject;
            this.tileObject.UpdateHandler += UpdateHandler;
            this.world = world;
            image = UIImagePool.Instance.Pop();
            image.Source = Source;
            image.Width = Width;
        }

        public virtual void UpdateHandler()
        {
            image.Margin = new Thickness(tileObject.Position.X, tileObject.Position.Y, 0, 0);
        }

        public virtual ICollection<object> GetObjects()
        {
            return new List<object>() { image };
        }

        public void RegisterTo(IDrawer drawer)
        {
            drawer.Register(this);
        }

        public virtual void Dispose()
        {
            UIImagePool.Instance.Push(image);
            image = null;
            tileObject.UpdateHandler -= UpdateHandler;
        }

        public void Unregister(IDrawer drawer)
        {
            drawer.Unregister(this);
        }

        public void Rotate(Position center, double angle, double targetAngleInDegree, bool finished)
        {
            double sin = Math.Sin(angle);
            double cos = Math.Cos(angle);
            int x = center.X + (int)(cos * (originalPosition.X - center.X) - sin * (originalPosition.Y - center.Y) - 0.5f);
            int y = center.Y + (int)(sin * (originalPosition.X - center.X) + cos * (originalPosition.Y - center.Y) - 0.5f);
            image.Margin = new Thickness(x, y, 0, 0);
        }

        public void StartRotate()
        {
            originalPosition = new Position((int)image.Margin.Left, (int)image.Margin.Top);
        }
    }
}
