using HYPERMAGE.Helpers;
using HYPERMAGE.Managers;
using HYPERMAGE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace HYPERMAGE.UI.UIElements
{
    public class BossBar : UIElement
    {
        public static List<Mob> mobs = [];

        public int length = 200;

        public float health;
        public static float maxHealth;
        public BossBar(Texture2D texture, Vector2 position, Mob mob) : base(texture, position)
        {
            mobs.Add(mob);    
            maxHealth += mob.health;

            foreach (UIElement bar in UIManager.uiList.ToList())
            {
                if (bar is BossBar && bar != this)
                {
                    UIManager.RemoveElement(this);
                }
            }
        }

        public override void Update()
        {
            health = 0f;

            foreach (Mob mob in mobs.ToList())
            {
                if (mob.health <= 0)
                {
                    mobs.Remove(mob);
                }

                else
                {
                    health += mob.health;
                }
            }

            if (health <= 0)
            {
                UIManager.RemoveElement(this);
                mobs.Clear();
                maxHealth = 0f;
            }
        }
        public override void Draw()
        {
            Globals.SpriteBatch.Draw(texture, position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.93f);
            Globals.SpriteBatch.Draw(texture, position + new Vector2(length + 5, 0), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.FlipHorizontally, 0.93f);

            Globals.SpriteBatch.Draw(Globals.GetBlankTexture(), new Rectangle((int)(position.X + 4), (int)(position.Y + 1), length, 3), null, Color.Red, 0f, Vector2.Zero, SpriteEffects.None, 0.92f);
            Globals.SpriteBatch.Draw(Globals.GetBlankTexture(), new Rectangle((int)(position.X + 4), (int)(position.Y + 1), (int)MathHelper.Lerp(0, length, health / maxHealth), 3), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.93f);
        }
    }
}
