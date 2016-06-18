using FoodMaze.Scripts.Game.Core;
using FoodMaze.Scripts.Game.Objects.Coordinates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodMaze.Scripts.Game.Objects
{
    public delegate void EventUpdateHandler();

    class TileObject
    {
        protected IWorld world;
        public int Width { get; set; }
        public Position Position { get; set; }
        public Tile CurrentTile { get; set; }
        public event EventUpdateHandler UpdateHandler;

        public TileObject(IWorld world)
        {
            this.world = world;
        }

        public virtual void Update(float seconds)
        {
            throw new NotImplementedException();
        }

        public virtual void InitPositionWithTile(Tile tile)
        {
            CurrentTile = tile;
            Position = world.FindTilePosition(CurrentTile);
        }

        public virtual void Notify()
        {
            UpdateHandler.Invoke();
        }

        public virtual bool IsCollideWith(TileObject other)
        {
            return Math.Abs(Position.X - other.Position.X + Width / 2 - other.Width / 2) < (Width + other.Width) / 2 &&
                    Math.Abs(Position.Y - other.Position.Y + Width / 2 - other.Width / 2) < (Width + other.Width) / 2;
        }
    }
}
