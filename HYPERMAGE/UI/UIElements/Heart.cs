using HYPERMAGE.Helpers;
using HYPERMAGE.Managers;
using HYPERMAGE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HYPERMAGE.UI.UIElements
{
    public class Heart : UIElement
    {
        public Heart(Texture2D texture, Vector2 position) : base(texture, position)
        {

        }

        public override void Draw(Player player, SpriteFont spriteFont)
        {
            for (int i = 0; i < player.health; i++)
            {
                Globals.SpriteBatch.Draw(_texture, new Vector2(position.X + 7 * i, position.Y), Color.White);
            }

            Globals.SpriteBatch.DrawString(spriteFont, $"{player.lives} x", new(122, 13), Color.White);
        }
    }
}
