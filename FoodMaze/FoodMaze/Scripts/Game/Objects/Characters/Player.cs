using FoodMaze.Scripts.Game.Objects.Coordinates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodMaze.Scripts.Game.Objects.Characters
{
    class Player : Character
    {
        public bool ShouldMove { get; set; }

        public Player(IWorld world) : base(world)
        {
            Type = CharacterType.Player;
            Speed = 30;
        }

        public override void Update(float seconds)
        {
            if (ShouldMove)
            {
                Move(seconds);
                Notify();
            }
        }

        public void RotateDirection(int uiWidth, Position offset)
        {
            InitPositionWithTile(world.GetTileAt(Context.Instance.TileHeight - 1 - CurrentTile.Y, CurrentTile.X));
            Position = new Position(Position.X + world.TileWidth() - Width - offset.Y, Position.Y + offset.X);
            Notify();
        }
    }
}
