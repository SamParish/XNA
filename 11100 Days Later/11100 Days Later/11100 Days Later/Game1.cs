using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace _11100_Days_Later
{
    public enum GameState
    {
        Start, Alive, Loose, Win, Paused
    }
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        InputHandler input = new InputHandler();

        SpriteFont font;
        SpriteFont menu;

        GameState gameState;

        Environment world;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            world = new Environment();

            graphics.PreferredBackBufferWidth = 1250;
            graphics.PreferredBackBufferHeight = 700;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();

            gameState = GameState.Start;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            font = Content.Load<SpriteFont>("SpriteFont1");
            menu = Content.Load<SpriteFont>("MenuFont");
            world.LoadContent(Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            input.UpdateInput();
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            if (input.IsEscPressed())
            {
                this.Exit();
            }

            // Gamestate management.
            if (input.IsEnterPressed())
            {
                if (gameState == GameState.Start || gameState == GameState.Paused)
                {
                    gameState = GameState.Alive;
                }
            }

            if (gameState == GameState.Alive)
            {
                // Run the main game..
                world.Update();

                if (world.terminator.isDead)
                {
                    gameState = GameState.Loose;
                }

                if (world.hasWon)
                {
                    gameState = GameState.Win;
                }
            }
            base.Update(gameTime);            
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // TODO: Add your drawing code here
            spriteBatch.Begin();

            if (gameState == GameState.Start)
            {
                GraphicsDevice.Clear(Color.White);
                spriteBatch.DrawString(menu, "Press Enter To Start ", new Vector2(50, 50), Color.Black);
            }
            if (gameState == GameState.Alive)
            {
                world.Draw(spriteBatch, graphics);
            }
            if (gameState == GameState.Loose)
            {
                GraphicsDevice.Clear(Color.Black);
                spriteBatch.DrawString(menu, "You Lose \nPress Esc To Escape", new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2), Color.White);
            }
            if (gameState == GameState.Win)
            {
                GraphicsDevice.Clear(Color.Black);
                spriteBatch.DrawString(menu, "Congratulations\nYou Defeated The Evil Spawns", new Vector2(375, 450), Color.White);
            }
            
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
