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
    class Character : Actor
    {
        MouseState mouseState = Mouse.GetState();

        InputHandler input = new InputHandler();


        public Character(Vector2 startPosition, int startHealth)
        {
            position = startPosition;
            health = startHealth;
            speed = new Vector2(0);
        }

        public void LoadContent(ContentManager content)
        {
            sprite = content.Load<Texture2D>("Player");
        }

        public override void Update()
        {
            base.Update();

            MouseState mouseState = Mouse.GetState();
            double yDiff = position.Y - mouseState.Y;
            double xDiff = -(position.X - mouseState.X);

            facing = (float)(90 - Math.Atan(yDiff / xDiff));


        }
    }
}
