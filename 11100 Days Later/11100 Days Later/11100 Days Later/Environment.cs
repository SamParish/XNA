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
    class Environment
    {
        public InputHandler input = new InputHandler();

        PlayableChar terminator;
        EvilSpawns evil;
        Lazer lazer;

        public Texture2D levelOne, crossHair, spawnTexture, lazerTexture;
        public float lazerDelay;
        int killCount = 0;

        public List<Lazer> lazerList;
        public List<EvilSpawns> badSpawnList;

        public Environment()
        {
            badSpawnList = new List<EvilSpawns>();
            lazerList = new List<Lazer>();

            lazerDelay = 10;

            terminator = new PlayableChar(new Vector2(200, 200));
        }

        public void LoadContent(ContentManager content)
        {
            levelOne = content.Load<Texture2D>("Background");
            crossHair = content.Load<Texture2D>("Crosshair");
            lazerTexture = content.Load<Texture2D>("360Lazer");
            spawnTexture = content.Load<Texture2D>("EvilSpawn");

            terminator.LoadContent(content);
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            spriteBatch.Draw(levelOne, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
            terminator.Draw(spriteBatch);

            //Crosshair.
            spriteBatch.Draw(
                crossHair,
                terminator.crossHairPosition,
                Color.White);

            foreach (EvilSpawns e in badSpawnList)
                e.Draw(spriteBatch);

            foreach (Lazer l in lazerList)
                l.Draw(spriteBatch);
        }

        public void Update()
        {
            input.UpdateInput();
            terminator.Update();            

            if (input.IsSpacePressed() || input.IsLeftClick())
            {
                Shoot();
            }
            UpdateShoot();

            UpdateSpawn();
            UpdateSpawnInfo();
        }

        public void UpdateSpawn()
        {
            Random randomSpawn = new Random();
            int spawnPointX = randomSpawn.Next(650, 1250);
            int spawnPointY = randomSpawn.Next(0, 700);

            Vector2 spawnPoint = new Vector2(spawnPointX, spawnPointY);

             evil = new EvilSpawns(spawnPoint, spawnTexture);
                evil.spawnMovement = new Vector2(evil.spawnMovement.X - evil.spawnTexture.Width / 2, evil.spawnMovement.Y);
                
                if (badSpawnList.Count() < 10)
                    badSpawnList.Add(evil);
        }

        public void UpdateSpawnInfo()
        {
            foreach (EvilSpawns e in badSpawnList)
            {
                Vector2 spawnDirection = Vector2.Normalize(terminator.position - e.spawnMovement);

                e.boundingBox = new Rectangle((int)e.spawnMovement.X, (int)e.spawnMovement.Y,e.spawnTexture.Width, e.spawnTexture.Height);
                
                e.spawnMovement += spawnDirection * e.speed;
            }
        }

        public void Shoot()
        {
            if (lazerDelay >= 0)
                lazerDelay--;

            if (lazerDelay <= 0)
            {
                lazer = new Lazer(lazerTexture, terminator.position);
                lazer.lazerPosition = new Vector2(terminator.position.X - lazer.lazerTexture.Width / 2, terminator.position.Y);
                lazer.isVisible = true;

                if (lazerList.Count() < 20)
                    lazerList.Add(lazer);
            }

            if (lazerDelay == 0)
                lazerDelay = 10;
        }

        public void UpdateShoot()
        {
            foreach (Lazer l in lazerList)
            {
                //Sets movement.
                l.lazerPosition += l.lazerDirection * terminator.speed;

                l.boundingbox = new Rectangle((int)l.lazerPosition.X, (int)l.lazerPosition.Y, l.lazerTexture.Width, l.lazerTexture.Height);

                if (l.lazerPosition.Y <= 0 || l.lazerPosition.Y >= 700 || l.lazerPosition.X <= 0 || l.lazerPosition.X >= 1250)
                {
                    l.isVisible = false;
                }
            }

            //Intersects
            foreach (EvilSpawns e in badSpawnList)
            {
                for (int i = 0; i < lazerList.Count(); i++)
                {
                    if (e.boundingBox.Intersects(lazerList[i].boundingbox))
                    {
                        lazerList[i].isVisible = false;
                        e.isVisible = false;
                        killCount++;
                    }
                }
            }

            for (int i = 0; i < lazerList.Count(); i++)
            {
                if (!lazerList[i].isVisible)
                {
                    lazerList.RemoveAt(i);
                    i--;
                }
            }

            for (int i = 0; i < badSpawnList.Count(); i++)
            {
                if (!badSpawnList[i].isVisible)
                {
                    badSpawnList.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}
