using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace FoodMaze.Scripts.Game.Core
{
    class Wall
    {
        public Point Start { get; set; }
        public Point End { get; set; }
        public bool IsHorizontal { get; set; }

        public Wall(Point start, Point end, bool isHorizontal)
        {
            Start = start;
            End = end;
            IsHorizontal = isHorizontal;
        }

        public bool IsAbleToMerge(Wall another)
        {
            if (IsHorizontal)
            {
                return End.X == another.Start.X;
            }

            return End.Y == another.Start.Y;
        }

        public Wall Clone()
        {
            return new Wall(Start.Clone(), End.Clone(), IsHorizontal);
        }

        public override String ToString()
        {
            return Start.ToString() + "->" + End.ToString();
        }
    }
}
