using FoodMaze.Scripts.Game.Core;
using FoodMaze.Scripts.Game.Objects.Touches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace FoodMaze.Scripts.Game.Objects
{
    class Context
    {
        private static Context instance;
        public static Context Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Context();
                }

                return instance;
            }
        }

        private Context() {}

        public Panel Renderer { get; set; }
        public int TileWidth { get; set; }
        public int TileHeight { get; set; }
        public int ScreenWidth { get; set; }
        public int ScreenHeight { get; set; }
        public IDrawer Drawer { get; set; }

        public void Init(Panel renderer, int tileWidth, int tileHeight, int screenWidth, int screenHeight)
        {
            Renderer = renderer;
            TileWidth = tileWidth;
            TileHeight = tileHeight;
            ScreenWidth = screenWidth;
            ScreenHeight = screenHeight;
            Drawer = new Drawer(renderer, screenWidth, screenHeight);
            TouchManager.Instance.Init(renderer);
        }
    }
}
