using HYPERMAGE.Helpers;
using HYPERMAGE.Managers;
using HYPERMAGE.Particles;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// i love megaclassing

namespace HYPERMAGE.Models
{
    public class Projectile
    {
        public Texture2D _texture;
        public Animation _anim;
        public Vector2 _position;
        public int _aiType;
        public float _speed;
        public float _damage;
        public float _lifespan;
        public float maxLifespan;
        public float _rotation;
        public float _scale;
        public Vector2 _origin;
        public bool _friendly;
        public int width;
        public int height;

        public Polygon hitbox;

        public Vector2 center;

        public Vector2 _velocity;

        public bool active = true;

        public int immunityFrameCounter;
        private int ai = 0;
        private int ai2 = 0;
        private double angle = 0;

        //add chaining
        public Projectile(Texture2D texture, Animation anim, Vector2 position, int aiType, float speed, float damage, Vector2 velocity, float lifespan, float rotation, float scale, bool friendly, int ai, int ai2)
        {
            _texture = texture;
            _anim = anim;
            _position = position;
            _aiType = aiType;
            _speed = speed;
            _damage = damage;
            _velocity = velocity;
            _lifespan = lifespan;
            _rotation = rotation;
            _scale = scale;
            _friendly = friendly;

            maxLifespan = lifespan;

            width = anim.frameWidth;
            height = anim.frameHeight;

            _origin = new(anim.frameWidth / 2, anim.frameHeight / 2);

            active = true;
        }

        public Projectile(Texture2D texture, Vector2 position, int aiType, float speed, float damage, Vector2 velocity, float lifespan, float rotation, float scale, bool friendly, int ai, int ai2)
        {
            _texture = texture;
            _position = position;
            _aiType = aiType;
            _speed = speed;
            _damage = damage;
            _velocity = velocity;
            _lifespan = lifespan;
            _rotation = rotation;
            _scale = scale;
            _friendly = friendly;

            maxLifespan = lifespan;

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
                Globals.SpriteBatch.Draw(_texture, _position, null, Color.White, _rotation, _origin, _scale, SpriteEffects.None, 0f);
            }
        }
        public void Update(Player player)
        {
            hitbox = PolygonFactory.CreateRectangle((int)_position.X, (int)_position.Y, (int)(width * _scale), (int)(height * _scale), _rotation);

            center = new Vector2(_position.X + width / 2, _position.Y + height / 2);

            if(_lifespan <= 0)
            {
                active = false;
            }

            _lifespan--;

            switch (_aiType)
            {
                case 0:
                    return;
                case 1: //firebolt

                    _position += ((_velocity * Globals.TotalSeconds) * _speed);

                    _rotation += 1.05f;

                    if (_lifespan % 2 == 0)
                    {
                        ParticleData projParticleData = new()
                        {
                            opacityStart = .8f,
                            opacityEnd = 0.1f,
                            sizeStart = 5,
                            sizeEnd = 0,
                            colorStart = Color.Gold,
                            colorEnd = Color.Red,
                            velocity = new(Globals.RandomFloat(-50, 50), Globals.RandomFloat(-50, 50)),
                            lifespan = 0.5f,
                            rotationSpeed = Globals.RandomFloat(-0.5f, 0.5f)
                        };

                        Particle projParticle = new(_position, projParticleData);
                        ParticleManager.AddParticle(projParticle);
                    }

                    for (int i = 0; i < MobManager.mobs.Count; i++)
                    {
                        if (MobManager.mobs[i].hitbox.IntersectsWith(hitbox) && _friendly)
                        {
                            MobManager.mobs[i]._velocity += _velocity / 100;
                            MobManager.mobs[i]._health -= _damage;

                            Explode(5, 200, center, 5, 1);
                        }
                    }
                    
                    if (!Globals.InBounds(hitbox))
                    {
                        Explode(5, 200, center, 5, 1);
                    }

                    return;

                case 2: //fireball
                    _position += ((_velocity * Globals.TotalSeconds) * _speed);

                    _rotation += 1.05f;

                    if (_lifespan % 2 == 0)
                    {
                        ParticleData projParticleData = new()
                        {
                            opacityStart = .8f,
                            opacityEnd = 0.1f,
                            sizeStart = 5,
                            sizeEnd = 0,
                            colorStart = Color.Gold,
                            colorEnd = Color.DarkRed,
                            velocity = new(Globals.RandomFloat(-100, 100), Globals.RandomFloat(-100, 100)),
                            lifespan = 0.5f,
                            rotationSpeed = Globals.RandomFloat(-0.5f, 0.5f)
                        };

                        Particle projParticle = new(_position, projParticleData);
                        ParticleManager.AddParticle(projParticle);

                        ParticleData projParticleData2 = new()
                        {
                            opacityStart = 1f,
                            opacityEnd = 1f,
                            sizeStart = 10,
                            sizeEnd = 0,
                            colorStart = Color.White,
                            colorEnd = Color.White,
                            velocity = new(Globals.RandomFloat(-100, 100), Globals.RandomFloat(-100, 100)),
                            lifespan = 0.2f,
                            rotationSpeed = Globals.RandomFloat(-0.5f, 0.5f)
                        };

                        Particle projParticle2 = new(_position, projParticleData2);
                        ParticleManager.AddParticle(projParticle2);
                    }

                    for (int i = 0; i < MobManager.mobs.Count; i++)
                    {
                        if (MobManager.mobs[i].hitbox.IntersectsWith(hitbox) && _friendly)
                        {
                            MobManager.mobs[i]._velocity += _velocity / 100;
                            MobManager.mobs[i]._health -= _damage;

                            Explode(10, 300, center, 50, 2);
                        }
                    }

                    if (!Globals.InBounds(hitbox))
                    {
                        Explode(10, 300, center, 50, _damage);
                    }

                    return;

                case 3: //kindle

                    _velocity += new Vector2(Globals.RandomFloat(-5, 5), Globals.RandomFloat(-5, 5));
                    _velocity /= 1.05f;

                    _position += ((_velocity * Globals.TotalSeconds) * _speed);

                    if (_lifespan % 3 == 0)
                    {
                        ParticleData projParticleData = new()
                        {
                            opacityStart = 1f,
                            opacityEnd = 0f,
                            sizeStart = 5,
                            sizeEnd = 2,
                            colorStart = Color.White,
                            colorEnd = Color.Gold,
                            velocity = new(Globals.RandomFloat(-30, 30), Globals.RandomFloat(-100, 0)),
                            lifespan = 0.5f,
                            rotationSpeed = Globals.RandomFloat(-0.5f, 0.5f)
                        };

                        Particle projParticle = new(_position, projParticleData);
                        ParticleManager.AddParticle(projParticle);
                    }

                    for (int i = 0; i < MobManager.mobs.Count; i++)
                    {
                        if (MobManager.mobs[i].hitbox.IntersectsWith(hitbox) && _friendly && _lifespan % 10 == 0)
                        {
                            ParticleData projParticleData2 = new()
                            {
                                opacityStart = .8f,
                                opacityEnd = 0.1f,
                                sizeStart = 5,
                                sizeEnd = 0,
                                colorStart = Color.Red,
                                colorEnd = Color.DarkRed,
                                velocity = new(Globals.RandomFloat(-200, 200), Globals.RandomFloat(-200, 200)),
                                lifespan = 0.5f,
                                rotationSpeed = Globals.RandomFloat(-0.5f, 0.5f)
                            };

                            Particle projParticle2 = new(_position, projParticleData2);
                            ParticleManager.AddParticle(projParticle2);

                            MobManager.mobs[i]._health -= _damage;
                        }
                    }

                    return;
                case 4: //bladeofflame
                    {
                        if (ai == 0)
                        {
                            angle = Math.Atan2(InputManager.MousePosition.Y - player.center.Y, InputManager.MousePosition.X - player.center.X) * 180 / Math.PI;
                            ai++;
                        }

                        float radius = 30;

                        Vector2 newCenter = new Vector2((float)(player.center.X + radius * Math.Cos(angle * Math.PI / 180)), (float)(player.center.Y + radius * Math.Sin(angle * Math.PI / 180)));

                        _position = newCenter.RotatedBy(MathHelper.ToRadians(Globals.NonLerp(-90, 145, _lifespan / maxLifespan)), player.center);

                        Vector2 directionToPlayer = Vector2.Normalize(player.center - _position);

                        _rotation = (float)(Math.Atan2(directionToPlayer.Y, directionToPlayer.X)) + MathHelper.ToRadians(270);

                        ParticleData projParticleData = new()
                        {
                            opacityStart = 1f,
                            opacityEnd = 0f,
                            sizeStart = 2,
                            sizeEnd = 1,
                            colorStart = Color.Yellow,
                            colorEnd = Color.Red,
                            velocity = new Vector2(Globals.RandomFloat(-20, 20), Globals.RandomFloat(-20, 20)),
                            lifespan = 0.25f,
                            rotationSpeed = Globals.RandomFloat(-0.5f, 0.5f)
                        };

                        ParticleData projParticleData2 = new()
                        {
                            opacityStart = 0.2f,
                            opacityEnd = 0f,
                            sizeStart = 3,
                            sizeEnd = 10,
                            colorStart = Color.Gray,
                            colorEnd = Color.DarkRed,
                            velocity = new Vector2(Globals.RandomFloat(-5, 5), Globals.RandomFloat(-5, 5)),
                            lifespan = 1f,
                            rotationSpeed = Globals.RandomFloat(-0.1f, 0.1f)
                        };


                        for (int i = 0; i < MobManager.mobs.Count; i++)
                        {
                            if (MobManager.mobs[i].hitbox.IntersectsWith(hitbox) && _friendly && MobManager.mobs[i].CanBeDamagedBy(this))
                            {
                                MobManager.mobs[i]._velocity += Vector2.Normalize(MobManager.mobs[i].center - player.center) * 3;
                                MobManager.mobs[i]._health -= _damage;
                                MobManager.mobs[i].DamagedBy(this);

                                for (int j = 0; j < 3; j++)
                                {
                                    ParticleData projParticleData3 = new()
                                    {
                                        opacityStart = 1f,
                                        opacityEnd = 0f,
                                        sizeStart = 8,
                                        sizeEnd = 1,
                                        colorStart = Color.White,
                                        colorEnd = Color.White,
                                        velocity = Vector2.Normalize(MobManager.mobs[i].center - player.center).RotatedBy(MathHelper.ToRadians(Globals.RandomFloat(-45, 45))) * Globals.RandomFloat(300, 500),
                                        lifespan = 0.25f,
                                        rotationSpeed = Globals.RandomFloat(-0.5f, 0.5f)
                                    };

                                    Particle projParticle = new(MobManager.mobs[i].center, projParticleData3);
                                    ParticleManager.AddParticle(projParticle);
                                }

                                GameManager.AddHitstop(7);
                            }
                        }

                        for (int i = 0; i < 3; i++)
                        {
                            Vector2 particleCenter = new Vector2((float)(player.center.X + ((radius - 7) * Globals.RandomFloat(1, 2f)) * Math.Cos(angle * Math.PI / 180)), (float)(player.center.Y + ((radius - 7) * Globals.RandomFloat(1, 2f)) * Math.Sin(angle * Math.PI / 180)));
                            Particle projParticle = new(particleCenter.RotatedBy(MathHelper.ToRadians(Globals.NonLerp(-90, 145, _lifespan / maxLifespan)), player.center), projParticleData);
                            Particle projParticle2 = new(particleCenter.RotatedBy(MathHelper.ToRadians(Globals.NonLerp(-90, 145, _lifespan / maxLifespan)), player.center), projParticleData2);
                            ParticleManager.AddParticle(projParticle);

                            if (i == 0 && _lifespan % 2 == 0)
                            {
                                ParticleManager.AddParticle(projParticle2);
                            }
                        }

                        return;
                    }
            }
        }
        public void Explode(int particleCount, float spread, Vector2 center, float range, float damage)
        {
            for (int i = 0; i < MobManager.mobs.Count; i++)
            {
                if (MobManager.mobs[i].hitbox.IntersectsWith(PolygonFactory.CreateRectangle((int)(center.X - range / 2), (int)(center.Y - range / 2), (int)range, (int)range)))
                {
                    MobManager.mobs[i]._health -= damage;
                    MobManager.mobs[i]._velocity += Vector2.Normalize(MobManager.mobs[i].center - center) * damage;
                }
            }


            for (int j = 0; j < particleCount; j++)
            {
                ParticleData projExplosionParticleData = new()
                {
                    opacityStart = 1f,
                    opacityEnd = 0f,
                    sizeStart = 5,
                    sizeEnd = 10,
                    colorStart = Color.Gray,
                    colorEnd = Color.Black,
                    velocity = new(Globals.RandomFloat(-spread, spread), Globals.RandomFloat(-spread, spread)),
                    lifespan = 0.5f,
                    rotationSpeed = Globals.RandomFloat(-0.05f, 0.05f),
                    resistance = Globals.RandomFloat(1.05f, 2f)
                };

                Particle projExplosionParticle = new(center, projExplosionParticleData);
                ParticleManager.AddParticle(projExplosionParticle);

                ParticleData projExplosionSparksParticleData = new()
                {
                    opacityStart = 1f,
                    opacityEnd = 0.7f,
                    sizeStart = 3,
                    sizeEnd = 0,
                    colorStart = Color.White,
                    colorEnd = Color.Gold,
                    velocity = new(Globals.RandomFloat(-spread, spread), Globals.RandomFloat(-spread, spread)),
                    lifespan = 0.3f,
                    rotationSpeed = Globals.RandomFloat(-0.5f, 0.5f)
                };

                Particle projExplosionSparkParticle = new(center, projExplosionSparksParticleData);
                ParticleManager.AddParticle(projExplosionSparkParticle);
            }

            active = false;
        }
    }
}
