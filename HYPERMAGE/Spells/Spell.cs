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
        public static int totalSpellTypes = 5;

        public float cooldown;

        public List<int> spellTraits = [];
        public List<int> boons = [];

        public int position;
        public int index;

        public int spellType;
        public int rank;
        public int cost;

        public float damage;
        public float speed = 1f;
        public float size = 1f;
        public float lifespan = 10f;
        public float knockback = 0f;

        public string description;
        public string name;

        public Texture2D icon;

        public Spell(int spellType, int rank) : this(spellType, rank, [])
        {

        }
        public Spell(int spellType, int rank, List<int> boons)
        {
            this.spellType = spellType;
            this.rank = rank;
            this.boons = boons;

            UpdateRankStats();

            switch (spellType)
            {
                case 0:
                    return;
                case 1: //firebolt
                    cost = 2;

                    spellTraits.Add(1);
                    spellTraits.Add(14);

                    icon = Globals.Content.Load<Texture2D>("firebolticon");
                    description = "LAUNCHES A BOLT OF FIERY MAGIC";
                    name = "FIREBOLT";
                    return;
                case 2: //fireball
                    cost = 4;

                    spellTraits.Add(1);
                    spellTraits.Add(15);

                    icon = Globals.Content.Load<Texture2D>("fireballicon");
                    description = "WILLS FORTH A DESTRUCTIVE FORCE OF EXPLOSIVE ARCANE FIRE";
                    name = "FIREBALL";
                    return;
                case 3: //kindle
                    cost = 1;

                    spellTraits.Add(1);
                    spellTraits.Add(25);

                    icon = Globals.Content.Load<Texture2D>("kindleicon");
                    description = "SUMMONS A SMALL KINDLING OF FLAME";
                    name = "KINDLE";
                    return;
                case 4: //blade of flame
                    cost = 3;

                    spellTraits.Add(1);
                    spellTraits.Add(21);

                    icon = Globals.Content.Load<Texture2D>("bladeofflameicon");
                    description = "SUMMONS A LARGE BLADE OF ARCANE FIRE";
                    name = "BLADE OF FLAME";
                    return;
                case 5: //disintigrate
                    cost = 5;

                    spellTraits.Add(1);
                    spellTraits.Add(20);
                    spellTraits.Add(25);

                    icon = Globals.Content.Load<Texture2D>("disintegrateicon");
                    description = "CALL UPON THE SUN TO SUMMON A PILLAR OF PURE ELEMENTAL FIRE";
                    name = "DISINTEGRATE";
                    return;
            }
        }
        public void Cast(float spread, Player player)
        {
            switch (spellType)
            {
                case 0:
                    return;
                case 1: // firebolt
                    Projectile firebolt = new(player.center, 1, speed, damage, 0, knockback, Vector2.Normalize(InputManager.MousePosition - player.center).RotatedBy(MathHelper.ToRadians(Globals.RandomFloat(-spread, spread))) * 200, lifespan, size);
                    ProjectileManager.AddProjectile(firebolt);
                    return;
                case 2: // fireball
                    Projectile fireball = new(player.center, 2, speed, damage, 0, knockback, Vector2.Normalize(InputManager.MousePosition - player.center).RotatedBy(MathHelper.ToRadians(Globals.RandomFloat(-spread, spread))) * 200, lifespan, size);
                    ProjectileManager.AddProjectile(fireball);
                    return;
                case 3: // kindle
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

                    Projectile kindle = new(InputManager.MousePosition, 3, speed, damage, -1, knockback, lifespan, 1, size);
                    ProjectileManager.AddProjectile(kindle);
                    return;

                case 4: // bladeofflame
                    Projectile bladeofflame = new(player.center, 4, speed, damage, -1, knockback, size);
                    ProjectileManager.AddProjectile(bladeofflame);
                    return;

                case 5: // disintegrate
                    Projectile disintegrate = new(new Vector2(InputManager.MousePosition.X, -10), 5, speed, damage, -1, knockback, lifespan, 10, size);
                    ProjectileManager.AddProjectile(disintegrate);
                    return;

            }
        }

        public void UpdateRankStats()
        {
            switch (spellType)
            {
                case 1: // firebolt
                    switch (rank)
                    {
                        case 1:
                            cooldown = 0.9f;
                            damage = 1f;
                            knockback = 1f;
                            size = 2f;
                            break;
                        case 2:
                            cooldown = 0.65f;
                            damage = 2f;
                            speed = 1.5f;
                            knockback = 1.25f;
                            size = 3f;
                            break;
                        case 3:
                            cooldown = 0.4f;
                            damage = 4f;
                            speed = 2.5f;
                            knockback = 1.5f;
                            size = 4f;
                            break;
                    }
                    break;
                case 2: // fireball
                    switch (rank)
                    {
                        case 1:
                            cooldown = 1.25f;
                            damage = 5f;
                            speed = 0.75f;
                            size = 6f;
                            knockback = 2f;
                            break;
                        case 2:
                            cooldown = 1f;
                            damage = 8f;
                            speed = 0.65f;
                            size = 8f;
                            knockback = 3f;
                            break;
                        case 3:
                            cooldown = 0.75f;
                            damage = 15f;
                            speed = 0.4f;
                            size = 10f;
                            knockback = 4f;
                            break;
                    }
                    break;
                case 3: // kindle
                    switch (rank)
                    {
                        case 1:
                            cooldown = 1.25f;
                            damage = 0.25f;
                            speed = 0.75f;
                            lifespan = 2f;
                            break;
                        case 2:
                            cooldown = 1f;
                            damage = 0.5f;
                            speed = 1f;
                            size = 1.5f;
                            lifespan = 3f;
                            break;
                        case 3:
                            cooldown = 0.75f;
                            damage = 0.75f;
                            speed = 1.5f;
                            size = 2f;
                            lifespan = 5f;
                            break;
                    }
                    break;
                case 4: // blade of flame
                    switch (rank)
                    {
                        case 1:
                            cooldown = 0.75f;
                            damage = 5f;
                            speed = 0.75f;
                            lifespan = 1f;
                            knockback = 1f;
                            break;
                        case 2:
                            cooldown = 0.45f;
                            damage = 8f;
                            speed = 1f;
                            size = 1.25f;
                            lifespan = 0.75f;
                            knockback = 1.25f;
                            break;
                        case 3:
                            cooldown = 0.2f;
                            damage = 15f;
                            speed = 1.5f;
                            size = 1.5f;
                            lifespan = 0.5f;
                            knockback = 1.5f;
                            break;
                    }
                    break;
                case 5: // disintegrate
                    switch (rank)
                    {
                        case 1:
                            cooldown = 1.5f;
                            damage = 20f;
                            lifespan = 0.5f;
                            break;
                        case 2:
                            cooldown = 0.8f;
                            damage = 30f;
                            size = 1.5f;
                            lifespan = 0.25f;
                            break;
                        case 3:
                            cooldown = 0.2f;
                            damage = 60f;
                            size = 3f;
                            lifespan = 0.1f;
                            break;
                    }
                    break;

            }
        }
        public void RankUp()
        {
            rank++;
            UpdateRankStats();
        }
    }
}
