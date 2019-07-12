using Bomberman.World.Grids;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman.World.Actors.Sprite
{
    class WalkingSprite : AnimatedSprite
    {
        // koľko Update bolo volaných pri tomto pohybe
        private int ticksElapsed = 0;
        // rýchlosť pohybu - koľko Update trvá, kým prejde jeden sektor
        public Stat MovementSpeed { get; private set; }
        // do ktorého sektoru sa práve hýbem - keď je zhodný so SectorLocation, tak sa nehýbem
        public Sector DestinationSector { get; private set; }
        // v ktorom sektore sa nachádzam (z ktorého vychádzam)
        public Sector SectorLocation { get; private set; }

        // presné súradnice, kde sa nachádzam
        public Vector2 Location
        {
            get
            {
                return SectorLocation.ToVector() + (DestinationSector.ToVector() - SectorLocation.ToVector()) * ticksElapsed / MovementSpeed.Value;
            }
        }

        // v ktorom sektore sa nachádza stred
        public Sector SectorLocationByCentralPoint
        {
            get
            {
                Vector2 centralPoint = Location + Size / 2;
                return Sector.FromVector(centralPoint);
            }
        }

        // ticksPerSector je počet Update() kým prejde jeden sektor (= MovementSpeed)
        public WalkingSprite(Texture2D texture, Appearance appearance, Sector location, int ticksPerSector) : base(texture, appearance)
        {
            MovementSpeed = new Stat(ticksPerSector, 3, 14, 100);
            SectorLocation = location;
            DestinationSector = location;
        }

        public new void Update()
        {
            if (SectorLocation != DestinationSector)
            {
                ++ticksElapsed;
                if (ticksElapsed >= MovementSpeed.Value)
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

        // kráčaj v danom smere ak nie je v pohybe, prípadne sa otoč
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

        public new void Draw(SpriteBatch spriteBatch, Vector2 offset, bool semiTransparent)
        {
            base.Draw(spriteBatch, Location + offset, semiTransparent);
        }

        // sme v destinácii?
        public bool AtDestination()
        {
            return SectorLocation == DestinationSector;
        }

        // zmenenie sektoru destinácie podľa orientácie zo súčasného sektoru
        private void PlanImmediateDestination(Facing facing)
        {
            Sector destination = SectorLocation.Neighbor(facing);
            DestinationSector = destination;
            Orientation = facing;
        }

        // vymeň súčasný sektor a destináciu a hýb sa späť - otočenie o 180 stupňov
        // len keď sme bližsie k zdrojovému sektoru (z ktorého sa pohybujeme)
        private void TurnBack(Facing facing)
        {
            Sector tempDestination = DestinationSector;
            DestinationSector = SectorLocation;
            SectorLocation = tempDestination;
            ticksElapsed = MovementSpeed.Value - ticksElapsed;
            Orientation = facing;
        }

        // sme bližsie k súčasnému sektoru ako k destinácii?
        private bool CloserToOrigin()
        {
            return ticksElapsed / MovementSpeed.Value < 0.5;
        }

        // sú f1, f2 opačné orientácie?
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

            return f1 == Facing.West && f2 == Facing.East || f1 == Facing.South && f2 == Facing.North;
        }
    }
}
