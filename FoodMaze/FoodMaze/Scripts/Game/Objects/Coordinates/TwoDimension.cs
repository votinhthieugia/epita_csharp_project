using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodMaze.Scripts.Game.Objects.Coordinates
{
    class TwoDimension
    {
        public int X { get; set; }
        public int Y { get; set; }

        public TwoDimension()
        {

        }

        public TwoDimension(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return "(" + X + "," + Y + ")";
        }
    }
}
