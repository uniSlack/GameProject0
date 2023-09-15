using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Threading.Tasks.Sources;

namespace GameProject0
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch spriteBatch;

        private SpriteFont silkScreen;

        private Texture2D castleBackground;
        private Texture2D bronzeKnight;
        private Texture2D darkRanger;

        private ArrowSprite[] arrows;
        private Shield shield;
        private Random r;

        private int score = 0;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            arrows = new ArrowSprite[]
            {
                //new ArrowSprite(new Vector2(GraphicsDevice.Viewport.Width, 100)),
                //new ArrowSprite(new Vector2(GraphicsDevice.Viewport.Width, 50)),
                //new ArrowSprite(new Vector2(GraphicsDevice.Viewport.Width, 150)),
                //new ArrowSprite(new Vector2(GraphicsDevice.Viewport.Width, 200)),
                //new ArrowSprite(new Vector2(GraphicsDevice.Viewport.Width, 250)),
                //new ArrowSprite(new Vector2(GraphicsDevice.Viewport.Width, 100)),
                //new ArrowSprite(new Vector2(GraphicsDevice.Viewport.Width, 50)),
                //new ArrowSprite(new Vector2(GraphicsDevice.Viewport.Width, 200)),
                //new ArrowSprite(new Vector2(GraphicsDevice.Viewport.Width, 250))
                //new ArrowSprite(){Position = new Vector2(200, 200), Fired = true }
                new ArrowSprite(),
                new ArrowSprite(),
                new ArrowSprite(),
                new ArrowSprite(),
                new ArrowSprite(),
                new ArrowSprite(),
                new ArrowSprite(),
                new ArrowSprite(),
                new ArrowSprite(),
                new ArrowSprite()
            };

            shield = new Shield(new Vector2(100,100));

            r = new Random();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            castleBackground = Content.Load<Texture2D>("castle_backround");
            silkScreen = Content.Load<SpriteFont>("SilkScreen");
            bronzeKnight = Content.Load<Texture2D>("BronzeKnight");
            darkRanger = Content.Load<Texture2D>("DarkRanger");

            foreach (var arrow in arrows) arrow.LoadContent(Content);
            shield.LoadContent(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            foreach(var arrow in arrows)
            {
                arrow.Update(gameTime, r, GraphicsDevice.Viewport.Width);
            }

            foreach (var arrow in arrows)
            {
                if (arrow.Bounds.CollidesWith(shield.Bounds))
                {
                    score += 100;
                    arrow.Blocked();
                }
            }

            shield.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            spriteBatch.Draw(castleBackground, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);

            string menuTitle = "Castle Defense";
            Vector2 mtLn = silkScreen.MeasureString(menuTitle);
            spriteBatch.DrawString(silkScreen, menuTitle, new Vector2(
                (GraphicsDevice.Viewport.Width/2) - (mtLn.X / 2),
                (GraphicsDevice.Viewport.Height/2) - (mtLn.Y / 2)),
                Color.White);

            spriteBatch.Draw(bronzeKnight, new Vector2(50, 305), new Rectangle(0, 0, 32, 32), Color.White, 0, new Vector2(0,0), 2, SpriteEffects.None, 0);
            spriteBatch.Draw(darkRanger, new Vector2(GraphicsDevice.Viewport.Width - 100, 305), 
                new Rectangle(0, 0, 32, 32), Color.White, 0, new Vector2(0,0), 2, SpriteEffects.FlipHorizontally, 0);

            spriteBatch.DrawString(silkScreen, "Exit with Escape Key", new Vector2(0, 0), Color.White, 0, new Vector2(0, 0), .25f, SpriteEffects.None, 0);
            spriteBatch.DrawString(silkScreen, $"Score: {score}", new Vector2(0, 20), Color.White, 0, new Vector2(0, 0), .5f, SpriteEffects.None, 0);

            foreach (var arrow in arrows) arrow.Draw(gameTime, spriteBatch);

            shield.Draw(gameTime, spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}