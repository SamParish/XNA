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
            input.UpdateInput();

            mouseState = Mouse.GetState();

            var mouseloc = new Vector2(mouseState.X, mouseState.Y);
            Vector2 direction = position - mouseloc;
            facing = (float)Math.Atan2(-direction.Y, -direction.X) + MathHelper.PiOver2;

            if (input.IsLeftPressed())
            {
                speed.X = -2;
            }
            else if (input.IsRightPressed())
            {
                speed.X = 2;
            }
            else
            {
                speed.X = 0;
            }

            if (input.IsUpPressed())
            {
                speed.Y = -2;
            }
            else if (input.IsDownPressed())
            {
                speed.Y = 2;
            }
            else
            {
                speed.Y = 0;
            }

            base.Update();
        }
    }
}
