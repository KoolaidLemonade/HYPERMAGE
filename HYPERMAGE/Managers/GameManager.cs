using HYPERMAGE.Helpers;
using HYPERMAGE.Models;
using HYPERMAGE.Particles;
using HYPERMAGE.Spells;
using HYPERMAGE.UI;
using HYPERMAGE.UI.UIElements;
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

        public static bool fadeout = false;

        public static float screenShakeTime;
        public static float screenShakePower;

        private static float transitionTimer;
        private static IScene nextScene;
        private static bool transition;

        public static bool death;
        public static int t;

        public static bool exit;
        public static void Init()
        {
            player = new Player(new(150, 100));

            SceneManager.AddScene(new MainMenu());

            Spellbook.Init();
        }

        public static void Update()
        {
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

            if (transition)
            {
                transitionTimer++;
            }

            if (transitionTimer >= 180)
            {
                UIManager.Clear();
                ParticleManager.Clear();
                MobManager.Clear();
                ProjectileManager.Clear();

                GameManager.wavesPower = 0.0f;
                GameManager.waves = false;
                GameManager.fadeout = false;

                SceneManager.RemoveScene();
                SceneManager.AddScene(nextScene);

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
    }
}
