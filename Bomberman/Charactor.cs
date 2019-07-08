using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman
{
    class Charactor : Actor
    {
        public Charactor(Texture2D texture) : base(texture, Appearance.Heroine, Sector.One, 20, new PlayerController())
        { }
    }
}
