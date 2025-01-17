using HYPERMAGE.Helpers;
using HYPERMAGE.Managers;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HYPERMAGE.UI.UIElements
{
    public class BuyXPButton : Button
    {
        private bool active = true; 

        private string text;
        public BuyXPButton(Vector2 position, Texture2D texture) : base(texture, position)
        {
            text = "BUY XP : " + ShopManager.xpCost;
        }

        public override void Draw()
        {
            Globals.SpriteBatch.Draw(Globals.Content.Load<Texture2D>("mana"), new Vector2(position.X - 7, position.Y + 1), null, active ? color : color * 0.5f, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.93f);
            Globals.SpriteBatch.DrawString(Globals.GetPixelFont(), text, new(position.X - Globals.GetPixelFont().MeasureString(text).X - 8, position.Y), active ? color : color * 0.5f, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.93f);

            if (mouseHovering && active)
            {
                Globals.SpriteBatch.Draw(Globals.GetBlankTexture(), new Rectangle((int)((int)position.X - 9 - Globals.GetPixelFont().MeasureString(text).X), (int)position.Y - 1, (int)Globals.GetPixelFont().MeasureString(text).X + 18, (int)Globals.GetPixelFont().MeasureString(text).Y + 1), null, Color.White * 0.5f, 0f, Vector2.Zero, SpriteEffects.None, 0.93f);
            }

            Globals.SpriteBatch.Draw(texture, position, null, active ? color : color * 0.5f, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.93f);
        }

        public override void Update()
        {
            if (GameManager.GetPlayer().level >= 10)
            {
                active = false;
            }

            base.Update();
        }
        public override void UpdateButtonHitbox()
        {
            buttonHitbox = new Rectangle((int)(position.X - Globals.GetPixelFont().MeasureString(text).X - 8), (int)position.Y, (int)Globals.GetPixelFont().MeasureString(text).X + 16, 8);
        }
        public override void Clicked()
        {
            if (active)
            {
                if (GameManager.GetPlayer().mana >= ShopManager.xpCost)
                {
                    GameManager.GetPlayer().mana -= ShopManager.xpCost;

                    GameManager.GetPlayer().AddXP(ShopManager.xpCost);
                }

                base.Clicked();
            }
        }
    }
}
