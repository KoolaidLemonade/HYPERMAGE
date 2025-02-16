using HYPERMAGE.Helpers;
using HYPERMAGE.Managers;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HYPERMAGE.UI.UIElements
{
    public class PlayButton : Button
    {
        private bool transition = false;
        public PlayButton(Vector2 position, SpriteFont font, String text, Color color) : base(position, font, text, color)
        {

        }
        public override void Clicked()
        {
            if (!transition)
            {
                SoundManager.ClearSong();
                SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("select"), 0.5f, 0f, 0f);

                transition = true;
                GameManager.TransitionScene(new StageTransition());
            }

            base.Clicked();
        }
    }
}
