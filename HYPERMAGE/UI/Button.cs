using HYPERMAGE.Helpers;
using HYPERMAGE.Managers;
using HYPERMAGE.Models;
using HYPERMAGE.Particles;
using HYPERMAGE.UI.UIElements;
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
        public SpriteFont font;
        public Color color;

        private string text;

        public bool mouseHovering;

        public Rectangle buttonHitbox;
        public Button(Vector2 position, SpriteFont font, string text, Color color) : base(position)
        {
            this.font = font;
            this.text = text;
            this.color = color;
        }
        public Button(Texture2D texture, Vector2 position) : base(texture, position)
        {
            this.color = Color.White;
            this.texture = texture;
        }

        public virtual void Clicked()
        {
            InputManager.buttonClicked = true;
        }

        public virtual void UpdateButtonHitbox()
        {
            if (text != null)
            {
                buttonHitbox = new Rectangle((int)position.X - 1, (int)position.Y, (int)font.MeasureString(text).X + 1, (int)font.MeasureString(text).Y);

            }

            if (texture != null)
            {
                buttonHitbox = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);

            }
        }

        public override void Update()
        {
            UpdateButtonHitbox();

            if (text != null)
            {
                if (buttonHitbox.Contains(InputManager.MousePosition))
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
            }

            if (texture != null)
            {
                if (buttonHitbox.Contains(InputManager.MousePosition))
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
            }

            base.Update();
        }

        public override void Draw()
        {
            if (text != null)
            {
                if (mouseHovering)
                {
                    Globals.SpriteBatch.Draw(Globals.GetBlankTexture(), new Rectangle((int)position.X - 1, (int)position.Y, (int)font.MeasureString(text).X + 1, (int)font.MeasureString(text).Y), Color.White * 0.5f);
                }

                Globals.SpriteBatch.DrawString(font, text, position, color);
            }

            if (texture != null)
            {
                if (mouseHovering)
                {
                    Globals.SpriteBatch.Draw(Globals.GetBlankTexture(), new Rectangle((int)position.X - 1, (int)position.Y - 1, (int)texture.Width + 2, (int)texture.Height + 2), Color.White * 0.5f);
                }

                Globals.SpriteBatch.Draw(texture, position, color);
            }
        }
    }
}
