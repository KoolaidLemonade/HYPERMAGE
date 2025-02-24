using HYPERMAGE.Helpers;
using HYPERMAGE.Managers;
using HYPERMAGE.Models;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace HYPERMAGE.Particles
{
    // from https://github.com/LubiiiCZ/DevQuickie/tree/master/Quickie003-ParticleSystem
    public class Particle
    {
        private readonly ParticleData data;
        private Animation anim;
        private Texture2D texture;
        private Vector2 position;
        private Vector2 velocity;
        private float lifespanLeft;
        private float lifespanAmount;
        private Color color;
        private float opacity;
        public bool isFinished = false;
        private float scale;
        private Vector2 origin;
        private float resistance;
        private float rotation;
        private float rotationSpeed;
        private bool fastScale;

        private bool spawnIndicator;
        private bool manaDrop;

        private bool flashing;
        private float flashingTimer;

        public bool friendly;

        private int width;
        private int height;

        private Vector2 center;
        private Polygon hitbox;
        public Particle(Vector2 pos, ParticleData data)
        {
            this.data = data;

            anim = data.anim;

            if (anim != null)
            {
                texture = data.texture;
            }

            lifespanLeft = data.lifespan;
            lifespanAmount = 1f;
            position = pos;
            color = data.colorStart;
            opacity = data.opacityStart;
            resistance = data.resistance;
            velocity = data.velocity;
            rotation = data.rotation;
            rotationSpeed = data.rotationSpeed;

            flashing = data.flashing;
            fastScale = data.fastScale;
            friendly = data.friendly;

            spawnIndicator = data.spawnIndicator;
            manaDrop = data.manaDrop;

            if (anim != null)
            {
                width = anim.frameWidth;
                height = anim.frameHeight;
            }

            else
            {
                width = data.texture.Width;
                height = data.texture.Height;
            }

            origin = new(width / 2f, height / 2f);

            center = position + origin;

            hitbox = PolygonFactory.CreateRectangle((int)position.X, (int)position.Y, (int)(width * scale), (int)(height * scale), rotation);
        }
        public void Update()
        {
            lifespanLeft -= Globals.TotalSeconds;

            position += velocity * Globals.TotalSeconds;

            center = position + origin;

            if (lifespanLeft <= 0f)
            {
                Kill();

                return;
            }

            lifespanAmount = MathHelper.Clamp(lifespanLeft / data.lifespan, 0, 1);

            color = Color.Lerp(data.colorEnd, data.colorStart, lifespanAmount);
            opacity = MathHelper.Clamp(MathHelper.Lerp(data.opacityEnd, data.opacityStart, lifespanAmount), 0, 1);

            if (fastScale)
            {
                scale = Globals.NonLerp(data.sizeEnd, data.sizeStart, lifespanAmount);

            }

            else
            {
                scale = MathHelper.Lerp(data.sizeEnd, data.sizeStart, lifespanAmount);
            }

            velocity /= resistance;

            rotation += rotationSpeed;

            if (flashing)
            {
                flashingTimer += Globals.TotalSeconds;
                
                if (flashingTimer > 0.1f)
                {
                    color = Color.White;
                }

                if (flashingTimer > 0.2f)
                {
                    flashingTimer = 0;
                }
            }

            if (manaDrop)
            {
                Debug.WriteLine(origin);
                hitbox = PolygonFactory.CreateRectangle((int)position.X, (int)position.Y, (int)(width * scale), (int)(height * scale), rotation);

                if (Globals.Distance(GameManager.GetPlayer().center, position) < 25f)
                {
                    velocity += Vector2.Normalize(position.DirectionTo(GameManager.GetPlayer().center)) * Globals.TotalSeconds * 350f;
                }

                if (hitbox.IntersectsWith(GameManager.GetPlayer().hitbox))
                {
                    SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("ding"), 1f, Globals.RandomFloat(-0.5f, 0f), 0f);
                    GameManager.GetPlayer().mana++;
                    Kill();
                }
            }

            if (anim != null)
            {
                anim.Update();
            }
        }

        public void Draw()
        {
            if (anim != null)
            {
                anim.Draw(position, color * opacity, rotation, origin, scale, SpriteEffects.None, 0.7f);
            }

            else
            {
                Globals.SpriteBatch.Draw(data.texture, position, null, color * opacity, rotation, origin, scale, SpriteEffects.None, 0.7f);
            }
        }

        public void Kill()
        {
            if (spawnIndicator)
            {
                SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("enemyspawning"), 1f, 0f, 0f);
            }

            if (manaDrop)
            {
                for (int i = 0; i < 4; i++)
                {
                    ParticleData deathParticleData = new()
                    {
                        opacityStart = 1f,
                        opacityEnd = 1f,
                        sizeStart = 4,
                        sizeEnd = 0,
                        colorStart = Color.White,
                        colorEnd = Color.Blue,
                        velocity = new(Globals.RandomFloat(-100, 100), Globals.RandomFloat(-100, 100)),
                        lifespan = 0.2f,
                        rotationSpeed = Globals.RandomFloat(-0.5f, 0.5f)
                    };

                    Particle deathParticle = new(center, deathParticleData);
                    ParticleManager.AddParticle(deathParticle);
                }
            }

            isFinished = true;
        }
    }
}