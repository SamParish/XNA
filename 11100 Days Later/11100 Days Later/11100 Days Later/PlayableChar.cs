using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace _11100_Days_Later
{
    class PlayableChar
    {
        public InputHandler input = new InputHandler();
        public MouseState mouseState = Mouse.GetState();

        public Rectangle boundingBox;
        public Texture2D characterTexture;
        public Vector2 position;
        public Vector2 crossHairPosition;
        public float speed = 5;
        public float facing;
        public int health;
        public bool isDead = false;

        public PlayableChar(Vector2 spawnPosition)
        {
            position = spawnPosition;
            health = 200;
        }

        public void LoadContent(ContentManager content)
        {
            characterTexture = content.Load<Texture2D>("Player");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //Playable entity.
            spriteBatch.Draw(
                characterTexture,
                position,
                new Rectangle(0, 0, characterTexture.Width, characterTexture.Height),
                Color.White,
                facing,
                new Vector2(characterTexture.Width / 2, characterTexture.Height / 2),
                1,
                SpriteEffects.None,
                0);
        }

        public void Update()
        {
            input.UpdateInput();

            boundingBox = new Rectangle((int)position.X, (int)position.Y, characterTexture.Width, characterTexture.Height);

            #region Mouse Code

            //Mouse operations: Gets the current value of the mouse position and sets the crosshair.
            mouseState = Mouse.GetState();
            var mousePosition = new Vector2(mouseState.X - 12, mouseState.Y - 11);
            crossHairPosition = mousePosition;

            //Sets the playableChar facing value to the mouse position.
            Vector2 direction = position - mousePosition;
            facing = (float)Math.Atan2(-direction.Y, -direction.X) + MathHelper.PiOver2;

            #endregion

            #region Keyboard Code

            //if (position.X <= 0)
            if (position.X <= 55)
            {
                position = new Vector2(55, position.Y);
            }
            if (position.X >= 1200)
            {
                position = new Vector2(1200, position.Y);
            }
            if (position.Y <= 55)
            {
                position = new Vector2(position.X, 55);
            }
            if (position.Y >= 650)
            {
                position = new Vector2(position.X, 650);
            }

            if (input.IsLeftPressed())
            {
                position.X -= speed;
            }
            else if (input.IsRightPressed())
            {
                position.X += speed;
            }

            if (input.IsUpPressed())
            {
                position.Y -= speed;
            }
            else if (input.IsDownPressed())
            {
                position.Y += speed;
            }

            if (health < 0)
            {
                isDead = true;
            }


            #endregion
        }
    }
}
