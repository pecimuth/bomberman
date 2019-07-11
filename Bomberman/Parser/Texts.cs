using Bomberman.World.Grids;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman.Parser
{
    struct Text
    {
        public Text(string content, Sector location)
        {
            Content = content;
            Location = location;
        }

        public string Content { get; }
        public Sector Location { get; }
    }

    class Texts
    {
        public static readonly string DataRowRegex = @"^TEXT -?[0-9]{1,9} -?[0-9]{1,9} .*$";
        private readonly List<Text> texts;

        public Texts()
        {
            texts = new List<Text>();
        }

        public void Read(ConfigReader configReader)
        {
            while (configReader.NextSplit(DataRowRegex, out string[] lineSplit))
            {
                Sector location = new Sector(int.Parse(lineSplit[1]), int.Parse(lineSplit[2]));
                string content = string.Join(" ", lineSplit.Skip(3));
                Add(content, location);
            }
        }

        private void Add(string content, Sector location)
        {
            Text text = new Text(content, location);
            texts.Add(text);
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font, Vector2 offset)
        {
            spriteBatch.Begin();
            foreach (Text text in texts)
            {
                Vector2 destination = offset + text.Location.ToVector();
                spriteBatch.DrawString(font, text.Content, destination, Color.White);
            }
            spriteBatch.End();
        }
    }
}
