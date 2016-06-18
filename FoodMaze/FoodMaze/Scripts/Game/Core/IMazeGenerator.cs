using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodMaze.Scripts.Game.Core
{
    interface IMazeGenerator
    {
        Maze Generate(int width, int height);
    }
}
