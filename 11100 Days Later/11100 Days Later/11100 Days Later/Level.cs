using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace _11100_Days_Later
{
    class Level
    {
        Character character;

        Texture2D background;

        int width, height;

        public Level()
        {
            character = new Character(new Vector2(200,200), 100);
        }

        public void LoadContent(ContentManager content)
        {
            background = content.Load<Texture2D>("background");
            character.LoadContent(content);
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            spriteBatch.Draw(background, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
            character.Draw(spriteBatch);
        }
        public void Update()
        {
            character.Update();
        }

    }
}
