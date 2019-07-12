using Bomberman.World.Grids;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman.Parser
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

        // číslo levelu
        public int Number { get; }
        public Grid Grid { get; }
        public ParsedMonsters Monsters { get; }
        // kde sa objaví hráč na začiatku
        public Sector StartSector { get; }
        // kde sa nachádza cieľ
        public Sector FinishSector { get; }
        public Texts Texts { get; }
    }
}
