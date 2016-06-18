using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodMaze.Scripts.Game.Core.PathFinding
{
    class Path
    {
        public Queue<PFTile> Tiles;

        public Path()
        {
            Tiles = new Queue<PFTile>();
        }

        public void AddToPath(PFTile tile)
        {
            Tiles.Enqueue(tile);
        }

        public void Print()
        {
            System.Diagnostics.Debug.Write("Path:");
            foreach (PFTile tile in Tiles) System.Diagnostics.Debug.Write(tile.ToString() + ", ");
        }
    }
}
