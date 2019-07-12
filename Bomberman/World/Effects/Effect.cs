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
    abstract class Effect
    {
        public readonly static Vector2 Size = new Vector2(32, 32);

        // je označený na zmazanie?
        public bool MarkedForRemoval { get; private set; }
        public Sector Location { get; private set; }
        // keď je true, tak pre Actora je políčko Location zaplnené - nemôže cez neho prejsť
        public bool RestrictActorMovement { get; private set; }
        // atlas
        protected Texture2D Texture { get; private set; }
        // koľko Update() zostáva kým sa zavolá OnTimeRanOut
        public int TicksLeft { get; private set; }
        private readonly Point pointOfOrigin;

        // pointOfOrigin je ľavý horný roh daného efektu v texture
        public Effect(Texture2D texture, bool restrictActorMovement, Sector location, int ticksLeft, Point pointOfOrigin)
        {
            Texture = texture;
            MarkedForRemoval = false;
            Location = location;
            RestrictActorMovement = restrictActorMovement;
            TicksLeft = ticksLeft;
            this.pointOfOrigin = pointOfOrigin;
        }

        // aktualizácia času, testovanie kolízii
        public virtual void Update(World world)
        {
            --TicksLeft;
            if (TicksLeft < 0)
            {
                OnTimeRanOut(world);
            }

            if (world.Charactor.Sprite.SectorLocationByCentralPoint == Location)
            {
                OnCharactorCollision(world.Charactor, world);
            }

            world
                .MonstersInSector(Location)
                .ForEach((monster) => OnMonsterCollision(monster, world));
        }

        // označenie na zmazanie
        public void Remove()
        {
            MarkedForRemoval = true;
        }

        // vypršal čas
        protected abstract void OnTimeRanOut(World world);

        // kolízia s Charactobom
        protected abstract void OnCharactorCollision(Charactor charactor, World world);

        // kolízia s nejakou príšerou
        protected abstract void OnMonsterCollision(Actor monster, World world);

        // vykreslenie s posunutím o offset
        public virtual void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            Rectangle source = new Rectangle(pointOfOrigin, Size.ToPoint());
            Rectangle destination = MakeDestinationRectangle(offset);
            spriteBatch.Begin();
            spriteBatch.Draw(Texture, destination, source, Color.White);
            spriteBatch.End();
        }

        // výpoćet obdĺžniku kam sa vykreslí, podľa posunutia
        private Rectangle MakeDestinationRectangle(Vector2 offset)
        {
            Vector2 destination = Location.ToVector() + offset;
            return new Rectangle(destination.ToPoint(), Size.ToPoint());
        }
    }
}
