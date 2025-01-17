using HYPERMAGE.Helpers;
using HYPERMAGE.Models;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace HYPERMAGE.UI
{
    public class TextBox : UIElement
    {
        public List<string> text;

        public int longestString = 0;

        private float layerDepth;
        public TextBox(float layerDepth, SpriteFont spriteFont, string text, Vector2 position, int maxWidth) : base(position)
        {
            this.layerDepth = layerDepth;

            this.text = Globals.LineBreakText(text, spriteFont, maxWidth, false);

            for (int i = 0; i < this.text.Count; i++)
            {
                if (Globals.GetPixelFont().MeasureString(this.text[i]).X > longestString)
                {
                    longestString = (int)Globals.GetPixelFont().MeasureString(this.text[i]).X;
                }
            }
        }
        public override void Update()
        {
            base.Update();
        }
        public override void Draw()
        {
            Globals.SpriteBatch.Draw(Globals.GetBlankTexture(), new Rectangle((int)position.X - 3, (int)(position.Y - 2), longestString + 5, 4 + text.Count * 8), null, Color.Black * 0.7f, 0f, Vector2.Zero, SpriteEffects.None, layerDepth - 0.0001f);

            for (int i = 0; i < text.Count; i++)
            {
                Globals.SpriteBatch.DrawString(Globals.GetPixelFont(), text[i], new(position.X, position.Y + (8 * i)), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, layerDepth);
                
                Globals.SpriteBatch.Draw(Globals.GetBlankTexture(), new Rectangle((int)position.X - 4, (int)(position.Y - 1 + (8 * i)), 1, (i == text.Count - 1 ? 7 : 8) + 4), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, layerDepth);

                Globals.SpriteBatch.Draw(Globals.GetBlankTexture(), new Rectangle((int)position.X + longestString + 2, (int)(position.Y - 1 + (8 * i)), 1, (i == text.Count - 1 ? 7 : 8) + 4), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, layerDepth);
            }

            Globals.SpriteBatch.Draw(Globals.GetBlankTexture(), new Rectangle((int)position.X - 3, (int)(position.Y - 2), longestString + 5, 1), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, layerDepth);

            Globals.SpriteBatch.Draw(Globals.GetBlankTexture(), new Rectangle((int)position.X - 3, (int)(position.Y - 2 + 4 + (text.Count * 8)), longestString + 5, 1), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, layerDepth);
        }

        public void AddText(string text, int maxWidth)
        {
            List <string> newText = Globals.LineBreakText(text, Globals.GetPixelFont(), maxWidth, true);

            for (int i = 0; i < newText.Count; i++)
            {
                this.text.Add(newText[i]);
            }

            for (int i = 0; i < this.text.Count; i++)
            {
                if (Globals.GetPixelFont().MeasureString(this.text[i]).X > longestString)
                {
                    longestString = (int)Globals.GetPixelFont().MeasureString(this.text[i]).X;
                }
            }
        }
    }
}
