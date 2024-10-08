using HYPERMAGE.Helpers;
using HYPERMAGE.Managers;
using HYPERMAGE.Spells;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// i love megaclassing

namespace HYPERMAGE.Models
{
    public class Mob
    {
        public Texture2D _texture;
        public Animation _anim;
        public Vector2 _position;
        public int _aiType;
        public float _speed;
        public float _health;
        public bool active;
        public Vector2 _origin;

        public Polygon hitbox;

        public Vector2 center;

        public Vector2 _velocity;

        public int width;
        public int height;
        public float rotation = 0;
        public float scale = 1f;

        private List <Projectile> projectileImmunity = [];
        private int immunityFrames = 20;
        public Mob(Texture2D texture, Animation anim, Vector2 position, int aiType, float speed, float health)
        {
            _texture = texture;
            _anim = anim;
            _position = position;
            _aiType = aiType;
            _speed = speed;
            _health = health;

            width = anim.frameWidth;
            height = anim.frameHeight;
            _origin = new Vector2(anim.frameWidth / 2, anim.frameHeight / 2);


            active = true;
        }

        public Mob(Texture2D texture, Vector2 position, int aiType, float speed, float health)
        {
            _texture = texture;
            _position = position;
            _aiType = aiType;
            _speed = speed;
            _health = health;

            width = texture.Width;
            height = texture.Height;

            _origin = new(texture.Width / 2, texture.Height / 2);

            active = true;
        }
        public void Draw()
        {
            if (_anim != null)
            {
                _anim.Draw(_position);
            }
            else
            {
                Globals.SpriteBatch.Draw(_texture, _position, null, Color.White, rotation, _origin, scale, SpriteEffects.None, 1f);
            }
        }

        private int timer = 0;
        private bool charging = false;
        public void Update(Player player)
        {
            if (projectileImmunity != null)
            {
                foreach (Projectile p in projectileImmunity.ToList())
                {
                    p.immunityFrameCounter++;

                    if (p.immunityFrameCounter > immunityFrames)
                    {
                        p.immunityFrameCounter = 0;
                        projectileImmunity.Remove(p);
                    }
                }
            }

            switch (_aiType)
            {
                case 0:
                    return;
                case 1: //bat
 
                    timer++;

                    hitbox = PolygonFactory.CreateRectangle((int)center.X, (int)center.Y, width, height);

                    center = new Vector2(_position.X + width / 2, _position.Y + width / 2);

                    if (Math.Abs(player.center.X - center.X) < 20 && Math.Abs(player.center.Y - center.Y) < 20)
                    {
                        if (!charging && timer > 200)
                        {
                            charging = true;
                            timer = 0;
                        }
                    }

                    if (_health <= 0)
                    {
                        active = false;
                    }

                    if (charging)
                    {
                        _velocity += Vector2.Normalize(player.center - center) * Globals.TotalSeconds * _speed * 6;

                        if(timer > 15)
                        {
                            charging = false;
                            timer = 0;
                        }
                    }
                    else
                    {
                        _velocity += (Vector2.Normalize(player.center - center) + new Vector2(Globals.RandomFloat(-1.5f, 1.5f), Globals.RandomFloat(-1.5f, 1.5f))) * Globals.TotalSeconds * _speed;
                    }

                    _velocity /= 1.05f;

                    _position += _velocity;

                    if (_position.X > 320 - 11)
                    {
                        _position.X = 320 - 11;
                    }

                    if (_position.X < 0)
                    {
                        _position.X = 0;
                    }

                    if (_position.Y > 180 - 6)
                    {
                        _position.Y = 180 - 6;
                    }

                    if (_position.Y < 33)
                    {
                        _position.Y = 33;
                    }

                    _anim.Update();

                    return;

            }
        }
        public void DamagedBy(Projectile projectile)
        {
            projectileImmunity.Add(projectile);
        }
        public bool CanBeDamagedBy(Projectile projectile)
        {
            if (projectileImmunity != null && projectileImmunity.Contains(projectile))
            {
                return false;
            }

            return true;
        }
    }
}
