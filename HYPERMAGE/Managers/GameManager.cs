using HYPERMAGE.Helpers;
using HYPERMAGE.Models;
using HYPERMAGE.Particles;
using HYPERMAGE.Spells;
using HYPERMAGE.UI;
using HYPERMAGE.UI.UIElements;
using System.Net.Mime;
using static System.Net.Mime.MediaTypeNames;

namespace HYPERMAGE.Managers
{
    public static class GameManager
    {
        private static Player _player;
        private static UIElement uiBorder;
        private static Heart heart;
        private static Mana mana;
        private static SpriteFont spriteFont;
        private static Mob bat;
        private static Button button;

        private static int hitstop = 0;
        public static void Init()
        {

            spriteFont = Globals.Content.Load<SpriteFont>("font");

            _player = new(new(151, 90));

            uiBorder = new(Globals.Content.Load<Texture2D>("ui1"), new(0, 0));
            heart = new(Globals.Content.Load<Texture2D>("heart"), new(133, 15));
            mana = new(Globals.Content.Load<Texture2D>("mana"), new(181, 14));
            button = new(Globals.GetBlankTexture(), new(100, 100), spriteFont, new(32, 9), "CHUNGUS", Color.White);
            UIManager.AddElement(button);
            UIManager.AddElement(uiBorder);
            UIManager.AddElement(heart);
            UIManager.AddElement(mana);

            Texture2D texture1 = Globals.Content.Load<Texture2D>("bat");

            /*Spell firebolt = new Spell(1, 1, 12, 1, 1, 1, 30);
            Spellbook.AddSpellPrimary(firebolt);
            Spellbook.AddSpellSecondary(firebolt);

            Spell fireball = new Spell(2, 1, 13, 1, 1, 1, 40);
            Spellbook.AddSpellPrimary(fireball);
            Spellbook.AddSpellSecondary(fireball);

            Spell kindle = new Spell(3, 1, 23, 1, 1, 1, 20);
            Spellbook.AddSpellPrimary(kindle);
            Spellbook.AddSpellSecondary(kindle);*/

            Spell bladeofflame = new Spell(4, 1, 19, 1, 1, 1, 20);
            Spellbook.AddSpellPrimary(bladeofflame);
            Spellbook.AddSpellSecondary(bladeofflame);

            for (int i = 0; i < 10; i++)
            {
                bat = new Mob(texture1, new Animation(texture1, 2, 1, 0.1f), new(i * 50, 50), 1, 1f, 30f);
                MobManager.AddMob(bat);
            }
        }

        public static void Update()
        {
            hitstop--;

            if (hitstop <= 0)
            {
                _player.Update();
                ProjectileManager.Update(_player);
                MobManager.Update(_player);
            }

            Spellbook.Update();

            InputManager.Update();

            ParticleManager.Update();

            UIManager.Update();
        }

        public static void Draw()
        {
            _player.Draw();

            ParticleManager.Draw();

            MobManager.Draw();

            ProjectileManager.Draw();

            UIManager.Draw(_player, spriteFont);
        }

        public static void AddHitstop(int hitstopFrames)
        {
            hitstop = hitstopFrames;
        }
    }
}
