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
        public void DrawEnemyVFX();
        public void DrawVFX();
        public void DrawBG();
        public void DrawUI();
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
            heart = new(Globals.Content.Load<Texture2D>("heart"), new(123, 8));
            mana = new(Globals.Content.Load<Texture2D>("mana"), new(181, 6));

            UIManager.AddElement(heart);
            UIManager.AddElement(mana);

            GameManager.GetPlayer().position = new(160 - 5.5f, 150);

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
                ParticleManager.Update();
            }

            Spellbook.Update();

            LevelManager.Update();

            UIManager.Update();
        }

        public void DrawEnemyVFX()
        {
            ParticleManager.DrawEnemyParticles();
            ProjectileManager.DrawEnemyProj();
        }
        public void DrawVFX()
        {
            ProjectileManager.Draw();

            ParticleManager.Draw();
        }
        public void Draw()
        {
            LevelManager.Draw();

            GameManager.player.Draw();

            MobManager.Draw();
        }
        public void DrawBG()
        {
            LevelManager.DrawBG();
        }
        public void DrawUI()
        {
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
                    resistance = 1.2f,
                    friendly = false
                };

                Particle particle = new(InputManager.MousePosition, ParticleData);
                ParticleManager.AddParticle(particle);
            }

            UIManager.Update();
            InputManager.Update();
            ParticleManager.Update();
        }
        public void DrawEnemyVFX()
        {
            ParticleManager.DrawEnemyParticles();
        }

        public void DrawVFX()
        {
            ParticleManager.Draw();            
        }
        public void Draw()
        {
            UIManager.Draw();
        }
        public void DrawUI()
        {
        }
        public void DrawBG()
        {

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
        }
        public void Update()
        {
           
        }
        public void DrawEnemyVFX()
        {

        }
        public void DrawUI()
        {
        }
        public void DrawVFX()
        {
        }

        public void DrawBG()
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

        private static float timer = 0.8f;
        private float sealrot;

        public Shop()
        {
        }
        public void Load()
        {
            spriteFont = Globals.Content.Load<SpriteFont>("font");

            shopkeep = new Animation(Globals.Content.Load<Texture2D>("lich"), 12, 1, 0.4f, 1);

            mana = new(Globals.Content.Load<Texture2D>("mana"), new(215, -20));
            heart = new(Globals.Content.Load<Texture2D>("heart"), new(276, -20));
            spellbook = new(new(280, -20), Globals.Content.Load<Texture2D>("spellbookicon"));

            xp = new(new(266, -20));
            buyXPButton = new(new(252, -20), Globals.Content.Load<Texture2D>("xpicon"));
            rerollButton = new(new(250, -20), Globals.Content.Load<Texture2D>("rerollicon"));

            lockButton = new(new(238, -20), Globals.Content.Load<Texture2D>("lockicon"));

            UIManager.AddElement(spellbook);
            UIManager.AddElement(lockButton);
            UIManager.AddElement(rerollButton);
            UIManager.AddElement(buyXPButton);
            UIManager.AddElement(xp);
            UIManager.AddElement(mana);
            UIManager.AddElement(heart);

            GameManager.player.position = new Vector2(160 - 5, 140);
            GameManager.player.center = new Vector2(160, 140 + 5);

            GameManager.bounds = new(0 - 30, 0 - 30, 320 + 30, 180 + 30);

            if (!ShopManager.locked)
            {
                ShopManager.Reroll();
            }

            for (int i = 0; i < 30; i++)
            {
                ParticleData spawnParticleData = new()
                {
                    opacityStart = 1f,
                    opacityEnd = 1f,
                    sizeStart = 12 - Globals.Random.Next(6),
                    sizeEnd = 0,
                    colorStart = Color.White,
                    colorEnd = Color.White,
                    velocity = new(Globals.RandomFloat(-200, 200), Globals.RandomFloat(-800, 50)),
                    lifespan = Globals.RandomFloat(0.2f, 0.8f),
                    rotationSpeed = Globals.RandomFloat(-0.5f, 0.5f)
                };

                Particle spawnParticle = new(GameManager.GetPlayer().center, spawnParticleData);
                ParticleManager.AddParticle(spawnParticle);
            }

            for (int i = 0; i < 15; i++)
            {
                ParticleData spawnParticleData = new()
                {
                    opacityStart = 1f,
                    opacityEnd = 1f,
                    sizeStart = 4 - Globals.Random.Next(4),
                    sizeEnd = 0,
                    colorStart = Color.White,
                    colorEnd = Color.White,
                    velocity = new(Globals.RandomFloat(-600, 600), Globals.RandomFloat(-200, 50)),
                    lifespan = Globals.RandomFloat(0.2f, 0.8f),
                    rotationSpeed = Globals.RandomFloat(-0.5f, 0.5f)
                };

                Particle spawnParticle = new(GameManager.GetPlayer().center, spawnParticleData);
                ParticleManager.AddParticle(spawnParticle);
            }

            GameManager.AddScreenShake(0.2f, 15f);
            GameManager.AddAbberationPowerForce(500, 50);

            SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("wavy"), 1f, 0f, 0f);

            SoundManager.PlaySong(Globals.Content.Load<Song>("shop"), 1f);
        }
        public void Update()
        {
            timer += Globals.TotalSeconds;
            sealrot += Globals.TotalSeconds;

            if (GameManager.GetPlayer().center.X > 160 - 30 && GameManager.GetPlayer().center.X < 160 + 30 && GameManager.GetPlayer().center.Y > 85 - 25 && GameManager.GetPlayer().center.Y < 85 + 25)
            {
                SpellbookUI.open = true;

                if (timer >= 2)
                {
                    timer = 0;
                }
            }

            else
            {
                SpellbookUI.open = false;
                SpellbookUI.closing = true;

                if (timer >= 2)
                {
                    timer = 0;

                    GameManager.AddAbberationPowerForce(500, 22);
                    GameManager.AddScreenShake(0.1f, 2f);
                    SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("lowbass"), 1f, Globals.RandomFloat(-1, 0f), 0);

                    for (int i = 0; i < 50; i++)
                    {
                        Vector2 pos = new Vector2(160, 86) + Vector2.One.RotatedBy(MathHelper.ToRadians(Globals.RandomFloat(0, 360))) * 33;
                        ParticleData pd = new()
                        {
                            sizeStart = Globals.RandomFloat(2f, 6f),
                            sizeEnd = 0,
                            colorStart = Color.White,
                            colorEnd = Color.White,
                            velocity = pos.DirectionTo(new(160, 90)) * Globals.RandomFloat(150, 330),
                            lifespan = Globals.RandomFloat(0.1f, 0.5f),
                            rotationSpeed = Globals.RandomFloat(-0.5f, 0.5f),
                            friendly = false
                        };

                        Particle p = new(pos, pd);
                        ParticleManager.AddParticle(p);

                    }
                }

                if (Globals.Random.Next(2) == 0)
                {
                    Vector2 pos = new Vector2(160, 86) + Vector2.One.RotatedBy(MathHelper.ToRadians(Globals.RandomFloat(0, 360))) * Globals.RandomFloat(60, 200);
                    ParticleData pd = new()
                    {
                        sizeStart = Globals.RandomFloat(2f, 3f),
                        opacityStart = 0,
                        opacityEnd = Globals.RandomFloat(0, 1),
                        sizeEnd = 0,
                        colorStart = Color.White,
                        colorEnd = Color.White,
                        velocity = pos.DirectionTo(new(160, 90)) * Globals.RandomFloat(150, 270),
                        lifespan = Globals.RandomFloat(0.5f, 1f),
                        rotationSpeed = Globals.RandomFloat(-0.5f, 0.5f),
                        resistance = 1.05f,
                        friendly = false
                    };

                    Particle p = new(pos, pd);
                    ParticleManager.AddParticle(p);
                }

            }

            if (GameManager.GetPlayer().position.Y > 180 + 10 || GameManager.GetPlayer().position.Y < 0 - 10 || GameManager.GetPlayer().position.X > 320 + 10 || GameManager.GetPlayer().position.X < 0 - 10)
            {
                GameManager.TransitionScene(new GameScene());
            }

            if (SpellbookUI.open || SpellbookUI.closing)
            {
                buyXPButton.position = new(252, Globals.NonLerp(-20, 134, SpellbookUI.openingTimer));
                xp.position = new(266, Globals.NonLerp(-20, 134, SpellbookUI.openingTimer));
                mana.position = new(215, Globals.NonLerp(-20, 145, SpellbookUI.openingTimer));
                heart.position = new(276, Globals.NonLerp(-20, 147, SpellbookUI.openingTimer));
                rerollButton.position = new(261, Globals.NonLerp(-20, 69, SpellbookUI.openingTimer));
                lockButton.position = new(256, Globals.NonLerp(-20, 98, SpellbookUI.openingTimer));
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
        public void DrawEnemyVFX()
        {
            ParticleManager.DrawEnemyParticles();

        }

        public void DrawVFX()
        {
            ParticleManager.Draw();
            ProjectileManager.Draw();
        }
        public void Draw()
        {
            shopkeep.Draw(new(158 - shopkeep.frameWidth / 2, 0), new (Color.White * 0.75f, 1f), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.8f);
            Globals.SpriteBatch.Draw(Globals.Content.Load<Texture2D>("seal"), new Vector2(160, 90), null, Color.White * 0.2f, sealrot / 5, new Vector2(72.5f, 72.5f), 1f, SpriteEffects.None, 0.79f);
            Globals.SpriteBatch.Draw(Globals.Content.Load<Texture2D>("seal"), new Vector2(160, 90), null, Color.White * 0.02f, -sealrot / 7, new Vector2(72.5f, 72.5f), 2f, SpriteEffects.None, 0.79f);


            GameManager.player.Draw();

            MobManager.Draw();
        }

        public void DrawBG()
        {

        }
        public void DrawUI()
        {
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

        public void DrawEnemyVFX()
        {

        }
        public void DrawVFX()
        {
        }

        public void DrawBG()
        {

        }

        public void DrawUI()
        {

        }
    }

    public class StageTransition : IScene
    {
        public Animation falling;
        public Vector2 fallingPos = new(15, -30);
        public Vector2 fallingVel;
        public bool upBump = false;

        public float timer;
        public float fadeIn;
        public float upgradeFadeIn;
        public bool upgradeShow = false;

        public float exitTimer;

        public List<string> lines = [];
        public float lineShiftTimer;
        public int lineShift;

        public List<Upgrade> upgrades = [];
        public StageTransition()
        {
        }
        public void Load()
        {
            falling = new Animation(Globals.Content.Load<Texture2D>("falling"), 3, 1, 0.3f, 1);

            lines.Add(GetPoemLine(5));
            lines.Add(GetPoemLine(6));
            lines.Add(GetPoemLine(7));
            lines.Add(GetPoemLine(8));

            for (int i = 0; i < 3; i++)
            {
                Upgrade upgrade = new Upgrade(new((45) + (80 * i), 30), Globals.Content.Load<Texture2D>("upgradeborder"));

                upgrade.opacity = 0f;

                upgrades.Add(upgrade);

                UIManager.AddElement(upgrade);
            }

            SoundManager.PlaySong(Globals.Content.Load<Song>("faf"), 1f);
        }
        public void Update()
        {
            ParticleManager.Update();
            InputManager.Update();
            UIManager.Update();

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

            if (fadeIn < 1 && !Upgrade.choiceMade)
            {
                fadeIn += Globals.TotalSeconds / 5;

            }

            if (timer > 8f || InputManager.Clicked)
            {
                upgradeShow = true;
            }

            if (upgradeShow && !Upgrade.choiceMade && upgradeFadeIn < 1)
            {
                upgradeFadeIn += Globals.TotalSeconds;
            }

            if (Upgrade.choiceMade)
            {
                if (!upBump)
                {
                    upBump = true;
                    fallingVel += new Vector2(0, -100);
                }   

                exitTimer += Globals.TotalSeconds;

                upgradeFadeIn -= Globals.TotalSeconds * 2;

                if (fadeIn > 0)
                {
                    fadeIn -= Globals.TotalSeconds / 3;
                }
            }

            foreach (var upgrade in upgrades)
            {
                upgrade.opacity = upgradeFadeIn;
            }

            fallingVel += fallingPos.DirectionTo(new Vector2(15, Upgrade.choiceMade ? 180 : 40) + new Vector2(Globals.RandomFloat(-5, 5), Globals.RandomFloat(-5, 5))) * (Upgrade.choiceMade ? 3.5f : 1);

            fallingPos += fallingVel * Globals.TotalSeconds;

            fallingVel /= 1.02f;

            falling.Update();

            if (exitTimer >= 3f)
            {
                GameManager.TransitionScene(new Shop());
            }

        }

        public void DrawEnemyVFX()
        {

        }
        public void DrawBG()
        {

        }
        public void Draw()
        {
            UIManager.Draw();

            falling.Draw(fallingPos, Color.White * fadeIn, 0f, Vector2.Zero, 0.7f, SpriteEffects.None, 0.8f);

            for (int i = 0; i < lines.Count; i++)
            {
                Globals.SpriteBatch.DrawString(Globals.GetPixelFont(), lines[i], new Vector2(150, 50 + (20 * i)), Color.White * fadeIn, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.8f);
            }

            GameManager.DrawScrollingTextBG(lines[lineShift] + " ", fadeIn / 20);

            Globals.SpriteBatch.Draw(Globals.GetBlankTexture(), new Rectangle(0, 0, 320, 180), null, Color.Black * (upgradeFadeIn / 1.1f), 0f, Vector2.Zero, SpriteEffects.None, 0.9f);
        }

        public void DrawVFX()
        {
            ParticleManager.Draw();
        }

        public void DrawUI()
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