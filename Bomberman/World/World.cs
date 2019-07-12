using Bomberman.Parser;
using Bomberman.World.Actors;
using Bomberman.World.Effects;
using Bomberman.World.Grids;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Effect = Bomberman.World.Effects.Effect;

namespace Bomberman.World
{
    enum LevelState
    {
        InProgress,
        Completed,
        Failed
    }

    class World
    {
        public Audio Audio { get; private set; }
        public Grid Grid { get; private set; }
        public Charactor Charactor { get; private set; }
        public List<Actor> Monsters { get; private set; }
        public List<Effect> Effects { get; private set; }
        public LevelState LevelState;
        public readonly int LevelNumber;
        // efekty ktoré čakajú na pridanie medzi efekty
        private readonly List<Effect> waitingEffects;
        private readonly Texture2D texture;
        private readonly Random random;
        // inverzná pravdepodobnosť, že po rozbití tehly sa spawne pickup
        private static readonly int inversePickupSpawnProbability = 5;

        // vytvorenie nového sveta pomocou levelLoadera v danom leveli levelNumber
        public World(Audio audio, Texture2D texture, LevelLoader levelLoader, int levelNumber)
        {
            Audio = audio;
            this.texture = texture;
            Charactor = levelLoader.MakeCharactor(levelNumber, texture);
            Grid = levelLoader.MakeGrid(levelNumber);
            Grid.Texture = texture;
            Monsters = levelLoader.MakeMonsters(levelNumber, texture);
            Effects = new List<Effect>();
            Effects.Add(levelLoader.MakeFinish(levelNumber, texture));
            waitingEffects = new List<Effect>();
            random = new Random();
            LevelState = LevelState.InProgress;
            LevelNumber = levelNumber;
            Audio.Play(Sound.Start);
        }

        // volanie Update metód všetkých efektov, príšer, charaktora
        // zmazanie príšer, efektov
        // riešenie kolízie príšera/Charactor
        public void Update(KeyboardState keyboardState)
        {
            Charactor.Update(keyboardState, this);

            Monsters.RemoveAll((monster) => monster.Health.Value == 0);
            foreach (Actor monster in Monsters)
            {
                monster.Update(keyboardState, this);
            }

            MonstersInSector(Charactor.Sprite.SectorLocationByCentralPoint)
                .ForEach((monster) =>
                {
                    monster.Damage(this);
                    Charactor.Damage(this);
                });

            Effects.RemoveAll((effect) => effect.MarkedForRemoval);
            foreach (Effect effect in Effects)
            {
                effect.Update(this);
            }
            Effects.AddRange(waitingEffects);
            waitingEffects.Clear();
        }

        // vykreslenie celého sveta s posunom
        public void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            Grid.Draw(spriteBatch, offset);
            foreach (Effect effect in Effects)
            {
                effect.Draw(spriteBatch, offset);
            }
            Charactor.Draw(spriteBatch, offset);
            foreach (Actor monster in Monsters)
            {
                monster.Draw(spriteBatch, offset);
            }
        }

        // ak je location podľaha, s nejakou pravdepodobnosťou sa tam spawne náhodný pickup
        public void MaybeSpawnRandomPickup(Sector location)
        {
            if (Grid.IsFloor(location) && random.Next(inversePickupSpawnProbability) == 0)
            {
                SpawnRandomPickup(location);
            }
        }

        // na location sa spawne náhodný pickup
        private void SpawnRandomPickup(Sector location)
        {
            Array types = Enum.GetValues(typeof(PickupType));
            PickupType pickupType = (PickupType)types.GetValue(random.Next(types.Length));
            Pickup pickup = new Pickup(texture, location, pickupType);
            waitingEffects.Add(pickup);
        }

        public void SpawnBomb(Sector location)
        {
            Bomb bomb = new Bomb(texture, location, Charactor.BombRadius.Value);
            waitingEffects.Add(bomb);
        }

        // na location sa spawne explózia s danou orientáciou, brokeBrick hovorí, či rozbila tehlu
        public void SpawnExplosion(Sector location, ExplosionOrientation orientation, bool brokeBrick)
        {
            Explosion explosion = new Explosion(texture, location, orientation, brokeBrick);
            waitingEffects.Add(explosion);
        }

        // vráti zoznam príšer, ktorých stred je v danom sektore
        public List<Actor> MonstersInSector(Sector sector)
        {
            return Monsters
                .FindAll((monster) => monster.Sprite.SectorLocationByCentralPoint == sector);
        }
    }
}
