using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodMaze.Scripts.Game.Core
{
    class RecursiveGenerator : IMazeGenerator
    {
        int maxX;
        int maxY;
        List<Wall> hWalls;
        List<Wall> vWalls;

        public RecursiveGenerator()
        {
        }

        public Maze Generate(int width, int height)
        {
            Maze maze = new Maze(width, height);
            hWalls = maze.HWalls;
            vWalls = maze.VWalls;

            Tile[] tiles = maze.Tiles;

            maxX = width;
            maxY = height;
            hWalls.Clear();
            vWalls.Clear();

            // Init 4 walls.
            InitBorderWalls();

            // Divide recursively the maze.
            Divide(new Point(0, 0), new Point(maxX, maxY));

            //PrintWalls();

            // Merge walls.
            MergeWalls();

            // Init tiles and neighbors.
            FindNeighbors(tiles);
            //PrintWalls();
            return maze;
        }

        void PrintWalls()
        {
            System.Diagnostics.Debug.WriteLine("-----------------------");
            foreach (Wall wall in hWalls)
            {
                System.Diagnostics.Debug.WriteLine(wall.ToString());
            }
            foreach (Wall wall in vWalls)
            {
                System.Diagnostics.Debug.WriteLine(wall.ToString());
            }
            System.Diagnostics.Debug.WriteLine("-----------------------");
        }

        void InitBorderWalls()
        {
            // Horizontal walls.
            hWalls.Add(new Wall(new Point(0, 0), new Point(maxX, 0), true));
            hWalls.Add(new Wall(new Point(0, maxY), new Point(maxX, maxY), true));

            // Vertical walls.
            vWalls.Add(new Wall(new Point(0, 0), new Point(0, maxY), false));
            vWalls.Add(new Wall(new Point(maxX, 0), new Point(maxX, maxY), false));
        }

        void Divide(Point topLeft, Point bottomRight)
        {
            //System.Diagnostics.Debug.WriteLine(topLeft.ToString() + " " + bottomRight.ToString());
            if (bottomRight.X - topLeft.X == 1 || bottomRight.Y - topLeft.Y == 1)
            {
                return;
            }

            Random rand = new Random();

            // Randomly divide the chamber.
            int v = rand.Next(topLeft.X + 1, bottomRight.X);
            int h = rand.Next(topLeft.Y + 1, bottomRight.Y);

            // Intersection point.
            Point intersection = new Point(v, h);

            // Choose 3 of 4 lines to make a hole.
            int unselected = rand.Next(0, 100) % 4;

            // Left wall.
            Point middleLeft = new Point(topLeft.X, h);
            if (unselected == 0)
            {
                hWalls.Add(new Wall(middleLeft.Clone(), intersection.Clone(), true));
            } else
            {
                MakeHole(middleLeft, intersection, true);
            }

            // Right wall.
            Point middleRight = new Point(bottomRight.X, h);
            if (unselected == 1)
            {
                hWalls.Add(new Wall(intersection.Clone(), middleRight.Clone(), true));
            } else
            {
                MakeHole(intersection, middleRight, true);
            }

            // Top wall.
            Point middleTop = new Point(v, topLeft.Y);
            if (unselected == 2)
            {
                vWalls.Add(new Wall(middleTop.Clone(), intersection.Clone(), false));
            } else
            {
                MakeHole(middleTop, intersection, false);
            }

            // Bottom wall.
            Point middleBottom = new Point(v, bottomRight.Y);
            if (unselected == 3)
            {
                vWalls.Add(new Wall(intersection.Clone(), middleBottom.Clone(), false));
            } else
            {
                MakeHole(intersection, middleBottom, false);
            }

            // Recursively divide the 4 smaller chambers.
            Divide(topLeft, intersection);
            Divide(intersection, bottomRight);
            Divide(middleLeft, middleBottom);
            Divide(middleTop, middleRight);
        }

        void MakeHole(Point start, Point end, bool isHorizontal)
        {
            Random rand = new Random();

            if (isHorizontal)
            {
                if (end.X > start.X + 1)
                {
                    int randX = rand.Next(start.X, end.X);
                    if (randX > start.X) hWalls.Add(new Wall(start.Clone(), new Point(randX, start.Y), true));
                    if (end.X > randX + 1) hWalls.Add(new Wall(new Point(randX + 1, start.Y), end.Clone(), true));
                }
            } else
            {
                if (end.Y > start.Y + 1)
                {
                    int randY = rand.Next(start.Y, end.Y);
                    if (randY > start.Y) vWalls.Add(new Wall(start.Clone(), new Point(start.X, randY), false));
                    if (end.Y > randY + 1) vWalls.Add(new Wall(new Point(start.X, randY + 1), end.Clone(), false));
                }
            }
        }

        void MergeWalls()
        {
            // Sort the walls first.
            hWalls.Sort(SortHWall);
            vWalls.Sort(SortVWall);

            //PrintWalls();

            // Merge hWalls.
            List<Wall> temps = new List<Wall>(hWalls);
            hWalls.Clear();
            Wall current = temps[0];
            int index = 1;
            while (index < temps.Count)
            {
                Wall wall = temps[index];
                if (current.Start.Y == wall.Start.Y && current.End.X == wall.Start.X)
                {
                    current.End.X = wall.End.X;
                }
                else
                {
                    hWalls.Add(current);
                    current = wall.Clone();
                }
                index++;
            }
            hWalls.Add(current);

            // Vertical walls.
            temps = new List<Wall>(vWalls);
            vWalls.Clear();
            current = temps[0];
            index = 1;
            while (index < temps.Count)
            {
                Wall wall = temps[index];
                if (current.Start.X == wall.Start.X && current.End.Y == wall.Start.Y)
                {
                    current.End.Y = wall.End.Y;
                }
                else
                {
                    vWalls.Add(current);
                    current = wall.Clone();
                }
                index++;
            }
            vWalls.Add(current);
            //PrintWalls();
        }

        void FindNeighbors(Tile[] tiles)
        {
            // Initialize all the tiles.
            for (int i = 0; i < tiles.Length; i++)
            {
                tiles[i] = new Tile(i % maxX, i / maxX);
            }

            // Find neighbors for each tile.
            for (int i = 0; i < tiles.Length; i++)
            {
                // Top tile.
                if (tiles[i].Y > 0) CheckNeighbor(tiles, i, i - maxX, false);

                // Bottom tile.
                if (tiles[i].Y < maxY - 1) CheckNeighbor(tiles, i, i + maxX, false);

                // Left tile.
                if (tiles[i].X > 0) CheckNeighbor(tiles, i, i - 1, true);

                // Right tile.
                if (tiles[i].X < maxX - 1) CheckNeighbor(tiles, i, i + 1, true);
                //System.Diagnostics.Debug.WriteLine(tiles[i].ToString() + " " + tiles[i].Neighbors.Count);
                //foreach (Tile tile in tiles[i].Neighbors) System.Diagnostics.Debug.Write(tile.ToString());
                //System.Diagnostics.Debug.WriteLine("");
            }
        }

        void CheckNeighbor(Tile[] tiles, int currentIndex, int neighborIndex, bool isHorizontal)
        {
            if (neighborIndex >= 0 && neighborIndex < tiles.Length && 
                !IsPathBlocked(tiles[currentIndex], tiles[neighborIndex], isHorizontal))
            {
                tiles[currentIndex].Neighbors.Add(tiles[neighborIndex]);
            }
        }

        bool IsPathBlocked(Tile tile1, Tile tile2, bool isHorizontal)
        {
            if (isHorizontal)
            {
                int wallX = tile1.X < tile2.X ? tile2.X : tile1.X;
                for (int i = 0; i < vWalls.Count; i++)
                {
                    if (vWalls[i].Start.X == wallX && vWalls[i].Start.Y <= tile1.Y && tile1.Y < vWalls[i].End.Y) return true;
                    if (vWalls[i].Start.X > wallX) break;
                }
            } else
            {
                int wallY = tile1.Y < tile2.Y ? tile2.Y : tile1.Y;
                for (int i = 0; i < hWalls.Count; i++) {
                    if (hWalls[i].Start.Y == wallY && hWalls[i].Start.X <= tile1.X && tile1.X < hWalls[i].End.X) return true;
                    if (hWalls[i].Start.Y > wallY) break;
                }
            }

            return false;
        }

        int SortHWall(Wall wall1, Wall wall2)
        {
            if (wall1.Start.Y < wall2.Start.Y) return -1;
            if (wall1.Start.Y > wall2.Start.Y) return 1;
            if (wall1.Start.X < wall2.Start.X) return -1;
            if (wall1.Start.X > wall2.Start.X) return 1;
            return 0;
        }

        int SortVWall(Wall wall1, Wall wall2)
        {
            if (wall1.Start.X < wall2.Start.X) return -1;
            if (wall1.Start.X > wall2.Start.X) return 1;
            if (wall1.Start.Y < wall2.Start.Y) return -1;
            if (wall1.Start.Y > wall2.Start.Y) return 1;
            return 0;
        }
    }
}
