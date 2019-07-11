using Bomberman.World.Actors;
using Bomberman.World.Grids;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman.Parser
{
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

    class ParsedMonsters
    {
        public static readonly string DataRowRegex = @"[SKELETON|SPIDER|SLIME|KNIGHT|BAT|GHOST] [0-9]{1,9} [0-9]{1,9}$";
        private readonly List<ParsedMonster> monsters;

        public ParsedMonsters()
        {
            monsters = new List<ParsedMonster>();
        }

        public void Read(ConfigReader configReader)
        {
            while (configReader.NextSplit(DataRowRegex, out string[] lineSplit))
            {
                string type = lineSplit[0];
                Sector sector = new Sector(int.Parse(lineSplit[1]), int.Parse(lineSplit[2]));
                Add(type, sector);
            }
        }

        private void Add(string type, Sector sector)
        {
            MonsterType monsterType;
            switch (type)
            {
                case "SKELETON":
                    monsterType = MonsterType.Skeleton;
                    break;
                case "SPIDER":
                    monsterType = MonsterType.Spider;
                    break;
                case "SLIME":
                    monsterType = MonsterType.Slime;
                    break;
                case "KNIGHT":
                    monsterType = MonsterType.Knight;
                    break;
                case "BAT":
                    monsterType = MonsterType.Bat;
                    break;
                case "GHOST":
                    monsterType = MonsterType.Ghost;
                    break;
                default: // impossible
                    monsterType = MonsterType.Skeleton;
                    break;
            }

            ParsedMonster parsedMonster = new ParsedMonster(monsterType, sector);
            monsters.Add(parsedMonster);
        }

        public List<Actor> MakeMonsters(Texture2D texture)
        {
            List<Actor> monsterActors = new List<Actor>();

            foreach (ParsedMonster monster in monsters)
            {
                Actor monsterActor = null;

                switch (monster.Type)
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
