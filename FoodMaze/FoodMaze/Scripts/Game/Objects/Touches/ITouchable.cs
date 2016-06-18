using FoodMaze.Scripts.Game.Objects.Touches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodMaze.Scripts.Game.Objects.Touches
{
    public delegate void OnTouchableDown(object sender, Touch touch);
    public delegate void OnTouchableUp(object sender, Touch touch);
    public delegate void OnTouchableCanceled(object sender, Touch touch);
    public delegate void OnTouchableHold(object sender, Touch touch);

    interface ITouchable
    {
        void UpdateTouch(Touch[] touches, int numTouches);
    }
}
