using HYPERMAGE.Helpers;
using HYPERMAGE.Managers;
using HYPERMAGE.Particles;
using HYPERMAGE.Spells;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// olddd

namespace HYPERMAGE.Models
{
    public class Mob
    {
        public Texture2D texture;
        public Animation anim;

        public Vector2 position;
        public Vector2 prevPosition;

        public int aiType;
        public float speed;
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
        public bool contactDamage = true;
        public float knockbackResist;
        public int spawnCost;

        public bool spawning;
        public float spawnTimer;

        private List <Projectile> projectileImmunity = [];
        public Mob(Vector2 position, int aiType)
        {
            this.position = position;
            this.aiType = aiType;

            switch (aiType)
            {
                case 0:
                    break;
                case 1: //bat
                    anim = new Animation(Globals.Content.Load<Texture2D>("bat"), 2, 1, 0.2f);
                    speed = 1f;
                    health = 2f;
                    knockbackResist = 0.6f;
                    spawnCost = 1;
                    break;
                case 2: //wisp
                    texture = Globals.Content.Load<Texture2D>("particle");
                    speed = 1f;
                    scale = 2f;
                    health = 3f;
                    knockbackResist = 0.6f;
                    spawnCost = 5;
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

            origin = new Vector2(width / scale / 2, height / scale / 2) ;

            center = position + origin;

            hitbox = PolygonFactory.CreateRectangle((int)center.X, (int)center.Y, width, height);

            active = true;
        }
        public void Draw()
        {
            if (spawning)
            {
                return;
            }

            if (anim != null)
            {
                anim.Draw(new((int)position.X, (int)position.Y), Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.8f);
            }

            else
            {
                Globals.SpriteBatch.Draw(texture, new((int)position.X, (int)position.Y), null, Color.White, rotation, origin, scale, SpriteEffects.None, 1f);
            }
        }

        private float timer = 0;

        private bool charging = false;
        public void Update()
        {
            if (spawning)
            {
                spawnTimer += Globals.TotalSeconds;

                if (spawnTimer > 2f)
                {
                    spawning = false;

                    for (int i = 0; i < Globals.Random.Next(5) + 5; i++)
                    {
                        ParticleData particleData = new()
                        {
                            lifespan = Globals.RandomFloat(0.1f, 0.3f),
                            sizeStart = 1f,
                            opacityEnd = 0f,
                            rotationSpeed = 0.15f,
                            velocity = new(Globals.RandomFloat(-100f, 100f), Globals.RandomFloat(-100f, 100f)),
                            resistance = Globals.RandomFloat(1, 1.25f)
                        };

                        Particle spawnParticle = new(center, particleData);
                        ParticleManager.AddParticle(spawnParticle);
                    }
                }    

                return;
            }

            if (anim != null)
            {
                anim.Update();
            }

            center = position + origin;

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

            foreach (Projectile p in ProjectileManager.projectiles.ToList())
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

            position += velocity * speed * Globals.TotalSeconds * 30;

            timer += Globals.TotalSeconds;

            prevPosition = position;

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

            switch (aiType)
            {
                case 0:
                    return;
                case 1: //bat

                    if (Math.Abs(GameManager.GetPlayer().center.X - center.X) < 20 && Math.Abs(GameManager.GetPlayer().center.Y - center.Y) < 20)
                    {
                        if (!charging && timer > 3)
                        {
                            charging = true;
                            timer = 0;
                        }
                    }

                    if (charging)
                    {
                        velocity += Vector2.Normalize(GameManager.GetPlayer().center - center) * Globals.TotalSeconds * speed * 6;

                        if(timer > 0.15)
                        {
                            charging = false;
                            timer = 0;
                        }
                    }
                    else
                    {
                        velocity += (Vector2.Normalize(GameManager.GetPlayer().center - center) + new Vector2(Globals.RandomFloat(-1.5f, 1.5f), Globals.RandomFloat(-1.5f, 1.5f))) * Globals.TotalSeconds * speed;
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

                    if (timer > 2)
                    {
                        Projectile projectile = new(center, -1, 1f, Vector2.Normalize(GameManager.GetPlayer().center - center) * 100f, 10f, center.DirectionTo(GameManager.GetPlayer().center).ToRotation() + MathHelper.ToRadians(90f));
                        ProjectileManager.AddProjectile(projectile);

                        timer = 0;
                    }
                    
                    return;
            }
        }

        public void Kill()
        {
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
            velocity += Vector2.Normalize(p.velocity) * p.knockback / knockbackResist;

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
