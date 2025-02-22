using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HYPERMAGE.Managers
{
    public static class UpgradeManager
    {
        public static int totalUpgrades = 10;
        public static List<int> usedUpgrades = [];
        public static string GetUpgradeName(int id)
        {
            switch (id)
            {
                case 1:
                    return "FIRE MASTERY";
                case 2:
                    return "PROTECTION";
                case 3:
                    return "MANA SURGE";
                case 4:
                    return "FOCUS";
                case 5:
                    return "DESTRUCTION";
                case 6:
                    return "ARMAMENT MASTERY";
                case 7:
                    return "INCANTATION MASTERY";
                case 8:
                    return "MISSILE MASTERY";
                case 9:
                    return "BOLT MASTERY";
                case 10:
                    return "LIFE";
            }

            return "";
        }
        public static string GetUpgradeDescription(int id)
        {
            switch (id)
            {
                case 1:
                    return "INCREASES PYROMANCY DAMAGE BY 20%";
                case 2:
                    return "GRANTS ONE EXTRA PERMANENT MAX HEALTH";
                case 3:
                    return "GRANTS TEN MANA, ONCE";
                case 4:
                    return "INCREASES INVULNERABILITY TIME";
                case 5:
                    return "INCREASES DAMAGE BY 10%";
                case 6:
                    return "INCREASES ARMAMENT DAMAGE BY 20%";
                case 7:
                    return "INCREASES INCANTATION DAMAGE BY 20%";
                case 8:
                    return "INCREASES MISSILE DAMAGE BY 20%";
                case 9:
                    return "INCREASES BOLT DAMAGE BY 20%";
                case 10:
                    return "GRANTS ONE EXTRA LIFE";
            }

            return "";
        }
        public static void AddUpgrade(int id)
        {
            usedUpgrades.Add(id);

            switch (id)
            {
                case 1:
                    return;
                case 2:
                    GameManager.GetPlayer().maxHealth++;
                    return;
                case 3:
                    GameManager.GetPlayer().mana += 10;
                    return;
                case 4:
                    GameManager.GetPlayer().immunityTime += 1;
                    return;
                case 5:
                    return;
                case 6:
                    return;
                case 7:
                    return;
                case 8:
                    return;
                case 9:
                    return;
                case 10:
                    GameManager.GetPlayer().lives++;
                    return;
                case 11:
                    return;
                case 12:
                    return;
                case 13:
                    return;
                case 14:
                    return;
                case 15:
                    return;
                case 16:
                    return;
                case 17:
                    return;
                case 18:
                    return;
                case 19:
                    return;
                case 20:
                    return;
                case 21:
                    return;
                case 22:
                    return;
            }
        }
    }
}
