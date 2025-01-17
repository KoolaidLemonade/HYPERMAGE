using HYPERMAGE.Helpers;
using HYPERMAGE.Managers;
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

        public override void Draw()
        {
            Globals.SpriteBatch.DrawString(Globals.GetPixelFont(), $"{GameManager.GetPlayer().mana}", position, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.93f);

            Globals.SpriteBatch.Draw(texture, new(position.X + Globals.GetPixelFont().MeasureString(new string(GameManager.GetPlayer().mana.ToString())).X + 1, position.Y + 1) , null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.93f);
        }
    }
}
