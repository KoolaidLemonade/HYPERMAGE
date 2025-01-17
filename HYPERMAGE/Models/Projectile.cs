using HYPERMAGE.Helpers;
using HYPERMAGE.Managers;
using HYPERMAGE.Particles;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HYPERMAGE.Models
{
    public class Projectile
    {
        public Texture2D texture;
        public Animation anim;
        public Vector2 position;
        public int aiType;
        public float maxLifespan;
        public float rotation;
        public Vector2 origin;
        public bool friendly;
        public int width;
        public int height;

        public Polygon hitbox;

        public Vector2 center;

        public Vector2 velocity;

        public bool active = true;

        public int immunityFrameCounter;
        public int immunityFrames;

        public float speed;
        public float damage;
        public float lifespan;
        public float scale;
        public int pierce;

        private float ai;
        private float ai2;

        private double angle = 0;

        public Projectile(Vector2 position, int aiType, float speed, float damage, int pierce, float lifespan) : this(position, aiType, speed, damage, pierce, Vector2.Zero, lifespan, 60, 0f, 1f, true, 0, 0)
        {

        }
        public Projectile(Vector2 position, int aiType, float speed, float damage, Vector2 velocity, float lifespan) : this(position, aiType, speed, damage, 0, velocity, lifespan, 60, 0f, 1f, true, 0, 0)
        {

        }
        public Projectile(Vector2 position, int aiType, float speed, float damage, int pierce, Vector2 velocity, float lifespan) : this(position, aiType, speed, damage, pierce, velocity, lifespan, 60, 0f, 1f, true, 0, 0)
        {

        }
        public Projectile(Vector2 position, int aiType, float speed, float damage, int pierce, Vector2 velocity, float lifespan, float size) : this(position, aiType, speed, damage, pierce, velocity, lifespan, 60, 0f, size, true, 0, 0)
        {

        }
        public Projectile(Vector2 position, int aiType, float speed, float damage, int pierce, float lifespan, int immunityFrames) : this(position, aiType, speed, damage, pierce, Vector2.Zero, lifespan, immunityFrames, 0f, 1f, true, 0, 0)
        {

        }
        public Projectile(Vector2 position, int aiType, float speed, float damage, int pierce, float lifespan, int immunityFrames, float scale) : this(position, aiType, speed, damage, pierce, Vector2.Zero, lifespan, immunityFrames, 0f, scale, true, 0, 0)
        {

        }
        public Projectile(Vector2 position, int aiType, float speed, float damage, int pierce, Vector2 velocity, float lifespan, int immunityFrames) : this(position, aiType, speed, damage, pierce, velocity, lifespan, immunityFrames, 0f, 1f, true, 0, 0)
        {

        }
        public Projectile(Vector2 position, int aiType, float speed, float damage, int pierce, Vector2 velocity, float lifespan, int immunityFrames, float scale) : this(position, aiType, speed, damage, pierce, velocity, lifespan, immunityFrames, 0f, scale, true, 0, 0)
        {

        }
        public Projectile(Vector2 position, int aiType, float speed, float damage, int pierce, Vector2 velocity, float lifespan, int immunityFrames, float rotation, float scale) : this(position, aiType, speed, damage, pierce, velocity, lifespan, immunityFrames, rotation, scale, true, 0, 0)
        {
        }
        public Projectile(Vector2 position, int aiType, float speed, float damage, int pierce, Vector2 velocity, float lifespan, int immunityFrames, float rotation, float scale, bool friendly, float ai, float ai2)
        {
            this.position = position;
            this.aiType = aiType;
            this.speed = speed;
            this.damage = damage;
            this.velocity = velocity;
            this.lifespan = lifespan;
            this.rotation = rotation;
            this.scale = scale;
            this.friendly = friendly;
            this.pierce = pierce;
            this.friendly = friendly;
            this.immunityFrames = immunityFrames;

            this.ai = ai;
            this.ai2 = ai2;

            maxLifespan = lifespan;

            switch (aiType)
            {
                case 0:
                    break;
                case 1: //firebolt
                    texture = Globals.Content.Load<Texture2D>("particle");
                    break;
                case 2: //fireball
                    texture = Globals.Content.Load<Texture2D>("particle");
                    break;
                case 3: //kindle
                    texture = Globals.Content.Load<Texture2D>("particle");
                    break;
                case 4: //blade of flame
                    texture = Globals.Content.Load<Texture2D>("bladeofflame");
                    break;
                case 5: //disintegrate
                    texture = Globals.GetBlankTexture();
                    break;

            }

            if (anim != null)
            {
                width = anim.frameWidth;
                height = anim.frameHeight;
            }

            else
            {
                width = texture.Width;
                height = texture.Height;
            }

            origin = new(width / 2, height / 2);

            active = true;

            hitbox = PolygonFactory.CreateRectangle((int)position.X, (int)position.Y, (int)(width * scale), (int)(height * scale), rotation);
        }

        // ****************************************** //
        public void Draw()
        {
            if (anim != null)
            {
                anim.Draw(position);
            }
            else
            {
                if (aiType == 5 && ai == 1)
                {
                    Globals.SpriteBatch.Draw(Globals.GetBlankTexture(), new Rectangle((int)position.X - 20 + Globals.Random.Next(3), 0, 40 + Globals.Random.Next(3), (int)position.Y + 2), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.8f);
                    Globals.SpriteBatch.Draw(Globals.GetBlankTexture(), new Rectangle((int)position.X - 25 + Globals.Random.Next(3), 0, 50 + Globals.Random.Next(3), (int)position.Y + 2), null, Color.Red, 0f, Vector2.Zero, SpriteEffects.None, 0.699f);
                    return;
                }

                Globals.SpriteBatch.Draw(texture, position, null, Color.White, rotation, origin, scale, SpriteEffects.None, 1f);
            }
        }

        Mob closestMob;
        List<float> mobDist = [];
        public void Update()
        {
            hitbox = PolygonFactory.CreateRectangle((int)position.X, (int)position.Y, (int)(width * scale), (int)(height * scale), rotation);
            center = position + origin;

            mobDist.Clear();

            if (MobManager.mobs.Count > 0)
            {
                for (int i = 0; i < MobManager.mobs.Count; i++)
                {
                    mobDist.Add(Globals.Distance(MobManager.mobs[i].center, center));
                    mobDist.Sort();

                    if (Globals.Distance(MobManager.mobs[i].center, center) == mobDist[0])
                    {
                        closestMob = MobManager.mobs[i];
                    }
                }
            }

            else
            {
                closestMob = null;
            }

            if (lifespan <= 0)
            {
                Kill();
            }

            switch (aiType)
            {
                case 0:
                    break;
                case 1: //firebolt
                    position += ((velocity * Globals.TotalSeconds) * speed);

                    rotation += 1.05f;

                    if (Globals.Random.Next(3) == 0)
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

                        Particle projParticle = new(position, projParticleData);
                        ParticleManager.AddParticle(projParticle);
                    }
                    
                    if (!Globals.InBounds(hitbox))
                    {
                        Kill();
                    }

                    break;

                case 2: //fireball
                    position += ((velocity * Globals.TotalSeconds) * speed);

                    rotation += 1.05f;

                    if (Globals.Random.Next(3) == 0)
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

                        Particle projParticle = new(position, projParticleData);
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

                        Particle projParticle2 = new(position, projParticleData2);
                        ParticleManager.AddParticle(projParticle2);
                    }

                    if (!Globals.InBounds(hitbox))
                    {
                        Kill();
                    }

                    break;  

                case 3: //kindle

                    if (closestMob != null)
                    {
                        if (Globals.Distance(closestMob.center, center) <= 15)
                        {
                            velocity += (closestMob.center - center);
                        }
                    }

                    velocity += new Vector2(Globals.RandomFloat(-5, 5), Globals.RandomFloat(-5, 5));
                    velocity /= 1.05f;

                    position += ((velocity * Globals.TotalSeconds) * speed);

                    if (Globals.Random.Next(3) == 0)
                    {
                        ParticleData projParticleData = new()
                        {
                            opacityStart = 1f,
                            opacityEnd = 0f,
                            sizeStart = 5 * scale,
                            sizeEnd = 2,
                            colorStart = Color.White,
                            colorEnd = Color.Gold,
                            velocity = new(Globals.RandomFloat(-30, 30), Globals.RandomFloat(-100, 0)),
                            lifespan = 0.5f,
                            rotationSpeed = Globals.RandomFloat(-0.5f, 0.5f)
                        };

                        Particle projParticle = new(position, projParticleData);
                        ParticleManager.AddParticle(projParticle);
                    }

                    break;
                case 4: //bladeofflame
                    {
                        if (ai == 0)
                        {
                            angle = Math.Atan2(InputManager.MousePosition.Y - GameManager.GetPlayer().center.Y, InputManager.MousePosition.X - GameManager.GetPlayer().center.X) * 180 / Math.PI;
                            ai++;
                        }

                        float radius = 30;

                        Vector2 newCenter = new Vector2((float)(GameManager.GetPlayer().center.X + radius * Math.Cos(angle * Math.PI / 180)), (float)(GameManager.GetPlayer().center.Y + radius * Math.Sin(angle * Math.PI / 180)));

                        position = newCenter.RotatedBy(MathHelper.ToRadians(Globals.NonLerp(-90, 120, lifespan / maxLifespan)), GameManager.GetPlayer().center);

                        Vector2 directionToPlayer = Vector2.Normalize(GameManager.GetPlayer().center - position);

                        rotation = (float)(Math.Atan2(directionToPlayer.Y, directionToPlayer.X)) + MathHelper.ToRadians(270);

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

                        for (int i = 0; i < 3; i++)
                        {
                            Vector2 particleCenter = new Vector2((float)(GameManager.GetPlayer().center.X + ((radius - 7) * Globals.RandomFloat(1, 2f)) * Math.Cos(angle * Math.PI / 180)), (float)(GameManager.GetPlayer().center.Y + ((radius - 7) * Globals.RandomFloat(1, 2f)) * Math.Sin(angle * Math.PI / 180)));
                            Particle projParticle = new(particleCenter.RotatedBy(MathHelper.ToRadians(Globals.NonLerp(-90, 120, lifespan / maxLifespan)), GameManager.GetPlayer().center), projParticleData);
                            Particle projParticle2 = new(particleCenter.RotatedBy(MathHelper.ToRadians(Globals.NonLerp(-90, 120, lifespan / maxLifespan)), GameManager.GetPlayer().center), projParticleData2);
                            ParticleManager.AddParticle(projParticle);

                            if (i == 0 && lifespan % 2 == 0)
                            {
                                ParticleManager.AddParticle(projParticle2);
                            }
                        }

                        break;
                    }
                case 5: // disintegrate
                    {
                        if (ai == 0)
                        {
                            if (lifespan == maxLifespan)
                            {
                                for (int i = 0; i < 10; i++)
                                {
                                    ParticleData projParticleData = new()
                                    {
                                        opacityStart = 1f,
                                        opacityEnd = 0f,
                                        sizeStart = 5,
                                        sizeEnd = 2,
                                        colorStart = Color.White,
                                        colorEnd = Color.White,
                                        velocity = new(0, Globals.RandomFloat(0, 1500)),
                                        lifespan = 0.5f,
                                        rotationSpeed = Globals.RandomFloat(-0.5f, 0.5f),
                                        resistance = 1.5f
                                    };

                                    Particle projParticle = new(new(position.X + Globals.RandomFloat(-20, 20), position.Y), projParticleData);
                                    ParticleManager.AddParticle(projParticle);
                                }
                            }
                        }

                        if (ai == 1)

                        {
                            position.X += Globals.Random.Next(-1, 2);

                            ParticleData projParticleData = new()
                            {
                                opacityStart = 1f,
                                opacityEnd = 1f,
                                sizeStart = 30,
                                sizeEnd = 0,
                                colorStart = Color.White,
                                colorEnd = Color.White,
                                velocity = new(Globals.RandomFloat(-600, 600), Globals.RandomFloat(-600, 0)),
                                lifespan = 0.15f,
                                rotationSpeed = Globals.RandomFloat(-0.5f, 0.5f),
                                resistance = 1.1f
                            };

                            Particle projParticle = new(new(position.X + Globals.RandomFloat(-20, 20), position.Y), projParticleData);
                            ParticleManager.AddParticle(projParticle);

                            ParticleData projParticleData2 = new()
                            {
                                opacityStart = 1f,
                                opacityEnd = 1f,
                                sizeStart = 15,
                                sizeEnd = 0,
                                colorStart = Color.White,
                                colorEnd = Color.White,
                                velocity = new(Globals.RandomFloat(-500, 500), 0),
                                lifespan = 0.25f,
                                rotationSpeed = Globals.RandomFloat(-0.5f, 0.5f),
                                resistance = 1.1f
                            };

                            Particle projParticle2 = new(new(position.X + Globals.RandomFloat(-20, 20), position.Y), projParticleData2);
                            ParticleManager.AddParticle(projParticle2);

                            ParticleData projParticleData3 = new()
                            {
                                opacityStart = 1f,
                                opacityEnd = 1f,
                                sizeStart = 25,
                                sizeEnd = 0,
                                colorStart = Color.White,
                                colorEnd = Color.White,
                                velocity = Vector2.Zero,
                                lifespan = 0.25f,
                                rotationSpeed = Globals.RandomFloat(-0.5f, 0.5f),
                                resistance = 1.5f
                            };

                            Particle projParticle3 = new(new(position.X + Globals.RandomFloat(-30, 30), position.Y), projParticleData3);
                            ParticleManager.AddParticle(projParticle3);
                        }

                        break;
                    }
            }

            lifespan -= Globals.TotalSeconds;
        }

        public void HitEnemy()
        {
            switch (aiType)
            {
                case 1:
                    break;
                case 2:
                    break;
                case 3:

                    ParticleData projParticleData = new()
                    {
                        opacityStart = 1f,
                        opacityEnd = 0f,
                        sizeStart = 2,
                        sizeEnd = 1,
                        colorStart = Color.White,
                        colorEnd = Color.Gold,
                        velocity = new(Globals.RandomFloat(-100, 100), Globals.RandomFloat(-300, 0)),
                        lifespan = 0.5f,
                        rotationSpeed = Globals.RandomFloat(-0.5f, 0.5f)
                    };

                    Particle projParticle = new(position, projParticleData);
                    ParticleManager.AddParticle(projParticle);

                    break;
            }
        }
        public void Kill()
        {
            switch (aiType)
            {
                case 1: //firebolt

                    break;
                case 2: //fireball

                    break;
                case 5: //disintegrate
                    {
                        if (ai == 0)
                        {
                            Projectile projectile = new(new(position.X, InputManager.MousePosition.Y), 5, speed, damage, -1, Vector2.Zero, 1f, 10, 0f, scale, true, 1, 0);
                            ProjectileManager.AddProjectile(projectile);

                            GameManager.AddScreenShake(0.2f, 15f);
                        }

                        if (ai == 1)
                        {
                            for (int i = 0 ; i < 15; i++)
                            {
                                ParticleData fadeParticleData = new()
                                {
                                    opacityStart = 0.7f,
                                    opacityEnd = 0f,
                                    sizeStart = Globals.RandomFloat(6f, 15f),
                                    sizeEnd = Globals.RandomFloat(0.5f, 6f),
                                    colorStart = Color.Gray,
                                    colorEnd = Color.Gray,
                                    velocity = new(Globals.RandomFloat(-100, 100), Globals.RandomFloat(-200, 0)),
                                    lifespan = Globals.RandomFloat(0.5f, 2f),
                                    rotationSpeed = Globals.RandomFloat(-0.1f, 0.1f),
                                    resistance = 1.05f
                                };

                                Particle fadeParticle = new(new(position.X + Globals.RandomFloat(-20, 20), position.Y), fadeParticleData);
                                ParticleManager.AddParticle(fadeParticle);

                                ParticleData fadeParticleData2 = new()
                                {
                                    opacityStart = 1f,
                                    opacityEnd = 0.3f,
                                    sizeStart = Globals.RandomFloat(2f, 3f),
                                    sizeEnd = 0f,
                                    colorStart = Color.Gold,
                                    colorEnd = Color.Gold,
                                    velocity = new(Globals.RandomFloat(-600, 600), Globals.RandomFloat(-700, 100)),
                                    lifespan = Globals.RandomFloat(0.3f, 0.6f),
                                    rotationSpeed = Globals.RandomFloat(-0.1f, 0.1f),
                                    resistance = 1.05f
                                };

                                Particle fadeParticle2 = new(new(position.X + Globals.RandomFloat(-20, 20), position.Y), fadeParticleData2);
                                ParticleManager.AddParticle(fadeParticle2);
                            }
                        }

                        break;
                    }
            }

            active = false;
        }
    }
}
