using HYPERMAGE.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HYPERMAGE.UI.UIElements
{
    public class ExitButton : Button
    {
        public ExitButton(Vector2 position, SpriteFont font, String text, Color color) : base(position, font, text, color)
        {

        }
        public override void Clicked()
        {
            if (!GameManager.fadeout)
            {
                GameManager.exit = true;
            }
        }
    }
}
