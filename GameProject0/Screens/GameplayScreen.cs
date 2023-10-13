﻿using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameProject0.StateManagement;
using System.Collections.Generic;
using System.Threading.Tasks.Sources;
using SharpDX.Direct2D1;
using System.Reflection.Metadata;

namespace GameProject0.Screens
{
    // This screen implements the actual game logic. It is just a
    // placeholder to get the idea across: you'll probably want to
    // put some more interesting gameplay in here!
    public class GameplayScreen : GameScreen
    {
        private ContentManager _content;
        private SpriteBatch spriteBatch;

        private float _pauseAlpha;
        private readonly InputAction _pauseAction;

        private SpriteFont silkScreen;

        private Texture2D castleBackground;
        private Texture2D bronzeKnight;
        private Texture2D darkRanger;

        private ArrowSprite[] arrows;
        private Shield shield;
        private Random r;

        private int score = 0;
        private int health = 100;
        private float arrowSpeed = 1;

        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            _pauseAction = new InputAction(
                new[] { Buttons.Start, Buttons.Back },
                new[] { Keys.Back, Keys.Escape }, true);
        }

        // Load graphics content for the game
        public override void Activate()
        {
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");

            spriteBatch = new SpriteBatch(GraphicsDevice);

            castleBackground = Content.Load<Texture2D>("castle_backround");
            silkScreen = Content.Load<SpriteFont>("SilkScreen");
            bronzeKnight = Content.Load<Texture2D>("BronzeKnight");
            darkRanger = Content.Load<Texture2D>("DarkRanger");

            foreach (var arrow in arrows) arrow.LoadContent(Content);
            shield.LoadContent(Content);
        }


        public override void Deactivate()
        {
            base.Deactivate();
        }

        public override void Unload()
        {
            _content.Unload();
        }

        // This method checks the GameScreen.IsActive property, so the game will
        // stop updating when the pause menu is active, or if you tab away to a different application.
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (health <= 0)
            {
                Exit();
                System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Game Over!", $"Your health reached zero!\nScore: {score}");
                //if (result == System.Windows.Forms.DialogResult.Yes)
            }

            // TODO: Add your update logic here
            foreach (var arrow in arrows)
            {
                arrow.Update(gameTime, r, GraphicsDevice.Viewport.Width, arrowSpeed);
            }

            foreach (var arrow in arrows)
            {
                if (arrow.Position.X < 0)
                {
                    health -= 10;
                    arrow.Blocked();
                }
                if (arrow.Bounds.CollidesWith(shield.Bounds))
                {
                    score += 100;
                    arrow.Blocked();
                }
            }

            shield.Update(gameTime);

            arrowSpeed += (float)gameTime.ElapsedGameTime.TotalSeconds / 500;

            base.Update(gameTime);
        }

        // Unlike the Update method, this will only be called when the gameplay screen is active.
        public override void HandleInput(GameTime gameTime, InputState input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            // Look up inputs for the active player profile.
            int playerIndex = (int)ControllingPlayer.Value;

            var keyboardState = input.CurrentKeyboardStates[playerIndex];
            var gamePadState = input.CurrentGamePadStates[playerIndex];

            // The game pauses either if the user presses the pause button, or if
            // they unplug the active gamepad. This requires us to keep track of
            // whether a gamepad was ever plugged in, because we don't want to pause
            // on PC if they are playing with a keyboard and have no gamepad at all!
            bool gamePadDisconnected = !gamePadState.IsConnected && input.GamePadWasConnected[playerIndex];

            PlayerIndex player;
            if (_pauseAction.Occurred(input, ControllingPlayer, out player) || gamePadDisconnected)
            {
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }
            else
            {
                // Otherwise move the player position.
                var movement = Vector2.Zero;

                if (keyboardState.IsKeyDown(Keys.Left))
                    movement.X--;

                if (keyboardState.IsKeyDown(Keys.Right))
                    movement.X++;

                if (keyboardState.IsKeyDown(Keys.Up))
                    movement.Y--;

                if (keyboardState.IsKeyDown(Keys.Down))
                    movement.Y++;

                var thumbstick = gamePadState.ThumbSticks.Left;

                movement.X += thumbstick.X;
                movement.Y -= thumbstick.Y;

                if (movement.Length() > 1)
                    movement.Normalize();

                _playerPosition += movement * 8f;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            // This game has a blue background. Why? Because!
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.CornflowerBlue, 0, 0);

            // Our player and enemy are both actually just text strings.
            var spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();

            spriteBatch.DrawString(_gameFont, "// TODO", _playerPosition, Color.Green);
            spriteBatch.DrawString(_gameFont, "Insert Gameplay Here",
                                   _enemyPosition, Color.DarkRed);

            spriteBatch.End();

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || _pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, _pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }
    }
}
