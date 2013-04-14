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

        public PlayableChar terminator;
        EvilSpawns evil;
        Lazer lazer;

        public SpriteFont font;
        public Rectangle healthBarRectangle, medPackRectangle;
        public Texture2D levelOne, crossHair, spawnTexture, lazerTexture, healthTexture, medPackTexture;
        public Vector2 healthBarPos, medPackPos;
        public float lazerDelay;
        int killCount = 0;
        public bool hasWon = false;

        public List<Lazer> lazerList;
        public List<EvilSpawns> badSpawnList;

        public Environment()
        {
            badSpawnList = new List<EvilSpawns>();
            lazerList = new List<Lazer>();
            lazerDelay = 10;
            terminator = new PlayableChar(new Vector2(200, 200));
            healthBarPos = new Vector2(50, 50);

            Random randomMed = new Random();
            int spawnPointX = randomMed.Next(50, 1200);
            int spawnPointY = randomMed.Next(50, 650);

            medPackPos = new Vector2(spawnPointX, spawnPointY);
        }

        public void LoadContent(ContentManager content)
        {
            font = content.Load<SpriteFont>("SpriteFont1");
            healthTexture = content.Load<Texture2D>("healthBar");
            medPackTexture = content.Load<Texture2D>("MedPack");
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

            spriteBatch.Draw(healthTexture, healthBarRectangle, Color.White);

            spriteBatch.Draw(medPackTexture, medPackRectangle, Color.White);

            spriteBatch.DrawString(font, "Kill Count: " + killCount.ToString(), new Vector2(260, 52), Color.White);

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

            healthBarRectangle = new Rectangle((int)healthBarPos.X, (int)healthBarPos.Y, terminator.health, 25);
            medPackRectangle = new Rectangle((int)medPackPos.X, (int)medPackPos.Y, medPackTexture.Width, medPackTexture.Height);

            if (input.IsSpacePressed() || input.IsLeftClick())
            {
                Shoot();
            }
            UpdateShoot();

            UpdateSpawn();
            UpdateSpawnInfo();

            if (killCount >= 200)
            {
                hasWon = true;
            }
        }

        public void UpdateSpawn()
        {
            Random randomSpawn = new Random();
            int spawnPointX = randomSpawn.Next(650, 1250);
            int spawnPointY = randomSpawn.Next(0, 700);

            Vector2 spawnPoint = new Vector2(spawnPointX, spawnPointY);

             evil = new EvilSpawns(spawnPoint, spawnTexture);
                evil.spawnMovement = new Vector2(evil.spawnMovement.X - evil.spawnTexture.Width / 2, evil.spawnMovement.Y);
                
                if (badSpawnList.Count() < 200)
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
            //Does an evil intersect a lazer?
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

            // check if an evil intersects player.
            foreach (EvilSpawns e in badSpawnList)
            {
                if (e.boundingBox.Intersects(terminator.boundingBox))
                {
                    terminator.health -= 1;
                }
            }

            //If user picks up med pack, add health.
            if (medPackRectangle.Intersects(terminator.boundingBox))
            {
                if (terminator.health < 170)
                {
                    terminator.health += 30;

                    //Generate a new location once picked up.
                    Random randomMed = new Random();
                    int spawnPointX = randomMed.Next(50, 1200);
                    int spawnPointY = randomMed.Next(50, 650);

                    medPackPos = new Vector2(spawnPointX, spawnPointY);
                }
            }

            // remove dead things
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
