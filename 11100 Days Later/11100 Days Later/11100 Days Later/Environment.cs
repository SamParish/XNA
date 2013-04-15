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

        public SpriteFont font;
        public Rectangle healthBarRectangle, medPackRectangle;
        public Texture2D levelOne, crossHair, spawnTexture, healthTexture, medPackTexture;
        public Vector2 healthBarPos, medPackPos;
        int killCount = 0;
        public bool hasWon = false;

        public List<EvilSpawns> badSpawnList;

        public Environment()
        {
            badSpawnList = new List<EvilSpawns>();
            
            terminator = new PlayableChar(new Vector2(200, 200));
            healthBarPos = new Vector2(50, 50);

            // Spawns Health kit.
            // This needs to be a seperate Method that is called here.
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

            foreach (Lazer l in terminator.lazerList)
                l.Draw(spriteBatch);
        }

        public void Update()
        {
            input.UpdateInput();
            terminator.Update();

            // I think Health should be part of the character..
            healthBarRectangle = new Rectangle((int)healthBarPos.X, (int)healthBarPos.Y, terminator.health, 25);
            medPackRectangle = new Rectangle((int)medPackPos.X, (int)medPackPos.Y, medPackTexture.Width, medPackTexture.Height);

            
            CheckCollisions();

            UpdateSpawn();
            UpdateSpawnInfo();

            if (killCount >= 50)
            {
                hasWon = true;
            }
        }

        // So this spawns new badguys if needed.
        public void UpdateSpawn()
        {
            Random randomSpawn = new Random();
            int spawnPointX = randomSpawn.Next(650, 1250);
            int spawnPointY = randomSpawn.Next(0, 700);

            Vector2 spawnPoint = new Vector2(spawnPointX, spawnPointY);

             evil = new EvilSpawns(spawnPoint, spawnTexture);
                evil.spawnMovement = new Vector2(evil.spawnMovement.X - evil.spawnTexture.Width / 2, evil.spawnMovement.Y);
                
                if (badSpawnList.Count() < 25)
                    badSpawnList.Add(evil);
        }

        // And this updates the positions of current bad guys. Should be withing the bad guys.cs
        public void UpdateSpawnInfo()
        {
            foreach (EvilSpawns e in badSpawnList)
            {
                Vector2 spawnDirection = Vector2.Normalize(terminator.position - e.spawnMovement);

                e.boundingBox = new Rectangle((int)e.spawnMovement.X, (int)e.spawnMovement.Y,e.spawnTexture.Width, e.spawnTexture.Height);
                
                e.spawnMovement += spawnDirection * e.speed;
            }
        }

        // Checks for collisions.
        public void CheckCollisions()
        {
            //Intersects
            //Does an evil intersect a lazer?
            foreach (EvilSpawns e in badSpawnList)
            {
                for (int i = 0; i < terminator.lazerList.Count(); i++)
                {
                    if (e.boundingBox.Intersects(terminator.lazerList[i].boundingbox))
                    {
                        terminator.lazerList[i].isVisible = false;
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
            for (int i = 0; i < terminator.lazerList.Count(); i++)
            {
                if (!terminator.lazerList[i].isVisible)
                {
                    terminator.lazerList.RemoveAt(i);
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
