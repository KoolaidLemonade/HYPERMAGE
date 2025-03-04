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
        Mob mob;
        public int length = 200;

        public float health;
        public float maxHealth;
        public BossBar(Texture2D texture, Vector2 position, Mob mob) : base(texture, position)
        {
            this.mob = mob;

            health = mob.health;
            maxHealth = mob.health;
        }

        public override void Update()
        {
            health = mob.health;

            if (health <= 0)
            {
                UIManager.RemoveElement(this);
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
