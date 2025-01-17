using HYPERMAGE.Helpers;
using HYPERMAGE.Managers;
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

            origin = new Vector2(width / 2, height / 2);

            active = true;
        }
        public void Draw()
        {
            if (anim != null)
            {
                anim.Draw(position);
            }

            else
            {
                Globals.SpriteBatch.Draw(texture, position, null, Color.White, rotation, origin, scale, SpriteEffects.None, 1f);
            }
        }

        private int timer = 0;
        private bool charging = false;
        public void Update()
        {
            center = position + origin;

            hitbox = PolygonFactory.CreateRectangle((int)position.X, (int)position.Y, width, height);

            timer++;

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

            switch (aiType)
            {
                case 0:
                    return;
                case 1: //bat

                    if (Math.Abs(GameManager.GetPlayer().center.X - center.X) < 20 && Math.Abs(GameManager.GetPlayer().center.Y - center.Y) < 20)
                    {
                        if (!charging && timer > 200)
                        {
                            charging = true;
                            timer = 0;
                        }
                    }

                    if (charging)
                    {
                        velocity += Vector2.Normalize(GameManager.GetPlayer().center - center) * Globals.TotalSeconds * speed * 6;

                        if(timer > 15)
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

                    position += velocity;

                    anim.Update();

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
    }
}
