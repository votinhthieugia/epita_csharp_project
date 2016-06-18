using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodMaze.Scripts.Game.Core.PathFinding
{
    class PFTile
    {
        public int X { get; set; }
        public int Y { get; set; }
        public float H { get; set; }
        public float G { get; set; }
        public List<PFTile> Neighbors { get; }
        public PFTile back { get; set; }

        public PFTile(int x, int y)
        {
            X = x;
            Y = y;
            Neighbors = new List<PFTile>();
        }

        public override string ToString()
        {
            return "(" + X + "," + Y + ")";
        }

        public bool IsEquals(PFTile obj)
        {
            return X == obj.X && Y == obj.Y;
        }
    }
}
