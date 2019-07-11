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
                                , line + 1
                                , expected
                                , found
                              )
               )
        { }
    }

    class ConfigReader
    {
        private string[] lines;
        private int lineNo;

        public ConfigReader()
        {
            lines = new string[0];
            lineNo = 0;
        }

        public void ReadTextFile(string filename)
        {
            lines = System.IO.File.ReadAllLines(filename);
            lineNo = 0;
        }

        public bool Next(string pattern, out string line)
        {
            if (!HasNext())
            {
                line = string.Empty;
                return false;
            }

            Regex rgx = new Regex(pattern);
            if (rgx.IsMatch(lines[lineNo]))
            {
                line = lines[lineNo];
                ++lineNo;
                return true;
            }

            line = string.Empty;
            return false;
        }

        public bool HasNext()
        {
            return lineNo < lines.Count();
        }

        public bool NextSplit(string pattern, out string[] lineSplit)
        {
            bool result = Next(pattern, out string line);
            lineSplit = (result) ? lineSplit = line.Split(null) : new string[0];
            return result;
        }

        public void AssertNextSplit(string pattern, out string[] lineSplit)
        {
            bool result = NextSplit(pattern, out lineSplit);
            if (!result)
            {
                throw new LevelParserException(lineNo, pattern, lines[lineNo]);
            }
        }

        public void AssertNext(string pattern, out string line)
        {
            bool result = Next(pattern, out line);
            if (!result)
            {
                throw new LevelParserException(lineNo, pattern, lines[lineNo]);
            }
        }
    }
}
