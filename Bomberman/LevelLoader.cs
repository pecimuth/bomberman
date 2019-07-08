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
    
    struct ParsedLevel
    {
        public ParsedLevel(int number, int gridWidth, int gridHeight, List<Block> blocks)
        {
            Number = number;
            GridWidth = gridWidth;
            GridHeight = gridHeight;
            Blocks = blocks;
        }

        public int Number { get; }
        public int GridWidth { get; }
        public int GridHeight { get;  }
        public List<Block> Blocks { get; }
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

            return new ParsedLevel(number, width, height, blocks);
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
    }
}
