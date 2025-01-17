using HYPERMAGE.Helpers;
using HYPERMAGE.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HYPERMAGE.UI.UIElements
{
    public class XP : UIElement
    {
        private bool maxLevel;

        public XP(Vector2 position) : base(position) 
        {
            
        }

        public override void Update()
        {
            if (GameManager.GetPlayer().level >= 10)
            {
                maxLevel = true;
            }

            base.Update();
        }
        public override void Draw()
        {
            if (!maxLevel)
            {
                Globals.SpriteBatch.DrawString(Globals.GetPixelFont(), "LVL " + GameManager.GetPlayer().level + "  :  " + GameManager.GetPlayer().xp.ToString() + " / " + GameManager.GetPlayer().xpToLevel, position, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.93f);

            }

            else
            {
                Globals.SpriteBatch.DrawString(Globals.GetPixelFont(), "LVL " + GameManager.GetPlayer().level, position, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.93f);
            }
        }
    }
}
