using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using GameProject0.Collisions;

namespace GameProject0
{
    public class ArrowSprite
    {

        private Texture2D texture;
        public Vector2 Position { get; set; }

        private double movementTimer;
        private double animationTimer;

        private int animationFrame = 1;

        public bool Fired { get; set; }

        private BoundingRectangle bounds; 
        public BoundingRectangle Bounds => bounds;

        public ArrowSprite()
        {
            Position = new Vector2(10000,10000);
            bounds = new BoundingRectangle(Position - new Vector2(16, 10), 32, 20); // loose as to be a bit easy
        }
        //public ArrowSprite(Vector2 position)
        //{
        //    Position = position;
        //    bounds = new BoundingRectangle(position - new Vector2(16, 10), 32, 20); // loose as to be a bit easy
        //}

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("WEAPON_arrow");
        }

        public void Update(GameTime gameTime, Random r, int width)
        {
            movementTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if (movementTimer > .1)
            {
                if (Fired)
                {
                    Position += new Vector2(-1, 0) * 500 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (Position.X < -64) 
                    {
                        Fired = false;
                        Position = new Vector2(width, Position.Y);
                    }
                }
                else
                {
                    if (r.NextDouble() < .02)
                    {
                        animationFrame = r.Next(7);
                        Position = new Vector2(width, r.Next(50, 250));
                        Fired = true;
                    }
                }
                movementTimer -= .1;
            }
            bounds.X = Position.X - 16;
            bounds.Y = Position.Y - 10;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if (animationTimer > .3)
            {
                animationFrame++;
                if (animationFrame > 7) animationFrame = 1;
                animationTimer -= .3;
            }

            var source = new Rectangle((animationFrame * 64) + 64, 64 * 3, 64, 64);
            if (Fired)
            {
                spriteBatch.Draw(texture, Position, source, Color.White, 0, new Vector2(0, 0), 1, SpriteEffects.FlipHorizontally, 0);
            }
        }

        public void Blocked()
        {
            Fired = false;
            Position = new Vector2(10000, 10000);
            bounds.X = Position.X - 16;
            bounds.Y = Position.Y - 10;
        }
    }
}
