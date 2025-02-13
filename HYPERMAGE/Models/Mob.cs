using HYPERMAGE.Helpers;
using HYPERMAGE.Managers;
using HYPERMAGE.Particles;
using HYPERMAGE.Spells;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

// olddd

namespace HYPERMAGE.Models
{
    public class Mob
    {
        public Texture2D texture;
        public Animation anim;
        public AnimationManager anims = new();

        public SoundEffect hitSound;
        public SoundEffect deathSound;

        public Vector2 position;
        public Vector2 prevPosition;

        private float timer = 0;
        public float ai;
        public float ai2;

        public int aiType;
        public float speed = 1f;
        public float health;
        public bool active;
        public Vector2 origin;

        public Polygon hitbox;

        public Vector2 center;

        public Vector2 velocity;

        public int width;
        public int height;
        public float rotation = 0;
        public float scale = 1f;

        public bool turnToPlayer = true;
        public int direction;

        public bool contactDamage = true;
        public float knockbackResist = 1f;
        public int spawnCost;
        public bool flying = false;

        public bool spawning;
        public float spawnTimer;

        private List <Projectile> projectileImmunity = [];
        public Mob(Vector2 position, int aiType)
        {
            this.aiType = aiType;

            switch (aiType)
            {
                case 0:
                    break;
                case 1: //bat
                    anim = new Animation(Globals.Content.Load<Texture2D>("bat"), 2, 1, 0.2f);
                    hitSound = Globals.Content.Load<SoundEffect>("hit");
                    deathSound = Globals.Content.Load<SoundEffect>("hit");

                    health = 2f;
                    knockbackResist = 0.6f;

                    spawnCost = 1;

                    flying = true;
                    break;
                case 2: //wisp
                    texture = Globals.Content.Load<Texture2D>("particle");
                    hitSound = Globals.Content.Load<SoundEffect>("hit");
                    deathSound = Globals.Content.Load<SoundEffect>("shoot");

                    scale = 2f;
                    health = 3f;
                    knockbackResist = 0.6f;
                    spawnCost = 5;
                    flying = true;

                    timer = Globals.RandomFloat(0, 2);
                    break;
                case 3: //wizard
                    anims.AddAnimation(0, new(Globals.Content.Load<Texture2D>("wizard"), 2, 2, 0.5f, 1));
                    anims.AddAnimation(1, new(Globals.Content.Load<Texture2D>("wizard"), 2, 2, 0.5f, 2));

                    hitSound = Globals.Content.Load<SoundEffect>("hit");
                    deathSound = Globals.Content.Load<SoundEffect>("bwowop");

                    contactDamage = false;

                    width = 23;
                    height = 24;

                    health = 15f;
                    spawnCost = 4;
                    break;
                case 4: //slime
                    anim = new Animation(Globals.Content.Load<Texture2D>("slime"), 2, 1, 0.2f);

                    hitSound = Globals.Content.Load<SoundEffect>("hit");
                    deathSound = Globals.Content.Load<SoundEffect>("hit");

                    health = 4f;
                    knockbackResist = 0.8f;
                    spawnCost = 1;

                    break;
                case 5: //mini slime
                    anim = new Animation(Globals.Content.Load<Texture2D>("minislime"), 2, 1, 0.2f);

                    hitSound = Globals.Content.Load<SoundEffect>("hit");
                    deathSound = Globals.Content.Load<SoundEffect>("hit");

                    health = 1f;
                    knockbackResist = 0.4f;
                    spawnCost = 1;

                    break;
                case 6: //sorcerer
                    anims.AddAnimation(0, new(Globals.Content.Load<Texture2D>("sorcerer"), 2, 2, 0.5f, 1));
                    anims.AddAnimation(1, new(Globals.Content.Load<Texture2D>("sorcerer"), 2, 2, 0.5f, 2));

                    hitSound = Globals.Content.Load<SoundEffect>("hit");
                    deathSound = Globals.Content.Load<SoundEffect>("bwowop");

                    contactDamage = false;

                    width = 23;
                    height = 21;

                    health = 10f;
                    spawnCost = 3;
                    break;
                case 7: //cleric
                    anims.AddAnimation(0, new(Globals.Content.Load<Texture2D>("cleric"), 2, 2, 0.5f, 1));
                    anims.AddAnimation(1, new(Globals.Content.Load<Texture2D>("cleric"), 2, 2, 0.5f, 2));

                    hitSound = Globals.Content.Load<SoundEffect>("hit");
                    deathSound = Globals.Content.Load<SoundEffect>("bwowop");

                    contactDamage = false;

                    health = 30f;
                    spawnCost = 5;
                    break;
                case 8: //totem
                    texture = Globals.Content.Load<Texture2D>("totem");
                    contactDamage = false;

                    health = 30f;
                    spawnCost = 6;
                    break;
            }

            if (anim != null)
            {
                width = anim.frameWidth;
                height = anim.frameHeight;
            }

            else if (texture != null)
            {
                width = texture.Width;
                height = texture.Height;
            }

            origin = new Vector2(width / 2, height / 2) ;

            this.position = position - origin;

            center = position + origin;

            hitbox = PolygonFactory.CreateRectangle((int)center.X, (int)center.Y, width, height);

            active = true;
        }
        public void Update()
        {
            Debug.WriteLine(position);

            center = position + origin;

            if (spawning)
            {
                spawnTimer += Globals.TotalSeconds;

                if (spawnTimer > 2f)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        ParticleData spawnParticleData = new()
                        {
                            opacityStart = 1f,
                            opacityEnd = 1f,
                            sizeStart = 6,
                            sizeEnd = 0,
                            colorStart = Color.White,
                            colorEnd = Color.White,
                            velocity = new(Globals.RandomFloat(-200, 200), Globals.RandomFloat(-200, 200)),
                            lifespan = 0.2f,
                            rotationSpeed = Globals.RandomFloat(-0.5f, 0.5f)
                        };

                        Particle spawnParticle = new(center, spawnParticleData);
                        ParticleManager.AddParticle(spawnParticle);
                    }

                    spawning = false;
                }

                return;
            }

            if (anim != null)
            {
                anim.Update();
            }

            hitbox = PolygonFactory.CreateRectangle((int)center.X, (int)center.Y, width, height);

            if (projectileImmunity != null)
            {
                foreach (Projectile p in projectileImmunity.ToList())
                {
                    p.immunityFrameCounter++;

                    if (p.immunityFrameCounter > p.immunityFrames)
                    {
                        p.immunityFrameCounter = 0;
                        projectileImmunity.Remove(p);
                    }
                }
            }

            if (health <= 0)
            {
                Kill();
            }

            foreach (Projectile p in ProjectileManager.projectiles)
            {
                if (p.hitbox.IntersectsWith(hitbox) && p.friendly && !projectileImmunity.Contains(p))
                {
                    projectileImmunity.Add(p);

                    HitByProj(p);
                    p.HitEnemy();

                    if (p.pierce == 0)
                    {
                        p.Kill();
                    }

                    else if (p.pierce > 0)
                    {
                        p.pierce--;
                    }

                    health -= p.damage;
                }
            }

            foreach (Mob m in MobManager.mobs)
            {
                if (m.flying && flying || !m.flying && !flying)
                {
                    if (m.hitbox.IntersectsWith(hitbox) && m != this)
                    {
                        velocity += m.center.DirectionTo(center) / (40 * knockbackResist);
                    }
                }
            }

            timer += Globals.TotalSeconds;

            prevPosition = position;

            position += velocity * speed * Globals.TotalSeconds * 30;

            if (!flying)
            {
                if (position.X < GameManager.groundBounds.X || position.X > GameManager.groundBounds.Z - width * scale)
                {
                    position.X = prevPosition.X;
                    velocity.X = 0f;
                }

                if (position.Y < GameManager.groundBounds.Y || position.Y > GameManager.groundBounds.W - height * scale)
                {
                    position.Y = prevPosition.Y;
                    velocity.Y = 0f;
                }
            }

            else
            {
                if (position.X < GameManager.bounds.X || position.X > GameManager.bounds.Z - width * scale)
                {
                    position.X = prevPosition.X;
                    velocity.X = 0f;
                }

                if (position.Y < GameManager.bounds.Y || position.Y > GameManager.bounds.W - height * scale)
                {
                    position.Y = prevPosition.Y;
                    velocity.Y = 0f;
                }
            }

            //

            if (turnToPlayer)
            {
                if (GameManager.GetPlayer().center.X - center.X < 0)
                {
                    direction = -1; 
                }

                else
                {
                    direction = 1;
                }    
            }

            switch (aiType)
            {
                case 0:
                    return;
                case 1: //bat

                    if (Math.Abs(GameManager.GetPlayer().center.X - center.X) < 30 && Math.Abs(GameManager.GetPlayer().center.Y - center.Y) < 30)
                    {
                        if (ai == 0 && timer > 3)
                        {
                            ai = 1;
                            timer = 0;
                        }
                    }

                    if (ai == 1)
                    {
                        velocity += Vector2.Normalize(GameManager.GetPlayer().center - center) * Globals.TotalSeconds * speed * 11;

                        if(timer > 0.35)
                        {
                            ai = 0;
                            timer = 0;
                        }
                    }

                    else
                    {
                        velocity += (Vector2.Normalize(GameManager.GetPlayer().center - center) + new Vector2(Globals.RandomFloat(-1.5f, 1.5f), Globals.RandomFloat(-1.5f, 1.5f))) * Globals.TotalSeconds * speed * 3;
                    }

                    velocity /= 1.05f;

                    return;
                case 2: //wisp

                    velocity += new Vector2(Globals.RandomFloat(-1, 1), Globals.RandomFloat(-1, 1));
                    velocity /= 2f;

                    if (Globals.Random.Next(3) == 0)
                    {
                        ParticleData particleData = new()
                        {
                            opacityStart = 1f,
                            opacityEnd = 0f,
                            sizeStart = 2 * scale,
                            sizeEnd = 0,
                            colorStart = Color.Wheat,
                            colorEnd = Color.Gray,
                            velocity = new(Globals.RandomFloat(-30, 30), Globals.RandomFloat(-100, 0)),
                            lifespan = 0.5f,
                            rotationSpeed = Globals.RandomFloat(-0.5f, 0.5f)
                        };

                        Particle particle = new(position, particleData);
                        ParticleManager.AddParticle(particle);
                    }

                    if (timer >= 2)
                    {
                        Projectile projectile = new(center, -1, 1f, Vector2.Normalize(GameManager.GetPlayer().center - center) * 100f, 10f, center.DirectionTo(GameManager.GetPlayer().center).ToRotation() + MathHelper.ToRadians(90f));
                        ProjectileManager.AddProjectile(projectile);

                        SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("hit"), 0.6f, 1, 0);

                        timer = 0;
                    }
                    
                    return;
                case 3: //wizard

                    anims.Update((int)ai);

                    if (Globals.Distance(center, GameManager.GetPlayer().center) > 150f && ai == 0)
                    {
                        velocity += (Vector2.Normalize(GameManager.GetPlayer().center - center) * Globals.TotalSeconds * speed);

                    }

                    if (timer >= 4)
                    {
                        if (ai <= 0)
                        {
                            SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("chirp"), 1, Globals.RandomFloat(-0.5f, 0.5f), 0);

                            for (int i = 0; i < 3; i++)
                            {
                                Vector2 projVelocity = Vector2.Normalize(GameManager.GetPlayer().center - center).RotatedBy(MathHelper.ToRadians(-25 + (i * 25))) * 60f;

                                Projectile projectile = new(center, -2, 1f, projVelocity, 10f, projVelocity.ToRotation() + MathHelper.ToRadians(90f));
                                ProjectileManager.AddProjectile(projectile);


                                for (int j = 0; j < 3; j++)
                                {
                                    ParticleData projParticleData = new()
                                    {
                                        opacityStart = 1f,
                                        opacityEnd = 1f,
                                        sizeStart = 5,
                                        sizeEnd = 0,
                                        colorStart = Color.White,
                                        colorEnd = Color.White,
                                        velocity = projVelocity.RotatedBy(MathHelper.ToRadians(Globals.RandomFloat(-20, 20))) * 2,
                                        lifespan = 0.5f,
                                        rotationSpeed = Globals.RandomFloat(-0.5f, 0.5f)
                                    };

                                    Particle projParticle = new(center, projParticleData);
                                    ParticleManager.AddParticle(projParticle);
                                }
                            }
                        }

                        ai = 1;

                        ai2 += Globals.TotalSeconds;

                        if (ai2 >= 1)
                        {
                            SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("chirp"), 1, Globals.RandomFloat(-0.5f, 0.5f), 0);

                            for (int i = 0; i < 3; i++)
                            {
                                Vector2 projVelocity = Vector2.Normalize(GameManager.GetPlayer().center - center).RotatedBy(MathHelper.ToRadians(-25 + (i * 25))) * 60f;

                                Projectile projectile = new(center, -2, 1f, projVelocity, 10f, projVelocity.ToRotation() + MathHelper.ToRadians(90f));
                                ProjectileManager.AddProjectile(projectile);

                                for (int j = 0; j < 3; j++)
                                {
                                    ParticleData projParticleData = new()
                                    {
                                        opacityStart = 1f,
                                        opacityEnd = 1f,
                                        sizeStart = 5,
                                        sizeEnd = 0,
                                        colorStart = Color.White,
                                        colorEnd = Color.White,
                                        velocity = projVelocity.RotatedBy(MathHelper.ToRadians(Globals.RandomFloat(-20, 20))) * 2,
                                        lifespan = 0.5f,
                                        rotationSpeed = Globals.RandomFloat(-0.5f, 0.5f)
                                    };

                                    Particle projParticle = new(center, projParticleData);
                                    ParticleManager.AddParticle(projParticle);
                                }
                            }

                            ai2 = 0;
                        }
                    }

                    if (timer >= 6)
                    {
                        timer = 0;
                        ai2 = 0;
                        ai = 0;
                    }

                    velocity /= 1.1f;

                    return;

                case 4: //slime
                    velocity += (Vector2.Normalize(GameManager.GetPlayer().center - center) * Globals.TotalSeconds * speed * 3);
                    velocity /= 1.05f;

                    return;
                case 5: //minislime
                    velocity += (Vector2.Normalize(GameManager.GetPlayer().center - center) * Globals.TotalSeconds * speed * 5);
                    velocity /= 1.05f;

                    return;

                case 6: //sorcerer
                    anims.Update((int)ai);

                    velocity /= 1.1f;

                    if (timer >= 4)
                    {
                        if (ai == 0)
                        {
                            Projectile projectile = new(center, -3, 1f, new(0, -100), 2f, 0f);
                            ProjectileManager.AddProjectile(projectile);

                            SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("shoot"), 0.8f, 0f, 0f);
                        }

                        ai = 1;

                    }

                    if (timer >= 5)
                    {
                        ai = 0;
                        timer = 0;
                    }

                    return;
                case 7: //cleric
                    anims.Update((int)ai);



                    return;
                case 8: //totem
                    return;
            }
        }

        public void Draw()
        {
            if (spawning)
            {
                return;
            }

            if (anims.getFirstAnim() != null)
            {
                anims.Draw(new((int)position.X, (int)position.Y), Color.White, 0f, Vector2.Zero, Vector2.One, direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0.8f);
            }

            else if (anim != null)
            {
                anim.Draw(new((int)position.X, (int)position.Y), Color.White, 0f, Vector2.Zero, Vector2.One, direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0.8f);
            }

            else
            {
                Globals.SpriteBatch.Draw(texture, new((int)position.X, (int)position.Y), null, Color.White, rotation, origin, scale, direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 1f);
            }
        }
        public void Kill()
        {
            SoundManager.PlaySound(deathSound, 1f, 1f, 0f);

            for (int i = 0; i < 10; i++)
            {
                ParticleData spawnParticleData = new()
                {
                    opacityStart = 1f,
                    opacityEnd = 1f,
                    sizeStart = 6,
                    sizeEnd = 0,
                    colorStart = Color.White,
                    colorEnd = Color.White,
                    velocity = new(Globals.RandomFloat(-200, 200), Globals.RandomFloat(-200, 200)),
                    lifespan = 0.2f,
                    rotationSpeed = Globals.RandomFloat(-0.5f, 0.5f)
                };

                Particle spawnParticle = new(center, spawnParticleData);
                ParticleManager.AddParticle(spawnParticle);
            }

            switch (aiType)
            {
                case 4: //slime
                    for (int i = 0; i < 3; i++)
                    {
                        Mob minislime = new Mob(center + new Vector2(Globals.RandomFloat(-5, 5), Globals.RandomFloat(-5, 5)), 5);
                        MobManager.AddMob(minislime);
                    }
                    break;
            }

            active = false;
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

        public void HitByProj(Projectile p)
        {
            if (p.velocity.X + p.velocity.Y > 0)
            {
                velocity += Vector2.Normalize(p.velocity) * p.knockback / knockbackResist;
            }
            else
            {
                velocity += Vector2.Normalize(GameManager.GetPlayer().center.DirectionTo(center)) * p.knockback / knockbackResist;

            }

            SoundManager.PlaySound(hitSound, 0.5f, Globals.RandomFloat (-0.5f, 0.5f), 0f);

            switch (aiType)
            {
                case 1: //bat

                    break;
            }
        }

        public void Spawn()
        {
            MobManager.AddMob(this);
            spawning = true;
        }
    }
}
