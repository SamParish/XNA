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
        public Texture2D characterTexture, lazerTexture;
        public Vector2 position;
        public Vector2 crossHairPosition;
        public float speed = 5;
        public float facing;
        public int health;
        public bool isDead = false;

        public List<Lazer> lazerList;
        int lazerDelay = 10;

        public PlayableChar(Vector2 spawnPosition)
        {
            position = spawnPosition;
            health = 200;

            lazerList = new List<Lazer>();
        }

        public void LoadContent(ContentManager content)
        {
            characterTexture = content.Load<Texture2D>("Player");
            lazerTexture = content.Load<Texture2D>("360Lazer");
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

            // Shoots a new lazer.
            if (input.IsSpacePressed() || input.IsLeftClick())
            {
                Shoot();
            }

            if (health < 0)
            {
                isDead = true;
            }


            #endregion
        }

        public void Shoot()
        {
            if (lazerDelay >= 0)
                lazerDelay--;

            if (lazerDelay <= 0)
            {
                Lazer lazer = new Lazer(lazerTexture, position);
                lazer.lazerPosition = new Vector2(position.X - lazer.lazerTexture.Width / 2, position.Y);
                lazer.isVisible = true;

                if (lazerList.Count() < 20)
                    lazerList.Add(lazer);
            }

            if (lazerDelay == 0)
                lazerDelay = 10;
        }
    }
}
