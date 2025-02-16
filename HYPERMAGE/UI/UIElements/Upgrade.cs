using HYPERMAGE.Helpers;
using HYPERMAGE.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HYPERMAGE.UI.UIElements
{
    public class Upgrade : Button
    {
        public int id;
        public Upgrade(Vector2 position, Texture2D texture) : base(texture, position)
        {
            this.texture = texture;
            this.position = position;

            id = Globals.Random.Next(UpgradeManager.totalUpgrades + 1);
        }


    }
}
