using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodMaze.Scripts.Game.Core.PathFinding
{
    interface IFinder
    {
        void Init(PFTile[] tiles, int maxX);
        void Dispose();
        Path Find(PFTile start, PFTile  end);
    }
}
