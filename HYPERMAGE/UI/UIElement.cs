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
        public Texture2D _texture;
        public Vector2 position;

        public UIElement(Texture2D texture, Vector2 position)
        {
            _texture = texture;
            this.position = position;
        }
        public UIElement(Vector2 position)
        {
            this.position = position;
        }
        public virtual void Update()
        {

        }

        public virtual void Draw(Player player, SpriteFont spriteFont)
        {
            Globals.SpriteBatch.Draw(_texture, position, Color.White);
        }
    }
}
