using HYPERMAGE.Helpers;
using HYPERMAGE.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace HYPERMAGE.UI.UIElements
{
    public class RerollButton : Button
    {
        public RerollButton(Vector2 position, Texture2D texture) : base(texture, position)
        {
        }

        public override void Draw()
        {
            Globals.SpriteBatch.Draw(Globals.Content.Load<Texture2D>("mana"), new Vector2(position.X - 7, position.Y + 1), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.93f);
            Globals.SpriteBatch.DrawString(Globals.GetPixelFont(), ShopManager.rerollCost.ToString(), new(position.X - Globals.GetPixelFont().MeasureString(ShopManager.rerollCost.ToString()).X - 8, position.Y), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.93f);
           
            if (mouseHovering)
            {
                Globals.SpriteBatch.Draw(Globals.GetBlankTexture(), new Rectangle((int)((int)position.X - 9 - Globals.GetPixelFont().MeasureString(ShopManager.rerollCost.ToString()).X), (int)position.Y, (int)Globals.GetPixelFont().MeasureString(ShopManager.rerollCost.ToString()).X + 20, (int)Globals.GetPixelFont().MeasureString(ShopManager.rerollCost.ToString()).Y + 1), null, Color.White * 0.5f, 0f, Vector2.Zero, SpriteEffects.None, 0.93f);
            }

            Globals.SpriteBatch.Draw(texture, position, null, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.93f);
        }

        public override void UpdateButtonHitbox()
        {
            buttonHitbox = new Rectangle((int)(position.X - Globals.GetPixelFont().MeasureString(ShopManager.rerollCost.ToString()).X - 8), (int)position.Y, (int)Globals.GetPixelFont().MeasureString(ShopManager.rerollCost.ToString()).X + 18, 8);
        }
        public override void Clicked()
        {
            if (GameManager.GetPlayer().mana >= ShopManager.rerollCost)
            {
                GameManager.GetPlayer().mana -= ShopManager.rerollCost;

                ShopManager.Reroll();
            }

            base.Clicked();
        }
    }
}
