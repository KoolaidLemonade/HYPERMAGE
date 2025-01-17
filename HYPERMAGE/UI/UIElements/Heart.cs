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

        public override void Draw()
        {
            for (int i = 0; i < GameManager.GetPlayer().health; i++)
            {
                Globals.SpriteBatch.Draw(texture, new Vector2(position.X + 7 * i, position.Y), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.93f);
            }

            Globals.SpriteBatch.DrawString(Globals.GetPixelFont(), $"{GameManager.GetPlayer().lives} x", new(position.X - 10, position.Y - 2), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.93f);
        }
    }
}
