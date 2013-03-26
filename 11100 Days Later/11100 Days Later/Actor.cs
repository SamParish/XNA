using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _11100_Days_Later
{
    class Actor
    {
        public bool isDead;

        public int health;

        public Vector2 position, speed;
        public float facing;  // acceleration, maxSpeed ??

        public Texture2D sprite;

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                sprite,
                position,
                new Rectangle(0, 0, sprite.Width, sprite.Height),
                Color.White,
                facing,
                new Vector2(sprite.Width / 2, sprite.Height / 2),
                1,
                SpriteEffects.None,
                0);
        }

        virtual public void Update()
        {
            if (health < 0)
            {
                isDead = true;
            }

            position = position + speed;
        }


    }
}
