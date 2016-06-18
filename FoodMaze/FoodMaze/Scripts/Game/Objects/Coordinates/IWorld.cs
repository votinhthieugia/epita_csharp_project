using FoodMaze.Scripts.Game.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodMaze.Scripts.Game.Objects.Coordinates
{
    interface IWorld
    {
        void Init(Maze maze, int width, int height, int offsetTop, int offsetLeft);
        Position FindTilePosition(Tile tile);
        Position FindWallPosition(Wall wall, out int scale);
        Position FindCenterPoint();
        bool HasPath(Tile current, Tile another);
        bool IsValidPosition(Position position);
        bool IsInTile(Position position, Tile tile);
        bool IsInTileHorizontally(Position position, Tile tile);
        bool IsInTileVertically(Position position, Tile tile);
        bool HasHWall(int x, int y);
        bool HasVWall(int x, int y);
        Tile GetTileAt(int x, int y);
        int TileWidth();
        int WallWidth();
    }
}
