using HYPERMAGE.Helpers;
using HYPERMAGE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HYPERMAGE.UI
{
    public class UIElement
    {
        public Texture2D texture;
        public Vector2 position;

        public UIElement(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            this.position = position;
        }
        public UIElement(Vector2 position)
        {
            this.position = position;
        }
        public virtual void Update()
        {

        }

        public virtual void Draw()
        {
            Globals.SpriteBatch.Draw(texture, position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.93f);
        }
    }
}
