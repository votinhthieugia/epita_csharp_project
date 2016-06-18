using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Input;

namespace FoodMaze.Scripts.Game.Objects.Touches
{
    public class Touch
    {
      
        public uint Id { get; set; }
        public int StartX { get; set; }
        public int StartY { get; set; }
        public int EndX { get; set; }
        public int EndY { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsCanceled { get; set; }
        public bool IsEnded { get; set; }

        public Touch()
        {
        }

        public Touch(uint id, int startX, int startY)
        {
            Id = id;
            StartX = startX;
            StartY = startY;
            X = StartX;
            Y = StartY;
        }
    }
}
