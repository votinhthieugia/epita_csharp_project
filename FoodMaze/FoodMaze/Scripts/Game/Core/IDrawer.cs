using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace FoodMaze.Scripts.Game.Core
{
    interface IDrawer
    {
        void Register(IDrawable drawable);
        void Unregister(IDrawable drawable);
        object GetRenderer();
    }
}
