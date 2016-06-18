using FoodMaze.Scripts.Game.Core;
using FoodMaze.Scripts.Game.Core.PathFinding;
using FoodMaze.Scripts.Game.Objects.Coordinates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodMaze.Scripts.Game.Objects.Characters
{
    class AIPlayer : Character
    {
        public Path Path { get; set; }
        private Position targetPosition;
        private PFTile targetTile;
        private bool shouldMove = false;
        private int frameId = 0;

        public AIPlayer(IWorld world) : base(world)
        {
            Type = CharacterType.AIPlayer;
            Speed = 23;
        }

        public void Go()
        {
            shouldMove = true;
            Direction = MoveDirection.NIL;
            NextMove();
        }

        public override void Update(float seconds)
        {
            frameId++;
            if (shouldMove)
            {
                bool shouldUpdate = true;
                switch (Direction)
                {
                    case MoveDirection.UP:
                        if (Position.Y <= targetPosition.Y)
                        {
                            Position.Y = targetPosition.Y + 1;
                            NextMove();
                            shouldUpdate = false;
                        }
                        break;
                    case MoveDirection.DOWN:
                        if (Position.Y >= targetPosition.Y)
                        {
                            Position.Y = targetPosition.Y + 1;
                            NextMove();
                            shouldUpdate = false;
                        }
                        break;
                    case MoveDirection.LEFT:
                        if (Position.X <= targetPosition.X)
                        {
                            Position.X = targetPosition.X + 1;
                            NextMove();
                            shouldUpdate = false;
                        }
                        break;
                    case MoveDirection.RIGHT:
                        if (Position.X >= targetPosition.X)
                        {
                            Position.X = targetPosition.X + 1;
                            NextMove();
                            shouldUpdate = false;
                        }
                        break;
                }

                if (shouldUpdate) Move(seconds);
            }
        }

        public void RotateDirection(int uiWidth, Position offset)
        {
            InitPositionWithTile(world.GetTileAt(Context.Instance.TileHeight - 1 - CurrentTile.Y, CurrentTile.X));
            Position = new Position(Position.X + world.TileWidth() - uiWidth - offset.Y, Position.Y + offset.X);
            Notify();

            switch (Direction)
            {
                case MoveDirection.UP: Direction = MoveDirection.RIGHT; break;
                case MoveDirection.RIGHT: Direction = MoveDirection.DOWN; break;
                case MoveDirection.DOWN: Direction = MoveDirection.LEFT; break;
                case MoveDirection.LEFT: Direction = MoveDirection.UP; break;
            }

            foreach (PFTile tile in Path.Tiles)
            {
                int tmp = tile.Y;
                tile.Y = tile.X;
                tile.X = Context.Instance.TileWidth - 1 - tmp;
            }

            int tmp2 = targetTile.Y;
            targetTile.Y = targetTile.X;
            targetTile.X = Context.Instance.TileWidth - 1 - tmp2;
            targetPosition = world.FindTilePosition(new Tile(targetTile.X, targetTile.Y));
        }

        private void NextMove()
        {
            targetTile = Path.Tiles.Count > 0? Path.Tiles.Dequeue() : null;
            while (Path.Tiles.Count > 0 && targetTile.IsEquals(CurrentTile)) targetTile = Path.Tiles.Dequeue();
            if (targetTile != null)
            {
                if (targetTile.X == CurrentTile.X)
                {
                    if (targetTile.Y > CurrentTile.Y) Direction = MoveDirection.DOWN;
                    else Direction = MoveDirection.UP;
                }
                else
                {
                    if (targetTile.X > CurrentTile.X) Direction = MoveDirection.RIGHT;
                    else Direction = MoveDirection.LEFT;
                }

                targetPosition = world.FindTilePosition(new Tile(targetTile.X, targetTile.Y));
            }
        }
    }
}
