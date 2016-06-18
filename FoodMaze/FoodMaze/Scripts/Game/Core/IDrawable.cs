using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodMaze.Scripts.Game.Core
{
    interface IDrawable
    {
        void RegisterTo(IDrawer drawer);
        void Unregister(IDrawer drawer);
        ICollection<object> GetObjects();
    }
}
