using HYPERMAGE.Helpers;
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

namespace HYPERMAGE.Managers
{
    public static class LevelManager
    {
        public static int level = 0;

        public static List<int> mobTypes = [];
        public static List<Mob> spawnWave = [];
        public static int spawnWaveCost;

        public static int levelCredits;
        public static int credits;
        private static float creditsTimer;

        private static bool endCred;

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
                SoundManager.PlaySong(song, 1f);
            }

            if (bgScrollTimer < 3)
            {
                bgScrollTimer += Globals.TotalSeconds;
            }

            bgPos.X -= bgScrollTimer / 8;

            if (bgPos.X < -bg.Width)
            {
                bgPos.X = 0;
            }

            bgPos2.X -= bgScrollTimer / 4;

            if (bgPos2.X < -bg2.Width)
            {
                bgPos2.X = 0;
            }

            creditsTimer += Globals.TotalSeconds;

            if (creditsTimer >= 1 && levelCredits > 0)
            {
                creditsTimer = 0;


                if (MobManager.mobs.Count == 0)
                {
                    levelCredits -= 2;
                    credits += 2;
                }

                else
                {
                    levelCredits--;
                    credits++;
                }
            }

            if (MobManager.mobs.Count == 0 ? Globals.Random.Next(100) == 0 && credits >= spawnWaveCost : Globals.Random.Next(200) == 0 && credits >= spawnWaveCost)
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
                GameManager.TransitionScene(new Shop());
            }
        }

        public static void SpawnNextWave()
        {
            ParticleData particleData = new()
            {
                texture = Globals.Content.Load<Texture2D>("spawnindicator"),
                sizeStart = 1f,
                sizeEnd = 1.75f,
                flashing = true,
                colorStart = Color.Red,
                colorEnd = Color.Red,
            };

            Particle spawnIndicator = new(spawnPos, particleData);
            ParticleManager.AddParticle(spawnIndicator);

            foreach (Mob mob in spawnWave)
            {
                credits -= mob.spawnCost;
                mob.Spawn();
            }

            GetNextSpawnWave();
        }

        public static void GetNextSpawnWave()
        {
            int targetCredits = Globals.Random.Next(10) + 2;

            spawnPos = new(Globals.RandomFloat(GameManager.bounds.X + 30, GameManager.bounds.Z - 30), Globals.RandomFloat(GameManager.bounds.Y + 30, GameManager.bounds.W - 30));

            spawnWave.Clear();

            while (targetCredits > 0)
            {
                Mob nextMob = new(new(spawnPos.X + Globals.RandomFloat(-20, 20), spawnPos.Y + Globals.RandomFloat(-20, 20)), mobTypes.ElementAt(Globals.Random.Next(0, mobTypes.Count)));

                if (nextMob.spawnCost <= targetCredits)
                {
                    spawnWave.Add(nextMob);
                    targetCredits -= nextMob.spawnCost;
                }
            }

            spawnWaveCost = 0;

            foreach (Mob mob in spawnWave)
            {
                spawnWaveCost += mob.spawnCost;
            }
        }

        public static void NextLevel()
        {
            mobTypes.Clear();
            endCred = false;
            level++;

            bgScrollTimer = 0;

            switch (level)
            {
                case 1:
                    bg = Globals.Content.Load<Texture2D>("bg");
                    bg2 = Globals.Content.Load<Texture2D>("bg2");
                    bg3 = Globals.Content.Load<Texture2D>("stars");
                    song = Globals.Content.Load<Song>("stage3");

                    levelCredits = 20;
                    //
                    mobTypes.Add(1);
                    mobTypes.Add(2);
                    return;
                case 2:
                    bg = Globals.Content.Load<Texture2D>("bg");
                    bg2 = Globals.Content.Load<Texture2D>("bg2");
                    bg3 = Globals.Content.Load<Texture2D>("stars");
                    song = Globals.Content.Load<Song>("stage3");

                    levelCredits = 30;
                    //
                    mobTypes.Add(1);
                    mobTypes.Add(2);
                    return;
                case 3:
                    bg = Globals.Content.Load<Texture2D>("bg");
                    bg2 = Globals.Content.Load<Texture2D>("bg2");
                    bg3 = Globals.Content.Load<Texture2D>("stars");
                    song = Globals.Content.Load<Song>("stage3");

                    levelCredits = 40;
                    //
                    mobTypes.Add(1);
                    mobTypes.Add(2);
                    return;
            }
        }
    }
}
