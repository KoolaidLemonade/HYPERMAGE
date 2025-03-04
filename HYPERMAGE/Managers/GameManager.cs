using HYPERMAGE.Helpers;
using HYPERMAGE.Models;
using HYPERMAGE.Particles;
using HYPERMAGE.Spells;
using HYPERMAGE.UI;
using HYPERMAGE.UI.UIElements;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections;
using System.Collections.Generic;
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

        private static float bgTextTimer;

        public static List<string> bigText = [];
        private static float bigTextTimer;
        private static bool bigTextActive;

        private static int letterCount;
        private static int nextLetter;

        public static float zoneSize = 1f;

        private static float timer;
        public static void Init()
        {
            player = new Player(new(150, 100));

            SceneManager.AddScene(new Shop());

            Spellbook.Init();
        }

        public static void Update()
        {
            timer += Globals.TotalSeconds;

            bgTextTimer += Globals.TotalSeconds;

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

                zoneSize = 2f;

                SceneManager.RemoveScene();
                SceneManager.AddScene(nextScene);
            }

            if (bgTextTimer >= 2)
            {
                bgTextTimer = 0;
            }    

            if (bigTextActive)
            {
                if (nextLetter < letterCount)
                {
                    bigTextTimer += Globals.TotalSeconds;

                    if (bigTextTimer >= 1f / letterCount)
                    {
                        bigTextTimer = 0;

                        GameManager.AddScreenShake(0.1f, 15f);
                        GameManager.AddAbberationPowerForce(2000, 50);
                        SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("hit2"), 0.7f, Globals.RandomFloat(-1, 0), 0f);


                        nextLetter++;
                    }

                    bigTextActive = false;
                }
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
                    Globals.SpriteBatch.DrawString(Globals.GetPixelFont(), text, new Vector2((width) * i, (height) * j) - new Vector2(bgTextTimer / speedX, bgTextTimer / speedX) + new Vector2(Globals.RandomFloat(-0.5f, 0.5f), 0) - new Vector2(width, height) - new Vector2(j % 2 == 0 ? width / 2 : 0, 0), Color.White * opacity, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.5f);
                }
            }
        }

        public static void DrawSpeedLines(Color color, Vector2 center)
        {
            for (int i = 0; i < Globals.Random.Next(5 + 3); i++)
            {
                Vector2 newPos = center + (Vector2.One * Globals.RandomFloat(20, 100)).RotatedBy(MathHelper.ToRadians(Globals.RandomFloat(0, 360)));

                Globals.SpriteBatch.Draw(Globals.Content.Load<Texture2D>("bigtriangle"), newPos, null, Globals.Random.Next(3) == 0 ? color : Color.White, newPos.DirectionTo(center).ToRotation() + MathHelper.ToRadians(90f), new(13, 0), 1f, SpriteEffects.None, 1f);
            }
        }

        public static void AddBigText(List<string> words)
        {
            bigText = words;

            letterCount = 0;

            nextLetter = 0;

            bgTextTimer = 0;

            for (int i = 0; i < bigText.Count; i++)
            {
                for (int j = 0; j < bigText[i].Length; j++)
                {
                    letterCount++;
                }
            }
        }
        public static void DrawBigText()
        {
            bigTextActive = true;

            int letters = 0;

            for (int i = 0; i < bigText.Count; i++)
            {
                for (int j = 0; j < bigText[i].Length; j++)
                {
                    letters++;

                    if (letters <= nextLetter)
                    {
                        Globals.SpriteBatch.Draw(Globals.Content.Load<Texture2D>("alphabet"), new Vector2((160 - (bigText[i].Length * 28 / 2)) + j * 28, ((90 - ((bigText.Count * 47) / 2)) + i * 47) + (float)Math.Sin((timer * 8) + letters)), new Rectangle((LetterLookup(bigText[i][j].ToString()) - 1) * 27, 0, 27, 44), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                    }    
                }
            }

            if (nextLetter < letterCount)
            {
                DrawSpeedLines(Color.Gold, new(160, 90));
            }
        }

        public static int LetterLookup(string letter)
        {
            switch (letter)
            {
                case "a":
                    return 1;
                case "b":
                    return 2;
                case "c":
                    return 3;
                case "d":
                    return 4;
                case "e":
                    return 5;
                case "f":
                    return 6;
                case "g":
                    return 7;
                case "h":
                    return 8;
                case "i":
                    return 9;
                case "j":
                    return 10;
                case "k":
                    return 11;
                case "l":
                    return 12;
                case "m":
                    return 13;
                case "n":
                    return 14;
                case "o":
                    return 15;
                case "p":
                    return 16;
                case "q":
                    return 17;
                case "r":
                    return 18;
                case "s":
                    return 19;
                case "t":
                    return 20;
                case "u":
                    return 21;
                case "v":
                    return 22;
                case "w":
                    return 23;
                case "x":
                    return 24;
                case "y":
                    return 25;
                case "z":
                    return 25;
            }

            return 0;
        }
    }
}
