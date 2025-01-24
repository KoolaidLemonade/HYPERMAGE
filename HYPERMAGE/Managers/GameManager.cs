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

        public static bool exit;
        public static void Init()
        {
            player = new Player(new(150, 100));

            SceneManager.AddScene(new MainMenu());

            Spellbook.Init();
        }

        public static void Update()
        {
            SceneManager.GetScene().Update();

            if (transition)
            {
                transitionTimer++;
            }

            if (transitionTimer >= 180)
            {

                UIManager.Clear();
                ParticleManager.Clear();

                GameManager.wavesPower = 0.0f;
                GameManager.waves = false;
                GameManager.fadeout = false;

                SceneManager.RemoveScene();
                SceneManager.AddScene(nextScene);

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

        public static void TransitionScene(IScene scene)
        {
            fadeout = true;
            nextScene = scene;
            transition = true;
        }
    }
}
