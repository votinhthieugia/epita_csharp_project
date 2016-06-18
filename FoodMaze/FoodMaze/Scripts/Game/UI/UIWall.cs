using FoodMaze.Scripts.Game.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using System.Collections.ObjectModel;
using FoodMaze.Scripts.Game.Objects;
using FoodMaze.Scripts.Game.Objects.Coordinates;

namespace FoodMaze.Scripts.Game.UI
{
    class UIWall : IDrawable
    {
        public Wall AttachedWall { get; set; }
        public IWorld World { get; set; }
        List<Image> images;
        List<Position> originalPositions;

        public UIWall()
        {
            originalPositions = new List<Position>();
            images = new List<Image>();
        }

        public UIWall(Wall wall, IWorld world)
        {
            images = new List<Image>();
            originalPositions = new List<Position>();
            Init(wall, world);
        }

        public void Init(Wall wall, IWorld world)
        {
            images.Clear();
            originalPositions.Clear();
            AttachedWall = wall;
            World = world;
            PrepareUI();
        }

        public void RegisterTo(IDrawer drawer)
        {
            drawer.Register(this);
        }

        public void Dispose()
        {
            foreach (Image image in images) {
                UIImagePool.Instance.Push(image);
            }
            images.Clear();
        }

        private void PrepareUI()
        {
            int numImages = 0;
            Position position = World.FindWallPosition(AttachedWall, out numImages);
            for (int i = 0; i < numImages; i++)
            {
                Image image = UIImagePool.Instance.Pop();
                image.Source = ImageManager.GetImageSource(ImageId.WALL);
                image.Width = World.WallWidth();
                image.Margin = AttachedWall.IsHorizontal ? new Thickness(position.X + i * World.WallWidth(), position.Y, 0, 0) :
                                                            new Thickness(position.X, position.Y + i * World.WallWidth(), 0, 0);
                images.Add(image);
                originalPositions.Add(new Position((int)image.Margin.Left, (int)image.Margin.Top));
            }
        }

        public ICollection<object> GetObjects()
        {
            return images.ToList<object>();
        }

        public void Unregister(IDrawer drawer)
        {
            drawer.Unregister(this);
        }

        public void Rotate(Position center, double angle, double targetAngleInDegree, bool finished)
        {
           
            for (int i = 0; i < images.Count; i++)
            {
                Position origin = originalPositions[i];
                double sin = Math.Sin(angle);
                double cos = Math.Cos(angle);
                int x = center.X + (int)(cos * (origin.X - center.X) - sin * (origin.Y - center.Y) - 0.5f);
                int y = center.Y + (int)(sin * (origin.X - center.X) + cos * (origin.Y - center.Y) - 0.5f);
                images[i].Margin = new Thickness(x, y, 0, 0);
            }
        }
    }
}
