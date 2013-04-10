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
    class EvilSpawns
    {
        public Rectangle boundingBox;
        public Texture2D spawnTexture;
        public Vector2 spawnMovement;
        public float speed = 2;
        public bool isVisible;

        public EvilSpawns(Vector2 pSpawnStart, Texture2D pNewTexture)
        {
            spawnMovement = pSpawnStart;
            spawnTexture = pNewTexture;
            isVisible = true;
        }

        public EvilSpawns()
        { }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                spawnTexture,
                spawnMovement,
                new Rectangle(0, 0, spawnTexture.Width, spawnTexture.Height),
                Color.White,
                0,
                new Vector2(spawnTexture.Width / 2, spawnTexture.Height / 2),
                1,
                SpriteEffects.None,
                0);
        }
    }
}
