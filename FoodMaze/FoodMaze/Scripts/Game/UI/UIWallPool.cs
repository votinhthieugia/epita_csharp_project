using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodMaze.Scripts.Game.UI
{
    class UIWallPool
    {
        private static UIWallPool instance;

        public static UIWallPool Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UIWallPool();
                }

                return instance;
            }
            private set { }
        }

        private List<UIWall> activeWalls;
        private Stack<UIWall> inactiveWalls;

        private UIWallPool()
        {
            activeWalls = new List<UIWall>();
            inactiveWalls = new Stack<UIWall>();
        }

        public UIWall Pop()
        {
            UIWall uiWall = null;

            if (inactiveWalls.Count == 0)
            {
                uiWall = new UIWall();
            }
            else
            {
                uiWall = inactiveWalls.Pop();
            }

            activeWalls.Add(uiWall);
            return uiWall;
        }

        public void Push(UIWall uiWall)
        {
            activeWalls.Remove(uiWall);
            inactiveWalls.Push(uiWall);
        }
    }
}
