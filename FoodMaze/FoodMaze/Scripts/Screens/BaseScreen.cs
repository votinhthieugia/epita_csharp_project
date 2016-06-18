using FoodMaze.Scripts.Game.Core;
using FoodMaze.Scripts.Game.Objects;
using FoodMaze.Scripts.Game.Objects.Touches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodMaze.Scripts.Screens
{
    abstract class BaseScreen
    {
        public IDrawer drawer;
        public ScreenId Type { get; set; }
        abstract public void Show();
        abstract public void Hide();
        abstract public void Init();
        abstract public void Update(float elapsedSeconds);
        abstract public void UpdateTouch(Touch[] touches, int numTouches);
    }
}
