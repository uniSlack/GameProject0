using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using GameProject0.Collisions;
using Microsoft.Xna.Framework.Input;

namespace GameProject0
{
    public class Shield
    {
        private Texture2D texture;
        public Vector2 Position { get; set; }

        private BoundingRectangle bounds;
        public BoundingRectangle Bounds => bounds;
        private KeyboardState keyboardState;

        public Shield(Vector2 position)
        {
            Position = position;
            bounds = new BoundingRectangle(position - new Vector2(32, 16), 64, 32);
        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("tempShield");
        }

        public void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();
            if ((keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W)) && Position.Y > 0) Position += new Vector2(0, -2);
            if ((keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S)) && Position.Y < 300) Position += new Vector2(0, 2);
            //bounds.X = Position.X - 32;
            bounds.Y = Position.Y - 16;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, null, Color.White, 0, new Vector2(0, 0), .2f, SpriteEffects.None, 0) ;
        }
    }
}
