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
        private int ticksElapsed = 0;
        public int TicksPerSector { get; }

        public Sector DestinationSector { get; private set; }
        public Sector SectorLocation { get; private set; }

        public Vector2 Location
        {
            get
            {
                return SectorLocation.ToVector() + (DestinationSector.ToVector() - SectorLocation.ToVector()) * ticksElapsed / TicksPerSector; 
            }
        }

        public Sector SectorLocationByCentralPoint
        {
            get
            {
                Vector2 centralPoint = Location + Size / 2;
                return Sector.FromVector(centralPoint);
            }
        }

        public WalkingSprite(Texture2D texture, Appearance appearance, Sector location, int ticksPerSector) : base(texture, appearance)
        {
            TicksPerSector = ticksPerSector;
            SectorLocation = location;
            DestinationSector = location;
        }

        public new void Update()
        {
            if (SectorLocation != DestinationSector)
            {
                ++ticksElapsed;
                if (ticksElapsed == TicksPerSector)
                {
                    ticksElapsed = 0;
                    SectorLocation = DestinationSector;

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
            return SectorLocation == DestinationSector;
        }

        private void PlanImmediateDestination(Facing facing)
        {
            Sector destination = SectorLocation.Neighbor(facing);
            DestinationSector = destination;
            Orientation = facing;
        }

        private void TurnBack(Facing facing)
        {
            Sector tempDestination = DestinationSector;
            DestinationSector = SectorLocation;
            SectorLocation = tempDestination;
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
