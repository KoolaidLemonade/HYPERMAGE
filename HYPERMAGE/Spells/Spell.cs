using HYPERMAGE.Helpers;
using HYPERMAGE.Managers;
using HYPERMAGE.Models;
using HYPERMAGE.Particles;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HYPERMAGE.Spells
{
    public class Spell
    {
        public static int totalSpellTypes = 8;

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
                case 5: //disintegrate
                    cost = 5;

                    spellTraits.Add(1);
                    spellTraits.Add(20);
                    spellTraits.Add(25);

                    icon = Globals.Content.Load<Texture2D>("disintegrateicon");
                    description = "CALL UPON THE SUN TO SUMMON A PILLAR OF PURE ELEMENTAL FIRE";
                    name = "DISINTEGRATE";
                    return;
                case 6: //sparks
                    cost = 1;

                    spellTraits.Add(1);
                    spellTraits.Add(22);

                    icon = Globals.Content.Load<Texture2D>("sparksicon");
                    description = "SUMMONS A FORMATION OF DWINDLING SPARKS";
                    name = "SPARKS";
                    return;
                case 7: // haste
                    cost = 3;

                    spellTraits.Add(1);
                    spellTraits.Add(17);

                    icon = Globals.Content.Load<Texture2D>("hasteicon");
                    description = "IMBUES YOUR NEXT SPELL WITH HASTE";
                    name = "HASTE";
                    return;
                case 8: // explosion
                    cost = 4;

                    spellTraits.Add(1);
                    spellTraits.Add(23);
                    spellTraits.Add(24);

                    icon = Globals.Content.Load<Texture2D>("explosionicon");
                    description = "CONJURES A MASSIVE FIERY EXPLOSION";
                    name = "EXPLOSION";
                    return;

            }
        }
        public void Cast(Player player)
        {
            float castDamage = damage;

            if (UpgradeManager.usedUpgrades.Contains(1) && spellTraits.Contains(1))
            {
                castDamage *= 1.2f;
            }

            if (UpgradeManager.usedUpgrades.Contains(5))
            {
                castDamage *= 1.1f;
            }

            if (UpgradeManager.usedUpgrades.Contains(7) && spellTraits.Contains(14))
            {
                castDamage *= 1.2f;
            }

            if (UpgradeManager.usedUpgrades.Contains(8) && spellTraits.Contains(15))
            {
                castDamage *= 1.2f;
            }

            if (UpgradeManager.usedUpgrades.Contains(9) && spellTraits.Contains(25))
            {
                castDamage *= 1.2f;
            }

            if (UpgradeManager.usedUpgrades.Contains(6) && spellTraits.Contains(21))
            {
                castDamage *= 1.2f;
            }

            switch (spellType)
            {
                case 0:
                    return;
                case 1: // firebolt
                    Projectile firebolt = new(player.center, 1, speed, castDamage, 0, knockback, Vector2.Normalize(InputManager.MousePosition - player.center) * 200, lifespan, size);
                    ProjectileManager.AddProjectile(firebolt);

                    SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("shoot"), 1, 0.6f, 0);
                    return;
                case 2: // fireball
                    Projectile fireball = new(player.center, 2, speed, castDamage, 0, knockback, Vector2.Normalize(InputManager.MousePosition - player.center) * 200, lifespan, size);
                    ProjectileManager.AddProjectile(fireball);

                    SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("heavyhit"), 0.3f, -0.8f, 0);
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

                    Projectile kindle = new(InputManager.MousePosition, 3, speed, castDamage, -1, knockback, lifespan, 5, size);
                    ProjectileManager.AddProjectile(kindle);

                    SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("shoot"), 1, 0, 0);
                    return;

                case 4: // bladeofflame
                    Projectile bladeofflame = new(player.center, 4, speed, castDamage, -1, knockback, Vector2.Zero, lifespan, 20, 0f, size, true, 0, 25 + (5 * rank));
                    ProjectileManager.AddProjectile(bladeofflame);

                    SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("shoot"), 1, 0, 0);
                    return;

                case 5: // disintegrate
                    Projectile disintegrate = new(new Vector2(InputManager.MousePosition.X, -10), 5, speed, castDamage, -1, knockback, lifespan, 10, size);
                    ProjectileManager.AddProjectile(disintegrate);

                    SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("smallexplosion"), 0.6f, 0.4f, 0);
                    GameManager.AddScreenShake(0.1f, 3f);

                    return;
                case 6: //sparks

                    for (int i = 0; i < rank + 3; i++)
                    {
                        Projectile spark = new(player.center, 6, speed * Globals.RandomFloat(0.75f, 1.25f), castDamage, 0, knockback, Vector2.Normalize(InputManager.MousePosition - player.center).RotatedBy(MathHelper.ToRadians(Globals.RandomFloat(-20f, 20f))) * 200, lifespan * Globals.RandomFloat(0.75f, 1.25f), size);
                        ProjectileManager.AddProjectile(spark);
                    }

                    SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("shoot"), 1, 0.6f, 0);
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
                            cooldown = 0.25f;
                            damage = 3f;
                            knockback = 1f;
                            size = 2f;
                            break;
                        case 2:
                            cooldown = 0.2f;
                            damage = 5f;
                            speed = 1.5f;
                            knockback = 1.25f;
                            size = 3f;
                            break;
                        case 3:
                            cooldown = 0.15f;
                            damage = 10f;
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
                            cooldown = 0.5f;
                            damage = 8f;
                            speed = 0.75f;
                            size = 6f;
                            knockback = 2f;
                            break;
                        case 2:
                            cooldown = 0.35f;
                            damage = 12f;
                            speed = 0.65f;
                            size = 8f;
                            knockback = 3f;
                            break;
                        case 3:
                            cooldown = 0.15f;
                            damage = 25f;
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
                            cooldown = 0.5f;
                            damage = 0.1f;
                            speed = 0.75f;
                            lifespan = 4f;
                            break;
                        case 2:
                            cooldown = 0.25f;
                            damage = 0.2f;
                            speed = 1f;
                            size = 2f;
                            lifespan = 6f;
                            break;
                        case 3:
                            cooldown = 0.2f;
                            damage = 0.5f;
                            speed = 1.5f;
                            size = 4f;
                            lifespan = 8f;
                            break;
                    }
                    break;
                case 4: // blade of flame
                    switch (rank)
                    {
                        case 1:
                            cooldown = 0.4f;
                            damage = 5f;
                            lifespan = 0.75f;
                            knockback = 2f;
                            break;
                        case 2:
                            cooldown = 0.3f;
                            damage = 8f;
                            size = 1.25f;
                            lifespan = 0.55f;
                            knockback = 2.5f;
                            break;
                        case 3:
                            cooldown = 0.2f;
                            damage = 15f;
                            size = 1.5f;
                            lifespan = 0.4f;
                            knockback = 3f;
                            break;
                    }
                    break;
                case 5: // disintegrate
                    switch (rank)
                    {
                        case 1:
                            cooldown = 0.5f;
                            damage = 20f;
                            lifespan = 0.5f;
                            break;
                        case 2:
                            cooldown = 0.4f;
                            damage = 30f;
                            size = 1.5f;
                            lifespan = 0.25f;
                            break;
                        case 3:
                            cooldown = 0.1f;
                            damage = 60f;
                            size = 3f;
                            lifespan = 0.1f;
                            break;
                    }
                    break;
                case 6: // sparks
                    switch (rank)
                    {
                        case 1:
                            size = 2.5f;
                            cooldown = 0.5f;
                            damage = 1f;
                            lifespan = 1f;
                            break;
                        case 2:
                            size = 3.5f;
                            cooldown = 0.4f;
                            damage = 2f;
                            size = 3.5f;
                            lifespan = 1.5f;
                            speed = 1.5f;
                            break;
                        case 3:
                            size = 4.5f;
                            cooldown = 0.1f;
                            damage = 3f;
                            size = 4f;
                            lifespan = 2.5f;
                            speed = 2f;
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
