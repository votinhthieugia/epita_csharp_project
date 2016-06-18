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
    class Character : TileObject
    {
        public CharacterType Type { get; set; }
        public MoveDirection Direction { get; set; }
        public int Speed { get; set; }

        public Character(IWorld world) : base(world)
        {
        }

        public virtual void Move(float elapsedSeconds)
        {
            Position tilePosition = world.FindTilePosition(CurrentTile);
            Position newPosition;
            Position checkPosition;
            Position otherPosition;
            Position changeTilePosition;
            
            bool isAllowed = true;
            Tile newTile = null;

            switch (Direction)
            {
                case MoveDirection.UP:
                    newPosition = new Position(Position.X, Position.Y - (int)(Speed * elapsedSeconds));
                    otherPosition = new Position(Position.X + Width, Position.Y);
                    checkPosition = newPosition;
                    newTile = world.GetTileAt(CurrentTile.X, CurrentTile.Y - 1);
                    changeTilePosition = new Position(newPosition.X, newPosition.Y + Width);
                    if (checkPosition.Y <= tilePosition.Y && 
                        (!world.IsInTileVertically(Position, CurrentTile) || !world.IsInTileVertically(otherPosition, CurrentTile)))
                        isAllowed = false;
                    if (isAllowed && !world.IsInTile(checkPosition, CurrentTile))
                        if (world.HasHWall(CurrentTile.X, CurrentTile.Y)) isAllowed = false;
                    break;
                case MoveDirection.RIGHT:
                    newPosition = new Position(Position.X + (int)(Speed * elapsedSeconds), Position.Y);
                    otherPosition = new Position(Position.X, Position.Y + Width);
                    checkPosition = new Position(newPosition.X + Width, newPosition.Y);
                    newTile = world.GetTileAt(CurrentTile.X + 1, CurrentTile.Y);
                    if (checkPosition.X >= tilePosition.X + world.TileWidth() && 
                        (!world.IsInTileHorizontally(Position, CurrentTile) || !world.IsInTileHorizontally(otherPosition, CurrentTile)))
                        isAllowed = false;
                    if (isAllowed && !world.IsInTile(checkPosition, CurrentTile))
                        if (world.HasVWall(CurrentTile.X + 1, CurrentTile.Y)) isAllowed = false;
                    changeTilePosition = newPosition;
                    break;
                case MoveDirection.DOWN:
                    newPosition = new Position(Position.X, Position.Y + (int)(Speed * elapsedSeconds));
                    otherPosition = new Position(Position.X + Width, Position.Y);
                    checkPosition = new Position(newPosition.X, newPosition.Y + Width);
                    newTile = world.GetTileAt(CurrentTile.X, CurrentTile.Y + 1);
                    if (checkPosition.Y >= tilePosition.Y + world.TileWidth() && 
                        (!world.IsInTileVertically(Position, CurrentTile) || !world.IsInTileVertically(otherPosition, CurrentTile)))
                        isAllowed = false;
                    if (isAllowed && !world.IsInTile(checkPosition, CurrentTile))
                        if (world.HasHWall(CurrentTile.X, CurrentTile.Y + 1)) isAllowed = false;
                    changeTilePosition = newPosition;
                    break;
                case MoveDirection.LEFT:
                    newPosition = new Position(Position.X - (int)(Speed * elapsedSeconds), Position.Y);
                    otherPosition = new Position(Position.X, Position.Y + Width);
                    checkPosition = newPosition;
                    newTile = world.GetTileAt(CurrentTile.X - 1, CurrentTile.Y);
                    changeTilePosition = new Position(newPosition.X + Width, newPosition.Y);
                    if (checkPosition.X <= tilePosition.X && 
                        (!world.IsInTileHorizontally(Position, CurrentTile) || !world.IsInTileHorizontally(otherPosition, CurrentTile)))
                        isAllowed = false;
                    if (isAllowed && !world.IsInTile(checkPosition, CurrentTile))
                        if (world.HasVWall(CurrentTile.X, CurrentTile.Y)) isAllowed = false;
                    break;
                default:
                    return;
            }

            if (isAllowed && newTile != null && world.IsInTile(changeTilePosition, newTile)) CurrentTile = newTile;

            if (isAllowed && world.IsValidPosition(checkPosition))
            {
                Position = newPosition;
                Notify();
            }
        }
        
        bool HasNeighbor(Tile neighbor)
        {
            List <PFTile> neighbors = CurrentTile.Neighbors;
            foreach (PFTile pfTile in neighbors)
                if (neighbor.X == pfTile.X && neighbor.Y == pfTile.Y) return true;
            return false;
        }
    }
}
