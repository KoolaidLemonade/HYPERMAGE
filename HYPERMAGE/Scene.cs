using HYPERMAGE.Helpers;
using HYPERMAGE.Managers;
using HYPERMAGE.Models;
using HYPERMAGE.Particles;
using HYPERMAGE.Spells;
using HYPERMAGE.UI;
using HYPERMAGE.UI.UIElements;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
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
        public void DrawVFX();
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
            GameManager.wavesPower = 0.0f;
            GameManager.waves = false;
            GameManager.fadeout = false;

            UIManager.Clear();
            ParticleManager.Clear();
            MobManager.Clear();
            ProjectileManager.Clear();

            SoundManager.ClearSounds();
            SoundManager.ClearSong();

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

            LevelManager.NextLevel();
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

        public void DrawVFX()
        {
            ProjectileManager.Draw();

            ParticleManager.Draw();
        }
        public void Draw()
        {
            LevelManager.DrawBG();

            GameManager.player.Draw();

            MobManager.Draw();

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

        public void DrawVFX()
        {
            ParticleManager.Draw();
        }
        public void Draw()
        {
            UIManager.Draw();
        }
    }

    public class IntroCutscene : IScene
    {
        private float introTimer;
        private static readonly float introStep = 4.5f;

        private static SpriteFont spriteFont;

        private static readonly string text1 = "A SILENT VOICE, IT CALLS YOUR NAME";
        private static readonly string text2 = "TEARING MIND AND SOUL IN TWAIN";
        private static readonly string text3 = "JOURNEY TO THE SEALED DOMAIN";
        private static readonly string text4 = "CRACK THE LOCKS AND BREAK MY CHAINS";

        private static bool voice = false;
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
                if (GameManager.waves)
                {
                    GameManager.AddScreenShake(0.1f, 10f);
                    GameManager.AddAbberationPowerForce(600, 50);

                    SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("lowbass"), 1f, 0f, 0f);
                }

                GameManager.wavesPower = 0.0f;
                GameManager.waves = false;
            }

            if (introTimer >= (introStep * 5) + 0.8 || InputManager.Clicked)
            {
                GameManager.wavesPower = 0.0f;
                GameManager.waves = false;

                UIManager.Clear();
                SoundManager.ClearSounds();

                SceneManager.RemoveScene();
                SceneManager.AddScene(new Shop());
            }
        }
        public void DrawVFX()
        {
        }
        public void Draw()
        {
            if (introTimer >= introStep)
            {
                if (!voice)
                {
                    SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("introvoice"), 1f, 0f, 0f);
                    voice = true;
                }

                Globals.SpriteBatch.DrawString(spriteFont, text1, new(235 - spriteFont.MeasureString(text1).X / 2, 60), Color.White);
            }

            if (introTimer >= introStep * 2)
            {
                Globals.SpriteBatch.DrawString(spriteFont, text2, new(235 - spriteFont.MeasureString(text1).X / 2, 80), Color.White);
            }

            if (introTimer >= introStep * 3)
            {
                Globals.SpriteBatch.DrawString(spriteFont, text3, new(235 - spriteFont.MeasureString(text1).X / 2, 100), Color.White);
            }

            if (introTimer >= introStep * 4)
            {
                Globals.SpriteBatch.DrawString(spriteFont, text4, new(235 - spriteFont.MeasureString(text1).X / 2, 120), Color.White);
            }

            if (introTimer <= introStep * 5)
            {
                Globals.SpriteBatch.Draw(Globals.Content.Load<Texture2D>("eye1"), new Vector2(30 + Globals.RandomFloat(-1f, 1f) * introTimer / 18, 85 + Globals.RandomFloat(-1f, 1f) * introTimer / 18), null, Color.White * (introTimer / 10), 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                Globals.SpriteBatch.Draw(Globals.Content.Load<Texture2D>("eye1"), new Vector2((150 - 42) + Globals.RandomFloat(-1f, 1f) * introTimer / 18, 85 + Globals.RandomFloat(-1f, 1f) * introTimer / 18), null, Color.White * (introTimer / 10), 0f, Vector2.Zero, 1f, SpriteEffects.FlipHorizontally, 1f);
            }

            else
            {
                Globals.SpriteBatch.Draw(Globals.Content.Load<Texture2D>("eye2"), new Vector2(30, 88), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                Globals.SpriteBatch.Draw(Globals.Content.Load<Texture2D>("eye2"), new Vector2(150 - 40, 88), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.FlipHorizontally, 1f);
            }
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

            SoundManager.PlaySong(Globals.Content.Load<Song>("shop"), 1f);
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

        public void DrawVFX()
        {
            ParticleManager.Draw();
            ProjectileManager.Draw();
        }
        public void Draw()
        {

            shopkeep.Draw(new(160 - shopkeep.frameWidth / 2, 0), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.8f);

            GameManager.player.Draw();

            MobManager.Draw();

            UIManager.Draw();
        }
    }

    public class Death : IScene
    {
        public Death()
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

        public void DrawVFX()
        {
        }
    }

    public class StageTransition : IScene
    {
        public Animation falling;
        public Vector2 fallingPos = new(15, -30);
        public Vector2 fallingVel;

        public float timer;
        public float fadeIn;

        public List<string> lines = [];
        public float lineShiftTimer;
        public int lineShift;
        public StageTransition()
        {
        }
        public void Load()
        {
            falling = new Animation(Globals.Content.Load<Texture2D>("falling"), 3, 1, 0.3f, 1);

            GameManager.waves = true;
            GameManager.wavesPower = 0f;

            lines.Add(GetPoemLine(5));
            lines.Add(GetPoemLine(6));
            lines.Add(GetPoemLine(7));
            lines.Add(GetPoemLine(8));

            SoundManager.PlaySong(Globals.Content.Load<Song>("faf"), 1f);
        }
        public void Update()
        {
            InputManager.Update();

            timer += Globals.TotalSeconds;
            lineShiftTimer += Globals.TotalSeconds;

            if (lineShiftTimer >= 1.5f)
            {
                lineShiftTimer = 0f;

                if (lineShift < lines.Count - 1)
                {
                    lineShift++;
                }

                else
                {
                    lineShift = 0;
                }
            }

            if (fadeIn < 1)
            {
                fadeIn += Globals.TotalSeconds / 5;

            }

            GameManager.wavesPower = fadeIn;

            fallingVel += fallingPos.DirectionTo(new Vector2(15, 40) + new Vector2(Globals.RandomFloat(-5, 5), Globals.RandomFloat(-5, 5)));

            fallingPos += fallingVel * Globals.TotalSeconds;

            fallingVel /= 1.1f;

            falling.Update();
        }
        public void Draw()
        {
            falling.Draw(fallingPos, Color.White * fadeIn, 0f, Vector2.Zero, 0.7f, SpriteEffects.None, 1f);

            for (int i = 0; i < lines.Count; i++)
            {
                Globals.SpriteBatch.DrawString(Globals.GetPixelFont(), lines[i], new Vector2(150, 50 + (20 * i)), Color.White * fadeIn);
            }

            GameManager.DrawScrollingTextBG(lines[lineShift] + " ", fadeIn / 20);
        }

        public void DrawVFX()
        {
        }

        public string GetPoemLine(int line)
        {
            switch (line)
            {
                case 1:
                    return "IN A DREAM, I WAS FALLING";
                case 2:
                    return "HEAVEN-SENT SUNSHARDS PIERCE THE VEIL";
                case 3:
                    return "AND THE SHIFTING FRACTURES DRIBBLE MAGICKS";
                case 4:
                    return "SCATTERING AMONGST THE GRAND FIRMAMENT";
                case 5:
                    return "IN A DREAM, I WAS FALLING";
                case 6:
                    return "FLEDGLINGS IMBIBE FROM THE EMPYREAN STREAM";
                case 7:
                    return "AND SWALLOW WHOLE EACH OTHERS' GLEAM";
                case 8:
                    return "BECOMING AN IMMORTAL BEING";
                case 9:
                    return "IN A DREAM, I WAS FALLING";
                case 10:
                    return "WOUND ONCE GAPING HAVE BEEN SEWN SHUT";
                case 11:
                    return "GREAT SEALS REPOSE WITHIN GOD'S GUT";
                case 12:
                    return "BUT - VOICELESS WHISPERS ARE NOT SILENT";
                case 13:
                    return "IN A DREAM, I WAS FALLING";
                case 14:
                    return "THOSE HANDS WHICH DEAL IN SHAPED ARCANE";
                case 15:
                    return "HAVE THRESHED INTO A SOUL AGAIN";
                case 16:
                    return "A DEATHLESS CYCLE THUS MAINTAINED";
                case 17:
                    return "IN A DREAM, I WAS FALLING";
                case 18:
                    return "ACTUALITIES TWISTING INTO NAUGHT";
                case 19:
                    return "BITTER CACOPHONIES OF EMPTINESS";
                case 20:
                    return "WHERE SOUND OUGHT TO LAY";
                case 21:
                    return "IN A DREAM, I WAS FALLING";
                case 22:
                    return "A DREAM THAT WAS NOT ALL A DREAM";
                case 23:
                    return "THOUGHTS OF WHICH NOT ALL WERE MINE-";
                case 24:
                    return "AND NEVER DID I CATCH MYSELF";
            }

            return "";
        }
    }
}