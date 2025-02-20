﻿using HYPERMAGE.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HYPERMAGE.Models;
using HYPERMAGE.Particles;
using System.Runtime.ExceptionServices;
using System.Diagnostics;
using HYPERMAGE.UI.UIElements;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace HYPERMAGE.Managers
{
    public static class LevelManager
    {
        public static int level = 0;

        public static List<Mob> spawnWave = [];
        public static int spawnWaveCost;

        public static List<int> validSpawns = [];

        public static int levelCredits;
        public static int credits;
        private static float creditsTimer;

        public static int levelMana;

        private static bool endCred;

        private static float endLevelTimer;
        public static bool endLevel;

        private static Vector2 spawnPos;

        private static Song song;

        private static Texture2D bg;
        private static Texture2D bg2;
        private static Texture2D bg3;

        private static Vector2 bgPos = Vector2.Zero;
        private static Vector2 bgPos2 = Vector2.Zero;

        private static float bgScrollTimer;
        public static void DrawBG()
        {
            Globals.SpriteBatch.Draw(bg3, Vector2.Zero, Color.White);

            Globals.SpriteBatch.Draw(bg, bgPos, Color.White);
            Globals.SpriteBatch.Draw(bg, bgPos + new Vector2(bg.Width, 0), Color.White);

            Globals.SpriteBatch.Draw(bg2, bgPos2, Color.White);
            Globals.SpriteBatch.Draw(bg2, bgPos2 + new Vector2(bg2.Width, 0), Color.White);
        }
        public static void Update()
        {
            if (bgScrollTimer == 0)
            {
                for (int i = 0; i < 30; i++)
                {
                    ParticleData spawnParticleData = new()
                    {
                        opacityStart = 1f,
                        opacityEnd = 1f,
                        sizeStart = 12 - Globals.Random.Next(6),
                        sizeEnd = 0,
                        colorStart = Color.White,
                        colorEnd = Color.White,
                        velocity = new(Globals.RandomFloat(-200, 200), Globals.RandomFloat(-800, 50)),
                        lifespan = Globals.RandomFloat(0.2f, 0.8f),
                        rotationSpeed = Globals.RandomFloat(-0.5f, 0.5f)
                    };

                    Particle spawnParticle = new(GameManager.GetPlayer().center, spawnParticleData);
                    ParticleManager.AddParticle(spawnParticle);
                }

                for (int i = 0; i < 15; i++)
                {
                    ParticleData spawnParticleData = new()
                    {
                        opacityStart = 1f,
                        opacityEnd = 1f,
                        sizeStart = 4 - Globals.Random.Next(4),
                        sizeEnd = 0,
                        colorStart = Color.White,
                        colorEnd = Color.White,
                        velocity = new(Globals.RandomFloat(-600, 600), Globals.RandomFloat(-200, 50)),
                        lifespan = Globals.RandomFloat(0.2f, 0.8f),
                        rotationSpeed = Globals.RandomFloat(-0.5f, 0.5f)
                    };

                    Particle spawnParticle = new(GameManager.GetPlayer().center, spawnParticleData);
                    ParticleManager.AddParticle(spawnParticle);
                }


                SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("wavy"), 1f, 0f, 0f);

                SoundManager.PlaySong(song, 1f);
            }

            if (bgScrollTimer < 3 && !endLevel)
            {
                bgScrollTimer += Globals.TotalSeconds;
            }

            bgPos.X -= bgScrollTimer / 12;

            if (bgPos.X < -bg.Width)
            {
                bgPos.X = 0;
            }

            bgPos2.X -= bgScrollTimer / 8;

            if (bgPos2.X < -bg2.Width)
            {
                bgPos2.X = 0;
            }


            if (MobManager.mobs.Count == 0)
            {
                creditsTimer += Globals.TotalSeconds;
            }

            else
            {
                creditsTimer += Globals.TotalSeconds / 2;
            }

            if (creditsTimer >= 1 && levelCredits > 0)
            {
                creditsTimer = 0;


                levelCredits--;
                credits++;
            }

            if (MobManager.mobs.Count == 0 ? credits >= spawnWaveCost : Globals.Random.Next(200) == 0 && credits >= spawnWaveCost)
            {
                SpawnNextWave();
            }

            if (spawnWaveCost > credits && levelCredits == 0 && !endCred)
            {
                credits += spawnWaveCost - credits;
                endCred = true;
            }

            if (credits == 0 && levelCredits == 0 && MobManager.mobs.Count == 0)
            {
                endLevel = true;
            }

            if (endLevel)
            {
                endLevelTimer += Globals.TotalSeconds;

                if (bgScrollTimer > 0)
                {
                    bgScrollTimer -= Globals.TotalSeconds;
                }
            }

            if (endLevelTimer >= 5f)
            {
                GameManager.TransitionScene(new StageTransition());
            }
        }

        public static void SpawnNextWave()
        {
            ParticleData particleData = new()
            {
                texture = Globals.Content.Load<Texture2D>("spawnindicator"),
                sizeStart = 0.1f,
                sizeEnd = 1f,
                flashing = true,
                colorStart = Color.Red,
                colorEnd = Color.Red,
                fastScale = true,
                spawnIndicator = true
            };

            Particle spawnIndicator = new(spawnPos, particleData);
            ParticleManager.AddParticle(spawnIndicator);

            foreach (Mob mob in spawnWave)
            {
                credits -= mob.spawnCost;
                mob.Spawn();
            }

            GetNextSpawnWave(validSpawns[Globals.Random.Next(validSpawns.Count)]);
        }

        public static void GetNextSpawnWave(int type)
        {
            spawnWave.Clear();
            spawnWaveCost = 0;

            if (type == 0)
            {
                spawnPos = new(Globals.RandomFloat(20, 300), 90);
                    
                Mob wizard = new(spawnPos, 3);
                spawnWave.Add(wizard);

                for (int i = 0; i < 3; i++)
                {
                    Mob wisp = new(spawnPos + new Vector2(-10, -10).RotatedBy(MathHelper.ToRadians(45 * i)), 2);

                    spawnWave.Add(wisp);
                }
            }

            if (type == 1)
            {
                spawnPos = new(Globals.RandomFloat(20, 300), 140);

                Mob wizard = new(spawnPos, 3);
                spawnWave.Add(wizard);
            }

            if (type == 2)
            {
                spawnPos = new(Globals.RandomFloat(20, 300), 20);

                for (int i = 0; i < 5; i++)
                {
                    Mob bat = new(spawnPos + new Vector2(Globals.RandomFloat(-10, 10), Globals.RandomFloat(-10, 10)), 1);

                    spawnWave.Add(bat);
                }
            }

            if (type == 3)
            {
                spawnPos = new(Globals.RandomFloat(20, 300), 30);

                for (int i = 0; i < 2; i++)
                {
                    Mob wisp = new(spawnPos + new Vector2(Globals.RandomFloat(-10, 10), Globals.RandomFloat(-10, 10)), 2);

                    spawnWave.Add(wisp);
                }
            }

            if (type == 4)
            {
                spawnPos = new(Globals.RandomFloat(20, 300), 140);

                Mob sorcerer = new(spawnPos, 6);
                spawnWave.Add(sorcerer);
            }

            int j = 0;

            foreach (Mob mob in spawnWave)
            {
                j++;

                if (j % 2 == 1)
                {
                    mob.manaDrop += 1 + (mob.spawnCost / 3);
                }

                spawnWaveCost += mob.spawnCost;
            }
        }

        public static void NextLevel()
        {
            validSpawns.Clear();
            endCred = false;
            level++;

            endLevel = false;
            endLevelTimer = 0;
            bgScrollTimer = 0;

            switch (level)
            {
                case 1:
                    bg = Globals.Content.Load<Texture2D>("bg");
                    bg2 = Globals.Content.Load<Texture2D>("bg2");
                    bg3 = Globals.Content.Load<Texture2D>("stars");
                    song = Globals.Content.Load<Song>("stage3");

                    GameManager.groundBounds = new(0, 60, 320, 180);
                    GameManager.bounds = new(0, 0, 320, 180);

                    levelCredits = 20;
                    //

                    validSpawns.Add(0);
                    validSpawns.Add(1);
                    validSpawns.Add(2);
                    validSpawns.Add(3);
                    validSpawns.Add(4);

                    break;
                case 2:
                    bg = Globals.Content.Load<Texture2D>("bg");
                    bg2 = Globals.Content.Load<Texture2D>("bg2");
                    bg3 = Globals.Content.Load<Texture2D>("stars");
                    song = Globals.Content.Load<Song>("stage3");

                    GameManager.groundBounds = new(0, 60, 320, 180);
                    GameManager.bounds = new(0, 0, 320, 180);

                    levelCredits = 40;
                    //
                    validSpawns.Add(0);
                    validSpawns.Add(1);
                    validSpawns.Add(2);
                    validSpawns.Add(3);
                    validSpawns.Add(4);

                    break;
                case 3:
                    bg = Globals.Content.Load<Texture2D>("bg");
                    bg2 = Globals.Content.Load<Texture2D>("bg2");
                    bg3 = Globals.Content.Load<Texture2D>("stars");
                    song = Globals.Content.Load<Song>("stage3");

                    GameManager.groundBounds = new(0, 60, 320, 180);
                    GameManager.bounds = new(0, 0, 320, 180);

                    levelCredits = 60;
                    //
                    validSpawns.Add(0);
                    validSpawns.Add(1);
                    validSpawns.Add(2);
                    validSpawns.Add(3);

                    break;
            }

            GetNextSpawnWave(validSpawns[Globals.Random.Next(validSpawns.Count)]);
        }
    }
}
