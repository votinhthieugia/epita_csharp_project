using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodMaze.Scripts.Game.Core.PathFinding
{
    class AStarFinder : IFinder
    {
        // Should only keep references.
        int maxX;
        PFTile[] tiles;
        List<PFTile> openTiles;
        List<PFTile> closedTiles;

        public AStarFinder()
        {
            openTiles = new List<PFTile>();
            closedTiles = new List<PFTile>();
        }

        public void Init(PFTile[] tiles, int maxX)
        {
            this.maxX = maxX;
            this.tiles = tiles;
        }

        public Path Find(PFTile start, PFTile end)
        {
            PFTile realStart = tiles[start.X + start.Y * maxX];
            PFTile realEnd = tiles[end.X + end.Y * maxX];
            Path path = new Path();
            CleanOldPath();
            CalculateHAndGValues(realStart, realEnd);
            openTiles.Add(realStart);
            CalculatePath(realStart, realEnd);

            PFTile current = realEnd;
            while (current != null)
            {
                path.AddToPath(current);
                current = current.back;
            }
            return path;
        }

        void CleanOldPath()
        {
            openTiles.Clear();
            closedTiles.Clear();
        }

        void CalculatePath(PFTile start, PFTile end)
        {
            if (openTiles.Count == 0)
            {
                return;
            }

            PFTile tile = openTiles[0];
            openTiles.RemoveAt(0);
            closedTiles.Add(tile);

            List<PFTile> neighbors = tile.Neighbors;
            for (int i = 0; i < neighbors.Count; i++)
            {
                PFTile neighbor = neighbors[i];
                float newG = neighbor.G + 1;
                if ((openTiles.Contains(neighbor) || closedTiles.Contains(neighbor)) && newG > neighbor.G) continue;
                neighbor.G = newG;

                openTiles.Remove(neighbor);
                closedTiles.Remove(neighbor);

                // Should be a sorted list. For now, use sort function.
                openTiles.Add(neighbor);
                openTiles.Sort(Compare);
                neighbor.back = tile;
            }

            CalculatePath(start, end);
        }
        
        public int Compare(PFTile tile1, PFTile tile2)
        {
            float f1 = tile1.G + tile1.H;
            float f2 = tile2.G + tile2.H;
            if (f1 < f2) return -1;
            else if (f1 > f2) return 1;
            return 0;
        }

        void CalculateHAndGValues(PFTile start, PFTile end)
        {
            for (int i = 0; i < tiles.Length; i++)
            {
                float xDiff = tiles[i].X - end.X;
                float yDiff = tiles[i].Y - end.Y;
                tiles[i].H = (float)Math.Sqrt(xDiff * xDiff + yDiff * yDiff);
                tiles[i].G = 100;
            }

            start.G = 0;
            end.H = 0;
        }

        public void Dispose()
        {
            openTiles.Clear();
            closedTiles.Clear();
            tiles = null;
        }
    }
}
