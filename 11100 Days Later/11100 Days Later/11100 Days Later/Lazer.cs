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
    class Lazer
    {
        public MouseState mouseState = Mouse.GetState();

        public Rectangle boundingbox;
        public Texture2D lazerTexture;
        public Vector2 lazerPosition, lazerDirection, mousePos;
        public bool isVisible;
        float speed;

        public Lazer(Texture2D pNewTexture, Vector2 pPosition)
        {
            speed = 10;
            mousePos = new Vector2(mouseState.X, mouseState.Y);

            lazerPosition = pPosition;

            lazerDirection = Vector2.Normalize(mousePos - lazerPosition);

            lazerTexture = pNewTexture;
            isVisible = false;

        }

        public Lazer()
        { }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                lazerTexture,
                lazerPosition,
                new Rectangle(0, 0, lazerTexture.Width, lazerTexture.Height),
                Color.White,
                0,
                new Vector2(lazerTexture.Width / 2, lazerTexture.Height / 2),
                1,
                SpriteEffects.None,
                0);
        }

        public void update()
        {
            //Sets movement.
            lazerPosition += lazerDirection * speed;

            boundingbox = new Rectangle((int)lazerPosition.X, (int)lazerPosition.Y, lazerTexture.Width, lazerTexture.Height);

            if (lazerPosition.Y <= 0 || lazerPosition.Y >= 700 || lazerPosition.X <= 0 || lazerPosition.X >= 1250)
            {
                isVisible = false;
            }
        }
    }
}
