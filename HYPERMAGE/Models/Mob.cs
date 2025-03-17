using HYPERMAGE.Helpers;
using HYPERMAGE.Managers;
using HYPERMAGE.Particles;
using HYPERMAGE.Spells;
using HYPERMAGE.UI.UIElements;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
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
        public float ai3;
        public float ai4;
        public float ai5;
        public float ai6;
        public float ai7;
        public float ai8;
        public float ai9;
        public float ai10;
        public float ai11;

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

        public int manaDrop;

        public bool turnToPlayer = true;
        public int direction;

        public bool contactDamage = true;
        public float knockbackResist = 1f;
        public bool knockbackImmune = false;
        public int spawnCost;
        public bool flying = false;
        public bool immune = false;

        public bool spawning;
        public float spawnTimer;

        public bool boss = false;
        public bool draw = true;
        public bool dying = false;

        private List <Projectile> projectileImmunity = [];
        public Mob(Vector2 position, int aiType)
        {
            this.aiType = aiType;
            this.position = position;

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
                    texture = Globals.Content.Load<Texture2D>("enemybullet1");
                    hitSound = Globals.Content.Load<SoundEffect>("hit");
                    deathSound = Globals.Content.Load<SoundEffect>("shoot");

                    health = 3f;
                    knockbackResist = 0.6f;
                    spawnCost = 3;
                    flying = true;

                    timer = Globals.RandomFloat(0, 1);
                    break;
                case 3: //wizard
                    anims.AddAnimation(0, new(Globals.Content.Load<Texture2D>("wizard"), 2, 2, 0.5f, 1));
                    anims.AddAnimation(1, new(Globals.Content.Load<Texture2D>("wizard"), 2, 2, 0.5f, 2));

                    hitSound = Globals.Content.Load<SoundEffect>("hit");
                    deathSound = Globals.Content.Load<SoundEffect>("bwowop");

                    contactDamage = false;

                    width = 23;
                    height = 24;

                    health = 18f;
                    spawnCost = 4;
                    break;
                case 4: //slime
                    anim = new Animation(Globals.Content.Load<Texture2D>("slime"), 2, 1, 0.2f);

                    hitSound = Globals.Content.Load<SoundEffect>("hit");
                    deathSound = Globals.Content.Load<SoundEffect>("hit");

                    health = 4f;
                    knockbackResist = 0.8f;
                    spawnCost = 2;

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

                    health = 12f;
                    spawnCost = 6;
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

                    hitSound = Globals.Content.Load<SoundEffect>("hit");
                    deathSound = Globals.Content.Load<SoundEffect>("shoot");

                    contactDamage = false;

                    health = 30f;
                    spawnCost = 6;
                    break;
                case 9: //empyrean wisp
                    hitSound = Globals.Content.Load<SoundEffect>("hit");
                    deathSound = Globals.Content.Load<SoundEffect>("shoot");

                    texture = Globals.Content.Load<Texture2D>("empyreanwisp");
                    contactDamage = false;

                    knockbackResist = 200f;
                    health = 120f;
                    boss = true;
                    flying = true;

                    UIManager.AddElement(new BossBar(Globals.Content.Load<Texture2D>("bossbar"), new(55, 170), this));

                    break;
                case 10: //eye monster
                    texture = Globals.Content.Load<Texture2D>("eyemonster1");

                    hitSound = Globals.Content.Load<SoundEffect>("hit");
                    deathSound = Globals.Content.Load<SoundEffect>("shoot");

                    contactDamage = false;

                    health = 8f;
                    spawnCost = 3;
                    flying = true;
                    break;
                case 11: //wall
                    hitSound = Globals.Content.Load<SoundEffect>("hit");
                    deathSound = Globals.Content.Load<SoundEffect>("hit2");

                    anims.AddAnimation(0, new(Globals.Content.Load<Texture2D>("walleye"), 1, 3, 0.5f, 1));
                    anims.AddAnimation(1, new(Globals.Content.Load<Texture2D>("walleye"), 1, 3, 0.5f, 2));
                    anims.AddAnimation(2, new(Globals.Content.Load<Texture2D>("walleye"), 1, 3, 0.5f, 3));

                    contactDamage = false;

                    width = 10;
                    height = 10;

                    knockbackImmune = true;
                    health = 40f;
                    boss = true;
                    flying = true;

                    UIManager.AddElement(new BossBar(Globals.Content.Load<Texture2D>("bossbar"), new(55, 170), this));

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

            origin = new Vector2(width / 2f, height / 2f) ;

            hitbox = PolygonFactory.CreateRectangle((int)position.X, (int)position.Y, width, height);

            active = true;
        }
        public void Update()
        {
            center = position;

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
                            velocity = new(Globals.RandomFloat(-200, 200), Globals.RandomFloat(-400, 200)),
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

            timer += Globals.TotalSeconds;

            if (anim != null)
            {
                anim.Update();
            }

            hitbox = PolygonFactory.CreateRectangle((int)position.X, (int)position.Y, width, height);

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

            if (health <= 0 && !dying)
            {
                Kill();
            }

            if (draw && !immune)
            {
                foreach (Projectile p in ProjectileManager.projectiles.ToList())
                {
                    if (projectileImmunity.Count != 0)
                    {
                        foreach (Projectile p2 in projectileImmunity.ToList())
                        {
                            if (p != p2 && p.aiType == p2.aiType && p.immuneSameType)
                            {
                                break;
                            }

                            else
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
                        }
                    }

                    else
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
                }

                if (!knockbackImmune)
                {
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
                }
            }

            if (!boss)
            {
                if (!flying)
                {
                    if (position.X < GameManager.groundBounds.X + 5)
                    {
                        velocity.X += Globals.TotalSeconds * Math.Abs(position.X - GameManager.groundBounds.X + 5);
                    }

                    if (position.X > GameManager.groundBounds.Z - 5)
                    {
                        velocity.X -= Globals.TotalSeconds * Math.Abs(position.X - (GameManager.groundBounds.Z - 5));
                    }

                    if (position.Y < GameManager.groundBounds.Y + 5)
                    {
                        velocity.Y += Globals.TotalSeconds * Math.Abs(position.Y - GameManager.groundBounds.Y + 5);
                    }

                    if (position.Y > GameManager.groundBounds.W - 5)
                    {
                        velocity.Y -= Globals.TotalSeconds * Math.Abs(position.Y - (GameManager.groundBounds.W - 5));
                    }
                }

                if (position.X < GameManager.bounds.X + 5)
                {
                    velocity.X += Globals.TotalSeconds * Math.Abs(position.X - GameManager.bounds.X + 5);
                }

                if (position.X > GameManager.bounds.Z - 5)
                {
                    velocity.X -= Globals.TotalSeconds * Math.Abs(position.X - (GameManager.bounds.Z - 5));
                }

                if (position.Y < GameManager.bounds.Y + 5)
                {
                    velocity.Y += Globals.TotalSeconds * Math.Abs(position.Y - GameManager.bounds.Y + 5);
                }

                if (position.Y > GameManager.bounds.W  - 5)
                {
                    velocity.Y -= Globals.TotalSeconds * Math.Abs(position.Y - (GameManager.bounds.W - 5));
                }
            }    

            position += velocity * speed * Globals.TotalSeconds * 30;

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
                    break;
                case 1: //bat

                    if (Math.Abs(GameManager.GetPlayer().center.X - center.X) < 30 && Math.Abs(GameManager.GetPlayer().center.Y - center.Y) < 50)
                    {
                        if (ai == 0 && timer > 3)
                        {
                            SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("chirp"), 0.6f, 1f, 0);

                            ai = 1;
                            timer = 0;
                        }
                    }

                    if (ai == 1)
                    {
                        velocity += Vector2.Normalize(GameManager.GetPlayer().center - center) * Globals.TotalSeconds * speed * 15;

                        if(timer > 0.45)
                        {
                            ai = 0;
                            timer = 0;
                        }
                    }

                    else
                    {
                        velocity += (Vector2.Normalize(GameManager.GetPlayer().center - center) + new Vector2(Globals.RandomFloat(-1.5f, 1.5f), Globals.RandomFloat(-1.5f, 1.5f))) * Globals.TotalSeconds * speed * 4;
                    }

                    velocity /= 1.05f;

                    break;
                case 2: //wisp

                    velocity += new Vector2(Globals.RandomFloat(-1, 1), Globals.RandomFloat(-1, 1));
                    velocity /= 2f;

                    if (Globals.Random.Next(3) == 0)
                    {
                        ParticleData particleData = new()
                        {
                            opacityStart = 1f,
                            opacityEnd = 0f,
                            sizeStart = Globals.RandomFloat(2, 4),
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

                    if (timer >= 3f)
                    {
                        Projectile projectile = new(center, -1, 1f, Vector2.Normalize(GameManager.GetPlayer().center - center) * 90f, 10f, center.DirectionTo(GameManager.GetPlayer().center).ToRotation() + MathHelper.ToRadians(90f));
                        ProjectileManager.AddProjectile(projectile);

                        SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("hit"), 0.6f, 1, 0);

                        timer = 0;
                    }

                    break;
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

                            for (int i = 0; i < 5; i++)
                            {
                                Vector2 projVelocity = Vector2.Normalize(GameManager.GetPlayer().center - center).RotatedBy(MathHelper.ToRadians(-25 + (i * 12.5f))) * 65f;

                                Projectile projectile = new(center, -2, 1f, projVelocity, 10f, 0f);
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
                                        rotationSpeed = Globals.RandomFloat(-0.5f, 0.5f),
                                        friendly = false
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

                            for (int i = 0; i < 5; i++)
                            {
                                Vector2 projVelocity = Vector2.Normalize(GameManager.GetPlayer().center - center).RotatedBy(MathHelper.ToRadians(-25 + (i * 12.5f))) * 65f;

                                Projectile projectile = new(center, -2, 1f, projVelocity, 10f, 0f);
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
                                        rotationSpeed = Globals.RandomFloat(-0.5f, 0.5f),
                                        friendly = false
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

                    break;

                case 4: //slime
                    velocity += (Vector2.Normalize(GameManager.GetPlayer().center - center) * Globals.TotalSeconds * speed * 3);
                    velocity /= 1.05f;

                    break;
                case 5: //minislime
                    velocity += (Vector2.Normalize(GameManager.GetPlayer().center - center) * Globals.TotalSeconds * speed * 5);
                    velocity /= 1.05f;

                    break;

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

                    break;
                case 7: //cleric
                    anims.Update((int)ai);



                    return;
                case 8: //totem
                    break;
                case 9: //empyrean wisp

                    if (ai9 == 0)
                    {
                        GameManager.AddScreenShake(1.2f, 20f);
                        GameManager.AddAbberationPowerForce(200, 100);

                        SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("cry"), 1f, -0.3f, 0f);
                        SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("cry"), 1f, -0.9f, 0f);
                        SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("wavy"), 2f, 0f, 0f);
                        SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("lowbass"), 2f, 0f, 0f);

                        for (int i = 0; i < 30; i++)
                        {
                            ParticleData pd2 = new()
                            {
                                opacityStart = 1f,
                                opacityEnd = 1f,
                                sizeStart = 10,
                                sizeEnd = 0,
                                colorStart = Color.White,
                                colorEnd = Color.White,
                                velocity = new(Globals.RandomFloat(-400, 400), Globals.RandomFloat(-400, 400)),
                                lifespan = Globals.RandomFloat(0.5f, 2f),
                                rotationSpeed = Globals.RandomFloat(-0.5f, 0.5f)
                            };

                            Particle p2 = new(position, pd2);
                            ParticleManager.AddParticle(p2);
                        }

                        ai9 = 1;
                    }

                    if (draw && ai10 == 0)
                    {
                        ParticleData pd = new()
                        {
                            opacityStart = 0.8f,
                            opacityEnd = 0f,
                            sizeStart = 12,
                            sizeEnd = 0,
                            colorStart = Color.White,
                            colorEnd = Color.Pink,
                            velocity = new(Globals.RandomFloat(-60, 60), Globals.RandomFloat(-300, 0)),
                            lifespan = 0.5f,
                            rotationSpeed = Globals.RandomFloat(-0.5f, 0.5f)
                        };

                        Particle p = new(position, pd);
                        ParticleManager.AddParticle(p);
                    }

                    if (timer >= 1.5f && ai == 0)
                    {
                        ai = 2;
                    }

                    velocity /= 1.2f;

                    ai8 += Globals.TotalSeconds;

                    if (dying)
                    {
                        if (ai3 == 0)
                        {
                            draw = true;

                            GameScene.AddHitstop(20);

                            GameManager.DrawLightOrangeScreenTint(0.2f);
                            GameManager.AddScreenShake(0.3f, 10f);
                            GameManager.AddAbberationPowerForce(1000, 100);

                            SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("hit2"), 1f, Globals.RandomFloat(-0.2f, 0.2f), 0f);
                            SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("bigexplosion"), 1f, Globals.RandomFloat(-0.2f, 0.2f), 0f);

                            velocity += new Vector2(Globals.RandomFloat(-10, 10), Globals.RandomFloat(-10, 10)) + position.DirectionTo(new(160, 90)) * 10;
                            for (int i = 0; i < 15; i++)
                            {
                                ParticleData pd = new()
                                {
                                    opacityStart = 1f,
                                    opacityEnd = 1f,
                                    sizeStart = Globals.RandomFloat(5, 20),
                                    sizeEnd = 0,
                                    colorStart = Color.White,
                                    colorEnd = Color.White,
                                    velocity = new(Globals.RandomFloat(-250, 250), Globals.RandomFloat(-250, 250)),
                                    lifespan = Globals.RandomFloat(0.2f, 0.5f),
                                    rotationSpeed = Globals.RandomFloat(-0.5f, 0.5f)
                                };

                                Particle p = new(position, pd);
                                ParticleManager.AddParticle(p);
                            }


                            for (int i = 0; i < 15; i++)
                            {
                                ParticleData pd = new()
                                {
                                    opacityStart = 1f,
                                    opacityEnd = 1f,
                                    sizeStart = Globals.RandomFloat(2, 5),
                                    sizeEnd = 0,
                                    colorStart = Color.White,
                                    colorEnd = Color.White,
                                    velocity = new(Globals.RandomFloat(-700, 700), Globals.RandomFloat(-700, 700)),
                                    lifespan = Globals.RandomFloat(0.1f, 0.3f),
                                    rotationSpeed = Globals.RandomFloat(-0.5f, 0.5f)
                                };

                                Particle p = new(position, pd);
                                ParticleManager.AddParticle(p);
                            }

                            ai3++;
                        }

                        ai2 += Globals.TotalSeconds;

                        if (ai3 < 4 ? ai2 >= 0.3f : ai2 >= 0.6f)
                        {
                            if (ai3 < 4)
                            {
                                GameManager.AddScreenShake(0.1f, 5f);
                                GameManager.AddAbberationPowerForce(1000, 100);

                                SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("hit2"), 1f, Globals.RandomFloat(-0.2f, 0.2f), 0f);
                                SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("bigexplosion"), 1f, Globals.RandomFloat(-0.2f, 0.2f), 0f);

                                velocity += new Vector2(Globals.RandomFloat(-10, 10), Globals.RandomFloat(-10, 10)) + position.DirectionTo(new(160, 90)) * 10;

                                for (int i = 0; i < 15; i++)
                                {
                                    ParticleData pd = new()
                                    {
                                        opacityStart = 1f,
                                        opacityEnd = 1f,
                                        sizeStart = Globals.RandomFloat(5, 20),
                                        sizeEnd = 0,
                                        colorStart = Color.White,
                                        colorEnd = Color.White,
                                        velocity = new(Globals.RandomFloat(-250, 250), Globals.RandomFloat(-250, 250)),
                                        lifespan = Globals.RandomFloat(0.2f, 0.5f),
                                        rotationSpeed = Globals.RandomFloat(-0.5f, 0.5f)
                                    };

                                    Particle p = new(position, pd);
                                    ParticleManager.AddParticle(p);
                                }

                                for (int i = 0; i < 15; i++)
                                {
                                    ParticleData pd = new()
                                    {
                                        opacityStart = 1f,
                                        opacityEnd = 1f,
                                        sizeStart = Globals.RandomFloat(2, 5),
                                        sizeEnd = 0,
                                        colorStart = Color.White,
                                        colorEnd = Color.White,
                                        velocity = new(Globals.RandomFloat(-700, 700), Globals.RandomFloat(-700, 700)),
                                        lifespan = Globals.RandomFloat(0.1f, 0.3f),
                                        rotationSpeed = Globals.RandomFloat(-0.5f, 0.5f)
                                    };

                                    Particle p = new(position, pd);
                                    ParticleManager.AddParticle(p);
                                }
                            }

                            else
                            {
                                GameScene.AddHitstop(20);

                                GameManager.DrawLightOrangeScreenTint(0.2f);
                                GameManager.AddScreenShake(0.4f, 10f);
                                GameManager.AddAbberationPowerForce(1000, 200);

                                SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("cry"), 1f, -0.3f, 0f);
                                SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("cry"), 1f, -0.9f, 0f);
                                SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("wavy"), 2f, 0f, 0f);
                                SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("lowbass"), 2f, 0f, 0f);
                                SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("hit2"), 1f, Globals.RandomFloat(-0.2f, 0.2f), 0f);
                                SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("bigexplosion"), 1f, Globals.RandomFloat(-0.2f, 0.2f), 0f);

                                for (int i = 0; i < 40; i++)
                                {
                                    ParticleData pd = new()
                                    {
                                        opacityStart = 1f,
                                        opacityEnd = 1f,
                                        sizeStart = Globals.RandomFloat(5, 20),
                                        sizeEnd = 0,
                                        colorStart = Color.White,
                                        colorEnd = Color.White,
                                        velocity = new(Globals.RandomFloat(-500, 500), Globals.RandomFloat(-500, 500)),
                                        lifespan = Globals.RandomFloat(0.2f, 2f),
                                        rotationSpeed = Globals.RandomFloat(-0.5f, 0.5f)
                                    };

                                    Particle p = new(position, pd);
                                    ParticleManager.AddParticle(p);
                                }


                                for (int i = 0; i < 15; i++)
                                {
                                    ParticleData pd = new()
                                    {
                                        opacityStart = 1f,
                                        opacityEnd = 1f,
                                        sizeStart = Globals.RandomFloat(2, 5),
                                        sizeEnd = 0,
                                        colorStart = Color.White,
                                        colorEnd = Color.White,
                                        velocity = new(Globals.RandomFloat(-700, 700), Globals.RandomFloat(-700, 700)),
                                        lifespan = Globals.RandomFloat(0.1f, 0.3f),
                                        rotationSpeed = Globals.RandomFloat(-0.5f, 0.5f)
                                    };

                                    Particle p = new(position, pd);
                                    ParticleManager.AddParticle(p);
                                }
                            }

                            ai2 = 0;

                            ai3++;
                        }

                        if (ai3 > 4)
                        {
                            GameScene.AddHitstop(20);
                            active = false;
                        }
                    }



                    if (ai == 1)
                    {
                        if (ai8 >= 12f)
                        {
                            ai2 += Globals.TotalSeconds;

                            if (ai2 >= 0.25f)
                            {
                                if (ai5 == 0)
                                {
                                    SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("shoot"), 1f, -0.3f, 0f);

                                    draw = false;

                                    for (int i = 0; i < 10; i++)
                                    {
                                        ParticleData pd = new()
                                        {
                                            opacityStart = 1f,
                                            opacityEnd = 1f,
                                            sizeStart = 6,
                                            sizeEnd = 0,
                                            colorStart = Color.White,
                                            colorEnd = Color.White,
                                            velocity = new(Globals.RandomFloat(-400, 400), Globals.RandomFloat(-400, 400)),
                                            lifespan = 0.2f,
                                            rotationSpeed = Globals.RandomFloat(-0.5f, 0.5f),
                                            friendly = false
                                        };

                                        Particle p = new(position, pd);
                                        ParticleManager.AddParticle(p);
                                    }

                                    position = new(160, 90);


                                    ai5 = 1;
                                }
                            }

                            if (ai2 >= 0.75f && ai2 < 0.8f)
                            {
                                SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("hit2"), 1f, -0.3f, 0f);
                                GameManager.AddScreenShake(0.1f, 5f);

                                for (int i = 0; i < 30; i++)
                                {
                                    ParticleData pd = new()
                                    {
                                        opacityStart = 1f,
                                        opacityEnd = 1f,
                                        sizeStart = 6,
                                        sizeEnd = 0,
                                        colorStart = Color.White,
                                        colorEnd = Color.Red,
                                        velocity = Vector2.One.RotatedBy(MathHelper.ToRadians(Globals.RandomFloat(0, 360))) * 300,
                                        lifespan = 0.2f,
                                        rotationSpeed = Globals.RandomFloat(-0.5f, 0.5f),
                                        friendly = false
                                    };

                                    Particle p = new(position, pd);
                                    ParticleManager.AddParticle(p);
                                }
                            }

                            if (ai2 >= 1.5f)
                            {
                                if (ai6 == 0)
                                {
                                    draw = true;
                                    SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("heavyhit"), 1f, -0.3f, 0f);

                                    GameManager.AddScreenShake(0.25f, 5f);
                                    GameManager.AddAbberationPowerForce(300, 20);

                                    for (int i = 0; i < 20; i++)
                                    {
                                        ParticleData pd = new()
                                        {
                                            opacityStart = 1f,
                                            opacityEnd = 1f,
                                            sizeStart = 6,
                                            sizeEnd = 0,
                                            colorStart = Color.White,
                                            colorEnd = Color.White,
                                            velocity = new(Globals.RandomFloat(-400, 400), Globals.RandomFloat(-400, 400)),
                                            lifespan = 0.2f,
                                            rotationSpeed = Globals.RandomFloat(-0.5f, 0.5f),
                                            friendly = false
                                        };

                                        Particle p = new(position, pd);
                                        ParticleManager.AddParticle(p);
                                    }

                                    ai6 = 1;
                                }
                            }

                            if (ai2 >= 1.75f && ai4 < 24)
                            {
                                ai3 += Globals.TotalSeconds;

                                if (ai3 >= 0.25f)
                                {
                                    ai4++;

                                    SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("hit2"), 1f, Globals.RandomFloat(-0.5f, 0), 0f);
                                    SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("bop"), 1f, Globals.RandomFloat(-0.5f, 0), 0f);

                                    for (int i = 0; i < 12; i++)
                                    {
                                        Projectile projectile = new(position, ai4 % 4 == 0 ? -5 : -4, 1f, (Vector2.One * (ai4 % 4 == 0 ? 50 : 70)).RotatedBy(MathHelper.ToRadians(MathHelper.Lerp(0, 360, i / 12f) + (ai4 * 10))), 10f);
                                        ProjectileManager.AddProjectile(projectile);
                                    }

                                    ai3 = 0;
                                }
                            }

                            if (ai4 == 24)
                            {
                                SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("wavy"), 2f, 0f, 0f);
                                SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("lastingexplosion"), 1f, Globals.RandomFloat(-0.5f, 0), 0f);


                                for (int i = 0; i < 10; i++)
                                {
                                    Projectile projectile = new(position, -5, 1f, (Vector2.One * 90).RotatedBy(MathHelper.ToRadians(MathHelper.Lerp(0, 360, i / 10f))), 10f);
                                    ProjectileManager.AddProjectile(projectile);
                                }

                                ai4++;
                            }

                            if (ai4 > 24)
                            {
                                ai7 += Globals.TotalSeconds;
                                ai10 = 1;
                            }

                            if (ai7 >= 4f)
                            {
                                SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("shoot"), 1f, -0.3f, 0f);
                                SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("heavyhit"), 1f, -0.3f, 0f);
                                SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("cry"), 1f, 0.5f, 0f);

                                GameManager.AddScreenShake(0.2f, 5f);
                                GameManager.AddAbberationPowerForce(300, 20);

                                for (int i = 0; i < 20; i++)
                                {
                                    ParticleData pd2 = new()
                                    {
                                        opacityStart = 1f,
                                        opacityEnd = 1f,
                                        sizeStart = 6,
                                        sizeEnd = 0,
                                        colorStart = Color.White,
                                        colorEnd = Color.White,
                                        velocity = new(Globals.RandomFloat(-250, 250), Globals.RandomFloat(-250, 250)),
                                        lifespan = Globals.RandomFloat(0.5f, 1f),
                                        rotationSpeed = Globals.RandomFloat(-0.5f, 0.5f),
                                        friendly = false
                                    };

                                    Particle p2 = new(position, pd2);
                                    ParticleManager.AddParticle(p2);
                                }

                                ai = 2;

                                ai2 = 0;
                                ai3 = 0;
                                ai4 = 0;
                                ai5 = 0;
                                ai6 = 0;
                                ai7 = 0;

                                ai8 = 0;
                                ai10 = 0;
                            }
                        }
                        
                        else
                        {
                            ai = 2;
                        }
                    }

                    if (ai == 2)
                    {
                        ai5 += Globals.TotalSeconds;

                        if (ai5 >= 0.25f)
                        {
                            if (ai2 == 0)
                            {
                                SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("shoot"), 1f, -0.3f, 0f);

                                draw = false;

                                for (int i = 0; i < 10; i++)
                                {
                                    ParticleData pd = new()
                                    {
                                        opacityStart = 1f,
                                        opacityEnd = 1f,
                                        sizeStart = 6,
                                        sizeEnd = 0,
                                        colorStart = Color.White,
                                        colorEnd = Color.White,
                                        velocity = new(Globals.RandomFloat(-400, 400), Globals.RandomFloat(-400, 400)),
                                        lifespan = 0.2f,
                                        rotationSpeed = Globals.RandomFloat(-0.5f, 0.5f),
                                        friendly = false
                                    };

                                    Particle p = new(position, pd);
                                    ParticleManager.AddParticle(p);
                                }

                                if (Globals.Random.Next(2) == 0)
                                {
                                    if (GameManager.GetPlayer().center.X < 160)
                                    {
                                        position = new Vector2(320 - 100, Globals.RandomFloat(50, 180 - 50));
                                    }

                                    else
                                    {
                                        position = new Vector2(100, Globals.RandomFloat(50, 180 - 50));
                                    }
                                }

                                else
                                {
                                    if (GameManager.GetPlayer().center.Y < 90)
                                    {
                                        position = new Vector2(Globals.RandomFloat(100, 320 - 100), 180 - 50);
                                    }

                                    else
                                    {
                                        position = new Vector2(Globals.RandomFloat(100, 320 - 100), 50);
                                    }
                                }


                                ai2 = 1;
                            }
                        }

                        if (ai5 >= 0.75f)
                        {
                            if (ai3 == 0)
                            {
                                draw = true;
                                SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("shoot"), 1f, -0.3f, 0f);

                                for (int i = 0; i < 10; i++)
                                {
                                    ParticleData pd = new()
                                    {
                                        opacityStart = 1f,
                                        opacityEnd = 1f,
                                        sizeStart = 6,
                                        sizeEnd = 0,
                                        colorStart = Color.White,
                                        colorEnd = Color.White,
                                        velocity = new(Globals.RandomFloat(-400, 400), Globals.RandomFloat(-400, 400)),
                                        lifespan = 0.2f,
                                        rotationSpeed = Globals.RandomFloat(-0.5f, 0.5f),
                                        friendly = false
                                    };

                                    Particle p = new(position, pd);
                                    ParticleManager.AddParticle(p);
                                }

                                ai3 = 1;
                            }
                        }

                        if (ai5 >= 1f)
                        {
                            SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("shoot"), 1f, Globals.RandomFloat(-0.5f, 0), 0f);
                            SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("hit2"), 1f, Globals.RandomFloat(-0.5f, 0), 0f);


                            for (int i = 0; i < 5; i++)
                            {
                                Projectile projectile = new(position, -1, 1f, position.DirectionTo(GameManager.GetPlayer().center).RotatedBy(MathHelper.ToRadians(-30 + (i * 15))) * 100, 10f);
                                ProjectileManager.AddProjectile(projectile);
                            }

                            velocity -= position.DirectionTo(GameManager.GetPlayer().center) * Globals.TotalSeconds * 600;

                            if (ai8 > 18f)
                            {
                                ai = 1;
                            }

                            else
                            {
                                ai = Globals.Random.Next(4) + 1;
                            }

                            ai2 = 0;
                            ai3 = 0;
                            ai4 = 0;
                            ai5 = 0;
                        }
                    }

                    if (ai == 3)
                    {
                        ai2 += Globals.TotalSeconds;

                        if (ai2 >= 0.5f)
                        {
                            if (ai3 == 0)
                            {
                                SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("cry"), 1f, -0.3f, 0f);

                                draw = false;

                                for (int i = 0; i < 10; i++)
                                {
                                    ParticleData pd = new()
                                    {
                                        opacityStart = 1f,
                                        opacityEnd = 1f,
                                        sizeStart = 6,
                                        sizeEnd = 0,
                                        colorStart = Color.White,
                                        colorEnd = Color.White,
                                        velocity = new(Globals.RandomFloat(-400, 400), Globals.RandomFloat(-400, 400)),
                                        lifespan = 0.2f,
                                        rotationSpeed = Globals.RandomFloat(-0.5f, 0.5f),
                                        friendly = false
                                    };

                                    Particle p = new(position, pd);
                                    ParticleManager.AddParticle(p);
                                }

                                ai3 = 1;
                            }
                        }

                        if (ai2 >= 1.5f)
                        {
                            if (ai3 == 1)
                            {
                                ai3 = 2;

                                draw = true;

                                ai6 = GameManager.GetPlayer().center.Y;

                                position = new(-20, ai6);

                                SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("hit"), 1f, Globals.RandomFloat(-0.5f, 0), 0f);


                                for (int i = 0; i < 100; i++)
                                {
                                    ParticleData pd = new()
                                    {
                                        opacityStart = 1f,
                                        opacityEnd = 1f,
                                        sizeStart = 5,
                                        sizeEnd = 0,
                                        colorStart = Color.White,
                                        colorEnd = Color.Red,
                                        velocity = new(Globals.RandomFloat(-25, 25), Globals.RandomFloat(-25, 25)),
                                        lifespan = 0.25f,
                                        rotationSpeed = Globals.RandomFloat(-0.5f, 0.5f),
                                        friendly = false
                                    };

                                    Particle p = new(new(Globals.RandomFloat(0, 320), ai6 + Globals.RandomFloat(-5, 5)), pd);
                                    ParticleManager.AddParticle(p);
                                }
                            }
                        }

                        if (ai2 >= 2f && ai3 == 2)
                        {
                            if (ai5 == 0)
                            {
                                ParticleData pd = new()
                                {
                                    opacityStart = 1f,
                                    opacityEnd = 1f,
                                    sizeStart = 6,
                                    sizeEnd = 0,
                                    colorStart = Color.White,
                                    colorEnd = Color.White,
                                    velocity = new(Globals.RandomFloat(-400, 400), Globals.RandomFloat(-400, 400)),
                                    lifespan = 0.2f,
                                    rotationSpeed = Globals.RandomFloat(-0.5f, 0.5f),
                                    friendly = false
                                };

                                Particle p = new(position, pd);
                                ParticleManager.AddParticle(p);

                                GameManager.AddScreenShake(0.4f, 3f);
                                GameManager.AddAbberationPowerForce(200, 10);
                                SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("bigexplosion"), 1f, Globals.RandomFloat(-0.5f, 0), 0f);

                                contactDamage = true;

                                ai5 = 1;
                            }

                            ai4 += Globals.TotalSeconds;

                            if (ai4 >= 0.03f)
                            {
                                SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("hit"), 1f, Globals.RandomFloat(-0.5f, 0), 0f);

                                Projectile projectile = new(position, -1, 1f, new Vector2(0, -100 * Globals.RandomFloat(0.75f, 1.25f)).RotatedBy(MathHelper.ToRadians(Globals.RandomFloat(-25, 25))), 10f);
                                ProjectileManager.AddProjectile(projectile);

                                Projectile projectile2 = new(position, -1, 1f, new Vector2(0, 100 * Globals.RandomFloat(0.75f, 1.25f)).RotatedBy(MathHelper.ToRadians(Globals.RandomFloat(-25, 25))), 10f);
                                ProjectileManager.AddProjectile(projectile2);

                                ai4 = 0;
                            }

                            velocity += position.DirectionTo(new(340, ai6)) * Globals.TotalSeconds * 150;

                            if (position.X >= 320)
                            {
                                ai2 = 3;

                                draw = false;
                                contactDamage = false;
                                position = new(30, ai6);
                            }
                        }

                        if (ai2 >= 3f)
                        {
                            SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("shoot"), 1f, -0.3f, 0f);

                            draw = true;

                            for (int i = 0; i < 10; i++)
                            {
                                ParticleData pd = new()
                                {
                                    opacityStart = 1f,
                                    opacityEnd = 1f,
                                    sizeStart = 6,
                                    sizeEnd = 0,
                                    colorStart = Color.White,
                                    colorEnd = Color.White,
                                    velocity = new(Globals.RandomFloat(-400, 400), Globals.RandomFloat(-400, 400)),
                                    lifespan = 0.2f,
                                    rotationSpeed = Globals.RandomFloat(-0.5f, 0.5f),
                                    friendly = false
                                };

                                Particle p = new(position, pd);
                                ParticleManager.AddParticle(p);
                            }

                            ai = 2;

                            ai2 = 0;
                            ai3 = 0;
                            ai4 = 0;
                            ai5 = 0;
                            ai6 = 0;
                        }
                    }

                    if (ai == 4)
                    {
                        ai4 += Globals.TotalSeconds;

                        if (ai4 >= 0.4f)
                        {
                            velocity += position.DirectionTo(GameManager.GetPlayer().center) * Globals.TotalSeconds * 35;

                            ai2 += Globals.TotalSeconds;

                            if (ai2 >= 0.15f)
                            {
                                velocity -= position.DirectionTo(GameManager.GetPlayer().center) * Globals.TotalSeconds * 300;

                                ai3++;

                                SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("hit2"), 1f, Globals.RandomFloat(-0.5f, 0), 0f);


                                Projectile projectile = new(position, -1, 1f, position.DirectionTo(GameManager.GetPlayer().center).RotatedBy(MathHelper.ToRadians(Globals.RandomFloat(-30, 30))) * 120, 10f);
                                ProjectileManager.AddProjectile(projectile);

                                if (ai3 % 5 == 0)
                                {
                                    Projectile projectile2 = new(position, -5, 1f, position.DirectionTo(GameManager.GetPlayer().center).RotatedBy(MathHelper.ToRadians(Globals.RandomFloat(-30, 30))) * 80, 10f);
                                    ProjectileManager.AddProjectile(projectile2);
                                }

                                ai2 = 0;
                            }
                        }

                        if (ai3 >= 25)
                        {
                            ai = Globals.Random.Next(4) + 1;

                            ai2 = 0;
                            ai3 = 0;
                            ai4 = 0;
                            ai5 = 0;
                        }
                    }

                    break;

                case 10: // eye monster
                    velocity /= 1.1f;

                    ai += Globals.TotalSeconds;

                    if (ai >= 3f)
                    {
                        for (int i = 0; i < 6; i++)
                        {
                            ParticleData pd = new()
                            {
                                opacityStart = 1f,
                                opacityEnd = 1f,
                                sizeStart = 6,
                                sizeEnd = 0,
                                colorStart = Color.White,
                                colorEnd = Color.White,
                                velocity = new(Globals.RandomFloat(-200, 200), Globals.RandomFloat(-200, 200)),
                                lifespan = 0.2f,
                                rotationSpeed = Globals.RandomFloat(-0.5f, 0.5f),
                                friendly = false
                            };

                            Particle p = new(position, pd);
                            ParticleManager.AddParticle(p);
                        }

                        SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("hit2"), 1f, Globals.RandomFloat(-0.5f, 0), 0f);

                        Projectile projectile = new(position, -5, 1f, position.DirectionTo(GameManager.GetPlayer().center) * 35, 10f);
                        ProjectileManager.AddProjectile(projectile);

                        for (int i = 0; i < 5; i++)
                        {
                            Projectile projectile2 = new(center, -2, 1f, position.DirectionTo(GameManager.GetPlayer().center).RotatedBy(MathHelper.ToRadians(Globals.RandomFloat(-15, 15))) * Globals.RandomFloat(30, 50), 10f, 0f);
                            ProjectileManager.AddProjectile(projectile2);
                        }

                        ai = 0;
                    }

                    if (Globals.Distance(position, GameManager.GetPlayer().center) < 50)
                    {
                        velocity -= position.DirectionTo(GameManager.GetPlayer().center) * Globals.TotalSeconds * 3;
                    }

                    break;
                case 11: //wall
                    velocity /= 1.2f + Globals.TotalSeconds;
                    velocity.X -= 5.8f * Globals.TotalSeconds;

                    anims.Update((int)ai);

                    if (position.X < 0)
                    {
                        position.X = 320;
                    }

                    ai8 += Globals.TotalSeconds;

                    if (ai2 == 0)
                    {
                        GameManager.AddScreenShake(0.6f, 10f);
                        GameManager.AddAbberationPowerForce(500, 20);

                        SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("hit2"), 0.4f, -0.3f, 0f);
                        SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("lowbass"), 0.4f, 0f, 0f);

                        for (int i = 0; i < 15; i++)
                        {
                            ParticleData pd2 = new()
                            {
                                opacityStart = 1f,
                                opacityEnd = 1f,
                                sizeStart = 6,
                                sizeEnd = 0,
                                colorStart = Color.White,
                                colorEnd = Color.White,
                                velocity = new(Globals.RandomFloat(-200, 200), Globals.RandomFloat(-200, 200)),
                                lifespan = Globals.RandomFloat(0.5f, 1f),
                                rotationSpeed = Globals.RandomFloat(-0.5f, 0.5f)
                            };

                            Particle p2 = new(position, pd2);
                            ParticleManager.AddParticle(p2);
                        }

                        ai2 = 1;
                        ai8 = Globals.RandomFloat(0, 8);
                    }

                    if (timer >= Globals.RandomFloat(0.8f, 2.4f) && ai4 == 0)
                    {
                        ai3 = 1;
                        ai4 = 1;
                        ai5 = Globals.RandomFloat(0, 5);
                    }

                    if (ai == 0)
                    {
                        immune = true;
                    }
                    else
                    {
                        immune = false;
                    }

                    if (ai3 == 1)
                    {
                        ai5 += Globals.TotalSeconds;

                        ai = ai5 < 6 ? 0 : ai5 < 7.5f ? 1 : 2;

                        if (ai5 >= 8f)
                        {
                            ai3 = Globals.Random.Next(ai8 >= 15f ? 3 : 2) + 2;
                            ai5 = 0;
                        }    
                    }

                    if (ai3 == 2)
                    {
                        ai = 2;

                        SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("hit2"), 1f, 0f, 0f);

                        for (int i = 0; i < 8; i++)
                        {
                            ParticleData pd2 = new()
                            {
                                opacityStart = 1f,
                                opacityEnd = 1f,
                                sizeStart = 6,
                                sizeEnd = 0,
                                colorStart = Color.White,
                                colorEnd = Color.White,
                                velocity = new(Globals.RandomFloat(-200, 200), Globals.RandomFloat(-200, 200)),
                                lifespan = Globals.RandomFloat(0.5f, 1f),
                                rotationSpeed = Globals.RandomFloat(-0.5f, 0.5f),
                                friendly = false
                            };

                            Particle p2 = new(position, pd2);
                            ParticleManager.AddParticle(p2);
                        }

                        Projectile proj = new(position, -5, 1f, position.DirectionTo(GameManager.GetPlayer().center) * 60, 10f);
                        ProjectileManager.AddProjectile(proj);

                        for (int i = 0; i < 8; i++)
                        {
                            Vector2 vel = position.DirectionTo(GameManager.GetPlayer().center) * 60;
                            Projectile proj2 = new(position + Vector2.One.RotatedBy(MathHelper.ToRadians(i * (360 / 8))) * 7, -2, 1f, vel, 10f);
                            ProjectileManager.AddProjectile(proj2);
                        }

                        ai3 = 1;
                    }

                    if (ai3 == 3)
                    {
                        ai = 2;

                        if (ai7 == 0)
                        {
                            SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("shoot"), 1f, 0f, 0f);
                            SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("hit2"), 1f, 0f, 0f);

                            ai7 = 1;
                        }

                        ai5 += Globals.TotalSeconds;

                        if (ai5 >= 0.04f)
                        {
                            SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("shoot"), 0.35f, Globals.RandomFloat(-0.5f, 0.5f), 0f);

                            ai5 = 0;
                            ai6++;

                            for (int i = 0; i < 3; i++)
                            {
                                ParticleData pd2 = new()
                                {
                                    opacityStart = 1f,
                                    opacityEnd = 1f,
                                    sizeStart = 1,
                                    sizeEnd = 0,
                                    colorStart = Color.White,
                                    colorEnd = Color.White,
                                    velocity = new(Globals.RandomFloat(-100, 100), Globals.RandomFloat(-100, 100)),
                                    lifespan = Globals.RandomFloat(0.05f, 0.2f),
                                    rotationSpeed = Globals.RandomFloat(-0.5f, 0.5f),
                                    friendly = false
                                };

                                Particle p2 = new(position, pd2);
                                ParticleManager.AddParticle(p2);
                            }

                            Projectile proj = new(position, -1, 1f, position.DirectionTo(GameManager.GetPlayer().position).RotatedBy(MathHelper.ToRadians(MathHelper.Lerp(-30, 30, ai6 / 8))) * 70f, 10f);
                            ProjectileManager.AddProjectile(proj);
                        }

                        if (ai6 >= 8)
                        {
                            ai3 = 1;
                            ai5 = 0;
                            ai6 = 0;
                            ai7 = 0;
                        }
                    }

                    if (ai3 == 4)
                    {

                        ai = 2;

                        if (ai6 == 0)
                        {
                            SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("magick2"), 1f, -0.4f, 0f);
                            SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("lowbass"), 1f, 0f, 0f);

                            for (int i = 0; i < 30; i++)
                            {
                                Vector2 pos = position + Vector2.One.RotatedBy(MathHelper.ToRadians(Globals.RandomFloat(0, 360))) * 50;
                                ParticleData pd2 = new()
                                {
                                    opacityStart = 1f,
                                    opacityEnd = 1f,
                                    sizeStart = 4,
                                    sizeEnd = 0,
                                    colorStart = Color.White,
                                    colorEnd = Color.White,
                                    velocity = pos.DirectionTo(position) * Globals.RandomFloat(400, 600) + velocity * 100,
                                    lifespan = Globals.RandomFloat(0.2f, 1f),
                                    rotationSpeed = Globals.RandomFloat(-0.5f, 0.5f),
                                    friendly = false
                                };

                                Particle p2 = new(pos, pd2);
                                ParticleManager.AddParticle(p2);
                            }

                            ai6 = 1;
                        }

                        ai5 += Globals.TotalSeconds;

                        if (ai5 >= 1.5f)
                        {
                            SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("magick"), 1f, -0.2f, 0f);
                            SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("death"), 1f, -0.2f, 0f);

                            for (int i = 0; i < 15; i++)
                            {
                                Projectile proj = new(position, -1, 1f, Vector2.One.RotatedBy(MathHelper.ToRadians(i * (360 / 15))) * 30, 10f);
                                ProjectileManager.AddProjectile(proj);
                            }

                            for (int i = 0; i < 15; i++)
                            {
                                Projectile proj = new(position, -1, 1f, Vector2.One.RotatedBy(MathHelper.ToRadians(i * (360 / 15))) * 40, 10f);
                                ProjectileManager.AddProjectile(proj);
                            }

                            for (int i = 0; i < 15; i++)
                            {
                                Projectile proj = new(position, -1, 1f, Vector2.One.RotatedBy(MathHelper.ToRadians(i * (360 / 15))) * 50, 10f);
                                ProjectileManager.AddProjectile(proj);
                            }

                            ai3 = 1;
                            ai5 = 0;
                            ai6 = 0;
                            ai7 = 0;
                            ai8 = 0;
                        }
                    }

                    break;
            }
        }

        public void Draw()
        {
            if (spawning || !draw)
            {
                return;
            }

            switch (aiType)
            {
                case 10:
                    Globals.SpriteBatch.Draw(Globals.Content.Load<Texture2D>("eyemonster2"), new Vector2((int)position.X + 5, (int)position.Y + 4 + (float)Math.Sin(timer * 3)) + new Vector2(1, 0).RotatedBy(position.DirectionTo(GameManager.GetPlayer().center).ToRotation()) * 3, null, Color.White, rotation, new((int)origin.X, (int)origin.Y), scale, direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0.91f);

                    Globals.SpriteBatch.Draw(texture, new((int)position.X, (int)position.Y + (float)Math.Sin(timer * 3) * 2.5f), null, Color.White, rotation, new((int)origin.X, (int)origin.Y), scale, direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0.9f);

                    return;
                case 9:
                    if (timer <= 1.2f)
                    {
                        GameManager.DrawSpeedLines(Color.Pink, position);
                    }

                    Globals.SpriteBatch.Draw(texture, new((int)position.X, (int)position.Y + (float)Math.Sin(timer * 5) * 2.5f), null, Color.White, rotation, new((int)origin.X, (int)origin.Y), scale, direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 1f);

                    return;
            }

            if (anims.getFirstAnim() != null)
            {
                anims.Draw(new((int)position.X, (int)position.Y), Color.White, 0f, new((int)origin.X, (int)origin.Y), scale, direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0.8f);
            }

            else if (anim != null)
            {
                anim.Draw(new((int)position.X, (int)position.Y), Color.White, 0f, new((int)origin.X, (int)origin.Y), scale, direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0.8f);
            }

            else
            {
                Globals.SpriteBatch.Draw(texture, new((int)position.X, (int)position.Y), null, Color.White, rotation, new((int)origin.X, (int)origin.Y), scale, direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0.9f);
            }
        }
        public void Kill()
        {
            dying = true;

            SoundManager.PlaySound(deathSound, 1f, 1f, 0f);

            for (int i = 0; i < 10; i++)
            {
                ParticleData deathParticleData = new()
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

                Particle deathParticle = new(center, deathParticleData);
                ParticleManager.AddParticle(deathParticle);
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

            if (manaDrop > 0)
            {
                for (int i = 0; i < manaDrop; i++)
                {
                    ParticleData manaData = new()
                    {
                        manaDrop = true,
                        lifespan = 5f,
                        anim = new Animation(Globals.Content.Load<Texture2D>("manadrop"), 5, 1, 0.2f, 1),
                        velocity = new(Globals.RandomFloat(-200, 200), Globals.RandomFloat(-200, 200))
                        
                    };

                    Particle manaDrop = new(center, manaData);
                    ParticleManager.AddParticle(manaDrop);
                }
            }

            if (aiType == 9)
            {
                ai = 5;

                ai2 = 0;
                ai3 = 0;
                ai4 = 0;
                ai5 = 0;
                ai6 = 0;

                return;
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
            if (!knockbackImmune)
            {
                if (p.velocity.X + p.velocity.Y > 0)
                {
                    velocity += Vector2.Normalize(p.velocity) * p.knockback / knockbackResist;
                }

                else
                {
                    velocity += Vector2.Normalize(p.center.DirectionTo(center)) * p.knockback / knockbackResist;
                }
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
            if (!boss)
            {
                spawning = true;
            }

            else
            {
                switch (aiType)
                {
                    case 9: //empyrean wisp

                        break;
                }
            }

            MobManager.AddMob(this);
        }
    }
}
