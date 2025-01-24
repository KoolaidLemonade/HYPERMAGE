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

        public static void Update()
        {
            creditsTimer += Globals.TotalSeconds;

            if (creditsTimer >= 1 && levelCredits > 0)
            {
                creditsTimer = 0;

                levelCredits--;
                credits++; 
            }

            if (Globals.Random.Next(200) == 0 && credits >= spawnWaveCost)
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

            switch (level)
            {
                case 1:
                    levelCredits = 20;
                    //
                    mobTypes.Add(1);
                    mobTypes.Add(2);
                    return;
                case 2:
                    levelCredits = 30;
                    //
                    mobTypes.Add(1);
                    mobTypes.Add(2);
                    return;
                case 3:
                    levelCredits = 40;
                    //
                    mobTypes.Add(1);
                    mobTypes.Add(2);
                    return;
            }
        }
    }
}
