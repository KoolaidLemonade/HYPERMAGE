using HYPERMAGE.Helpers;
using HYPERMAGE.Managers;
using HYPERMAGE.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HYPERMAGE.UI
{
    public class Button : UIElement
    {
        private SpriteFont font;
        private Vector2 size;
        private Color color; 

        private string text;

        private bool mouseHovering;
        public Button(Texture2D texture, Vector2 position, SpriteFont font, Vector2 size, string text, Color color) : base(texture, position)
        {
            this.font = font;
            this.size = size;
            this.text = text;
            this.color = color;
        }

        public override void Update()
        {
            if (new Rectangle((int)position.X - 2, (int)position.Y, (int)size.X, (int)size.Y).Contains(InputManager.MousePosition))
            {
                mouseHovering = true;

                if (InputManager.Clicked)
                {
                    Clicked();
                }
            }

            else
            {
                mouseHovering = false;
            }

            base.Update();
        }

        public override void Draw(Player player, SpriteFont spriteFont)
        {
            if (mouseHovering)
            {
                Globals.SpriteBatch.Draw(Globals.GetBlankTexture(), new Rectangle((int)position.X-2, (int)position.Y, (int)size.X, (int)size.Y), Color.White * 0.5f);
            }

            Globals.SpriteBatch.DrawString(font, text, position, color);
        }

        public virtual void Clicked()
        {
            Debug.WriteLine("I'm Clicked!");
        }
    }
}
