using HYPERMAGE.Helpers;
using HYPERMAGE.Models;
using HYPERMAGE.Particles;
using HYPERMAGE.Spells;
using HYPERMAGE.UI;
using HYPERMAGE.UI.UIElements;
using Microsoft.Xna.Framework.Audio;
using System.Diagnostics;
using System.Net.Mime;
using static System.Formats.Asn1.AsnWriter;
using static System.Net.Mime.MediaTypeNames;

namespace HYPERMAGE.Managers
{
    public static class GameManager
    {
        public static Player player;

        public static bool waves;
        public static float wavesPower;

        public static Vector4 bounds;
        public static Vector4 groundBounds;

        public static bool fadeout = false;

        public static float screenShakeTime;
        public static float screenShakePower;

        private static float transitionTimer;
        private static IScene nextScene;
        private static bool transition;

        public static bool damageStatic;
        public static float staticPowerPrev;
        public static float staticPower = 0.95f;

        public static float abberationPower = 1f;
        public static float decayRate;

        public static bool death;
        public static int t;

        public static bool exit;

        private static float timer;
        public static void Init()
        {
            player = new Player(new(150, 100));

            SceneManager.AddScene(new MainMenu());

            Spellbook.Init();
        }

        public static void Update()
        {
            timer += Globals.TotalSeconds;

            if (!death)
            {
                SceneManager.GetScene().Update();
            }

            else
            {
                t++;

                if (t >= 4)
                {
                    SceneManager.GetScene().Update();

                    t = 0;
                }
            }

            if (abberationPower > 1f)
            {
                abberationPower -= Globals.TotalSeconds * decayRate;
            }

            else
            {
                abberationPower = 1f;
            }

            if (damageStatic)
            {
                if (staticPowerPrev <= 0)
                {
                    staticPowerPrev = staticPower;
                    staticPower = 0.05f;
                }

                staticPower += Globals.TotalSeconds;

                if (staticPower >= staticPowerPrev)
                {
                    staticPower = staticPowerPrev;
                    damageStatic = false;
                    staticPowerPrev = 0;
                }
            }

            if (transition)
            {
                transitionTimer += Globals.TotalSeconds;
            }

            if (transitionTimer >= 3)
            {
                if (death)
                {
                    player.health = 3;
                    player.lives--;

                    player.immune = false;
                    player.flashing = false;

                    death = false;
                }

                transition = false;
                transitionTimer = 0;

                SceneManager.RemoveScene();
                SceneManager.AddScene(nextScene);
            }

            if (timer >= 2)
            {
                timer = 0;
            }    
        }

        public static void Draw()
        {
            SceneManager.GetScene().Draw();
        }

        public static Player GetPlayer()
        {
            return player;
        }

        public static void AddScreenShake(float time, float power)
        {
            screenShakeTime = time;
            screenShakePower = power;
        }

        public static void PlayerDeath()
        {
            fadeout = true;
            nextScene = new Shop();
            transition = true;
            death = true;

            SoundManager.ClearSong();
        }
        public static void TransitionScene(IScene scene)
        {
            fadeout = true;
            nextScene = scene;
            transition = true;

            SoundManager.ClearSong();
        }

        public static void AddAbberationPowerForce(float decay, float intensity)
        {
            decayRate = decay;
            abberationPower += intensity;
        }

        public static void DrawScrollingTextBG(string text, float opacity)
        {
            int width = (int)Globals.GetPixelFont().MeasureString(text).X;
            int height = (int)Globals.GetPixelFont().MeasureString(text).Y;

            float speedX = 2f / width;
            float speedY = 2f / height;


            for (int i = 0; i < (320 / width) * 5; i++)
            {
                for (int j = 0; j < (180 / height) * 5; j++)
                {
                    Globals.SpriteBatch.DrawString(Globals.GetPixelFont(), text, new Vector2((width) * i, (height) * j) - new Vector2(timer / speedX, timer / speedX) + new Vector2(Globals.RandomFloat(-0.5f, 0.5f), 0) - new Vector2(width, height) - new Vector2(j % 2 == 0 ? width / 2 : 0, 0), Color.White * opacity, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.5f);
                }
            }
        }
    }
}
