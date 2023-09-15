using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using GameProject0.Collisions;

namespace GameProject0
{
    public class Shield
    {
        private Texture2D texture;
        public Vector2 Position { get; set; }

        private BoundingRectangle bounds;
        public BoundingRectangle Bounds => bounds;

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
            
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, null, Color.White, 0, new Vector2(0, 0), .2f, SpriteEffects.None, 0) ;
        }
    }
}
