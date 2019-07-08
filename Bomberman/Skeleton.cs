using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman
{
    class Skeleton : Actor
    {
        public Skeleton(Texture2D texture, Sector startSector) : base(texture, Appearance.Skeleton, startSector, 50, new ChaseCharactorController())
        { }
    }
}
