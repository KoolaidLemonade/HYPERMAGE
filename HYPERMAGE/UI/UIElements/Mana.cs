using HYPERMAGE.Helpers;
using HYPERMAGE.Models;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HYPERMAGE.UI.UIElements
{
    public class Mana : UIElement
    {
        public Mana(Texture2D texture, Vector2 position) : base(texture, position)
        {

        }

        public override void Draw(Player player, SpriteFont spriteFont)
        {
            Globals.SpriteBatch.DrawString(spriteFont, $"{player.mana}", new(180 - new string($"{player.mana}").Length * 4, 13), Color.White);

            base.Draw(player, spriteFont);
        }
    }
}
