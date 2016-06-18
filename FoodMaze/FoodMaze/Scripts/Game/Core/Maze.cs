using FoodMaze.Scripts.Game.Core.PathFinding;
using FoodMaze.Scripts.Game.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodMaze.Scripts.Game.Core
{
    class Maze
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public Tile[] Tiles { get; set; }
        public List<Wall> HWalls;
        public List<Wall> VWalls;
        
        public Maze(int width, int height)
        {
            Width = width;
            Height = height;
            Tiles = new Tile[Width * Height];
            HWalls = new List<Wall>();
            VWalls = new List<Wall>();
        }

        public Tile GetAt(int x, int y)
        {
            if (0 <= x && x < Width && 0 <= y && y < Height)
            {
                return Tiles[y * Width + x];
            }
            return null;
        }

        public Maze Rotate()
        {
            Maze maze = new Maze(Height, Width);
            foreach (Wall wall in HWalls)
            {
                Wall vWall = new Wall(new Point(Width - wall.Start.Y, wall.Start.X), new Point(Width - wall.End.Y, wall.End.X), false);
                maze.VWalls.Add(vWall);
            }
            foreach (Wall wall in VWalls)
            {
                Wall hWall = new Wall(new Point(Height - wall.End.Y, wall.End.X), new Point(Height - wall.Start.Y, wall.Start.X), true);
                maze.HWalls.Add(hWall);
            }
            for (int i = 0; i < Tiles.Length; i++)
            {
                Tile tile = new Tile(Width - 1 - Tiles[i].Y, Tiles[i].X);
                maze.Tiles[tile.X + Height * tile.Y] = tile;
            }
            for (int i = 0; i < Tiles.Length; i++)
            {
                Tile tile = maze.Tiles[Width - 1 - Tiles[i].Y + Tiles[i].X * Height];
                List<PFTile> neighbors = Tiles[i].Neighbors;
                foreach (PFTile pfTile in neighbors)
                {
                    tile.Neighbors.Add(maze.Tiles[Width - 1 - pfTile.Y + pfTile.X * Height]);
                }
            }
            return maze;
        }

        public void PrintWalls()
        {
            System.Diagnostics.Debug.WriteLine("-----------------------");
            foreach (Wall wall in HWalls)
            {
                System.Diagnostics.Debug.WriteLine(wall.ToString());
            }
            foreach (Wall wall in VWalls)
            {
                System.Diagnostics.Debug.WriteLine(wall.ToString());
            }
            System.Diagnostics.Debug.WriteLine("-----------------------");
        }

        public void PrintNeighbors()
        {
            foreach (Tile tile in Tiles)
            {
                System.Diagnostics.Debug.WriteLine(tile.ToString() + " neighbors:");
                foreach (PFTile pf in tile.Neighbors) System.Diagnostics.Debug.WriteLine(pf.ToString() + " ");
                System.Diagnostics.Debug.WriteLine("=========================");
            }
        }
    }
}
