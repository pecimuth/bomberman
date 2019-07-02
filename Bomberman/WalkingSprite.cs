using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman
{
    class WalkingSprite : AnimatedSprite
    {
        private Sector currentSector;
        private Sector destinationSector;
        private int ticksElapsed = 0;
        public int TicksPerSector { get; }
    
        public Vector2 Location
        {
            get
            {
                return currentSector.ToVector() + (destinationSector.ToVector() - currentSector.ToVector()) * ticksElapsed / TicksPerSector; 
            }
        }

        public WalkingSprite(Texture2D texture, Appearance appearance, Sector location, int ticksPerSector = 20) : base(texture, appearance)
        {
            TicksPerSector = ticksPerSector;
            currentSector = location;
            destinationSector = location;
        }

        public new void Update()
        {
            if (currentSector != destinationSector)
            {
                ++ticksElapsed;
                if (ticksElapsed == TicksPerSector)
                {
                    ticksElapsed = 0;
                    currentSector = destinationSector;

                    if (AtDestination())
                    {
                        Moving = false;
                    }
                }
            }
            base.Update();
        }

        public void Walk(Facing facing)
        {
            Moving = true;  
            if (AtDestination())
            {
                PlanImmediateDestination(facing);
            }
            else if (CloserToOrigin() && IsOppositeOrientation(Orientation, facing))
            {
                TurnBack(facing);
            }
            
        }

        public new void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            base.Draw(spriteBatch, Location + offset);
        }

        public bool AtDestination()
        {
            return currentSector == destinationSector;
        }

        private void PlanImmediateDestination(Facing facing)
        {
            Sector destination = currentSector.Neighbor(facing);
            destinationSector = destination;
            Orientation = facing;
        }

        private void TurnBack(Facing facing)
        {
            Sector tempDestination = destinationSector;
            destinationSector = currentSector;
            currentSector = tempDestination;
            ticksElapsed = TicksPerSector - ticksElapsed;
            Orientation = facing;
        }

        private bool CloserToOrigin()
        {
            return ticksElapsed / TicksPerSector < 0.5;
        }

        private static bool IsOppositeOrientation(Facing f1, Facing f2)
        {
            if (f1 == f2)
            {
                return false;
            }

            if ((int)f1 > (int)f2)
            {
                return IsOppositeOrientation(f2, f1);
            }

            return (f1 == Facing.West && f2 == Facing.East) || (f1 == Facing.South && f2 == Facing.North);
        }
    }
}
