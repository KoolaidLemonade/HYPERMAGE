using HYPERMAGE.Helpers;
using HYPERMAGE.Managers;
using HYPERMAGE.Particles;
using HYPERMAGE.Spells;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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
        public Vector2 hitboxOffset;
        public Vector2 hitboxSize;
        public Vector2 hitboxOrigin;

        public Vector2 center;

        public Vector2 velocity;

        public bool active = true;

        public int immunityFrameCounter;
        public int immunityFrames;
        public bool immuneSameType = false;

        public float speed;
        public float damage;
        public float lifespan;
        public float scale;
        public int pierce;
        public float knockback;

        public bool parryable = true;

        private float ai;
        private float ai2;

        public bool explosify;
        public float explosifyDamage;


        private double angle = 0;
        public Projectile(Vector2 position, int aiType, float speed, Vector2 velocity, float lifespan) : this(position, aiType, speed, 1, -1, 0f, velocity, lifespan, 60, 0f, 1f, false, 0, 0)
        {

        }
        public Projectile(Vector2 position, int aiType, float speed, Vector2 velocity, float lifespan, float rotation) : this(position, aiType, speed, 1, -1, 0f, velocity, lifespan, 60, rotation, 1f, false, 0, 0)
        {

        }
        public Projectile(Vector2 position, int aiType, float speed, float damage, Vector2 velocity, float lifespan) : this(position, aiType, speed, damage, -1, 0f, velocity, lifespan, 60, 0f, 1f, false, 0, 0)
        {

        }

        /*-------------------------------------------*/
        public Projectile(Vector2 position, int aiType, float speed, float damage, int pierce, float knockback, float lifespan) : this(position, aiType, speed, damage, pierce, knockback, Vector2.Zero, lifespan, 60, 0f, 1f, true, 0, 0)
        {

        }
        public Projectile(Vector2 position, int aiType, float speed, float damage, float knockback, Vector2 velocity, float lifespan) : this(position, aiType, speed, damage, 0, knockback, velocity, lifespan, 60, 0f, 1f, true, 0, 0)
        {

        }
        public Projectile(Vector2 position, int aiType, float speed, float damage, int pierce, float knockback, Vector2 velocity, float lifespan) : this(position, aiType, speed, damage, pierce, knockback, velocity, lifespan, 60, 0f, 1f, true, 0, 0)
        {

        }
        public Projectile(Vector2 position, int aiType, float speed, float damage, int pierce, float knockback, Vector2 velocity, float lifespan, float size) : this(position, aiType, speed, damage, pierce, knockback, velocity, lifespan, 60, 0f, size, true, 0, 0)
        {

        }
        public Projectile(Vector2 position, int aiType, float speed, float damage, int pierce, float knockback, float lifespan, int immunityFrames) : this(position, aiType, speed, damage, pierce, knockback, Vector2.Zero, lifespan, immunityFrames, 0f, 1f, true, 0, 0)
        {

        }
        public Projectile(Vector2 position, int aiType, float speed, float damage, int pierce, float knockback, float lifespan, int immunityFrames, float scale) : this(position, aiType, speed, damage, pierce, knockback, Vector2.Zero, lifespan, immunityFrames, 0f, scale, true, 0, 0)
        {

        }
        public Projectile(Vector2 position, int aiType, float speed, float damage, int pierce, float knockback, Vector2 velocity, float lifespan, int immunityFrames) : this(position, aiType, speed, damage, pierce, knockback, velocity, lifespan, immunityFrames, 0f, 1f, true, 0, 0)
        {

        }
        public Projectile(Vector2 position, int aiType, float speed, float damage, int pierce, float knockback, Vector2 velocity, float lifespan, int immunityFrames, float scale) : this(position, aiType, speed, damage, pierce, knockback, velocity, lifespan, immunityFrames, 0f, scale, true, 0, 0)
        {

        }
        public Projectile(Vector2 position, int aiType, float speed, float damage, int pierce, float knockback, Vector2 velocity, float lifespan, int immunityFrames, float rotation, float scale) : this(position, aiType, speed, damage, pierce, knockback, velocity, lifespan, immunityFrames, rotation, scale, true, 0, 0)
        {
        }
        public Projectile(Vector2 position, int aiType, float speed, float damage, int pierce, float knockback, Vector2 velocity, float lifespan, int immunityFrames, float rotation, float scale, bool friendly, float ai, float ai2)
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
            this.knockback = knockback;

            this.ai = ai;
            this.ai2 = ai2;

            maxLifespan = lifespan;

            switch (aiType)
            {
                // enemy projectiles
                case -5:
                    texture = Globals.Content.Load<Texture2D>("enemybullet2");
                    hitboxOffset = new(0, 0);
                    hitboxSize = new(9, 9);

                    parryable = false;

                    break;
                case -4:
                    texture = Globals.Content.Load<Texture2D>("enemybullet4");
                    hitboxOffset = new(0, 0);
                    hitboxSize = new(3, 3);
                    break;
                case -3:
                    texture = Globals.Content.Load<Texture2D>("enemybullet2");
                    hitboxOffset = new(0, 0);
                    hitboxSize = new(9, 9);

                    parryable = false;

                    break;
                case -2:
                    texture = Globals.Content.Load<Texture2D>("enemybullet1");
                    hitboxOffset = new(0, 0);
                    hitboxSize = new(4, 4);
                    break;
                case -1:
                    texture = Globals.Content.Load<Texture2D>("enemybullet4");
                    hitboxOffset = new(0, 0);
                    hitboxSize = new(3, 3);
                    break;
                case 0:
                    break;
                // friendly projectiles
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
                case 6: // sparks
                    texture = Globals.Content.Load<Texture2D>("particle");
                    break;
                case 7:
                    texture = null;
                    break;
                case 8: // ray of light
                    hitboxSize = new(10, ai2);
                    immuneSameType = true;
                    texture = null;
                    break;
            }

            if (anim != null)
            {
                width = anim.frameWidth;
                height = anim.frameHeight;
                origin = new(width / 2f, height / 2f);
            }

            else if (texture != null)
            {
                width = texture.Width;
                height = texture.Height;
                origin = new(width / 2f, height / 2f);
            }

            active = true;

            center = position + origin;


            if (hitboxOffset == Vector2.Zero && hitboxSize == Vector2.Zero)
            {
                hitbox = PolygonFactory.CreateRectangle((int)position.X, (int)position.Y, (int)(width * scale), (int)(height * scale), rotation);
            }

            else
            {
                hitbox = PolygonFactory.CreateRectangle((int)position.X + (int)hitboxOffset.X, (int)position.Y + (int)hitboxOffset.Y, (int)(hitboxSize.X * scale), (int)(hitboxSize.Y * scale), rotation);
            }
        }
        public void Draw()
        {
            if (aiType == 5 && ai == 1)
            {
                Globals.SpriteBatch.Draw(Globals.GetBlankTexture(), new Rectangle((int)position.X - 20 + Globals.Random.Next(3), 0, 40 + Globals.Random.Next(3), (int)position.Y + 2), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.8f);
                Globals.SpriteBatch.Draw(Globals.GetBlankTexture(), new Rectangle((int)position.X - 25 + Globals.Random.Next(3), 0, 50 + Globals.Random.Next(3), (int)position.Y + 2), null, Color.Red, 0f, Vector2.Zero, SpriteEffects.None, 0.699f);
                return;
            }

            if (aiType == 7)
            {
                Globals.SpriteBatch.Draw(Globals.GetBlankTexture(), new Rectangle((int)position.X, (int)position.Y, (int)(width - ai), (int)(height - ai)), null, Color.White, rotation + Globals.RandomFloat(0, 10), new Vector2(0.5f, 0.5f), SpriteEffects.None, 0.8f);
                Globals.SpriteBatch.Draw(Globals.GetBlankTexture(), new Rectangle((int)position.X, (int)position.Y, (int)(width - ai), (int)(height - ai)), null, Color.White, -rotation + Globals.RandomFloat(0, -10), new Vector2(0.5f, 0.5f), SpriteEffects.None, 0.8f);

                return;
            }

            if (aiType == 8)
            {
                Globals.SpriteBatch.Draw(Globals.GetBlankTexture(), new Rectangle((int)position.X, (int)position.Y, 10, (int)(ai2 - ai)), null, Color.White, rotation, new Vector2(0.5f, 0.5f), SpriteEffects.None, 0.8f);
                return;
            }

            if (anim != null)
            {
                anim.Draw(new((int)position.X, (int)position.Y), Color.White, rotation, origin, scale, SpriteEffects.None, 1f);
            }

            else if (texture != null)
            {
                Globals.SpriteBatch.Draw(texture, new(position.X, position.Y), null, Color.White, rotation, origin, scale, SpriteEffects.None, friendly ? 0.8f : 1f);
            }
        }

        Mob closestMob;
        List<float> mobDist = [];
        public void Update()
        {
            if (anim != null)
            {
                anim.Update();
            }

            if (hitboxOffset == Vector2.Zero && hitboxSize == Vector2.Zero)
            {
                hitbox = PolygonFactory.CreateRectangle((int)position.X, (int)position.Y, (int)(width * scale), (int)(height * scale), rotation);
            }

            else
            {
                hitbox = PolygonFactory.CreateRectangle((int)position.X + (int)hitboxOffset.X, (int)position.Y + (int)hitboxOffset.Y, (int)(hitboxSize.X * scale), (int)(hitboxSize.Y * scale), rotation);
            }

            center = position + origin;

            if (lifespan <= 0)
            {
                Kill();
            }

            switch (aiType)
            {
                //enemy projectiles
                case -5:
                    position += velocity * Globals.TotalSeconds * speed;


                    ai += Globals.TotalSeconds;

                    if (ai > 0.1f)
                    {
                        ParticleData ParticleData = new()
                        {
                            sizeStart = 5f,
                            sizeEnd = 0,
                            colorStart = Color.White,
                            colorEnd = Color.White,
                            velocity = new(Globals.RandomFloat(-50, 50), Globals.RandomFloat(-50, 50)),
                            lifespan = 0.25f,
                            rotationSpeed = 1f,
                            resistance = 1.2f,
                            friendly = false
                        };

                        Particle particle = new(position, ParticleData);
                        ParticleManager.AddParticle(particle);

                        ai = 0;
                    }

                    break;
                case -4: //sorcerer
                    position += velocity * Globals.TotalSeconds * speed;
                    rotation = velocity.ToRotation() + MathHelper.ToRadians(90);
                    break;
                case -3: //sorcerer
                    position += velocity * Globals.TotalSeconds * speed;
                    velocity /= 1.05f;

                    if (lifespan <= 0.15 && ai == 0)
                    {
                        ai = 1;

                        SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("hit"), 0.8f, 0f, 0f);

                        for (int i = 0; i < Globals.Random.Next(4) + 8; i++)
                        {
                            ParticleData projParticleData = new()
                            {
                                sizeStart = 2,
                                sizeEnd = 0,
                                velocity = (Vector2.One * 100f).RotatedBy(MathHelper.ToRadians(Globals.RandomFloat(0, 360))),
                                lifespan = 0.5f,
                                rotationSpeed = Globals.RandomFloat(-0.5f, 0.5f),
                                friendly = false

                            };

                            Particle projParticle = new(position, projParticleData);
                            ParticleManager.AddParticle(projParticle);
                        }

                    }

                    break;
                case -2: //wizard
                    position += velocity * Globals.TotalSeconds * speed;

                    break;
                case -1: //wisp
                    position += velocity * Globals.TotalSeconds * speed;
                    rotation = velocity.ToRotation() + MathHelper.ToRadians(90);

                    ai += Globals.TotalSeconds;

                    if (ai > 0.1f)
                    {
                        ParticleData ParticleData = new()
                        {
                            opacityStart = 1f,
                            opacityEnd = 0f,
                            sizeStart = 2f,
                            sizeEnd = 0,
                            colorStart = Color.White,
                            colorEnd = Color.White,
                            velocity = new(Globals.RandomFloat(-50, 50), Globals.RandomFloat(-50, 50)),
                            lifespan = 0.2f,
                            rotationSpeed = 1f,
                            resistance = 1.2f,
                            friendly = false
                        };

                        Particle particle = new(position, ParticleData);
                        ParticleManager.AddParticle(particle);

                        ai = 0;
                    }

                    break;
                case 0:
                    break;
                //friendly projectiles
                case 1: //firebolt
                    position += velocity * Globals.TotalSeconds * speed;

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
                            velocity = new(Globals.RandomFloat(-200, 200), Globals.RandomFloat(-200, 200)),
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

                    UpdateMobPositions();

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
                            sizeStart = 2 * scale,
                            sizeEnd = 1,
                            colorStart = Color.Yellow,
                            colorEnd = Color.DarkRed,
                            velocity = new(Globals.RandomFloat(-30, 30), Globals.RandomFloat(-150, 0)),
                            lifespan = 0.5f,
                            rotationSpeed = Globals.RandomFloat(-0.5f, 0.5f)
                        };

                        Particle projParticle = new(position, projParticleData);
                        ParticleManager.AddParticle(projParticle);
                    }

                    if (Globals.Random.Next(25) == 0)
                    {
                        ParticleData projParticleData = new()
                        {
                            opacityStart = 1f,
                            opacityEnd = 0f,
                            sizeStart = 1f * scale * Globals.RandomFloat(0.5f, 1.5f),
                            sizeEnd = 0,
                            colorStart = Color.White,
                            colorEnd = Color.White,
                            velocity = new(Globals.RandomFloat(-150, 150), Globals.RandomFloat(-150, 150)),
                            lifespan = 0.5f * Globals.RandomFloat(0.5f, 1.5f),
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

                        float radius = ai2;

                        Vector2 newCenter = new Vector2((float)(GameManager.GetPlayer().center.X + radius * Math.Cos(angle * Math.PI / 180)), (float)(GameManager.GetPlayer().center.Y + radius * Math.Sin(angle * Math.PI / 180)));

                        position = newCenter.RotatedBy(MathHelper.ToRadians(Globals.NonLerp(-90, 120, lifespan / maxLifespan)), GameManager.GetPlayer().center);

                        Vector2 directionToPlayer = Vector2.Normalize(GameManager.GetPlayer().center - position);

                        rotation = (float)(Math.Atan2(directionToPlayer.Y, directionToPlayer.X)) + MathHelper.ToRadians(270);

                        ParticleData projParticleData = new()
                        {
                            opacityStart = 1f,
                            opacityEnd = 0f,
                            sizeStart = 2,
                            sizeEnd = 0,
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
                            sizeStart = 1,
                            sizeEnd = 5,
                            colorStart = Color.Gray,
                            colorEnd = Color.DarkRed,
                            velocity = new Vector2(Globals.RandomFloat(-5, 5), Globals.RandomFloat(-5, 5)),
                            lifespan = 1f,
                            rotationSpeed = Globals.RandomFloat(-0.1f, 0.1f)
                        };

                        for (int i = 0; i < 3; i++)
                        {
                            Vector2 particleCenter = new Vector2((float)(GameManager.GetPlayer().center.X + ((radius - (scale * 9)) * Globals.RandomFloat(1, 2f)) * Math.Cos(angle * Math.PI / 180)), (float)(GameManager.GetPlayer().center.Y + ((radius - (scale * 9)) * Globals.RandomFloat(1, 2f)) * Math.Sin(angle * Math.PI / 180)));
                            Particle projParticle = new(particleCenter.RotatedBy(MathHelper.ToRadians(Globals.NonLerp(-90, 120, lifespan / maxLifespan)), GameManager.GetPlayer().center), projParticleData);
                            Particle projParticle2 = new(particleCenter.RotatedBy(MathHelper.ToRadians(Globals.NonLerp(-90, 120, lifespan / maxLifespan)), GameManager.GetPlayer().center), projParticleData2);
                            ParticleManager.AddParticle(projParticle);

                            if (Globals.Random.Next(5) == 0)
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
                                sizeStart = 10,
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
                                sizeStart = 8,
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
                                sizeStart = 10,
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
                case 6:
                    {
                        velocity += new Vector2(Globals.RandomFloat(-5, 5), Globals.RandomFloat(-5, 5));
                        rotation += 1f;
                        velocity /= 1.05f;

                        position += ((velocity * Globals.TotalSeconds) * speed);

                        if (Globals.Random.Next(40) == 0)
                        {
                            ParticleData projParticleData = new()
                            {
                                opacityStart = 1f,
                                opacityEnd = 0f,
                                sizeStart = 0.5f * scale * Globals.RandomFloat(0.5f, 1.5f),
                                sizeEnd = 0,
                                colorStart = Color.White,
                                colorEnd = Color.White,
                                velocity = new(Globals.RandomFloat(-120, 120), Globals.RandomFloat(-120, 120)),
                                lifespan = 0.5f * Globals.RandomFloat(0.5f, 1.5f),
                                rotationSpeed = Globals.RandomFloat(-0.5f, 0.5f)
                            };

                            Particle projParticle = new(position, projParticleData);
                            ParticleManager.AddParticle(projParticle);
                        }

                        if (Globals.Random.Next(3) == 0)
                        {
                            ParticleData projParticleData = new()
                            {
                                opacityStart = 1f,
                                opacityEnd = 0f,
                                sizeStart = 1 * scale * Globals.RandomFloat(0.5f, 1.5f),
                                sizeEnd = 1,
                                colorStart = Color.Yellow,
                                colorEnd = Color.DarkRed,
                                velocity = new(Globals.RandomFloat(-30, 30), Globals.RandomFloat(-30, 30)),
                                lifespan = 0.5f * Globals.RandomFloat(0.5f, 1.5f),
                                rotationSpeed = Globals.RandomFloat(-0.5f, 0.5f)
                            };

                            Particle projParticle = new(position, projParticleData);
                            ParticleManager.AddParticle(projParticle);
                        }

                        break;
                    }
                case 7: //explode hitbox
                    {
                        ai += Globals.TotalSeconds * (200 + ai * 10);

                        rotation++;
                        break;
                    }
                case 8: //ray of light
                    {
                        ai += Globals.TotalSeconds * (ai2 + ai * 2);

                        if (ai >= ai2)
                        {
                            Kill();
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
                case 8:
                    for (int j = 0; j < 10; j++)
                    {
                        ParticleData pd = new()
                        {
                            sizeStart = Globals.RandomFloat(2f, 8f),
                            sizeEnd = 0,
                            colorStart = Color.White,
                            colorEnd = Color.White,
                            velocity = -position.DirectionTo(GameManager.GetPlayer().center).RotatedBy(MathHelper.ToRadians(Globals.RandomFloat(-30, 30))) * Globals.RandomFloat(200, 500),
                            lifespan = Globals.RandomFloat(0.2f, 0.6f),
                            rotationSpeed = Globals.RandomFloat(-0.5f, 0.5f)
                        };

                        Particle p = new(position, pd);
                        ParticleManager.AddParticle(p);
                    }

                    break;
            }
        }
        public void Kill()
        {
            if (explosify)
            {
                for (int i = 0; i < 10; i++)
                {
                    ParticleData pd = new()
                    {
                        opacityStart = .8f,
                        opacityEnd = 0.1f,
                        sizeStart = 4 + Globals.Random.Next(3),
                        sizeEnd = 0,
                        colorStart = Color.White,
                        colorEnd = Color.Gold,
                        velocity = new(Globals.RandomFloat(-200, 200), Globals.RandomFloat(-200, 200)),
                        lifespan = Globals.RandomFloat(0.5f, 1f),
                        rotationSpeed = Globals.RandomFloat(-0.5f, 0.5f)
                    };

                    Particle p = new(position, pd);
                    ParticleManager.AddParticle(p);
                }

                for (int i = 0; i < 10; i++)
                {
                    ParticleData pd = new()
                    {
                        sizeStart = 1 + Globals.Random.Next(3),
                        sizeEnd = 0,
                        colorStart = Color.White,
                        colorEnd = Color.White,
                        velocity = new(Globals.RandomFloat(-400, 400), Globals.RandomFloat(-400, 400)),
                        lifespan = 0.2f,
                        rotationSpeed = Globals.RandomFloat(-0.5f, 0.5f)
                    };

                    Particle p = new(position, pd);
                    ParticleManager.AddParticle(p);
                }

                SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("bigexplosion"), 0.4f, -0.2f, 0f);

                CreateExplosion(position, explosifyDamage, 1f, 50);
            }

            switch (aiType)
            {
                case -3: //sorcerer
                    for (int i = 0; i < Globals.Random.Next(5) + 5; i++)
                    {
                        ParticleData projParticleData = new()
                        {
                            sizeStart = 8,
                            sizeEnd = 0,
                            velocity = (Vector2.One * 50f).RotatedBy(MathHelper.ToRadians(Globals.RandomFloat(0, 360))),
                            lifespan = 0.5f,
                            rotationSpeed = Globals.RandomFloat(-0.5f, 0.5f),
                            friendly = false
                        };

                        Particle projParticle = new(position, projParticleData);
                        ParticleManager.AddParticle(projParticle);
                    }

                    for (int i = 0; i < 16; i++)
                    {
                        Projectile projectile = new(position, -4, 1f, (Vector2.One * (i % 2 == 0 ? 50 : 70)).RotatedBy(MathHelper.ToRadians(MathHelper.Lerp(0, 360, i / 16f))), 5f);
                        ProjectileManager.AddProjectile(projectile);
                    }

                    SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("smallexplosion"), 0.7f, 0f + Globals.RandomFloat(0, 0.5f), 0f);


                    break;
                case 1: //firebolt
                    for (int i = 0; i < Globals.Random.Next(5) + 5; i++)
                    {
                        ParticleData projParticleData = new()
                        {
                            opacityStart = .8f,
                            opacityEnd = 0.1f,
                            sizeStart = 8,
                            sizeEnd = 0,
                            colorStart = Color.Gold,
                            colorEnd = Color.Red,
                            velocity = (velocity * Globals.RandomFloat(0f, 1.5f)).RotatedBy(MathHelper.ToRadians(Globals.RandomFloat(-45, 45))),
                            lifespan = 0.5f,
                            rotationSpeed = Globals.RandomFloat(-0.5f, 0.5f)
                        };

                        Particle projParticle = new(position, projParticleData);
                        ParticleManager.AddParticle(projParticle);
                    }

                    for (int i = 0; i < Globals.Random.Next(2) + 3; i++)
                    {
                        ParticleData projParticleData = new()
                        {
                            opacityStart = .8f,
                            opacityEnd = 0.1f,
                            sizeStart = 5,
                            sizeEnd = 0,
                            colorStart = Color.White,
                            colorEnd = Color.White,
                            velocity = (velocity * Globals.RandomFloat(0f, 1.5f)).RotatedBy(MathHelper.ToRadians(Globals.RandomFloat(-45, 45))) * 1.5f,
                            lifespan = 0.25f,
                            rotationSpeed = Globals.RandomFloat(-0.5f, 0.5f)
                        };

                        Particle projParticle = new(position, projParticleData);
                        ParticleManager.AddParticle(projParticle);
                    }

                    SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("smallexplosion"), 0.4f, Globals.RandomFloat(0.5f, 1f), 0f);

                    break;
                case 2: //fireball
                    for (int i = 0; i < Globals.Random.Next(5) + 5; i++)
                    {
                        ParticleData projParticleData = new()
                        {
                            opacityStart = .8f,
                            opacityEnd = 0.1f,
                            sizeStart = 8,
                            sizeEnd = 0,
                            colorStart = Color.Gold,
                            colorEnd = Color.Red,
                            velocity = (velocity * Globals.RandomFloat(0f, 1.5f)).RotatedBy(MathHelper.ToRadians(Globals.RandomFloat(-45, 45))),
                            lifespan = 0.5f,
                            rotationSpeed = Globals.RandomFloat(-0.5f, 0.5f)
                        };

                        Particle projParticle = new(position, projParticleData);
                        ParticleManager.AddParticle(projParticle);
                    }

                    for (int i = 0; i < Globals.Random.Next(2) + 6; i++)
                    {
                        ParticleData projParticleData = new()
                        {
                            opacityStart = .8f,
                            opacityEnd = 0.1f,
                            sizeStart = 5,
                            sizeEnd = 0,
                            colorStart = Color.White,
                            colorEnd = Color.White,
                            velocity = (velocity * Globals.RandomFloat(0f, 1.5f)).RotatedBy(MathHelper.ToRadians(Globals.RandomFloat(-45, 45))) * 1.5f,
                            lifespan = 0.25f,
                            rotationSpeed = Globals.RandomFloat(-0.5f, 0.5f)
                        };

                        Particle projParticle = new(position, projParticleData);
                        ParticleManager.AddParticle(projParticle);
                    }

                    SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("bigexplosion"), 0.4f, -0.2f, 0f);

                    for (int i = 0; i < 10; i++)
                    {
                        ParticleData pd = new()
                        {
                            opacityStart = .8f,
                            opacityEnd = 0.1f,
                            sizeStart = 4 + Globals.Random.Next(3),
                            sizeEnd = 0,
                            colorStart = Color.White,
                            colorEnd = Color.Gold,
                            velocity = new(Globals.RandomFloat(-200, 200), Globals.RandomFloat(-200, 200)),
                            lifespan = Globals.RandomFloat(0.5f, 1f),
                            rotationSpeed = Globals.RandomFloat(-0.5f, 0.5f)
                        };

                        Particle p = new(position, pd);
                        ParticleManager.AddParticle(p);
                    }

                    for (int i = 0; i < 10; i++)
                    {
                        ParticleData pd = new()
                        {
                            sizeStart = 1 + Globals.Random.Next(3),
                            sizeEnd = 0,
                            colorStart = Color.White,
                            colorEnd = Color.White,
                            velocity = new(Globals.RandomFloat(-400, 400), Globals.RandomFloat(-400, 400)),
                            lifespan = 0.2f,
                            rotationSpeed = Globals.RandomFloat(-0.5f, 0.5f)
                        };

                        Particle p = new(position, pd);
                        ParticleManager.AddParticle(p);
                    }

                    CreateExplosion(position, damage / 4f, knockback, 50);

                    break;
                case 5: //disintegrate
                    {
                        if (ai == 0)
                        {
                            Projectile projectile = new(new(position.X, InputManager.MousePosition.Y), 5, speed, damage, -1, 0f, Vector2.Zero, 1f, 10, 0f, scale, true, 1, 0);
                            ProjectileManager.AddProjectile(projectile);

                            GameManager.AddScreenShake(0.2f, 15f);
                            GameManager.AddAbberationPowerForce(500, 50);

                            SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("cry"), 0.5f, 0f, 0f);
                            SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("lastingexplosion"), 0.7f, -1f, 0f);
                            SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("hit"), 0.7f, 0f, 0f);
                        }

                        if (ai == 1)
                        {
                            for (int i = 0; i < 15; i++)
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

                            SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("shoot"), 0.6f, 1f, 0);

                        }

                        break;
                    }
                case 6:
                    for (int i = 0; i < 2; i++)
                    {
                        ParticleData pd = new()
                        {
                            opacityStart = .8f,
                            opacityEnd = 0.1f,
                            sizeStart = 2,
                            sizeEnd = 0,
                            colorStart = Color.White,
                            colorEnd = Color.Gold,
                            velocity = new(Globals.RandomFloat(-100, 100), Globals.RandomFloat(-100, 100)),
                            lifespan = 0.5f,
                            rotationSpeed = Globals.RandomFloat(-0.5f, 0.5f)
                        };

                        SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("hit"), 0.6f, Globals.RandomFloat(0f, 1f), 0);

                        Particle p = new(center, pd);
                        ParticleManager.AddParticle(p);
                    }

                    break;
            }

            active = false;
        }

        public void HitPlayer()
        {
            switch (aiType)
            {
                case -1:
                    for (int i = 0; i < 5; i++)
                    {
                        ParticleData particleData = new()
                        {
                            sizeStart = 5,
                            sizeEnd = 0,
                            colorStart = Color.White,
                            colorEnd = Color.Gray,
                            velocity = new(Globals.RandomFloat(-200, 200), Globals.RandomFloat(-200, 200)),
                            lifespan = 0.5f,
                            rotationSpeed = Globals.RandomFloat(-0.5f, 0.5f)
                        };

                        Particle particle = new(center, particleData);
                        ParticleManager.AddParticle(particle);
                    }

                    Kill();
                    return;
            }
        }

        public void UpdateMobPositions()
        {
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
        }

        public void CreateExplosion(Vector2 pos, float damage, float knockback, int size)
        {
            Projectile explosion = new(pos, 7, 0f, damage, -1, knockback, Vector2.Zero, 0.1f, 200, 0f, 1f, true, 0, 0);
            explosion.width = size;
            explosion.height = size;

            ProjectileManager.AddProjectile(explosion);
        }
    }
}
