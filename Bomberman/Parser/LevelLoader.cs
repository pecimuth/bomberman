using Bomberman.World.Actors;
using Bomberman.World.Effects;
using Bomberman.World.Grids;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bomberman.Parser
{
    class LevelLoader
    {
        // udržuje načítane levely, s ktorých sa dajú vytvárať potrebné inštancie objektov
        private readonly Dictionary<int, ParsedLevel> levels;

        private LevelLoader(Dictionary<int, ParsedLevel> levels)
        {
            this.levels = levels;
        }

        // filename je cesta k súboru konfiguráciou
        // z tohto súboru sa načítajú levely - vytvorí inštanciu LevelLoader
        public static LevelLoader FromTextFile(string filename)
        {
            Dictionary<int, ParsedLevel> levels = new Dictionary<int, ParsedLevel>();

            ConfigReader configReader = new ConfigReader();
            configReader.ReadTextFile(filename);

            for (int levelNumber = 1; configReader.HasNext(); ++levelNumber)
            {
                ParsedLevel parsedLevel = ParseOneLevel(levelNumber, configReader);
                levels[parsedLevel.Number] = parsedLevel;
            }

            return new LevelLoader(levels);
        }

        // prečítanie jedneho levelu z konfiguračného súboru
        // číslo levelu músi byť rovné levelNumber
        private static ParsedLevel ParseOneLevel(int levelNumber, ConfigReader configReader)
        {
            configReader.AssertNext(@"^LEVEL " + levelNumber.ToString() + "$", out string line);

            configReader.AssertNextSplit(@"^GRID [0-9]{1,9} [0-9]{1,9}$", out string[] lineSplit);
            int width = int.Parse(lineSplit[1]);
            int height = int.Parse(lineSplit[2]);

            string gridLinePattern = @"^[012 ]{0," + width.ToString() + "}$";
            List<Block> blocks = new List<Block>();
            for (int lineIterator = 0; lineIterator < height; ++lineIterator)
            {
                configReader.AssertNext(gridLinePattern, out line);
                for (int lineIter = 0; lineIter < width; ++lineIter)
                {
                    if (lineIter < line.Count() && line[lineIter] != ' ')
                    {
                        char blockCharacter = line[lineIter];
                        int blockNumber = blockCharacter - '0';
                        blocks.Add((Block)blockNumber);
                    }
                    else
                    {
                        blocks.Add(Block.None);
                    }
                }
            }
            Grid grid = new Grid(width, height, blocks);

            configReader.AssertNextSplit(@"^START [0-9]{1,9} [0-9]{1,9}$", out lineSplit);
            Sector startSector = new Sector(int.Parse(lineSplit[1]), int.Parse(lineSplit[2]));

            configReader.AssertNextSplit(@"^FINISH [0-9]{1,9} [0-9]{1,9}$", out lineSplit);
            Sector finishSector = new Sector(int.Parse(lineSplit[1]), int.Parse(lineSplit[2]));

            ParsedMonsters monsters = new ParsedMonsters();
            monsters.Read(configReader);

            Texts texts = new Texts();
            texts.Read(configReader);

            return new ParsedLevel(levelNumber, grid, monsters, startSector, finishSector, texts);
        }

        // ďalšie metódy slúžia na vytvorenie konkrétnych inštancií objektov do hry podľa konfigurácie

        public Grid MakeGrid(int levelNumber)
        {
            ParsedLevel level = GetParsedLevel(levelNumber);
            return new Grid(level.Grid.Width, level.Grid.Height, level.Grid.Blocks);
        }

        public List<Actor> MakeMonsters(int levelNumber, Texture2D texture)
        {
            ParsedLevel level = GetParsedLevel(levelNumber);
            return level.Monsters.MakeMonsters(texture);
        }

        public Finish MakeFinish(int levelNumber, Texture2D texture)
        {
            ParsedLevel level = GetParsedLevel(levelNumber);
            Finish finish = new Finish(texture, level.FinishSector);
            return finish;
        }

        public Charactor MakeCharactor(int levelNumber, Texture2D texture)
        {
            ParsedLevel level = GetParsedLevel(levelNumber);
            Charactor finish = new Charactor(texture, level.StartSector);
            return finish;
        }

        public Texts GetTexts(int levelNumber)
        {
            ParsedLevel level = GetParsedLevel(levelNumber);
            return level.Texts;
        }

        public int LevelCount()
        {
            return levels.Count;
        }

        private ParsedLevel GetParsedLevel(int levelNumber)
        {
            if (!levels.ContainsKey(levelNumber))
            {
                throw new ArgumentException(string.Format("Tried to load level {0:d}, but it was not parsed.", levelNumber));
            }

            return levels[levelNumber];
        }
    }
}
