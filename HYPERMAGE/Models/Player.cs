using HYPERMAGE.Helpers;
using HYPERMAGE.Managers;
using HYPERMAGE.Particles;
using HYPERMAGE.Spells;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace HYPERMAGE.Models;
public class Player
{
    private static Texture2D _texture;
    private readonly Animation _anim;

    public Vector2 center;
    public Vector2 _position; 
    public Vector2 _velocity;

    private float _speed = 20f;
    public int mana = 50;
    public int health = 3;
    public int lives = 3;

    public int primarySpellCounter = 0;
    public int primarySpellCooldown = 0;

    public int secondarySpellCooldown = 0;

    public bool explosify = false;
    public Player(Vector2 pos)
    {
        _texture ??= Globals.Content.Load<Texture2D>("player");
        _anim = new(_texture, 5, 1, 0.1f);
        _position = pos;
    }

    public void Update()
    {
        center = new(_position.X + 5.5f, _position.Y + 7f); 

        if (InputManager.Moving)
        {
            _velocity += (Vector2.Normalize(InputManager.Direction) * _speed) * Globals.TotalSeconds;
        }

        secondarySpellCooldown--;

        if (InputManager.RightClicked && secondarySpellCooldown <= 0)
        {
            secondarySpellCooldown = 300;

            for (int i = 0; i < Spellbook.spellsSecondary.Count; i++)
            {
                Spellbook.spellsSecondary[i].Cast(center, i + 5, this);
            }
        }

        primarySpellCooldown--;

        if (InputManager.LeftMouseDown && primarySpellCooldown <= 0)
        {
            primarySpellCooldown = Spellbook.spellsPrimary[primarySpellCounter]._cooldown;

            Spellbook.spellsPrimary[primarySpellCounter].Cast(center, 0f, this);

            primarySpellCounter++;

            if (primarySpellCounter >= Spellbook.spellsPrimary.Count)
            {
                primarySpellCounter = 0;
            }
        }

        if (InputManager._dashing == true)
        {
            if (InputManager.Direction == Vector2.Zero)
            {
                _velocity += (Vector2.Normalize(InputManager._lastDirection) * _speed * 30) * Globals.TotalSeconds;
                InputManager._dashing = false;

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
                        velocity = Vector2.Normalize(InputManager._lastDirection).RotatedBy(n) * m,
                        lifespan = 0.5f,
                        rotationSpeed = b
                    };

                    Particle dashParticle = new(new(_position.X + _texture.Width / 6 / 2, _position.Y + _texture.Height / 2), dashParticleData);
                    ParticleManager.AddParticle(dashParticle);

                }
            }
            else
            {
                _velocity += (Vector2.Normalize(InputManager.Direction) * _speed * 30) * Globals.TotalSeconds;
                InputManager._dashing = false;

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

                    Particle dashParticle = new(new(_position.X + _texture.Width / 6 / 2, _position.Y + _texture.Height / 2), dashParticleData);
                    ParticleManager.AddParticle(dashParticle);
                }
            }

        }

        _position += _velocity;

        _velocity.X /= (float)1.30;

        _velocity.Y /= (float)1.30;

        if (_position.X > 309)
        {
            _position.X = 309;
        }

        if (_position.X < 0)
        {
            _position.X = 0;
        }

        if (_position.Y > 166)
        {
            _position.Y = 166;
        }

        if (_position.Y < 33)
        {
            _position.Y = 33;
        }

        _anim.Update();
    }

    public void Draw()
    {
        _anim.Draw(_position);
    }
}