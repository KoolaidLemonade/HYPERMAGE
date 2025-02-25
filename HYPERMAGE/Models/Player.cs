using HYPERMAGE.Helpers;
using HYPERMAGE.Managers;
using HYPERMAGE.Particles;
using HYPERMAGE.Spells;
using HYPERMAGE.UI.UIElements;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace HYPERMAGE.Models;
public class Player
{
    private static Texture2D texture;
    private readonly Animation anim;

    public Vector2 center;

    private Vector2 nextPosition;
    public Vector2 position;
    public Vector2 velocity;

    public Polygon hitbox;

    public int width;
    public int height;
    public Vector2 origin;

    public float speed = 1.7f;
    public float acceleration = 120f;

    public float immunityTime = 1f;
    public float immunityTimer;
    public bool immune;

    public bool flashing;
    public float flashingTimer;
    public Color flashColor;
    public Color flashColor1;
    public Color flashColor2;

    public int mana = 554;
    public int maxHealth = 3;
    public int health = 3;
    public int lives = 3;

    public int xp = 0;
    public int xpToLevel = 10;
    public int level = 1;

    private float dashTimer;
    private float oldSpeed;
    public float dashLength = 0.15f;
    public Player(Vector2 pos)
    {
        texture ??= Globals.Content.Load<Texture2D>("player");
        anim = new(texture, 5, 1, 0.1f);
        position = pos;

        width = anim.frameWidth;
        height = anim.frameHeight;

        origin = new Vector2(width / 2f, height / 2f);

        center = position + origin;

        hitbox = PolygonFactory.CreateRectangle((int)center.X - 1, (int)center.Y - 1, 1, 1);
    }

    public void Update()
    {
        if (InputManager.Moving)
        {
            velocity += Vector2.Normalize(InputManager.Direction) * acceleration * Globals.TotalSeconds;
        }

        if (InputManager.dashing == true)
        {
            if (dashTimer <= 0)
            {
                SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("shoot"), 1, 1, 0);

                oldSpeed = speed;

                speed *= 5;

                if (InputManager.Direction == Vector2.Zero)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        Random r = new();
                        float n = Globals.RandomFloat(-0.2f, 0.2f);
                        int m = r.Next(400, 500);
                        float b = Globals.RandomFloat(-0.1f, 0.1f);

                        ParticleData dashParticleData = new()
                        {
                            sizeStart = 5,
                            sizeEnd = 0,
                            colorStart = Color.White,
                            colorEnd = Color.White,
                            velocity = Vector2.Normalize(-InputManager.lastDirection).RotatedBy(n) * m,
                            lifespan = 0.5f,
                            rotationSpeed = b
                        };

                        Particle dashParticle = new(center + InputManager.lastDirection * 20, dashParticleData);
                        ParticleManager.AddParticle(dashParticle);
                    }
                }

                else
                {
                    for (int i = 0; i < 5; i++)
                    {
                        Random r = new();
                        float n = Globals.RandomFloat(-0.2f, 0.2f);
                        int m = r.Next(400, 500);
                        float b = Globals.RandomFloat(-0.1f, 0.1f); ;

                        ParticleData dashParticleData = new()
                        {
                            sizeStart = 5,
                            sizeEnd = 0,
                            colorStart = Color.White,
                            colorEnd = Color.White,
                            velocity = Vector2.Normalize(-InputManager.Direction).RotatedBy(n) * m,
                            lifespan = 0.5f,
                            rotationSpeed = b
                        };

                        Particle dashParticle = new(center + InputManager.Direction * 20, dashParticleData);
                        ParticleManager.AddParticle(dashParticle);
                    }
                }
            }

            dashTimer += Globals.TotalSeconds;

            if (dashTimer <= dashLength)
            {
                if (InputManager.Direction == Vector2.Zero)
                {
                    velocity += Vector2.Normalize(InputManager.lastDirection) * acceleration * 200 * Globals.TotalSeconds;
                }

                else
                {
                    velocity += Vector2.Normalize(InputManager.Direction) * acceleration * 200 * Globals.TotalSeconds;
                }
            }

            if (dashTimer >= dashLength)
            {
                SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("hit"), 1, 1, 0);

                for (int i = 0; i < 8; i++)
                {
                    ParticleData spawnParticleData = new()
                    {
                        opacityStart = 1f,
                        opacityEnd = 1f,
                        sizeStart = 6,
                        sizeEnd = 0,
                        colorStart = Color.White,
                        colorEnd = Color.White,
                        velocity = new(Globals.RandomFloat(-150, 150), Globals.RandomFloat(-150, 150)),
                        lifespan = 0.2f,
                        rotationSpeed = Globals.RandomFloat(-0.5f, 0.5f)
                    };

                    Particle spawnParticle = new(center, spawnParticleData);
                    ParticleManager.AddParticle(spawnParticle);
                }

                speed = oldSpeed;
                dashTimer = 0;
                InputManager.dashing = false;
            }
        }

        velocity.X = Math.Clamp(velocity.X, -speed, speed);
        velocity.Y = Math.Clamp(velocity.Y, -speed, speed);

        velocity *= Globals.TotalSeconds;

        //

        if (position.X < GameManager.bounds.X)
        {
            velocity.X += Globals.TotalSeconds * Math.Abs(position.X - GameManager.bounds.X);
        }

        if (position.X > GameManager.bounds.Z - width)
        {
            velocity.X -= Globals.TotalSeconds * Math.Abs(position.X - (GameManager.bounds.Z - width));
        }

        if (position.Y < GameManager.bounds.Y)
        {
            velocity.Y += Globals.TotalSeconds * Math.Abs(position.Y - GameManager.bounds.Y);
        }

        if (position.Y > GameManager.bounds.W - height)
        {
            velocity.Y -= Globals.TotalSeconds * Math.Abs(position.Y - (GameManager.bounds.W - height));
        }

        position += velocity * Globals.TotalSeconds * (InputManager.slow ? 1000 : 2000);

        // 

        if (InputManager.LeftMouseDown && !InputManager.buttonClicked)
        {
            Spellbook.CastFrontSpellPrimary();
        }

        if (InputManager.RightMouseDown && !InputManager.buttonClicked)
        {
            Spellbook.CastFrontSpellSecondary();
        }

        //

        if (!immune)
        {
            foreach (Mob mob in MobManager.mobs)
            {
                if (mob.hitbox.IntersectsWith(hitbox) && mob.contactDamage && !immune && !mob.spawning)
                {
                    Damage(1);
                } 
            }

            foreach (Projectile projectile in ProjectileManager.projectiles)
            {
                if (projectile.hitbox.IntersectsWith(hitbox) && !projectile.friendly && !immune)
                {
                    Damage((int)projectile.damage);
                    projectile.HitPlayer();
                }
            }
        }

        if (immune)
        {
            immunityTimer += Globals.TotalSeconds;
        }

        if (immunityTimer > immunityTime)
        {
            immune = false;
            immunityTimer = 0;

            flashing = false;
        }

        //

        if (flashing)
        {
            flashingTimer += Globals.TotalSeconds;

            if (flashingTimer > 0.1f)
            {
                flashColor = flashColor1;
            }

            if (flashingTimer > 0.2f)
            {
                flashingTimer = 0;
                flashColor = flashColor2;
            }
        }


        center = position + origin;

        hitbox = PolygonFactory.CreateRectangle((int)center.X, (int)center.Y, 1, 1);

        //

        anim.Update();
    }

    public void Damage(int damage)
    {
        health -= damage;

        GameManager.AddScreenShake(0.2f, 8f);
        GameManager.AddAbberationPowerForce(500, 50);

        GameManager.damageStatic = true;

        SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("death"), 1, 0, 0);

        if (health <= 0)
        {
            GameManager.PlayerDeath();

            for (int i = 0; i < 20; i++)
            {
                ParticleData ParticleData = new()
                {
                    opacityStart = 1f,
                    opacityEnd = 0f,
                    sizeStart = 3,
                    sizeEnd = 1,
                    colorStart = Color.White,
                    colorEnd = Color.White,
                    velocity = new(Globals.RandomFloat(-300, 300), Globals.RandomFloat(-300, 300)),
                    lifespan = 0.2f,
                    rotationSpeed = 1f,
                    resistance = 1.2f
                };

                Particle particle = new(center, ParticleData);
                ParticleManager.AddParticle(particle);
            }
        }

        immune = true;
        flashing = true;

        flashColor1 = Color.Red;
        flashColor2 = Color.White;
    }
    public void Draw()
    {
        anim.Draw(new((int)position.X, (int)position.Y), flashing ? flashColor : Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.8f);
    }

    public void AddXP(int xp)
    {
        if (level < 10)
        {
            this.xp += xp;

            if (this.xp >= xpToLevel)
            {
                LevelUp();
            }
        }
    }
    public void LevelUp()
    {
        SpellbookUI.Level();

        switch (level)
        {
            case 1:
                {
                    Spellbook.spellCountPrimary++;
                    Spellbook.spellCountSecondary++;

                    xp -= xpToLevel;
                    xpToLevel = 20;
                    level++;
                    return;
                }
            case 2:
                {
                    Spellbook.spellCountPrimary++;
                    Spellbook.spellCountSecondary++;

                    xp -= xpToLevel;
                    xpToLevel = 30;
                    level++;
                    return;
                }
            case 3:
                {
                    Spellbook.spellCountPrimary++;
                    Spellbook.spellCountSecondary++;

                    xp -= xpToLevel;
                    xpToLevel = 40;
                    level++;
                    return;
                }
            case 4:
                {
                    Spellbook.spellCountPrimary++;
                    Spellbook.spellCountSecondary++;

                    xp -= xpToLevel;
                    xpToLevel = 50;
                    level++;
                    return;
                }
            case 5:
                {
                    Spellbook.spellCountPrimary++;
                    Spellbook.spellCountSecondary++;

                    xp -= xpToLevel;
                    xpToLevel = 60;
                    level++;
                    return;
                }
            case 6:
                {
                    Spellbook.spellCountPrimary++;
                    Spellbook.spellCountSecondary++;

                    xp -= xpToLevel;
                    xpToLevel = 70;
                    level++;
                    return;
                }
            case 7:
                {
                    Spellbook.spellCountPrimary++;
                    Spellbook.spellCountSecondary++;

                    xp -= xpToLevel;
                    xpToLevel = 80;
                    level++;
                    return;
                }
            case 8:
                {
                    Spellbook.spellCountPrimary++;
                    Spellbook.spellCountSecondary++;

                    xp -= xpToLevel;
                    xpToLevel = 90;
                    level++;
                    return;
                }
            case 9:
                {
                    Spellbook.spellCountPrimary++;
                    Spellbook.spellCountSecondary++;

                    xp -= xpToLevel;
                    xpToLevel = 100;
                    level++;
                    return;
                }
            case 10:
                {
                    Spellbook.spellCountPrimary++;
                    Spellbook.spellCountSecondary++;

                    xp -= xpToLevel;
                    xpToLevel = 100;
                    level++;
                    return;
                }
        }
    }
}