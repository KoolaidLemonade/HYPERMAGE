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
        public static int level = 11;
        public static int stage = 0;

        public static List<Mob> spawnWave = [];
        public static int spawnWaveCost;

        public static List<int> validSpawns = [];
        public static int bossID;

        public static int totalLevelCredits;
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
        private static Texture2D bg4;

        private static Vector2 bgPos = Vector2.Zero;
        private static Vector2 bgPos2 = Vector2.Zero;
        private static Vector2 bgPos3 = Vector2.Zero;

        public static float bgScrollTimer;

        public static bool bossStage;
        private static float bossSpawnTimer;
        private static bool bossSpawned;

        private static bool stageStart = false;

        private static float nextZoneSize;
        private static float zoneSizeTimer;

        private static float ai1;
        private static float ai2;
        private static float ai3;
        private static float ai4;

        public static void DrawBG2()
        {
            if (bg4 != null)
            {
                Globals.SpriteBatch.Draw(bg4, bgPos3, Color.White);
                Globals.SpriteBatch.Draw(bg4, bgPos3 + new Vector2(bg4.Width, 0), Color.White);
            }

            Globals.SpriteBatch.Draw(bg, bgPos, Color.White);
            Globals.SpriteBatch.Draw(bg, bgPos + new Vector2(bg.Width, 0), Color.White);

            Globals.SpriteBatch.Draw(bg2, bgPos2, Color.White);
            Globals.SpriteBatch.Draw(bg2, bgPos2 + new Vector2(bg2.Width, 0), Color.White);

        }
        public static void DrawBG()
        {
            Globals.SpriteBatch.Draw(bg3, Vector2.Zero, Color.White);
        }
        public static void Draw()
        {
            if (bossSpawnTimer >= 2f && bossSpawnTimer < 5f)
            {
                GameManager.DrawBigText();
            }
        }
        public static void Update()
        {
            if (!stageStart)
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

                GameManager.AddScreenShake(0.2f, 15f);
                GameManager.AddAbberationPowerForce(500, 50);

                SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("wavy"), 1f, 0f, 0f);

                SoundManager.PlaySong(song, 1f);

                stageStart = true;
            }

            if (!bossStage)
            {

                if (bgScrollTimer < 3 && !endLevel)
                {
                    bgScrollTimer += Globals.TotalSeconds;
                }

                bgPos.X -= bgScrollTimer / 12;

                if (bgPos.X < -bg.Width)
                {
                    bgPos.X = 0;
                }

                bgPos2.X -= bgScrollTimer / 5;

                if (bgPos2.X < -bg2.Width)
                {
                    bgPos2.X = 0;
                }

                bgPos3.X -= bgScrollTimer / 20;

                if (bg4 != null)
                {
                    if (bgPos3.X < -bg4.Width)
                    {
                        bgPos3.X = 0;
                    }
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


                    levelCredits -= MobManager.mobs.Count == 0 ? stage * 2 : stage;
                    credits += MobManager.mobs.Count == 0 ? stage * 2 : stage;
                }

                if (MobManager.mobs.Count == 0 ? credits >= spawnWaveCost : Globals.Random.Next(200) == 0 && credits >= spawnWaveCost)
                {
                    SpawnNextWave();
                }

                if (spawnWaveCost > credits && levelCredits <= 0 && !endCred)
                {
                    credits += spawnWaveCost - credits;
                    endCred = true;
                }

                if (credits <= 0 && levelCredits <= 0 && MobManager.mobs.Count == 0)
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
            
            else
            {
                bossSpawnTimer += Globals.TotalSeconds;
                Vector2 spawnPos;
                switch (bossID)
                {
                    case 9:
                        spawnPos = new Vector2(160, 40);

                        if (bossSpawnTimer >= 6f && !bossSpawned)
                        {
                            nextZoneSize = 0.4f;

                            bossSpawned = true;

                            Mob boss = new(spawnPos, bossID);
                            boss.Spawn();
                        }
                        break;
                    case 11:
                        spawnPos = new Vector2(40, 68);

                        if (bossSpawnTimer >= 6f && !bossSpawned)
                        {
                            nextZoneSize = 0.4f;

                            bossSpawned = true;

                            for (int i = 0; i < 5; i++)
                            {
                                Mob boss = new(spawnPos + new Vector2(i * 64, 0), bossID);

                                boss.Spawn();
                            }
                        }
                        break;
                    case 12:
                        spawnPos = new Vector2(-40, 65);

                        if (bossSpawnTimer >= 6f && !bossSpawned)
                        {

                            bossSpawned = true;

                            Mob boss = new(spawnPos, bossID);
                            boss.Spawn();
                        }
                        break;
                }

                switch (bossID)
                {
                    case 9:
                        if (bgScrollTimer < 4 && !endLevel)
                        {
                            bgScrollTimer += Globals.TotalSeconds;
                        }

                        bgPos.X -= bgScrollTimer / 15;

                        if (bgPos.X < -bg.Width)
                        {
                            bgPos.X = 0;
                        }

                        bgPos2.X -= bgScrollTimer / 10;

                        if (bgPos2.X < -bg2.Width)
                        {
                            bgPos2.X = 0;
                        }

                        if (bossSpawned && MobManager.mobs.Count == 0)
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

                        break;
                    case 11:
                        if (bossSpawned && !endLevel)
                        {
                            ai1 += Globals.TotalSeconds;

                            if (ai1 >= Math.Pow(MobManager.mobs.Count, 2.3f) / 5)
                            {
                                if (ai2 == 0)
                                {
                                    GameManager.AddScreenShake(0.1f, 5f);
                                    SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("bigexplosion"), 0.8f, 0f, 0f);

                                    ai4 = Globals.Random.Next(4);

                                    Vector2 pos = Vector2.Zero;
                                    Vector2 vel = Vector2.Zero;

                                    for (int i = 0; i < 50; i++)
                                    {
                                        if (ai4 == 0)
                                        {
                                            pos = new(Globals.RandomFloat(0, 320), 0);
                                            vel = new(Globals.RandomFloat(-20, 20), Globals.RandomFloat(200, 400));
                                        }
                                        if (ai4 == 1)
                                        {
                                            pos = new(Globals.RandomFloat(0, 320), 180);
                                            vel = new(Globals.RandomFloat(-20, 20), Globals.RandomFloat(-200, -400));
                                        }
                                        if (ai4 == 2)
                                        {
                                            pos = new(0, Globals.RandomFloat(0, 180));
                                            vel = new(Globals.RandomFloat(200, 500), Globals.RandomFloat(-20, -20));

                                        }
                                        if (ai4 == 3)
                                        {
                                            pos = new(320, Globals.RandomFloat(0, 180));
                                            vel = new(Globals.RandomFloat(-200, -500), Globals.RandomFloat(-20, -20));

                                        }

                                        ParticleData pd2 = new()
                                        {
                                            opacityStart = 1f,
                                            opacityEnd = 1f,
                                            sizeStart = 8,
                                            sizeEnd = 0,
                                            colorStart = Color.White,
                                            colorEnd = Color.White,
                                            velocity = vel,
                                            lifespan = Globals.RandomFloat(0.2f, 1f),
                                            rotationSpeed = Globals.RandomFloat(-0.5f, 0.5f),
                                            friendly = false
                                        };

                                        Particle p2 = new(pos, pd2);
                                        ParticleManager.AddParticle(p2);
                                    }


                                    ai2 = 1;
                                }
                            }

                            if (ai2 == 1)
                            {
                                ai3 += Globals.TotalSeconds;

                                if (ai3 >= 1f)
                                {
                                    GameManager.AddScreenShake(0.2f, 10f);
                                    SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("lastingexplosion"), 0.8f, 0f, 0f);

                                    if (ai4 == 0)
                                    {
                                        for (int i = 0; i < 30; i++)
                                        {
                                            Projectile proj = new(new(i * (320 / 30), 0), -2, 1f, new(0, Globals.RandomFloat(80, 90)), 10f);
                                            ProjectileManager.AddProjectile(proj);
                                        }

                                        for (int i = 0; i < 10; i++)
                                        {
                                            Projectile proj = new(new(i * (320 / 10), 0), -6, 1f, new Vector2(0, Globals.RandomFloat(120, 160)).RotatedBy(MathHelper.ToRadians(Globals.RandomFloat(-30, 30))), Globals.RandomFloat(1.7f, 2.3f));
                                            ProjectileManager.AddProjectile(proj);
                                        }
                                    }
                                    if (ai4 == 1)
                                    {
                                        for (int i = 0; i < 30; i++)
                                        {
                                            Projectile proj = new(new(i * (320 / 30), 180), -2, 1f, new(0, -Globals.RandomFloat(80, 90)), 10f);
                                            ProjectileManager.AddProjectile(proj);
                                        }

                                        for (int i = 0; i < 10; i++)
                                        {
                                            Projectile proj = new(new(i * (320 / 10), 180), -6, 1f, new Vector2(0, Globals.RandomFloat(-120, -160)).RotatedBy(MathHelper.ToRadians(Globals.RandomFloat(-30, 30))), Globals.RandomFloat(1.7f, 2.3f));
                                            ProjectileManager.AddProjectile(proj);
                                        }
                                    }
                                    if (ai4 == 2)
                                    {
                                        for (int i = 0; i < 20; i++)
                                        {
                                            Projectile proj = new(new(0, i * (180 / 20)), -2, 1f, new(Globals.RandomFloat(80, 90), 0), 10f);
                                            ProjectileManager.AddProjectile(proj);
                                        }

                                        for (int i = 0; i < 6; i++)
                                        {
                                            Projectile proj = new(new(0, i * (180 / 6)), -6, 1f, new Vector2(Globals.RandomFloat(120, 160), 0).RotatedBy(MathHelper.ToRadians(Globals.RandomFloat(-30, 30))), Globals.RandomFloat(1.7f, 2.3f));
                                            ProjectileManager.AddProjectile(proj);
                                        }
                                    }
                                    if (ai4 == 3)
                                    {
                                        for (int i = 0; i < 20; i++)
                                        {
                                            Projectile proj = new(new(320, i * (180 / 20)), -2, 1f, new(-Globals.RandomFloat(80, 90), 0), 10f);
                                            ProjectileManager.AddProjectile(proj);
                                        }

                                        for (int i = 0; i < 6; i++)
                                        {
                                            Projectile proj = new(new(320, i * (180 / 6)), -6, 1f, new Vector2(Globals.RandomFloat(-120, -160), 0).RotatedBy(MathHelper.ToRadians(Globals.RandomFloat(-30, 30))), Globals.RandomFloat(1.7f, 2.3f));
                                            ProjectileManager.AddProjectile(proj);
                                        }
                                    }


                                    ai1 = 0;
                                    ai2 = 0;
                                    ai3 = 0;
                                    ai4 = 0;
                                }
                            }
                        }


                        if (bgScrollTimer < 4 && !endLevel)
                        {
                            bgScrollTimer += Globals.TotalSeconds;
                        }

                        bgPos.X -= bgScrollTimer / 15;

                        if (bgPos.X < -bg.Width)
                        {
                            bgPos.X = 0;
                        }

                        bgPos2.X -= bgScrollTimer / 10;

                        if (bgPos2.X < -bg2.Width)
                        {
                            bgPos2.X = 0;
                        }

                        if (bossSpawned && MobManager.mobs.Count == 0)
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

                        break;

                    case 12:
                        if (bossSpawned)
                        {
                            if (bgScrollTimer < 4 && !endLevel)
                            {
                                bgScrollTimer += Globals.TotalSeconds;
                            }

                            foreach (Mob boss in MobManager.mobs)
                            {
                                if (boss.ai6 == 3 && boss.ai10 < 120)
                                {
                                    if (bgScrollTimer < 15)
                                    {
                                        bgScrollTimer += Globals.TotalSeconds * 2;
                                    }
                                }

                                else
                                {
                                    if (bgScrollTimer > 4 && !endLevel)
                                    {
                                        bgScrollTimer -= Globals.TotalSeconds * 3;
                                    }
                                }

                                if (boss.ai == 3)
                                {
                                    if (bgScrollTimer < 15)
                                    {
                                        bgScrollTimer += Globals.TotalSeconds * 5;
                                    }
                                }
                            }
                        }

                        bgPos.X -= bgScrollTimer / 15;

                        if (bgPos.X < -bg.Width)
                        {
                            bgPos.X = 0;
                        }

                        bgPos2.X -= bgScrollTimer / 10;

                        if (bgPos2.X < -bg2.Width)
                        {
                            bgPos2.X = 0;
                        }

                        bgPos3.X -= bgScrollTimer / 20;


                        if (bg4 != null)
                        {
                            if (bgPos3.X < -bg4.Width)
                            {
                                bgPos3.X = 0;
                            }
                        }


                        if (bossSpawned && MobManager.mobs.Count == 0)
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

                        break;
                }
            }

            if (nextZoneSize > 0)
            {
                zoneSizeTimer += Globals.TotalSeconds;

                GameManager.zoneSize = Globals.NonLerp(GameManager.zoneSize, nextZoneSize, zoneSizeTimer / 5);

                GameManager.bounds = new(
                    Globals.NonLerp(160 - (GameManager.zoneSize * 320), 160 - (nextZoneSize * 320), zoneSizeTimer / 5),
                    Globals.NonLerp(90 - (GameManager.zoneSize * 180), 90 - (nextZoneSize * 180), zoneSizeTimer / 5),
                    Globals.NonLerp(160 + (GameManager.zoneSize * 320), 160 + (nextZoneSize * 320), zoneSizeTimer / 5),
                    Globals.NonLerp(90 + (GameManager.zoneSize * 180), 90 + (nextZoneSize * 180), zoneSizeTimer / 5));

                if (zoneSizeTimer > 5)
                {
                    nextZoneSize = 0;
                    zoneSizeTimer = 0;
                }
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

            nextZoneSize = (((float)levelCredits / (float)totalLevelCredits) / 4) + 0.3f;
            zoneSizeTimer = 0;
        }

        public static void GetNextSpawnWave(int type)
        {
            spawnWave.Clear();
            spawnWaveCost = 0;

            if (type == 0)
            {
                spawnPos = new(Globals.RandomFloat(GameManager.bounds.X + 35, GameManager.bounds.Z - 35), GameManager.bounds.W - 45);
                    
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
                spawnPos = new(Globals.RandomFloat(GameManager.bounds.X + 35, GameManager.bounds.Z - 35), GameManager.bounds.W - 45);

                Mob wizard = new(spawnPos, 3);
                spawnWave.Add(wizard);
            }

            if (type == 2)
            {
                spawnPos = new(Globals.RandomFloat(GameManager.bounds.X + 35, GameManager.bounds.Z - 35), GameManager.bounds.Y + 35);

                for (int i = 0; i < 5; i++)
                {
                    Mob bat = new(spawnPos + new Vector2(Globals.RandomFloat(-10, 10), Globals.RandomFloat(-10, 10)), 1);

                    spawnWave.Add(bat);
                }
            }

            if (type == 3)
            {
                spawnPos = new(Globals.RandomFloat(GameManager.bounds.X + 35, GameManager.bounds.Z - 35), GameManager.bounds.Y + 35);

                for (int i = 0; i < 2; i++)
                {
                    Mob wisp = new(spawnPos + new Vector2(Globals.RandomFloat(-10, 10), Globals.RandomFloat(-10, 10)), 2);

                    spawnWave.Add(wisp);
                }
            }

            if (type == 4)
            {
                spawnPos = new(Globals.RandomFloat(GameManager.bounds.X + 20, GameManager.bounds.Z - 20), GameManager.bounds.W - 45);

                Mob sorcerer = new(spawnPos, 6);
                spawnWave.Add(sorcerer);
            }

            if (type == 5)
            {
                spawnPos = new(Globals.RandomFloat(GameManager.bounds.X + 20, GameManager.bounds.Z - 20), GameManager.bounds.Y + 25);

                Mob eye = new(spawnPos, 10);
                spawnWave.Add(eye);
            }

            if (type == 6)
            {
                spawnPos = new(Globals.RandomFloat(GameManager.bounds.X + 35, GameManager.bounds.Z - 35), GameManager.bounds.Y + 35);

                Mob bat = new(spawnPos, 1);

                spawnWave.Add(bat);
            }

            if (type == 7)
            {
                spawnPos = new(Globals.RandomFloat(GameManager.bounds.X + 35, GameManager.bounds.Z - 35), GameManager.bounds.W - 35);

                for (int i = 0; i < 3; i++)
                {
                    Mob slime = new(spawnPos + new Vector2(Globals.RandomFloat(-10, 10), Globals.RandomFloat(-10, 10)), 4);

                    spawnWave.Add(slime);
                }
            }

            if (type == 8)
            {
                spawnPos = new(Globals.RandomFloat(GameManager.bounds.X + 35, GameManager.bounds.Z - 35), GameManager.bounds.W - 35);

                Mob sorcerer = new(spawnPos, 6);
                spawnWave.Add(sorcerer);

                for (int i = 0; i < 2; i++)
                {
                    Mob slime = new(spawnPos + new Vector2(Globals.RandomFloat(-10, 10), Globals.RandomFloat(-10, 10)), 4);

                    spawnWave.Add(slime);
                }
            }

            if (type == 9)
            {
                spawnPos = new(Globals.RandomFloat(GameManager.bounds.X + 20, GameManager.bounds.Z - 20), GameManager.bounds.Y + 25);

                Mob eye = new(spawnPos, 10);
                spawnWave.Add(eye);

                for (int i = 0; i < 2; i++)
                {
                    Mob wisp = new(spawnPos + new Vector2(Globals.RandomFloat(-10, 10), Globals.RandomFloat(-10, 10)), 2);

                    spawnWave.Add(wisp);
                }
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

            stageStart = false;

            endLevel = false;
            endLevelTimer = 0;
            bgScrollTimer = 0;

            switch (level)
            {
                case 0:
                    stage = 1;

                    bg = Globals.Content.Load<Texture2D>("bg");
                    bg2 = Globals.Content.Load<Texture2D>("bg2");
                    bg3 = Globals.Content.Load<Texture2D>("stars");
                    song = Globals.Content.Load<Song>("stage3");

                    GameManager.groundBounds = new(0, 90, 320, 180);
                    GameManager.bounds = new(0, 0, 320, 180);

                    levelCredits = 20;
                    validSpawns.Add(1);

                    bossStage = false;
                    GameManager.fog = true;

                    break;
                case 1:
                    stage = 1;

                    bg = Globals.Content.Load<Texture2D>("bg");
                    bg2 = Globals.Content.Load<Texture2D>("bg2");
                    bg3 = Globals.Content.Load<Texture2D>("stars");
                    bg4 = null;

                    song = Globals.Content.Load<Song>("stage3");

                    GameManager.groundBounds = new(0, 90, 320, 180);
                    GameManager.bounds = new(0, 0, 320, 180);

                    levelCredits = 20;
                    //

                    //validSpawns.Add(0);
                    validSpawns.Add(1);
                    validSpawns.Add(2);
                    validSpawns.Add(3);
                    //validSpawns.Add(4);
                    //validSpawns.Add(5);
                    validSpawns.Add(6);
                    validSpawns.Add(7);
                    //validSpawns.Add(8);


                    bossStage = false;
                    GameManager.fog = true;

                    break;
                case 2:
                    bg = Globals.Content.Load<Texture2D>("bg");
                    bg2 = Globals.Content.Load<Texture2D>("bg2");
                    bg3 = Globals.Content.Load<Texture2D>("stars");
                    bg4 = null;

                    song = Globals.Content.Load<Song>("stage3");

                    GameManager.groundBounds = new(0, 90, 320, 180);
                    GameManager.bounds = new(0, 0, 320, 180);

                    levelCredits = 40;
                    //
                    //validSpawns.Add(0);
                    validSpawns.Add(1);
                    validSpawns.Add(2);
                    validSpawns.Add(3);
                    //validSpawns.Add(4);
                    //validSpawns.Add(5);
                    validSpawns.Add(6);
                    validSpawns.Add(7);
                    //validSpawns.Add(8);

                    bossStage = false;
                    GameManager.fog = true;

                    break;
                case 3:
                    bg = Globals.Content.Load<Texture2D>("bg");
                    bg2 = Globals.Content.Load<Texture2D>("bg2");
                    bg3 = Globals.Content.Load<Texture2D>("stars");
                    bg4 = null;

                    song = Globals.Content.Load<Song>("stage3");

                    GameManager.groundBounds = new(0, 90, 320, 180);
                    GameManager.bounds = new(0, 0, 320, 180);

                    levelCredits = 60;
                    //
                    validSpawns.Add(0);
                    validSpawns.Add(1);
                    validSpawns.Add(2);
                    validSpawns.Add(3);
                    //validSpawns.Add(4);
                    //validSpawns.Add(5);
                    validSpawns.Add(6);
                    validSpawns.Add(7);
                    //validSpawns.Add(8);

                    bossStage = false;
                    GameManager.fog = true;

                    break;

                case 4:
                    bossStage = true;
                    bossSpawned = false;

                    bg = Globals.Content.Load<Texture2D>("bg");
                    bg2 = Globals.Content.Load<Texture2D>("bg2");
                    bg3 = Globals.Content.Load<Texture2D>("stars");
                    bg4 = null;

                    song = Globals.Content.Load<Song>("rgrgrg");

                    GameManager.groundBounds = new(0, 90, 320, 180);
                    GameManager.bounds = new(0, 0, 320, 180);

                    validSpawns.Add(9);

                    bossID = validSpawns[Globals.Random.Next(validSpawns.Count)];

                    GameManager.AddBigText(GetBossName(bossID));
                    GameManager.fog = true;

                    break;
                case 5:
                    stage = 2;

                    bg = Globals.Content.Load<Texture2D>("bg3");
                    bg2 = Globals.Content.Load<Texture2D>("bg4");
                    bg3 = Globals.Content.Load<Texture2D>("stars2");
                    bg4 = null;

                    song = Globals.Content.Load<Song>("asasa");

                    GameManager.groundBounds = new(0, 100, 320, 180);
                    GameManager.bounds = new(0, 0, 320, 180);

                    levelCredits = 80;

                    validSpawns.Add(0);
                    validSpawns.Add(1);
                    validSpawns.Add(2);
                    validSpawns.Add(3);
                    validSpawns.Add(4);
                    validSpawns.Add(5);
                    validSpawns.Add(6);
                    validSpawns.Add(7);
                    validSpawns.Add(8);
                    validSpawns.Add(9);

                    bossStage = false;
                    GameManager.fog = true;

                    break;
                case 6:
                    stage = 2;

                    bg = Globals.Content.Load<Texture2D>("bg3");
                    bg2 = Globals.Content.Load<Texture2D>("bg4");
                    bg3 = Globals.Content.Load<Texture2D>("stars2");
                    bg4 = null;

                    song = Globals.Content.Load<Song>("asasa");

                    GameManager.groundBounds = new(0, 100, 320, 180);
                    GameManager.bounds = new(0, 0, 320, 180);

                    levelCredits = 80;

                    validSpawns.Add(0);
                    validSpawns.Add(1);
                    validSpawns.Add(2);
                    validSpawns.Add(3);
                    validSpawns.Add(4);
                    validSpawns.Add(5);
                    validSpawns.Add(6);
                    validSpawns.Add(7);
                    validSpawns.Add(8);
                    validSpawns.Add(9);

                    bossStage = false;
                    GameManager.fog = true;

                    break;
                case 7:
                    stage = 2;

                    bg = Globals.Content.Load<Texture2D>("bg3");
                    bg2 = Globals.Content.Load<Texture2D>("bg4");
                    bg3 = Globals.Content.Load<Texture2D>("stars2");
                    bg4 = null;

                    song = Globals.Content.Load<Song>("asasa");

                    GameManager.groundBounds = new(0, 100, 320, 180);
                    GameManager.bounds = new(0, 0, 320, 180);

                    levelCredits = 80;

                    validSpawns.Add(0);
                    validSpawns.Add(1);
                    validSpawns.Add(2);
                    validSpawns.Add(3);
                    validSpawns.Add(4);
                    validSpawns.Add(5);
                    validSpawns.Add(6);
                    validSpawns.Add(7);
                    validSpawns.Add(8);
                    validSpawns.Add(9);

                    bossStage = false;
                    GameManager.fog = true;

                    break;
                case 8:
                    stage = 2;

                    bg = Globals.Content.Load<Texture2D>("wall2");
                    bg2 = Globals.Content.Load<Texture2D>("wall");
                    bg3 = Globals.Content.Load<Texture2D>("stars2");
                    bg4 = null;

                    song = Globals.Content.Load<Song>("rgrgrg");

                    GameManager.groundBounds = new(0, 100, 320, 180);
                    GameManager.bounds = new(0, 0, 320, 180);

                    validSpawns.Add(11);

                    bossID = validSpawns[Globals.Random.Next(validSpawns.Count)];

                    GameManager.AddBigText(GetBossName(bossID));

                    bossStage = true;
                    GameManager.fog = true;

                    break;

                case 9:
                    stage = 3;

                    bg = Globals.Content.Load<Texture2D>("bg6");
                    bg2 = Globals.Content.Load<Texture2D>("bg5");
                    bg3 = Globals.Content.Load<Texture2D>("stars3");
                    song = Globals.Content.Load<Song>("asasa");
                    bg4 = Globals.Content.Load<Texture2D>("bg7");


                    GameManager.groundBounds = new(0, 100, 320, 180);
                    GameManager.bounds = new(0, 0, 320, 180);

                    levelCredits = 80;

                    validSpawns.Add(0);
                    validSpawns.Add(1);
                    validSpawns.Add(2);
                    validSpawns.Add(3);
                    validSpawns.Add(4);
                    validSpawns.Add(5);
                    validSpawns.Add(6);
                    validSpawns.Add(7);
                    validSpawns.Add(8);
                    validSpawns.Add(9);

                    bossStage = false;
                    GameManager.fog = true;

                    break;
                case 10:
                    stage = 3;

                    bg = Globals.Content.Load<Texture2D>("bg6");
                    bg2 = Globals.Content.Load<Texture2D>("bg5");
                    bg3 = Globals.Content.Load<Texture2D>("stars3");
                    song = Globals.Content.Load<Song>("asasa");
                    bg4 = Globals.Content.Load<Texture2D>("bg7");


                    GameManager.groundBounds = new(0, 100, 320, 180);
                    GameManager.bounds = new(0, 0, 320, 180);

                    levelCredits = 80;

                    validSpawns.Add(0);
                    validSpawns.Add(1);
                    validSpawns.Add(2);
                    validSpawns.Add(3);
                    validSpawns.Add(4);
                    validSpawns.Add(5);
                    validSpawns.Add(6);
                    validSpawns.Add(7);
                    validSpawns.Add(8);
                    validSpawns.Add(9);

                    bossStage = false;
                    GameManager.fog = true;

                    break;
                case 11:
                    stage = 3;

                    bg = Globals.Content.Load<Texture2D>("bg6");
                    bg2 = Globals.Content.Load<Texture2D>("bg5");
                    bg3 = Globals.Content.Load<Texture2D>("stars3");
                    bg4 = Globals.Content.Load<Texture2D>("bg7");

                    song = Globals.Content.Load<Song>("asasa");

                    GameManager.groundBounds = new(0, 100, 320, 180);
                    GameManager.bounds = new(0, 0, 320, 180);

                    levelCredits = 80;

                    validSpawns.Add(0);
                    validSpawns.Add(1);
                    validSpawns.Add(2);
                    validSpawns.Add(3);
                    validSpawns.Add(4);
                    validSpawns.Add(5);
                    validSpawns.Add(6);
                    validSpawns.Add(7);
                    validSpawns.Add(8);
                    validSpawns.Add(9);

                    bossStage = false;
                    GameManager.fog = true;

                    break;
                case 12:
                    stage = 3;

                    bg = Globals.Content.Load<Texture2D>("bg6");
                    bg2 = Globals.Content.Load<Texture2D>("bg5");
                    bg3 = Globals.Content.Load<Texture2D>("stars3");
                    bg4 = Globals.Content.Load<Texture2D>("bg7");

                    song = Globals.Content.Load<Song>("aewfgeg");

                    GameManager.groundBounds = new(0, 100, 320, 180);
                    GameManager.bounds = new(0, 0, 320, 180);

                    validSpawns.Add(12);

                    bossID = validSpawns[Globals.Random.Next(validSpawns.Count)];

                    GameManager.AddBigText(GetBossName(bossID));

                    bossStage = true;
                    GameManager.fog = true;

                    break;
            }

            if (!bossStage)
            {
                GetNextSpawnWave(validSpawns[Globals.Random.Next(validSpawns.Count)]);
            }

            totalLevelCredits = levelCredits;
        }

        public static List<string> GetBossName(int id)
        {
            List<string> words = [];

            switch (id)
            {
                case 9:
                    words.Add("empyrean");
                    words.Add("wisp");
                    break;
                case 11:
                    words.Add("wall");
                    words.Add("of");
                    words.Add("magick");

                    break;
                case 12:
                    words.Add("enchanted");
                    words.Add("beasthound");
                    break;
            }

            return words;
        }
    }
}
