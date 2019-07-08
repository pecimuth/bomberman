using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bomberman
{
    class LevelParserException : Exception
    {
        public LevelParserException(int line, string expected, string found) :
            base(
                 string.Format(
                                 "Parser error on line {0:d}\nExpected: {1}\nFound: {2}\n"
                                ,line
                                ,expected
                                ,found
                              )
               )
        { }
    }
    
    enum MonsterType
    {
        Skeleton,
        Spider,
        Slime,
        Knight,
        Bat,
        Ghost
    }

    struct ParsedMonster
    {
        public ParsedMonster(MonsterType type, Sector startSector)
        {
            Type = type;
            StartSector = startSector;
        }
 
        public MonsterType Type { get; }
        public Sector StartSector { get; }
    }

    struct ParsedLevel
    {
        public ParsedLevel(int number, int gridWidth, int gridHeight, List<Block> blocks, List<ParsedMonster> monsters)
        {
            Number = number;
            GridWidth = gridWidth;
            GridHeight = gridHeight;
            Blocks = blocks;
            Monsters = monsters;
        }

        public int Number { get; }
        public int GridWidth { get; }
        public int GridHeight { get;  }
        public List<Block> Blocks { get; }
        public List<ParsedMonster> Monsters { get; }
    }

    class LevelLoader
    {
        private readonly Dictionary<int, ParsedLevel> levels;
 
        private LevelLoader(Dictionary<int, ParsedLevel> levels)
        {
            this.levels = levels;
        }

        public static LevelLoader FromTextFile(string filename)
        {
            Dictionary<int, ParsedLevel> levels = new Dictionary<int, ParsedLevel>();


            string[] lines = System.IO.File.ReadAllLines(filename);
            
            for (int lineNo = 0; lineNo < lines.Count(); ++lineNo)
            {
                ParsedLevel parsedLevel = ParseOneLevel(lines, ref lineNo);
                levels[parsedLevel.Number] = parsedLevel;
            }

            return new LevelLoader(levels);
        }

        private static ParsedLevel ParseOneLevel(string[] lines, ref int lineNo)
        {
            LineShouldExistAndMatch(lines, lineNo, @"^LEVEL [0-9]{1,9}$");
            string[] lineSplit = lines[lineNo].Split(null);
            int number = int.Parse(lineSplit[1]);

            LineShouldExistAndMatch(lines, ++lineNo, @"^GRID [0-9]{1,9} [0-9]{1,9}$");
            lineSplit = lines[lineNo].Split(null);
            int width = int.Parse(lineSplit[1]);
            int height = int.Parse(lineSplit[2]);

            string gridLinePattern = @"^[0-2]{" + width.ToString() + "}$";
            List<Block> blocks = new List<Block>();
            for (int lineIterator = 0; lineIterator < height; ++lineIterator)
            {
                LineShouldExistAndMatch(lines, ++lineNo, gridLinePattern);
                foreach (char blockCharacter in lines[lineNo])
                {
                    int blockNumber = blockCharacter - '0';
                    blocks.Add((Block)blockNumber);
                }
            }

            LineShouldExistAndMatch(lines, ++lineNo, @"^MONSTERS [0-9]{1,9}$");
            lineSplit = lines[lineNo].Split(null);
            int monsterCount = int.Parse(lineSplit[1]);
            List<ParsedMonster> monsters = new List<ParsedMonster>();
            for (int monsterIterator = 0; monsterIterator < monsterCount; ++monsterIterator)
            {
                LineShouldExistAndMatch(lines, ++lineNo, @"[SKELETON|SPIDER|SLIME|KNIGHT|BAT|GHOST] [0-9]{1,9} [0-9]{1,9}$");
                lineSplit = lines[lineNo].Split(null);
                MonsterType type;
                switch (lineSplit[0])
                {
                    case "SKELETON":
                        type = MonsterType.Skeleton;
                        break;
                    case "SPIDER":
                        type = MonsterType.Spider;
                        break;
                    case "SLIME":
                        type = MonsterType.Slime;
                        break;
                    case "KNIGHT":
                        type = MonsterType.Knight;
                        break;
                    case "BAT":
                        type = MonsterType.Bat;
                        break;
                    case "GHOST":
                        type = MonsterType.Ghost;
                        break;
                    default: // impossible
                        type = MonsterType.Skeleton;
                        break;
                }
                Sector sector = new Sector(int.Parse(lineSplit[1]), int.Parse(lineSplit[2]));
                ParsedMonster parsedMonster = new ParsedMonster(type, sector);
                monsters.Add(parsedMonster);
            }

            return new ParsedLevel(number, width, height, blocks, monsters);
        }

        private static void LineShouldExistAndMatch(string[] lines, int lineArrayIndex, string pattern)
        {
            if (lines.Count() - lineArrayIndex <= 0)
            {
                throw new LevelParserException(lineArrayIndex + 1, pattern, "End of file.");
            }

            Regex rgx = new Regex(pattern);
            if (!rgx.IsMatch(lines[lineArrayIndex]))
            {
                throw new LevelParserException(lineArrayIndex + 1, pattern, lines[lineArrayIndex]);
            }
        }

        public Grid MakeGrid(int levelNumber)
        {
            if (!levels.ContainsKey(levelNumber))
            {
                throw new ArgumentException(string.Format("Tried to create grid for level {0:d}, but it was not parsed.", levelNumber));
            }

            ParsedLevel level = levels[levelNumber];
            Grid grid = new Grid(level.GridWidth, level.GridHeight, level.Blocks);
            return grid;
        }

        public List<Actor> MakeMonsters(int levelNumber, Texture2D texture)
        {
            if (!levels.ContainsKey(levelNumber))
            {
                throw new ArgumentException(string.Format("Tried to create monsters for level {0:d}, but it was not parsed.", levelNumber));
            }

            ParsedLevel level = levels[levelNumber];
            List<Actor> monsterActors = new List<Actor>();

            foreach (ParsedMonster monster in level.Monsters)
            {
                Actor monsterActor = null;

                switch(monster.Type)
                {
                    case MonsterType.Skeleton:
                        monsterActor = new Skeleton(texture, monster.StartSector);
                        break;
                    case MonsterType.Spider:
                        monsterActor = new Spider(texture, monster.StartSector);
                        break;
                    default:
                        monsterActor = new Spider(texture, monster.StartSector);
                        break;
                }
     
                if (monsterActor != null)
                {
                    monsterActors.Add(monsterActor);
                }
            }

            return monsterActors;
        }
    }
}
