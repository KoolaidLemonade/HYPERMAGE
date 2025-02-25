using HYPERMAGE.Helpers;
using HYPERMAGE.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace HYPERMAGE
{
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private RenderTarget2D vfxMaster;
        private RenderTarget2D vfxEnemy;
        private RenderTarget2D vfx;
        private RenderTarget2D game;

        private RenderTarget2D renderTarget;
        private RenderTarget2D renderTarget2;
        private RenderTarget2D renderTarget3;
        private RenderTarget2D renderTarget4;
        private RenderTarget2D renderTarget5;
        private RenderTarget2D renderTarget6;

        private RenderTarget2D voroDisplacement;
        private RenderTarget2D zoneTarget;
        private RenderTarget2D warpTarget;
        private RenderTarget2D abberationTarget;

        private Effect blur;
        private Effect waves;
        private Effect transition;
        private Effect shake;
        private Effect invert;
        private Effect noise;
        private Effect warp;
        private Effect abberation;
        private Effect zone;
        private Effect voro;
        private Effect perlin;
        private Effect outline;

        private float time;
        private float time2;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = true;
            graphics.HardwareModeSwitch = false;

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            vfxEnemy = new RenderTarget2D(GraphicsDevice, 320, 180);
            vfx = new RenderTarget2D(GraphicsDevice, 320, 180);
            game = new RenderTarget2D(GraphicsDevice, 320, 180);
            vfxMaster = new RenderTarget2D(GraphicsDevice, 320, 180);

            renderTarget = new RenderTarget2D(GraphicsDevice, 320, 180);
            renderTarget2 = new RenderTarget2D(GraphicsDevice, 320, 180);
            renderTarget3 = new RenderTarget2D(GraphicsDevice, 320, 180);
            renderTarget4 = new RenderTarget2D(GraphicsDevice, 320, 180);
            renderTarget5 = new RenderTarget2D(GraphicsDevice, 1920, 1080);
            renderTarget6 = new RenderTarget2D(GraphicsDevice, 1920, 1080);

            voroDisplacement = new RenderTarget2D(GraphicsDevice, 320, 180);
            zoneTarget = new RenderTarget2D(GraphicsDevice, 320, 180);
            warpTarget = new RenderTarget2D(GraphicsDevice, 1920, 1080);
            abberationTarget = new RenderTarget2D(GraphicsDevice, 1920, 1080);

            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;
            graphics.ApplyChanges();

            spriteBatch = new SpriteBatch(GraphicsDevice);
            Globals.SpriteBatch = spriteBatch;

            Globals.Content = Content;

            GameManager.Init();

            base.Initialize();
        }


        List<float> scanlines = [];
        int scanlineCount = 3;
        int scanlineThickness = 200;
        protected override void LoadContent()
        {
            waves = Content.Load<Effect>("waves");
            blur = Content.Load<Effect>("blur");
            transition = Content.Load<Effect>("transition");
            shake = Content.Load<Effect>("shake");
            invert = Content.Load<Effect>("invert");
            noise = Content.Load<Effect>("noise");
            warp = Content.Load<Effect>("warp");
            abberation = Content.Load<Effect>("abberation");
            zone = Content.Load<Effect>("zone");
            voro = Content.Load<Effect>("voro");
            perlin = Content.Load<Effect>("perlin");
            outline = Content.Load<Effect>("outline");

            voro.Parameters["PaletteTexture"].SetValue(Globals.Content.Load<Texture2D>("voropalette"));

            zone.Parameters["center"].SetValue(new Vector2(0.5f, 0.5f));

            noise.Parameters["power"].SetValue(0.85f);

            warp.Parameters["resolution"].SetValue(new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight));

            for (int i = 0; i < scanlineCount; i++)
            {
                scanlines.Add(i * graphics.PreferredBackBufferHeight / scanlineCount - scanlineThickness);
            }
        }

        float t;
        float tt;
        protected override void Update(GameTime gameTime)
        {
            t += Globals.TotalSeconds;
            tt++;

            zone.Parameters["range"].SetValue(GameManager.zoneSize);

            Vector2 mouseUV = new Vector2(0, 0) - (new Vector2(-InputManager.MousePosition.X, -InputManager.MousePosition.Y) / new Vector2(320, 180));
            abberation.Parameters["mousePosition"].SetValue(mouseUV);

            abberation.Parameters["power"].SetValue(GameManager.abberationPower);

            if (GameManager.damageStatic)
            {
                noise.Parameters["power"].SetValue(GameManager.staticPower);
            }

            for (int i = 0; i < scanlines.Count; i++)
            {
                if (scanlines[i] > graphics.PreferredBackBufferHeight - scanlineThickness)
                {
                    scanlines[i] = -scanlineThickness;
                }

                else
                {

                }

                scanlines[i] += 25;
            }

            if (t >= 1)
            {
                t = 0;
                tt = 0;
            }


            if (GameManager.exit)
            {
                Exit();
            }

            Globals.Update(gameTime);

            GameManager.Update();

            base.Update(gameTime);

            time += Globals.TotalSeconds;

            waves.Parameters["power"].SetValue(GameManager.wavesPower);
            waves.Parameters["time"].SetValue(time);
            noise.Parameters["time"].SetValue(time);
            zone.Parameters["time"].SetValue(time);
            voro.Parameters["time"].SetValue(time);
            perlin.Parameters["time"].SetValue(time);
            //outline.Parameters["time"].SetValue(time);

            if (GameManager.fadeout)
            {
                time2 += Globals.TotalSeconds;
                transition.Parameters["time"].SetValue(time2);
            }

            else
            {
                time2 = 0;
            }

            if (GameManager.screenShakeTime > 0)
            {
                Matrix view = Matrix.CreateTranslation(Globals.RandomFloat(-GameManager.screenShakePower, GameManager.screenShakePower), Globals.RandomFloat(-GameManager.screenShakePower, GameManager.screenShakePower), 0);

                int width = GraphicsDevice.Viewport.Width;
                int height = GraphicsDevice.Viewport.Height;

                Matrix projection = Matrix.CreateOrthographicOffCenter(0, width, height, 0, 0, 1);

                shake.Parameters["WorldViewProjection"].SetValue(view * projection);

                GameManager.screenShakeTime -= Globals.TotalSeconds;
            }

            else
            {
                Matrix view = Matrix.Identity;

                int width = GraphicsDevice.Viewport.Width;
                int height = GraphicsDevice.Viewport.Height;

                Matrix projection = Matrix.CreateOrthographicOffCenter(0, width, height, 0, 0, 1);

                shake.Parameters["WorldViewProjection"].SetValue(view * projection);
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(vfx);
            GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin(blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointClamp, sortMode: SpriteSortMode.FrontToBack);
            SceneManager.GetScene().DrawVFX();
            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(vfxEnemy);
            GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin(blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointClamp, sortMode: SpriteSortMode.FrontToBack);
            SceneManager.GetScene().DrawEnemyVFX();
            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(game);
            spriteBatch.Begin(blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointClamp, sortMode: SpriteSortMode.FrontToBack);
            SceneManager.GetScene().Draw();
            spriteBatch.End();

            invert.Parameters["InvertTex"].SetValue(game);

            GraphicsDevice.SetRenderTarget(voroDisplacement);

            spriteBatch.Begin(blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointClamp, effect: perlin);
            spriteBatch.Draw(Globals.GetBlankTexture(), new Rectangle(0, 0, 320, 180), new Color(Color.White, 0));
            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(renderTarget);

            voro.Parameters["DisplacementTexture"].SetValue(voroDisplacement);

            spriteBatch.Begin(blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointClamp, effect: voro);
            spriteBatch.Draw(voroDisplacement, new Rectangle(0, 0, 320, 180), new Color(Color.White, 0));
            spriteBatch.End();

            spriteBatch.Begin(blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointClamp, sortMode: SpriteSortMode.FrontToBack);
            SceneManager.GetScene().DrawBG();
            spriteBatch.End();

            spriteBatch.Begin(blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointClamp, sortMode: SpriteSortMode.FrontToBack);
            spriteBatch.Draw(game, Vector2.Zero, new Color(Color.White, 0));
            spriteBatch.End();


            spriteBatch.Begin(blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointClamp, sortMode: SpriteSortMode.FrontToBack, effect: invert);
            spriteBatch.Draw(vfx, Vector2.Zero, new Color(Color.White, 0));
            spriteBatch.End();

            spriteBatch.Begin(blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointClamp, sortMode: SpriteSortMode.FrontToBack);
            spriteBatch.Draw(vfxEnemy, new Vector2(1, 0), new Color(Color.Red, 0));
            spriteBatch.Draw(vfxEnemy, new Vector2(-1, 0), new Color(Color.Red, 0));
            spriteBatch.Draw(vfxEnemy, new Vector2(0, 1), new Color(Color.Red, 0));
            spriteBatch.Draw(vfxEnemy, new Vector2(0, -1), new Color(Color.Red, 0));

            spriteBatch.End();

            spriteBatch.Begin(blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointClamp, sortMode: SpriteSortMode.FrontToBack, effect: invert);
            spriteBatch.Draw(vfxEnemy, Vector2.Zero, new Color(Color.White, 0));
            spriteBatch.End();


            GraphicsDevice.SetRenderTarget(zoneTarget);

            spriteBatch.Begin(blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointClamp, effect: voro);
            spriteBatch.Draw(voroDisplacement, new Rectangle(0, 0, 320, 180), new Color(Color.White, 0));
            spriteBatch.End();

            spriteBatch.Begin(blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointClamp, sortMode: SpriteSortMode.FrontToBack, effect: zone);
            spriteBatch.Draw(renderTarget, Vector2.Zero, new Color(Color.White, 0));
            spriteBatch.End();

            spriteBatch.Begin(blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointClamp, sortMode: SpriteSortMode.FrontToBack);
            SceneManager.GetScene().DrawUI();
            spriteBatch.End();

            //

            GraphicsDevice.SetRenderTarget(renderTarget2);

            blur.Parameters["offset"].SetValue(new Vector2(1f / renderTarget.Width, 0));

            spriteBatch.Begin(blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointClamp, effect: blur);
            spriteBatch.Draw(zoneTarget, Vector2.Zero, new Color(Color.White, 0));
            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(renderTarget3);

            blur.Parameters["offset"].SetValue(new Vector2(0, 1f / renderTarget.Height));

            spriteBatch.Begin(blendState: LightenBlend, samplerState: SamplerState.PointClamp, effect: blur);
            spriteBatch.Draw(renderTarget2, Vector2.Zero, new Color(Color.White, 0));
            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(renderTarget5);

            spriteBatch.Begin(blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(renderTarget4, new Rectangle(0, 0, 1920, 1080), new Color(Color.White, 0));
            spriteBatch.End();

            spriteBatch.Begin(blendState: LightenBlend);
            spriteBatch.Draw(renderTarget3, new Rectangle(0, 0, 1920, 1080), new Color(Color.SlateBlue, 0.5f) * 0.4f);
            spriteBatch.End();

            spriteBatch.Begin(blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointClamp, effect: shake);
            spriteBatch.Draw(zoneTarget, new Rectangle(0, 0, 1920, 1080), new Color(Color.White, 0));
            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(renderTarget6);

            spriteBatch.Begin(blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointClamp, effect: GameManager.fadeout ? transition : null);
            spriteBatch.Draw(renderTarget5, Vector2.Zero, new Color(Color.White, 0));
            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(warpTarget);

            spriteBatch.Begin(blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointClamp, effect: noise);
            spriteBatch.Draw(Globals.GetBlankTexture(), new Rectangle(0, 0, 1920, 1080), Color.White);
            spriteBatch.End();

            spriteBatch.Begin(blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointClamp, effect: shake);
            spriteBatch.Draw(renderTarget6, Vector2.Zero, new Color(Color.White, 0));


            for (int i = 0; i < scanlines.Count; i++)
            {
                spriteBatch.Draw(Globals.GetBlankTexture(), new Rectangle(0, (int)scanlines[i], 1920, scanlineThickness), new Color(Color.Black, 0.08f));

            }

            spriteBatch.Draw(Content.Load<Texture2D>("crt"), new Rectangle(0, 0, 1920, 1080), new Color(Color.White, 0.5f) * 0.7f);
            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(abberationTarget);

            spriteBatch.Begin(blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointClamp, effect: warp);
            spriteBatch.Draw(warpTarget, Vector2.Zero, new Color(Color.White, 0));
            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);

            spriteBatch.Begin(blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointClamp, effect: abberation);
            spriteBatch.Draw(abberationTarget, Vector2.Zero, new Color(Color.White, 0));
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public static BlendState LightenBlend = new BlendState
        {
            AlphaBlendFunction = BlendFunction.Max,
            ColorBlendFunction = BlendFunction.Max,
            ColorSourceBlend = Blend.One,
            ColorDestinationBlend = Blend.One,
            AlphaSourceBlend = Blend.One,
            AlphaDestinationBlend = Blend.One
        };
    }
}
