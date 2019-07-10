using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman
{
    class World
    {
        public Grid Grid { get; private set; }
        public Charactor Charactor { get; private set; }
        public List<Actor> Monsters { get; private set; }
        public List<Effect> Effects { get; private set; }
        private List<Effect> waitingEffects;
        private readonly Texture2D texture;
        private readonly Random random;
        private static readonly int inversePickupSpawnProbability = 2;

        public World(Texture2D texture, LevelLoader levelLoader, int levelNumber)
        {
            this.texture = texture;
            Charactor = new Charactor(texture);
            Grid = levelLoader.MakeGrid(levelNumber);
            Grid.Texture = texture;
            Monsters = levelLoader.MakeMonsters(levelNumber, texture);
            Effects = new List<Effect>();
            waitingEffects = new List<Effect>();
            random = new Random();
        }

        public void Update(KeyboardState keyboardState)
        {
            Charactor.Update(keyboardState, this);

            Monsters.RemoveAll((Actor monster) => monster.Health.Value == 0);
            foreach (Actor monster in Monsters)
            {
                monster.Update(keyboardState, this);
            }

            Effects.RemoveAll((Effect effect) => effect.MarkedForRemoval);
            foreach (Effect effect in Effects)
            {
                effect.Update(this);
            }
            Effects.AddRange(waitingEffects);
            waitingEffects.Clear();
        }

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

        public void MaybeSpawnRandomPickup(Sector location)
        {
            if (Grid.IsFloor(location) && random.Next(inversePickupSpawnProbability) == 0)
            {
                SpawnRandomPickup(location);
            }
        }

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

        public void SpawnExplosion(Sector location, ExplosionOrientation orientation, bool brokeBrick)
        {
            Explosion explosion = new Explosion(texture, location, orientation, brokeBrick);
            waitingEffects.Add(explosion);
        }

        public List<Actor> MonstersInSector(Sector sector)
        {
            return Monsters
                .FindAll((Actor monster) => monster.Sprite.SectorLocationByCentralPoint == sector);
        }
    }
}
