using HYPERMAGE.Helpers;
using HYPERMAGE.Managers;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HYPERMAGE.UI.UIElements
{
    public class LockButton : Button
    {
        public LockButton(Vector2 position, Texture2D texture) : base(texture, position)
        {
        }

        public override void Draw()
        {
            if (mouseHovering)
            {
                Globals.SpriteBatch.Draw(Globals.GetBlankTexture(), new Rectangle((int)position.X - 1, (int)position.Y - 1, (int)texture.Width / 2 + 2, (int)texture.Height + 2), null, Color.White * 0.5f, 0f, Vector2.Zero, SpriteEffects.None, 0.93f);
            }

            Globals.SpriteBatch.Draw(texture, new Vector2((int)position.X, (int)position.Y), new Rectangle(ShopManager.locked ? 0 : 7, 0, 7, 8), color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.93f);
        }

        public override void UpdateButtonHitbox()
        {
            buttonHitbox = new Rectangle((int)position.X, (int)position.Y, texture.Width / 2, texture.Height);
        }

        public override void Clicked()
        {
            SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("hit"), 0.5f, 1f, 0f);

            if (ShopManager.locked)
            {
                ShopManager.locked = false;
            }

            else
            {
                ShopManager.locked = true;
            }


            base.Clicked();
        }
    }
}
