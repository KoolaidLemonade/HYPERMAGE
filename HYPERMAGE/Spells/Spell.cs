using HYPERMAGE.Helpers;
using HYPERMAGE.Managers;
using HYPERMAGE.Models;
using HYPERMAGE.Particles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HYPERMAGE.Spells
{
    // i love megaclassing
    public class Spell
    {
        public int _cooldown;

        public int _spellTrait;
        public int _spellTrait2;
        public int _spellTrait3;

        public int _spellType;
        public int _rank;
        public int _count;
        public int _cost;

        public bool secondary = true;

        //add chaining
        public Spell(int spellType, int spellTrait, int spellTrait2, int spellTrait3, int rank, int count, int cost, int cooldown)
        {
            _spellType = spellType;
            _cooldown = cooldown;
            _spellTrait = spellTrait;
            _spellTrait2 = spellTrait2;
            _spellTrait3 = spellTrait3;
            _rank = rank;
            _count = count;
            _cost = cost;
            _cooldown = cooldown;
        }
        public Spell(int spellType, int spellTrait, int spellTrait2, int rank, int count, int cost, int cooldown)
        {
            _spellType = spellType;
            _cooldown = cooldown;
            _spellTrait = spellTrait;
            _spellTrait2 = spellTrait2;
            _rank = rank;
            _count = count;
            _cost = cost;
            _cooldown = cooldown;
        }
        public void Cast(Vector2 center, float spread, Player player)
        {
            switch (_spellType)
            {
                case 0:
                    return;
                case 1: //firebolt
                    Projectile firebolt = new(Globals.Content.Load<Texture2D>("particle"), center, 1, 1.5f, 1f, Vector2.Normalize(InputManager.MousePosition - center).RotatedBy(MathHelper.ToRadians(Globals.RandomFloat(-spread, spread))) * 300, 600, 0f, 2f, true, 0, 0);
                    ProjectileManager.AddProjectile(firebolt);
                    return;
                case 2: //fireball
                    Projectile fireball = new(Globals.Content.Load<Texture2D>("particle"), center, 2, 0.5f, 3f, Vector2.Normalize(InputManager.MousePosition - center).RotatedBy(MathHelper.ToRadians(Globals.RandomFloat(-spread, spread))) * 300, 600, 0f, 5f, true, 0, 0);
                    ProjectileManager.AddProjectile(fireball);
                    return;
                case 3: //kindle
                    for (int i = 0; i < 5; i++)
                    {
                        ParticleData kindleParticleData = new()
                        {
                            opacityStart = .8f,
                            opacityEnd = 0.1f,
                            sizeStart = 5,
                            sizeEnd = 0,
                            colorStart = Color.White,
                            colorEnd = Color.Gold,
                            velocity = new(Globals.RandomFloat(-200, 200), Globals.RandomFloat(-200, 200)),
                            lifespan = 0.5f,
                            rotationSpeed = Globals.RandomFloat(-0.5f, 0.5f)
                        };

                        Particle kindleParticle = new(InputManager.MousePosition, kindleParticleData);
                        ParticleManager.AddParticle(kindleParticle);
                    }

                    Projectile kindle = new(Globals.Content.Load<Texture2D>("particle"), InputManager.MousePosition, 3, 1f, 3f, Vector2.Zero, 90, 0f, 1f, true, 0, 0);
                    ProjectileManager.AddProjectile(kindle);
                    return;

                case 4: //bladeofflame

                    if (secondary)
                    {
                        Projectile bladeofflame = new(Globals.Content.Load<Texture2D>("bladeofflame"), player.center, 4, 1f, 7f, Vector2.Zero, 15, 0f, 1f, true, 0, 1);
                        ProjectileManager.AddProjectile(bladeofflame);
                    }

                    else
                    {
                        Projectile bladeofflame = new(Globals.Content.Load<Texture2D>("bladeofflame"), player.center, 4, 1f, 7f, Vector2.Zero, 15, 0f, 1f, true, 0, 0);
                        ProjectileManager.AddProjectile(bladeofflame);
                    }
                    return;
            }
        }
    }
}
