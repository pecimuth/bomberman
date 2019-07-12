using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bomberman.Parser
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
        // pole riadkov konfiguračnéhó súboru
        private string[] lines;
        // index spracovaného riadku v poli lines
        private int lineNo;

        public ConfigReader()
        {
            lines = new string[0];
            lineNo = 0;
        }

        // prečíta súbor, výstup si uloží
        public void ReadTextFile(string filename)
        {
            lines = System.IO.File.ReadAllLines(filename);
            lineNo = 0;
        }

        // ak ďalší riadok vyhovuje patternu, vráti riadok do line a true, inak false
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

        // má riadok, ktorý ešte nevrátil?
        public bool HasNext()
        {
            return lineNo < lines.Count();
        }

        // ako Next, akurát riadok hneď rozdelí poďla bielych znakov
        public bool NextSplit(string pattern, out string[] lineSplit)
        {
            bool result = Next(pattern, out string line);
            lineSplit = result ? lineSplit = line.Split(null) : new string[0];
            return result;
        }

        // ako NextSplit, ale keď sa riadok nezhoduje s patternom, vráti LevelParserException
        public void AssertNextSplit(string pattern, out string[] lineSplit)
        {
            bool result = NextSplit(pattern, out lineSplit);
            if (!result)
            {
                throw new LevelParserException(lineNo, pattern, lines[lineNo]);
            }
        }

        // ako Next, ale keď sa riadok nezhoduje s patternom, vráti LevelParserException
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
