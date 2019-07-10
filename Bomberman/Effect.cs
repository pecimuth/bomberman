﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman
{
    class Effect
    {
        public readonly static Vector2 Size = new Vector2(32, 32);
        public bool MarkedForRemoval { get; private set; }
        public Sector Location { get; private set; }
        public bool RestrictActorMovement { get; private set; }
        protected Texture2D Texture { get; private set; }
        public int TicksLeft { get; private set; }
        private readonly Point pointOfOrigin;

        public delegate void ActorCollisionCallback(Actor actor, Effect effect, World world);
        public ActorCollisionCallback OnActorCollision;
 
        public delegate void BeforeRemovalCallback(Effect effect, World world);
        public BeforeRemovalCallback OnBeforeRemoval;

        public Effect(Texture2D texture, bool restrictActorMovement, Sector location, int ticksLeft, Point pointOfOrigin)
        {
            Texture = texture;
            MarkedForRemoval = false;
            Location = location;
            RestrictActorMovement = restrictActorMovement;
            TicksLeft = ticksLeft;
            this.pointOfOrigin = pointOfOrigin;
        }

        public void Remove()
        {
            MarkedForRemoval = true;
        }

        public virtual void Update(World world)
        {
            --TicksLeft;
            if (TicksLeft < 0)
            {
                OnBeforeRemoval?.Invoke(this, world);
                Remove();
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            Rectangle source = new Rectangle(pointOfOrigin, Size.ToPoint());
            Rectangle destination = MakeDestinationRectangle(offset);
            spriteBatch.Begin();
            spriteBatch.Draw(Texture, destination, source, Color.White);
            spriteBatch.End();
        }

        private Rectangle MakeDestinationRectangle(Vector2 offset)
        {
            Vector2 destination = Location.ToVector() + offset;
            return new Rectangle(destination.ToPoint(), Size.ToPoint());
        }
    }
}