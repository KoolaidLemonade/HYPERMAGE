using HYPERMAGE.Helpers;
using HYPERMAGE.Managers;
using HYPERMAGE.Particles;
using HYPERMAGE.Spells;
using HYPERMAGE.UI.UIElements;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace HYPERMAGE.Models;
public class Player
{
    private static Texture2D texture;
    private readonly Animation anim;

    public Vector2 center;

    private Vector2 prevPosition;
    public Vector2 position;
    public Vector2 velocity;

    public Polygon hitbox;

    public float speed = 1.7f;
    public float acceleration = 150f;

    public int mana = 555;
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

        hitbox = PolygonFactory.CreateRectangle((int)(position.X + anim.frameWidth / 2), (int)(position.Y + anim.frameHeight / 2), 3, 3);
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
                            velocity = Vector2.Normalize(InputManager.lastDirection).RotatedBy(n) * m,
                            lifespan = 0.5f,
                            rotationSpeed = b
                        };

                        Particle dashParticle = new(new(position.X + texture.Width / 6 / 2, position.Y + texture.Height / 2), dashParticleData);
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
                            velocity = Vector2.Normalize(InputManager.Direction).RotatedBy(n) * m,
                            lifespan = 0.5f,
                            rotationSpeed = b
                        };

                        Particle dashParticle = new(new(position.X + texture.Width / 6 / 2, position.Y + texture.Height / 2), dashParticleData);
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
                speed = oldSpeed;
                dashTimer = 0;
                InputManager.dashing = false;
            }
        }

        velocity.X = Math.Clamp(velocity.X, -speed, speed);
        velocity.Y = Math.Clamp(velocity.Y, -speed, speed);

        velocity /= 1.5f;

        prevPosition = position;

        position += velocity;

        //

        if (position.X < GameManager.bounds.X || position.X > GameManager.bounds.Z - anim.frameWidth)
        {
            position.X = prevPosition.X;
            velocity.X = 0f;
        }

        if (position.Y < GameManager.bounds.Y || position.Y > GameManager.bounds.W - anim.frameHeight)
        {
            position.Y = prevPosition.Y;
            velocity.Y = 0f;
        }

        center = new(position.X + anim.frameWidth / 2, position.Y + anim.frameHeight / 2);

        hitbox.SetPosition(center);

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

        anim.Update();
    }

    public void Draw()
    {
        Globals.SpriteBatch.Draw(Globals.GetBlankTexture(), new Rectangle((int)hitbox.GetPosition().X - 1, (int)hitbox.GetPosition().Y - 1, 3, 3), null, Color.Black * 0.5f, 0f, Vector2.Zero, SpriteEffects.None, 0.9999f);

        Globals.SpriteBatch.Draw(Globals.GetBlankTexture(), new Rectangle((int)hitbox.GetPosition().X, (int)hitbox.GetPosition().Y, 1, 1), null, Color.Red, 0f, Vector2.Zero, SpriteEffects.None, 1f);

        anim.Draw(new((int)position.X, (int)position.Y));
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