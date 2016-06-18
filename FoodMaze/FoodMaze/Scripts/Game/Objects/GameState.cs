using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodMaze.Scripts.Game.Objects
{
    public enum GameState
    {
        Ready,
        Playing,
        Paused,
        Finished
    }

    public enum GameSubstate
    {
        Normal,
        Rotating
    }
}
