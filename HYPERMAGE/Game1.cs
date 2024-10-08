using HYPERMAGE.Helpers;
using HYPERMAGE.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HYPERMAGE
{
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private RenderTarget2D renderTarget;
        private RenderTarget2D renderTarget2;
        private RenderTarget2D renderTarget3;

        private Effect blur;
        private Effect waves;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void LoadContent()
        {
            blur = Content.Load<Effect>("blur");
            waves = Content.Load<Effect>("waves");
        }

        protected override void Initialize()
        {

            renderTarget = new RenderTarget2D(GraphicsDevice, 320, 180);
            renderTarget2 = new RenderTarget2D(GraphicsDevice, 320, 180);
            renderTarget3 = new RenderTarget2D(GraphicsDevice, 320, 180);

            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;
            graphics.ApplyChanges();

            spriteBatch = new SpriteBatch(GraphicsDevice);
            Globals.SpriteBatch = spriteBatch;

            Globals.Content = Content;

            GameManager.Init();

            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            Globals.Update(gameTime);

            GameManager.Update();

            base.Update(gameTime);
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

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(renderTarget);
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointClamp);
            GameManager.Draw();
            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(renderTarget2);

            blur.Parameters["offset"].SetValue(new Vector2(1f / renderTarget.Width, 0));

            spriteBatch.Begin(blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointClamp, effect: blur);
            spriteBatch.Draw(renderTarget, Vector2.Zero, new Color(Color.White, 0));
            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(renderTarget3);

            blur.Parameters["offset"].SetValue(new Vector2(0, 1f / renderTarget.Height));

            spriteBatch.Begin(blendState: LightenBlend, samplerState: SamplerState.PointClamp, effect: blur);
            spriteBatch.Draw(renderTarget2, Vector2.Zero, new Color(Color.White, 0));
            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);

            spriteBatch.Begin(blendState: LightenBlend);
            spriteBatch.Draw(renderTarget3, new Rectangle(0, 0, 1920, 1080), new Color(Color.SlateBlue, 0.5f) * 0.4f);
            spriteBatch.End();

            spriteBatch.Begin(blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(renderTarget, new Rectangle(0, 0, 1920, 1080), new Color(Color.White, 0));
            spriteBatch.End();

            spriteBatch.Begin(blendState: BlendState.AlphaBlend);
            spriteBatch.Draw(Content.Load<Texture2D>("crt"), new Rectangle(0, 0, 1920, 1080), new Color(Color.White, 0.5f) * 0.7f);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
