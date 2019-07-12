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
    class Spider : Actor
    {
        // dosadí parametre Spidera konštruktoru Actor
        // texture je atlas, startSector kde sa objaví
        public Spider(Texture2D texture, Sector startSector) : base(texture, Appearance.Spider, startSector, 70, new WalkByWallsController())
        { }
    }
}
