using HYPERMAGE.Helpers;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace HYPERMAGE.Particles
{
    // from https://github.com/LubiiiCZ/DevQuickie/tree/master/Quickie003-ParticleSystem
    public class Particle
    {
        private readonly ParticleData _data;
        private Vector2 _position;
        private Vector2 _velocity;
        private float _lifespanLeft;
        private float _lifespanAmount;
        private Color _color;
        private float _opacity;
        public bool isFinished = false;
        private float _scale;
        private Vector2 _origin;
        private float _resistance;
        private float _rotation;
        private float _rotationSpeed;

        private Rectangle hitbox;
        private int width;
        private int height;

        public Particle(Vector2 pos, ParticleData data)
        {
            _data = data;
            _lifespanLeft = data.lifespan;
            _lifespanAmount = 1f;
            _position = pos;
            _color = data.colorStart;
            _opacity = data.opacityStart;
            _origin = new(_data.texture.Width / 2, _data.texture.Height / 2);
            _resistance = data.resistance;
            _velocity = data.velocity;
            _rotation = data.rotation;
            _rotationSpeed = data.rotationSpeed;

            width = data.texture.Width;
            height = data.texture.Height;

        }
        public void Update()
        {
            hitbox = new Rectangle((int)_position.X, (int)_position.Y, width, height);

            _lifespanLeft -= Globals.TotalSeconds;
            if (_lifespanLeft <= 0f)
            {
                isFinished = true;
                return;
            }

            _lifespanAmount = MathHelper.Clamp(_lifespanLeft / _data.lifespan, 0, 1);
            _color = Color.Lerp(_data.colorEnd, _data.colorStart, _lifespanAmount);
            _opacity = MathHelper.Clamp(MathHelper.Lerp(_data.opacityEnd, _data.opacityStart, _lifespanAmount), 0, 1);
            _scale = MathHelper.Lerp(_data.sizeEnd, _data.sizeStart, _lifespanAmount) / _data.texture.Width;
            _position += _velocity * Globals.TotalSeconds;

            _velocity /= _resistance;

            _rotation += _rotationSpeed;
        }

        public void Draw()
        {
            Globals.SpriteBatch.Draw(_data.texture, _position, null, _color * _opacity, _rotation, _origin, _scale, SpriteEffects.None, 1f);
        }
    }
}