using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman
{
    struct ParsedLevel
    {
        public ParsedLevel(int number, Grid grid, ParsedMonsters monsters, Sector startSector, Sector finishSector, Texts texts)
        {
            Number = number;
            Grid = grid;
            Monsters = monsters;
            StartSector = startSector;
            FinishSector = finishSector;
            Texts = texts;
        }

        public int Number { get; }
        public Grid Grid { get; }
        public ParsedMonsters Monsters { get; }
        public Sector StartSector { get; }
        public Sector FinishSector { get; }
        public Texts Texts { get; }
    }
}
