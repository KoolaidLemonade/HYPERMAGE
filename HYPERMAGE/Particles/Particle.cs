using HYPERMAGE.Helpers;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace HYPERMAGE.Particles
{
    // from https://github.com/LubiiiCZ/DevQuickie/tree/master/Quickie003-ParticleSystem
    public class Particle
    {
        private readonly ParticleData data;
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

        private Rectangle hitbox;
        private int width;
        private int height;

        public Particle(Vector2 pos, ParticleData data)
        {
            this.data = data;
            lifespanLeft = data.lifespan;
            lifespanAmount = 1f;
            position = pos;
            color = data.colorStart;
            opacity = data.opacityStart;
            origin = new(data.texture.Width / 2, data.texture.Height / 2);
            resistance = data.resistance;
            velocity = data.velocity;
            rotation = data.rotation;
            rotationSpeed = data.rotationSpeed;

            width = data.texture.Width;
            height = data.texture.Height;
        }
        public void Update()
        {
            hitbox = new Rectangle((int)position.X, (int)position.Y, width, height);

            lifespanLeft -= Globals.TotalSeconds;
            if (lifespanLeft <= 0f)
            {
                isFinished = true;
                return;
            }

            lifespanAmount = MathHelper.Clamp(lifespanLeft / data.lifespan, 0, 1);
            color = Color.Lerp(data.colorEnd, data.colorStart, lifespanAmount);
            opacity = MathHelper.Clamp(MathHelper.Lerp(data.opacityEnd, data.opacityStart, lifespanAmount), 0, 1);
            scale = MathHelper.Lerp(data.sizeEnd, data.sizeStart, lifespanAmount) / data.texture.Width;
            position += velocity * Globals.TotalSeconds;

            velocity /= resistance;

            rotation += rotationSpeed;
        }

        public void Draw()
        {
            Globals.SpriteBatch.Draw(data.texture, position, null, color * opacity, rotation, origin, scale, SpriteEffects.None, 0.7f);
        }
    }
}