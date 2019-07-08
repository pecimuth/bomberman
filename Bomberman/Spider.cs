using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman
{
    class Spider : Actor
    {
        public Spider(Texture2D texture, Sector startSector) : base(texture, Appearance.Spider, startSector, 70, new WalkByWallsController())
        { }
    }
}
