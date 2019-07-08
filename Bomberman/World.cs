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

        public World(Texture2D texture, LevelLoader levelLoader, int levelNumber)
        {
            Charactor = new Charactor(texture);
            Grid = levelLoader.MakeGrid(levelNumber);
            Grid.Texture = texture;
            Monsters = levelLoader.MakeMonsters(levelNumber, texture);
        }

        public void Update(KeyboardState keyboardState)
        {
            Charactor.Update(keyboardState, this);
            foreach (Actor monster in Monsters)
            {
                monster.Update(keyboardState, this);
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            Grid.Draw(spriteBatch, offset);
            Charactor.Draw(spriteBatch, offset);
            foreach (Actor monster in Monsters)
            {
                monster.Draw(spriteBatch, offset);
            }
        }
    }
}
