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

        public World(Texture2D texture, LevelLoader levelLoader, int levelNumber)
        {
            this.texture = texture;
            Charactor = new Charactor(texture);
            Grid = levelLoader.MakeGrid(levelNumber);
            Grid.Texture = texture;
            Monsters = levelLoader.MakeMonsters(levelNumber, texture);
            Effects = new List<Effect>();
            waitingEffects = new List<Effect>();
        }

        public void Update(KeyboardState keyboardState)
        {
            Charactor.Update(keyboardState, this);
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

        public void SpawnBomb(Sector location)
        {
            Bomb bomb = new Bomb(texture, location);
            waitingEffects.Add(bomb);
        }

        public void SpawnExplosion(Sector location, ExplosionOrientation orientation)
        {
            Explosion explosion = new Explosion(texture, location, orientation);
            waitingEffects.Add(explosion);
        }
    }
}
