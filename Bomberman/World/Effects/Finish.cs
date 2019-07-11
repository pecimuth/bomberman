using Bomberman.World.Actors;
using Bomberman.World.Grids;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman.World.Effects
{
    class Finish : Effect
    {
        private static readonly Point pointOfOrigin = new Point(96, 256);
        public Finish(Texture2D texture, Sector location) :
            base(
                texture
                , false
                , location
                , 0
                , pointOfOrigin
            )
        { }

        protected override void OnCharactorCollision(Charactor charactor, World world)
        {
            if (charactor.Health.Value > charactor.Health.MinValue && world.LevelState == LevelState.InProgress)
            {
                world.LevelState = LevelState.Completed;
            }
        }

        protected override void OnMonsterCollision(Actor monster, World world)
        { }

        protected override void OnTimeRanOut(World world)
        { }
    }
}
