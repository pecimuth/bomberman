using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman
{
    enum Appearance
    {
        Knight = 0,
        Hero = 1,
        Heroine = 2,
        Skeleton = 3,
        Slime = 4,
        Bat = 5,
        Ghost = 6,
        Spider = 7
    }

    class AnimatedSprite
    {
        public Facing Orientation { get; set; } = Facing.South;
        public Texture2D Texture { get; }
        public Appearance Appearance { get; }
        public readonly static Vector2 Size = new Vector2(32, 32);

        private bool previouslyMoving;
        public bool Moving { get; set; }
 
        private readonly int ticksPerAnimationFrame = 8;
        private int tickCounter;

        private readonly int totalAnimationFrames = 3;
        private readonly int standstillFrame = 1;
        private int animationFrame;

        private Vector2 OneSpriteAtlasSize
        {
            get
            {
                return new Vector2(totalAnimationFrames, 4) * Size;
            }
        }
    
        private Vector2 PointOfOrigin => new Vector2((int)Appearance % 4, (int)Appearance / 4) * OneSpriteAtlasSize;

        public AnimatedSprite(Texture2D texture, Appearance appearance)
        {
            Texture = texture;
            Appearance = appearance;
            ResetAnimation();
        }
 
        private void ResetAnimation()
        {
            tickCounter = 0;
            animationFrame = standstillFrame;
        }

        private Rectangle MakeSourceRectangle()
        {
            Vector2 alpha = new Vector2(animationFrame, (float)Orientation);
            Vector2 frameLocation = PointOfOrigin + alpha * Size;
            return new Rectangle(frameLocation.ToPoint(), Size.ToPoint());
        }

        public void Update()
        {
            if (Moving)
            {
                ++tickCounter;
                if (tickCounter == ticksPerAnimationFrame)
                {
                    tickCounter = 0;
                    NextMovementFrame();
                }
            }

            if (!Moving && !previouslyMoving)
            {
                ResetAnimation();
            }

            previouslyMoving = Moving;
        }

        private void NextMovementFrame()
        {
            animationFrame = (animationFrame + 1) % totalAnimationFrames;
            if (animationFrame == standstillFrame)
            {
                animationFrame = (animationFrame + 1) % totalAnimationFrames;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 location, bool semiTransparent)
        {
            Rectangle source = MakeSourceRectangle();
            Rectangle destination = new Rectangle(location.ToPoint(), Size.ToPoint());
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            Color color = (semiTransparent) ? Color.White * 0.7f : Color.White;
            spriteBatch.Draw(Texture, destination, source, color);
            spriteBatch.End();
        }
    }
}
