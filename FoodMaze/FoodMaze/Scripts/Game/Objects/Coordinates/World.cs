using FoodMaze.Scripts.Game.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodMaze.Scripts.Game.Objects.Coordinates
{
    class World : IWorld
    {
        public Maze Maze { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int OffsetLeft { get; set; }
        public int OffsetTop { get; set; }
        private int tileWidth;
        private int feasibleWidth;
        private int[] hWallOffsets;
        private int[] vWallOffsets;
        private bool[] hasHWalls;
        private bool[] hasVWalls;
        private int leftMargin;
        private int rightMargin;
        private int topMargin;
        private int bottomMargin;

        public void Init(Maze maze, int width, int height, int offsetTop, int offsetLeft)
        {
            Maze = maze;
            Width = width;
            Height = height;
            OffsetLeft = offsetLeft;
            OffsetTop = offsetTop;
            PreCalculatePositions();
        }

        private void PreCalculatePositions()
        {
            feasibleWidth = Math.Min(Width, Height);
            tileWidth = feasibleWidth * 4 / (5 * Maze.Width + 1);
            while (tileWidth % 4 != 0) tileWidth--;

            // Count num walls at each tile.
            hWallOffsets = new int[Maze.Width + 1];
            vWallOffsets = new int[Maze.Height + 1];

            // Vertical.
            hasHWalls = new bool[Maze.Height + 1];
            for (int i = 0; i < Maze.HWalls.Count; i++) hasHWalls[Maze.HWalls[i].Start.Y] = true;
            
            // Prefix sum.
            int count = 0;
            for (int i = 0; i < Maze.Height + 1; i++)
            {
                vWallOffsets[i] = i * TileWidth() + count * WallWidth();
                if (hasHWalls[i]) count++;
                //System.Diagnostics.Debug.WriteLine(hasHWalls[i] + " vWalls:" + i + " " + vWallOffsets[i]);
            }

            // Horizontal.
            hasVWalls = new bool[Maze.Width + 1];
            for (int i = 0; i < Maze.VWalls.Count; i++) hasVWalls[Maze.VWalls[i].Start.X] = true;
            
            count = 0;
            for (int i = 0; i < Maze.Width + 1; i++)
            {
                hWallOffsets[i] = i * TileWidth() + count * WallWidth();
                if (hasVWalls[i]) count++;
                //System.Diagnostics.Debug.WriteLine(hasVWalls[i] + " hWalls:" + i + " " + hWallOffsets[i]);
            }

            OffsetLeft += (Width - hWallOffsets[Maze.Width]) / 2;
            OffsetTop += (Height - vWallOffsets[Maze.Height]) / 2;

            leftMargin = OffsetLeft + hWallOffsets[0] + WallWidth();
            rightMargin = OffsetLeft + hWallOffsets[Maze.Width];
            topMargin = OffsetTop + vWallOffsets[0] + WallWidth();
            bottomMargin = OffsetTop + vWallOffsets[Maze.Height];
        }

        public Position FindTilePosition(Tile tile)
        {
            int x = FindStartPosition(tile.X, true);
            if (hasVWalls[tile.X]) x += WallWidth();
            int y = FindStartPosition(tile.Y, false);
            if (hasHWalls[tile.Y]) y += WallWidth();
            return new Position(x, y);
        }

        public Position FindWallPosition(Wall wall, out int scale)
        {
            Position position = new Position(FindStartPosition(wall.Start.X, true), FindStartPosition(wall.Start.Y, false));
            if (wall.IsHorizontal)
            {
                scale = (int)((FindStartPosition(wall.End.X, true) - position.X) / WallWidth());
                if (wall.End.X == Maze.Width || hasVWalls[wall.End.X]) scale++;
            }
            else
            {
                scale = (int)((FindStartPosition(wall.End.Y, false) - position.Y) / WallWidth());
                if (wall.End.Y == Maze.Height || hasHWalls[wall.End.Y]) scale++;
            }
            return position;
        }

        private int FindStartPosition(int startPoint, bool isHorizontal)
        {
            return isHorizontal? hWallOffsets[startPoint] + OffsetLeft : vWallOffsets[startPoint] + OffsetTop;
        }

        public bool HasPath(Tile current, Tile another)
        {
            return false;
        }

        public int TileWidth()
        {
            return tileWidth;
        }

        public int WallWidth()
        {
            return tileWidth / 4;
        }

        public bool IsValidPosition(Position position)
        {
            return leftMargin <= position.X && position.X <= rightMargin &&
                    topMargin <= position.Y && position.Y <= bottomMargin; 
        }

        public bool IsInTile(Position position, Tile tile)
        {
            Position tilePosition = FindTilePosition(tile);
            return tilePosition.X <= position.X && position.X <= tilePosition.X + tileWidth &&
                    tilePosition.Y <= position.Y && position.Y <= tilePosition.Y + tileWidth;
        }

        public bool IsInTileHorizontally(Position position, Tile tile)
        {
            Position tilePosition = FindTilePosition(tile);
            return tilePosition.Y <= position.Y && position.Y <= tilePosition.Y + tileWidth;
        }

        public bool IsInTileVertically(Position position, Tile tile)
        {
            Position tilePosition = FindTilePosition(tile);
            return tilePosition.X <= position.X && position.X <= tilePosition.X + tileWidth;
        }

        public Tile GetTileAt(int x, int y)
        {
            return Maze.GetAt(x, y);
        }

        public bool HasHWall(int x, int y)
        {
            foreach (Wall wall in Maze.HWalls)
                if (wall.Start.X <= x && x < wall.End.X && wall.Start.Y == y) return true;
            return false;
        }

        public bool HasVWall(int x, int y)
        {
            foreach (Wall wall in Maze.VWalls)
                if (wall.Start.X == x && wall.Start.Y <= y && y < wall.End.Y) return true;
            return false;
        }

        public Position FindCenterPoint()
        {
            return new Position((leftMargin + rightMargin) / 2, (topMargin + bottomMargin) / 2);
        }
    }
}
