using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodMaze.Scripts.Game.Core
{
    class Point
    {
        private int x;
        public int X { get; set; }
        public int Y { get; set; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        internal Point Clone()
        {
            return new Point(X, Y);
        }

        public override String ToString()
        {
            return "(" + X + "," + Y + ")";
        }
    }
}
