using HYPERMAGE.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HYPERMAGE.Models;

namespace HYPERMAGE.Managers
{
    public static class LevelManager
    {
        public static int stage = 0;
        public static int level = 0;

        public static void Update()
        {
            if (Globals.Random.Next(300) == 0)
            {
                MobManager.SpawnMob(1, new(160, 90));
            }
        }
    }
}
