using Bomberman.World.Actors.Controllers;
using Bomberman.World.Actors.Sprite;
using Bomberman.World.Grids;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman.World.Actors
{
    class Skeleton : Actor
    {
        // dosadí parametre Skeletona konštruktoru Actor
        // texture je atlas, startSector kde sa objaví
        public Skeleton(Texture2D texture, Sector startSector) : base(texture, Appearance.Skeleton, startSector, 50, new ChaseCharactorController())
        { }
    }
}
