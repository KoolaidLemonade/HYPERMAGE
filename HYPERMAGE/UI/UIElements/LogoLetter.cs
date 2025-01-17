using HYPERMAGE.Helpers;
using HYPERMAGE.Managers;
using HYPERMAGE.Particles;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HYPERMAGE.UI.UIElements
{
    public class LogoLetter: Button
    {
        private int index;

        private bool grabbed;
        private Vector2 offset;
        private Vector2 originalPosition;

        private Vector2 prevPosition;
        private Vector2 velocity;

        private float timer;
        private float drawOffset;
        public LogoLetter(int index, Vector2 position, Texture2D texture) : base(texture, position)
        {
            this.index = index;

            originalPosition = position;

            timer = index;
        }

        public override void Draw()
        {
            Globals.SpriteBatch.Draw(texture, new((int)position.X, (int)(position.Y + drawOffset)), new Rectangle(27 * index, 0, 27, 44), color, 0f, Vector2.Zero, 1f, SpriteEffects.None, index >= 5 ? 0.99f : 1f);
        }

        public override void Update()
        {
            timer += 0.05f;

            drawOffset = (float)Math.Sin(timer) * 2.5f;

            if (grabbed && InputManager.LeftMouseDown)
            {
                velocity -= position - (InputManager.MousePosition - offset);
            }

            else
            {
                grabbed = false;
            }

            velocity -= (position - originalPosition) / 2;

            prevPosition = position;

            position += velocity * Globals.TotalSeconds;

            velocity /= 1.02f;

            if (position.X < GameManager.bounds.X || position.X > GameManager.bounds.Z - texture.Width / 9)
            {

                if (position.X < GameManager.bounds.X)
                {
                    if (Math.Abs(velocity.X) >= 20f)
                    {
                        for (int i = 0; i < Math.Abs(velocity.X / 70); i++)
                        {
                            ParticleData particleData = new()
                            {
                                sizeStart = 5,
                                sizeEnd = 0,
                                colorStart = Color.White,
                                colorEnd = Color.White,
                                velocity = new Vector2(velocity.X * -Globals.RandomFloat(0, 0.5f), velocity.Y),
                                lifespan = 0.5f,
                                rotationSpeed = 0.1f
                            };

                            Particle particle = new(new(0, position.Y + Globals.RandomFloat(0, texture.Height)), particleData);
                            ParticleManager.AddParticle(particle);
                        }
                    }
                }

                if (position.X > GameManager.bounds.Z - texture.Width / 9)
                {
                    if (Math.Abs(velocity.X) >= 20f)
                    {
                        for (int i = 0; i < Math.Abs(velocity.X / 70); i++)
                        {
                            ParticleData particleData = new()
                            {
                                sizeStart = 5,
                                sizeEnd = 0,
                                colorStart = Color.White,
                                colorEnd = Color.White,
                                velocity = new(velocity.X * -Globals.RandomFloat(0, 0.5f), velocity.Y),
                                lifespan = 0.5f,
                                rotationSpeed = 0.1f
                            };

                            Particle particle = new(new(320, position.Y + Globals.RandomFloat(0, texture.Height)), particleData);
                            ParticleManager.AddParticle(particle);
                        }
                    }
                }

                if (Math.Abs(velocity.X) >= 20f)
                {
                    SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("hit2"), 0.3f, Globals.RandomFloat(-0.5f, 0.5f), 0f);
                }

                if (Math.Abs(velocity.X) >= 350f)
                {
                    GameManager.AddScreenShake(0.15f, 5);
                }

                position.X = prevPosition.X;
                velocity.X *= -0.8f;
            }

            if (position.Y < GameManager.bounds.Y || position.Y > GameManager.bounds.W - texture.Height)
            {
                if (position.Y < GameManager.bounds.Y)
                {
                    if (Math.Abs(velocity.Y) >= 20f)
                    {
                        for (int i = 0; i < Math.Abs(velocity.Y / 70); i++)
                        {
                            ParticleData particleData = new()
                            {
                                sizeStart = 5,
                                sizeEnd = 0,
                                colorStart = Color.White,
                                colorEnd = Color.White,
                                velocity = new(velocity.X, velocity.Y * -Globals.RandomFloat(0, 0.5f)),
                                lifespan = 0.5f,
                                rotationSpeed = 0.1f
                            };

                            Particle particle = new(new(position.X + Globals.RandomFloat(0, texture.Width / 9), 0), particleData);
                            ParticleManager.AddParticle(particle);
                        }
                    }
                }

                if (position.Y > GameManager.bounds.W - texture.Height)
                {
                    if (Math.Abs(velocity.Y) >= 20f)
                    {
                        for (int i = 0; i < Math.Abs(velocity.Y / 70); i++)
                        {
                            ParticleData particleData = new()
                            {
                                sizeStart = 5,
                                sizeEnd = 0,
                                colorStart = Color.White,
                                colorEnd = Color.White,
                                velocity = new(velocity.X, velocity.Y * -Globals.RandomFloat(0, 0.5f)),
                                lifespan = 0.5f,
                                rotationSpeed = 0.1f
                            };

                            Particle particle = new(new(position.X + Globals.RandomFloat(0, texture.Width / 9), 180), particleData);
                            ParticleManager.AddParticle(particle);
                        }
                    }
                }

                if (Math.Abs(velocity.Y) >= 20f)
                {
                    SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("hit2"), 0.3f, Globals.RandomFloat(-0.5f, 0.5f), 0f);
                }

                if (Math.Abs(velocity.Y) >= 350f)
                {
                    GameManager.AddScreenShake(0.15f, 5);
                }

                position.Y = prevPosition.Y;
                velocity.Y *= -0.8f;
            }

            base.Update();
        }

        public override void UpdateButtonHitbox()
        {
            buttonHitbox = new Rectangle((int)position.X, (int)position.Y, texture.Width / 9, texture.Height);
        }

        public override void Clicked()
        {
            SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("bop"), 0.5f, Globals.RandomFloat(-0.2f, 0.2f), 0f);

            grabbed = true;
            offset = InputManager.MousePosition - position;
            base.Clicked();
        }
    }
}
