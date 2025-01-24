using HYPERMAGE.Helpers;
using HYPERMAGE.Managers;
using HYPERMAGE.Models;
using HYPERMAGE.Particles;
using HYPERMAGE.Spells;
using HYPERMAGE.UI;
using HYPERMAGE.UI.UIElements;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace HYPERMAGE
{
    public interface IScene
    {
        public void Load();
        public void Update();
        public void Draw();
    }

    public static class SceneManager
    {
        private static Stack<IScene> sceneStack = new();
        public static void AddScene(IScene scene)
        {
            scene.Load();
            sceneStack.Push(scene);
        }

        public static void RemoveScene()
        {
            sceneStack.Pop();
        }

        public static IScene GetScene()
        {
            return sceneStack.Peek();
        }
    }

    public class GameScene : IScene
    {
        public GameScene()
        {
        }

        private static Heart heart;
        private static Mana mana;

        private static int hitstop = 0;
        public void Load()
        {
            heart = new(Globals.Content.Load<Texture2D>("heart"), new(133, 8));
            mana = new(Globals.Content.Load<Texture2D>("mana"), new(181, 6));

            UIManager.AddElement(heart);
            UIManager.AddElement(mana);

            GameManager.GetPlayer().position = new(160, 90);

            GameManager.bounds = new(0, 0, 320, 180);

            LevelManager.NextLevel();
            LevelManager.GetNextSpawnWave();
        }
        public void Update()
        {
            InputManager.Update();

            hitstop--;

            if (hitstop <= 0)
            {
                GameManager.player.Update();
                ProjectileManager.Update();
                MobManager.Update();
            }

            Spellbook.Update();

            ParticleManager.Update();

            LevelManager.Update();

            UIManager.Update();
        }
        public void Draw()
        {
            GameManager.player.Draw();

            ParticleManager.Draw();

            MobManager.Draw();

            ProjectileManager.Draw();

            UIManager.Draw();
        }
        public static void AddHitstop(int hitstopFrames)
        {
            hitstop = hitstopFrames;
        }
    }

    public class MainMenu : IScene
    {
        public MainMenu()
        {
        }

        private static PlayButton playButton;
        private static ExitButton exitButton;

        private static UIElement logo;

        private static SpriteFont spriteFont;

        private static List<LogoLetter> logoLetters = [];
        public void Load()
        {
            spriteFont = Globals.Content.Load<SpriteFont>("font");
            logo = new(Globals.Content.Load<Texture2D>("logo"), new(35, 74));
            playButton = new(new(160 - spriteFont.MeasureString("PLAY").X / 2, 110), spriteFont, "PLAY", Color.White);
            exitButton = new(new(160 - spriteFont.MeasureString("EXIT").X / 2, 125), spriteFont, "EXIT", Color.White);

            for (int i = 0; i < 9; i++)
            {
                if (i >= 5)
                {
                    logoLetters.Add(new LogoLetter(i, new(36 + (i * 28), 24), Globals.Content.Load<Texture2D>("logoletters")));
                }

                else
                {
                    logoLetters.Add(new LogoLetter(i, new(35 + (i * 28), 24), Globals.Content.Load<Texture2D>("logoletters")));
                }


                UIManager.AddElement(logoLetters[i]);
            }

            UIManager.AddElement(playButton);
            UIManager.AddElement(exitButton);
            UIManager.AddElement(logo);

            GameManager.waves = true;
            GameManager.wavesPower = 1f;

            GameManager.bounds = new(0, 0, 320, 180);

            SoundManager.PlaySong(Globals.Content.Load<Song>("title"), 0.5f);
        }
        public void Update()
        {
            if (InputManager.Clicked)
            {
                for (int i = 0; i < 8; i++)
                {
                    ParticleData ParticleData = new()
                    {
                        opacityStart = 1f,
                        opacityEnd = 0f,
                        sizeStart = 3,
                        sizeEnd = 1,
                        colorStart = Color.White,
                        colorEnd = Color.White,
                        velocity = new(Globals.RandomFloat(-200, 200), Globals.RandomFloat(-200, 200)),
                        lifespan = 0.2f,
                        rotationSpeed = 1f,
                        resistance = 1.2f
                    };

                    Particle particle = new(InputManager.MousePosition, ParticleData);
                    ParticleManager.AddParticle(particle);
                }
            }

            if (InputManager.LeftMouseDown)
            {
                ParticleData ParticleData = new()
                {
                    opacityStart = 1f,
                    opacityEnd = 0f,
                    sizeStart = 3,
                    sizeEnd = 1,
                    colorStart = Color.White,
                    colorEnd = Color.White,
                    velocity = new(Globals.RandomFloat(-20, 20), Globals.RandomFloat(-80, 20)),
                    lifespan = 0.4f,
                    rotationSpeed = 1f,
                    resistance = 1.2f
                };

                Particle particle = new(InputManager.MousePosition, ParticleData);
                ParticleManager.AddParticle(particle);
            }

            UIManager.Update();
            InputManager.Update();
            ParticleManager.Update();
        }
        public void Draw()
        {
            ParticleManager.Draw();
            UIManager.Draw();
        }
    }

    public class IntroCutscene : IScene
    {
        private float introTimer;
        private static readonly int introStep = 4;

        private static SpriteFont spriteFont;

        private static readonly string text1 = "A SILENT VOICE, IT CALLS YOUR NAME";
        private static readonly string text2 = "TEARING MIND AND SOUL IN TWAIN";
        private static readonly string text3 = "JOURNEY TO THE SEALED DOMAIN";
        private static readonly string text4 = "CRACK THE LOCKS AND BREAK MY CHAINS";
        public IntroCutscene()
        {
        }
        public void Load()
        {
            spriteFont = Globals.Content.Load<SpriteFont>("font");

            GameManager.waves = true;
        }
        public void Update()
        {
            introTimer += Globals.TotalSeconds;

            GameManager.wavesPower = (float)Math.Pow((introTimer / 15), 2);

            InputManager.Update();

            if (introTimer >= introStep * 5)
            {
                GameManager.wavesPower = 0.0f;
                GameManager.waves = false;
            }

            if (introTimer >= (introStep * 5) + 0.5 || InputManager.Clicked)
            {
                GameManager.wavesPower = 0.0f;
                GameManager.waves = false;

                UIManager.Clear();

                SceneManager.RemoveScene();
                SceneManager.AddScene(new Shop());
            }
        }
        public void Draw()
        {
            if (introTimer <= introStep * 5)
            {
                if (introTimer >= introStep)
                {
                    Globals.SpriteBatch.DrawString(spriteFont, text1, new(160 - spriteFont.MeasureString(text1).X / 2, 30), Color.White);
                }

                if (introTimer >= introStep * 2)
                {
                    Globals.SpriteBatch.DrawString(spriteFont, text2, new(160 - spriteFont.MeasureString(text1).X / 2, 50), Color.White);
                }

                if (introTimer >= introStep * 3)
                {
                    Globals.SpriteBatch.DrawString(spriteFont, text3, new(160 - spriteFont.MeasureString(text1).X / 2, 110), Color.White);
                }

                if (introTimer >= introStep * 4)
                {
                    Globals.SpriteBatch.DrawString(spriteFont, text4, new(160 - spriteFont.MeasureString(text1).X / 2, 130), Color.White);
                }

                Globals.SpriteBatch.Draw(Globals.Content.Load<Texture2D>("eye1"), new Vector2(60 + Globals.RandomFloat(-1f, 1f) * introTimer / 18, 85 + Globals.RandomFloat(-1f, 1f) * introTimer / 18), null, Color.White * (introTimer / 15), 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                Globals.SpriteBatch.Draw(Globals.Content.Load<Texture2D>("eye1"), new Vector2((260 - 42) + Globals.RandomFloat(-1f, 1f) * introTimer / 18, 85 + Globals.RandomFloat(-1f, 1f) * introTimer / 18), null, Color.White * (introTimer / 15), 0f, Vector2.Zero, 1f, SpriteEffects.FlipHorizontally, 1f);
            }

            else
            {
                Globals.SpriteBatch.Draw(Globals.Content.Load<Texture2D>("eye2"), new Vector2(60 , 75), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                Globals.SpriteBatch.Draw(Globals.Content.Load<Texture2D>("eye2"), new Vector2(260 - 40, 75), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.FlipHorizontally, 1f);
            }
        }
    }

    public class PauseMenu : IScene
    {
        public PauseMenu()
        {
        }
        public void Load()
        {
        }
        public void Update()
        {

        }
        public void Draw()
        {
        }
    }

    public class Shop : IScene
    {
        private static SpriteFont spriteFont;
        private static Animation shopkeep;

        private static Mana mana;
        private static Heart heart;
        private static XP xp;
        private static BuyXPButton buyXPButton;
        private static RerollButton rerollButton;
        private static LockButton lockButton;
        private static SpellbookUI spellbook;
        public Shop()
        {
        }
        public void Load()
        {
            spriteFont = Globals.Content.Load<SpriteFont>("font");

            shopkeep = new Animation(Globals.Content.Load<Texture2D>("shopkeep"), 5, 1, 1.5f, 1);

            mana = new(Globals.Content.Load<Texture2D>("mana"), new(210, 5));
            heart = new(Globals.Content.Load<Texture2D>("heart"), new(250, 7));
            spellbook = new(new(280, 5), Globals.Content.Load<Texture2D>("spellbookicon"));

            xp = new(new(70, 5));
            buyXPButton = new(new(50, 5), Globals.Content.Load<Texture2D>("xpicon"));
            rerollButton = new(new(122, 50), Globals.Content.Load<Texture2D>("rerollicon"));

            lockButton = new(new(360 - 164, 50), Globals.Content.Load<Texture2D>("lockicon"));

            UIManager.AddElement(spellbook);
            UIManager.AddElement(lockButton);
            UIManager.AddElement(rerollButton);
            UIManager.AddElement(buyXPButton);
            UIManager.AddElement(xp);
            UIManager.AddElement(mana);
            UIManager.AddElement(heart);

            GameManager.waves = true;
            GameManager.wavesPower = 1f;

            GameManager.player.position = new Vector2(160 - 5, 140);

            GameManager.bounds = new(0 - 30, 0 - 30, 320 + 30, 180 + 30);

            if (!ShopManager.locked)
            {
                ShopManager.Reroll();
            }
        }
        public void Update()
        {
            if (GameManager.GetPlayer().position.Y > 180 + 10 || GameManager.GetPlayer().position.Y < 0 - 10 || GameManager.GetPlayer().position.X > 320 + 10 || GameManager.GetPlayer().position.X < 0 - 10)
            {
                GameManager.TransitionScene(new GameScene());
            }

            if (SpellbookUI.open || SpellbookUI.closing)
            {
                buyXPButton.position = new(Globals.NonLerp(50, 252, SpellbookUI.openingTimer), Globals.NonLerp(5, 134, SpellbookUI.openingTimer));
                xp.position = new(Globals.NonLerp(70, 266, SpellbookUI.openingTimer), Globals.NonLerp(5, 134, SpellbookUI.openingTimer));
                mana.position = new(Globals.NonLerp(210, 215, SpellbookUI.openingTimer), Globals.NonLerp(5, 145, SpellbookUI.openingTimer));
                heart.position = new(Globals.NonLerp(250, 276, SpellbookUI.openingTimer), Globals.NonLerp(7, 147, SpellbookUI.openingTimer));
                rerollButton.position = new(Globals.NonLerp(122, 250, SpellbookUI.openingTimer), Globals.NonLerp(50, 29, SpellbookUI.openingTimer));
                lockButton.position = new(Globals.NonLerp(360 - 164, 238, SpellbookUI.openingTimer), Globals.NonLerp(50, 41, SpellbookUI.openingTimer));
            }


            shopkeep.Update();

            GameManager.player.Update();

            InputManager.Update();

            Spellbook.Update();

            ParticleManager.Update();

            ProjectileManager.Update();

            ShopManager.Update();

            UIManager.Update();
        }
        public void Draw()
        {

            shopkeep.Draw(new(160 - shopkeep.frameWidth / 2, 0));

            GameManager.player.Draw();

            ParticleManager.Draw();

            MobManager.Draw();

            ProjectileManager.Draw();

            UIManager.Draw();
        }
    }

    public class StageTransition : IScene
    {
        public StageTransition()
        {
        }
        public void Load()
        {
        }
        public void Update()
        {

        }
        public void Draw()
        {
        }
    }
}