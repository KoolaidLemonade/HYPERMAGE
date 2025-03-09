using HYPERMAGE.Helpers;
using HYPERMAGE.Managers;
using HYPERMAGE.Models;
using HYPERMAGE.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace HYPERMAGE.Spells
{
    public class Spell
    {
        public static int totalSpellTypes = 40;

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

        public static bool explosify;
        public static float explosifyDamage;

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
                    description = "IMBUES YOUR NEXT SPELL WITH EXPLOSIVE POWER";
                    name = "EXPLOSIFY";
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

                case 9: //snowball
                    cost = 1;

                    spellTraits.Add(2);
                    spellTraits.Add(18);

                    icon = Globals.Content.Load<Texture2D>("snowballicon");
                    description = "CONJURES A SMALL SNOW BALL";
                    name = "SNOWBALL";

                    return;
                case 10: //ice bolt
                    cost = 2;

                    spellTraits.Add(2);
                    spellTraits.Add(14);

                    icon = Globals.Content.Load<Texture2D>("icebolticon");
                    description = "LAUNCHES A BOLT OF ICY MAGICK";
                    name = "ICE BOLT";

                    return;
                case 11: //frost wave
                    cost = 2;

                    spellTraits.Add(2);
                    spellTraits.Add(24);

                    icon = Globals.Content.Load<Texture2D>("frostwaveicon");
                    description = "CONJURES A SLOW WAVE OF FROST";
                    name = "FROST WAVE";

                    return;
                case 12: //ice beam
                    cost = 3;

                    spellTraits.Add(2);
                    spellTraits.Add(20);

                    icon = Globals.Content.Load<Texture2D>("icebeamicon");
                    description = "SHOOTS A LASER OF ARCANE ICE";
                    name = "ICE BEAM";

                    return;
                case 13: //frost blast
                    cost = 3;

                    spellTraits.Add(2);
                    spellTraits.Add(23);

                    icon = Globals.Content.Load<Texture2D>("frostblasticon");
                    description = "CONJURES A FORWARD EXPLOSIVE BLAST OF FROST MAGICK";
                    name = "FROST BLAST";

                    return;
                case 14: //wall of ice
                    cost = 2;

                    spellTraits.Add(2);
                    spellTraits.Add(19);

                    icon = Globals.Content.Load<Texture2D>("walloficeicon");
                    description = "SUMMONS A SMALL BLOCKADE OF ICICLES THAT CAN BLOCK PROJECTILES";
                    name = "WALL OF ICE";

                    return;
                case 15: //ice spear
                    cost = 4;

                    spellTraits.Add(2);
                    spellTraits.Add(21);

                    icon = Globals.Content.Load<Texture2D>("icespearicon");
                    description = "STAB WITH A SPEAR OF TRUE ICE";
                    name = "ICE SPEAR";

                    return;
                case 16: //entomb
                    cost = 5;

                    spellTraits.Add(2);
                    spellTraits.Add(16);
                    spellTraits.Add(19);

                    icon = Globals.Content.Load<Texture2D>("entombicon");
                    description = "EMPOWERS YOUR NEXT SPELL TO FREEZE AN ENEMY IN A PILLAR OF ICE";
                    name = "ENTOMB";

                    return;
                case 17: //zap
                    cost = 1;

                    spellTraits.Add(4);
                    spellTraits.Add(20);
                    spellTraits.Add(25);


                    icon = Globals.Content.Load<Texture2D>("zapicon");
                    description = "CASTS A SMALL BEAM OF ELECTRIC ENERGY";
                    name = "ZAP";

                    return;
                case 18: //ball lightning
                    cost = 3;

                    spellTraits.Add(4);
                    spellTraits.Add(18);

                    icon = Globals.Content.Load<Texture2D>("balllightningicon");
                    description = "FIRE A BALL OF LIGHTNING THAT ZAPS NEARBY ENEMIES";
                    name = "BALL LIGHTNING";

                    return;
                case 19: //electrify
                    cost = 4;

                    spellTraits.Add(4);
                    spellTraits.Add(17);

                    icon = Globals.Content.Load<Texture2D>("electrifyicon");
                    description = "IMBUES YOUR NEXT SPELL WITH ELECTRICITY";
                    name = "ELECTRIFY";

                    return;
                case 20: //thunderspark
                    cost = 3;

                    spellTraits.Add(4);
                    spellTraits.Add(22);

                    icon = Globals.Content.Load<Texture2D>("thundersparkicon");
                    description = "CASTS A FORMATION OF ELECTRIC SPARKS";
                    name = "THUNDERSPARK";

                    return;
                case 21: //electrowave
                    cost = 2;

                    spellTraits.Add(4);
                    spellTraits.Add(23);

                    icon = Globals.Content.Load<Texture2D>("electrowaveicon");
                    description = "EMANATE A WAVE OF STUNNING ELECTRIC MAGICK";
                    name = "ELECTROWAVE";

                    return;
                case 22: //lightning bolt
                    cost = 4;

                    spellTraits.Add(4);
                    spellTraits.Add(25);

                    icon = Globals.Content.Load<Texture2D>("lightningbolticon");
                    description = "SUMMON FORTH A BOLT OF LIGHTNING FROM THE FIRMAMENT";
                    name = "LIGHTNING BOLT";

                    return;
                case 23: //chain lightning
                    cost = 5;

                    spellTraits.Add(4);
                    spellTraits.Add(20);

                    icon = Globals.Content.Load<Texture2D>("chainlightningicon");
                    description = "CONJURE A MASSIVE BEAM OF PURE ARCANE LIGHTNING THAT ARCS BETWEEN ENEMIES";
                    name = "CHAIN LIGHTNING";

                    return;
                case 24: //force bolt
                    cost = 1;

                    spellTraits.Add(5);
                    spellTraits.Add(14);

                    icon = Globals.Content.Load<Texture2D>("forcebolticon");
                    description = "CASTS A SMALL BOLT OF FORCE MAGICK";
                    name = "FORCE BOLT";

                    return;
                case 25: //magic missile
                    cost = 1;

                    spellTraits.Add(5);
                    spellTraits.Add(15);

                    icon = Globals.Content.Load<Texture2D>("magicmissileicon");
                    description = "CASTS AN MISSILE OF ARCANE ENERGY";
                    name = "MAGICK MISSILE";

                    return;
                case 26: //push
                    cost = 2;

                    spellTraits.Add(5);
                    spellTraits.Add(23);
                    spellTraits.Add(24);

                    icon = Globals.Content.Load<Texture2D>("pushicon");
                    description = "BLASTS AWAY NEARBY ENEMIES";
                    name = "PUSH";

                    return;
                case 27: //magic missiles
                    cost = 3;

                    spellTraits.Add(5);
                    spellTraits.Add(15);

                    icon = Globals.Content.Load<Texture2D>("magicmissilesicon");
                    description = "CASTS AN ARRAY OF ARCANE MISSILES";
                    name = "MAGICK MISSILES";

                    return;
                case 28: //barrier
                    cost = 3;

                    spellTraits.Add(5);
                    spellTraits.Add(19);

                    icon = Globals.Content.Load<Texture2D>("barriericon");
                    description = "ERECTS A BARRIER AROUND THE CASTER THAT BLOCKS PROJECTILES";
                    name = "BARRIER";

                    return;
                case 29: //sunder mind
                    cost = 4;

                    spellTraits.Add(5);
                    spellTraits.Add(16);

                    icon = Globals.Content.Load<Texture2D>("sundermindicon");
                    description = "IMBUES YOUR NEXT SPELL TO CAUSE PSYCHIC DESTRUCTION";
                    name = "SUNDER MIND";

                    return;
                case 30: //flask
                    cost = 2;

                    spellTraits.Add(7);
                    spellTraits.Add(18);

                    icon = Globals.Content.Load<Texture2D>("flaskicon");
                    description = "CONJURE A FLASK OF MISCELLANEOUS ELEMENT";
                    name = "FLASK";

                    return;
                case 31: //alchemize
                    cost = 3;

                    spellTraits.Add(7);
                    spellTraits.Add(17);

                    icon = Globals.Content.Load<Texture2D>("alchemizeicon");
                    description = "EMPOWER YOUR NEXT SPELL TO DO BONUS DAMAGE BASED ON CURRENT MANA";
                    name = "ALCHEMIZE";

                    return;
                case 32: //deconstruct
                    cost = 3;

                    spellTraits.Add(7);
                    spellTraits.Add(16);

                    icon = Globals.Content.Load<Texture2D>("deconstructicon");
                    description = "EMPOWER YOUR NEXT SPELL TO GRANT BONUS MANA IF IT KILLS AN ENEMY";
                    name = "DECONSTRUCT";

                    return;
                case 33: //golden missile
                    cost = 4;

                    spellTraits.Add(7);
                    spellTraits.Add(15);

                    icon = Globals.Content.Load<Texture2D>("goldenmissileicon");
                    description = "CAST A BEAUTIFUL ALCHEMIC MISSILE OF GOLD";
                    name = "GOLDEN MISSILE";

                    return;
                case 34: //philosophora
                    cost = 5;

                    spellTraits.Add(7);
                    spellTraits.Add(25);

                    icon = Globals.Content.Load<Texture2D>("philosophoraicon");
                    description = "TO NOTHING";
                    name = "PHILOSOPHORA";

                    return;
                case 35: //ray of light
                    cost = 1;

                    spellTraits.Add(9);
                    spellTraits.Add(20);

                    icon = Globals.Content.Load<Texture2D>("rayoflighticon");
                    description = "FIRE A BEAM OF SEARING HOLY LIGHT";
                    name = "RAY OF LIGHT";

                    return;
                case 36: //purify
                    cost = 2;

                    spellTraits.Add(9);
                    spellTraits.Add(17);

                    icon = Globals.Content.Load<Texture2D>("purifyicon");
                    description = "EMPOWER YOUR NEXT SPELL TO BE FASTER AND STRONGER";
                    name = "BLESS";

                    return;
                case 37: //smite
                    cost = 3;

                    spellTraits.Add(9);
                    spellTraits.Add(25);

                    icon = Globals.Content.Load<Texture2D>("smiteicon");
                    description = "SUMMON FORTH HEAVENS' JUDGMENT";
                    name = "SMITE";

                    return;
                case 38: //lightbringer
                    cost = 2;

                    spellTraits.Add(9);
                    spellTraits.Add(21);

                    icon = Globals.Content.Load<Texture2D>("lightbringericon");
                    description = "WIELD THE LIGHTBRINGER MACE";
                    name = "LIGHTBRINGER";

                    return;
                case 39: //protection
                    cost = 4;

                    spellTraits.Add(9);
                    spellTraits.Add(18);
                    spellTraits.Add(19);

                    icon = Globals.Content.Load<Texture2D>("protectionicon");
                    description = "PROTECTS THE USER FOR A SHORT TIME OR UNTIL THEY CAST ANOTHER SPELL";
                    name = "PROTECTION";

                    return;
                case 40: //consecrate
                    cost = 5;

                    spellTraits.Add(9);
                    spellTraits.Add(22);
                    spellTraits.Add(24);


                    icon = Globals.Content.Load<Texture2D>("consecrateicon");
                    description = "LET THE GROUND BENEARTH US, AND ALL THAT INHABITS IT, BECOME PURE";
                    name = "CONSECRATE";

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

                    if (explosify)
                    {
                        firebolt.explosify = true;
                        firebolt.explosifyDamage = explosifyDamage;

                        explosify = false;
                    }

                    ProjectileManager.AddProjectile(firebolt);

                    SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("shoot"), 1, 0.6f, 0);
                    return;
                case 2: // fireball
                    Projectile fireball = new(player.center, 2, speed, castDamage, 0, knockback, Vector2.Normalize(InputManager.MousePosition - player.center) * 200, lifespan, size);


                    if (explosify)
                    {
                        fireball.explosify = true;
                        fireball.explosifyDamage = explosifyDamage;

                        explosify = false;
                    }


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

                    if (explosify)
                    {
                        kindle.explosify = true;
                        kindle.explosifyDamage = explosifyDamage;

                        explosify = false;
                    }

                    ProjectileManager.AddProjectile(kindle);

                    SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("shoot"), 1, 0, 0);
                    return;

                case 4: // bladeofflame
                    Projectile bladeofflame = new(player.center, 4, speed, castDamage, -1, knockback, Vector2.Zero, lifespan, 20, 0f, size, true, 0, 25 + (5 * rank));

                    if (explosify)
                    {
                        bladeofflame.explosify = true;
                        bladeofflame.explosifyDamage = explosifyDamage;

                        explosify = false;
                    }

                    ProjectileManager.AddProjectile(bladeofflame);

                    SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("shoot"), 1, 0, 0);
                    return;

                case 5: // disintegrate
                    Projectile disintegrate = new(new Vector2(InputManager.MousePosition.X, -10), 5, speed, castDamage, -1, knockback, lifespan, 10, size);

                    if (explosify)
                    {
                        disintegrate.explosify = true;
                        disintegrate.explosifyDamage = explosifyDamage;

                        explosify = false;
                    }

                    ProjectileManager.AddProjectile(disintegrate);

                    SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("smallexplosion"), 0.6f, 0.4f, 0);
                    GameManager.AddScreenShake(0.1f, 3f);

                    return;
                case 6: //sparks

                    for (int i = 0; i < rank + 3; i++)
                    {
                        Projectile spark = new(player.center, 6, speed * Globals.RandomFloat(0.75f, 1.25f), castDamage, 0, knockback, Vector2.Normalize(InputManager.MousePosition - player.center).RotatedBy(MathHelper.ToRadians(Globals.RandomFloat(-20f, 20f))) * 200, lifespan * Globals.RandomFloat(0.75f, 1.25f), size);

                        if (explosify)
                        {
                            spark.explosify = true;
                            spark.explosifyDamage = explosifyDamage / (rank + 3);
                        }

                        ProjectileManager.AddProjectile(spark);
                    }

                    explosify = false;

                    SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("shoot"), 1, 0.6f, 0);
                    return;
                case 7: // explosify

                    explosify = true;
                    explosifyDamage = damage;

                    for (int i = 0; i < 5; i++)
                    {
                        ParticleData pd = new()
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

                        Particle p = new(player.center, pd);
                        ParticleManager.AddParticle(p);
                    }

                    SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("chirp"), 1, 0.6f, 0);
                    return;
                case 8: // explosion

                    

                    SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("lastingexplosion"), 1, 0.6f, 0);
                    return;
                case 9: //snowball

                    return;
                case 10: //ice bolt

                    return;
                case 11: //frost wave

                    return;
                case 12: //ice beam

                    return;
                case 13: //frost blast

                    return;
                case 14: //wall of ice

                    return;
                case 15: //ice spear

                    return;
                case 16: //entomb

                    return;
                case 17: //zap

                    return;
                case 18: //ball lightning

                    return;
                case 19: //electrify

                    return;
                case 20: //thunderspark

                    return;
                case 21: //electrowave

                    return;
                case 22: //lightning bolt

                    return;
                case 23: //chain lightning

                    return;
                case 24: //force bolt

                    return;
                case 25: //magic missile

                    return;
                case 26: //push

                    return;
                case 27: //magic missiles

                    return;
                case 28: //barrier

                    return;
                case 29: //sunder mind

                    return;
                case 30: //flask

                    return;
                case 31: //alchemize

                    return;
                case 32: //deconstruct

                    return;
                case 33: //golden missile

                    return;
                case 34: //philosophora

                    return;
                case 35: //ray of light

                    Vector2 nextCenter;
                    int count = 0;

                    for (int i = 0; i < 40; i++)
                    {
                        count++;
                        nextCenter = player.center + player.center.DirectionTo(InputManager.MousePosition) * 9 * i;

                        if (nextCenter.X > GameManager.bounds.X && nextCenter.X < GameManager.bounds.Z && nextCenter.Y > GameManager.bounds.Y && nextCenter.Y < GameManager.bounds.W)
                        {
                            Projectile ray = new(nextCenter, 8, 0f, castDamage, -1, knockback, Vector2.Zero, lifespan, 60, player.center.DirectionTo(InputManager.MousePosition).ToRotation(), 1f, true, 0f, size);

                            if (explosify && i % 3 == 0)
                            {
                                ray.explosify = true;
                                ray.explosifyDamage = explosifyDamage / 3;

                            }

                            ProjectileManager.AddProjectile(ray);
                        }

                        else
                        {
                            for (int j = 0; j < 10; j++)
                            {
                                ParticleData pd = new()
                                {
                                    sizeStart = Globals.RandomFloat(4f, 7f),
                                    sizeEnd = 0,
                                    colorStart = Color.White,
                                    colorEnd = Color.White,
                                    velocity = new(Globals.RandomFloat(-80, 80), Globals.RandomFloat(-80, 80)),
                                    lifespan = Globals.RandomFloat(0.5f, 0.8f),
                                    rotationSpeed = Globals.RandomFloat(-0.5f, 0.5f)
                                };

                                Particle p = new(nextCenter, pd);
                                ParticleManager.AddParticle(p);
                            }

                            for (int j = 0; j < 10; j++)
                            {
                                ParticleData pd = new()
                                {
                                    sizeStart = Globals.RandomFloat(2f, 4f),
                                    sizeEnd = 0,
                                    colorStart = Color.White,
                                    colorEnd = Color.White,
                                    velocity = new(Globals.RandomFloat(-350, 350), Globals.RandomFloat(-350, 350)),
                                    lifespan = Globals.RandomFloat(0.2f, 0.4f),
                                    rotationSpeed = Globals.RandomFloat(-0.5f, 0.5f)
                                };

                                Particle p = new(nextCenter, pd);
                                ParticleManager.AddParticle(p);
                            }

                            break;
                        }
                    }

                    explosify = false;

                    for (int i = 0; i < 10; i++)
                    {
                        ParticleData pd = new()
                        {
                            sizeStart = Globals.RandomFloat(2f, size / 2f),
                            sizeEnd = 0,
                            colorStart = Color.White,
                            colorEnd = Color.White,
                            velocity = new(Globals.RandomFloat(-200, 200), Globals.RandomFloat(-200, 200)),
                            lifespan = Globals.RandomFloat(0.1f, 0.5f),
                            rotationSpeed = Globals.RandomFloat(-0.5f, 0.5f)
                        };

                        Particle p = new(player.center, pd);
                        ParticleManager.AddParticle(p);
                    }

                    SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("magick2"), 0.4f, 0f, 0);
                    SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("hit2"), 1f, 1f, 0);
                    SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("magick"), 5f, 1f, 0);
                    SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("chirp"), 1f, 1f, 0);


                    return;
                case 36: //purify

                    return;
                case 37: //smite

                    return;
                case 38: //lightbringer

                    return;
                case 39: //protection

                    return;
                case 40: //consecrate

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
                            lifespan = 1.5f;
                            speed = 1.5f;
                            break;
                        case 3:
                            size = 4.5f;
                            cooldown = 0.1f;
                            damage = 3f;
                            lifespan = 2.5f;
                            speed = 2f;
                            break;
                    }
                    break;
                case 7: // explosify
                    switch (rank)
                    {
                        case 1:
                            damage = 3f;
                            cooldown = 0.1f;
                            break;
                        case 2:
                            damage = 6f;
                            cooldown = 0.05f;
                            break;
                        case 3:
                            damage = 10f;
                            cooldown = 0.01f;
                            break;
                    }
                    break;
                case 8: // explosion
                    switch (rank)
                    {
                        case 1:

                            break;
                        case 2:

                            break;
                        case 3:

                            break;
                    }
                    break;
                case 9: //snowball
                    switch (rank)
                    {
                        case 1:

                            break;
                        case 2:

                            break;
                        case 3:

                            break;
                    }
                    break;
                case 10: //ice bolt
                    switch (rank)
                    {
                        case 1:

                            break;
                        case 2:

                            break;
                        case 3:

                            break;
                    }
                    break;
                case 11: //frost wave
                    switch (rank)
                    {
                        case 1:

                            break;
                        case 2:

                            break;
                        case 3:

                            break;
                    }
                    break;
                case 12: //ice beam
                    switch (rank)
                    {
                        case 1:

                            break;
                        case 2:

                            break;
                        case 3:

                            break;
                    }
                    break;
                case 13: //frost blast
                    switch (rank)
                    {
                        case 1:

                            break;
                        case 2:

                            break;
                        case 3:

                            break;
                    }
                    break;
                case 14: //wall of ice
                    switch (rank)
                    {
                        case 1:

                            break;
                        case 2:

                            break;
                        case 3:

                            break;
                    }
                    break;
                case 15: //ice spear
                    switch (rank)
                    {
                        case 1:

                            break;
                        case 2:

                            break;
                        case 3:

                            break;
                    }
                    break;
                case 16: //entomb
                    switch (rank)
                    {
                        case 1:

                            break;
                        case 2:

                            break;
                        case 3:

                            break;
                    }
                    break;
                case 17: //zap
                    switch (rank)
                    {
                        case 1:

                            break;
                        case 2:

                            break;
                        case 3:

                            break;
                    }
                    break;
                case 18: //ball lightning
                    switch (rank)
                    {
                        case 1:

                            break;
                        case 2:

                            break;
                        case 3:

                            break;
                    }
                    break;
                case 19: //electrify
                    switch (rank)
                    {
                        case 1:

                            break;
                        case 2:

                            break;
                        case 3:

                            break;
                    }
                    break;
                case 20: //thunderspark
                    switch (rank)
                    {
                        case 1:

                            break;
                        case 2:

                            break;
                        case 3:

                            break;
                    }
                    break;
                case 21: //electrowave
                    switch (rank)
                    {
                        case 1:

                            break;
                        case 2:

                            break;
                        case 3:

                            break;
                    }
                    break;
                case 22: //lightning bolt
                    switch (rank)
                    {
                        case 1:

                            break;
                        case 2:

                            break;
                        case 3:

                            break;
                    }
                    break;
                case 23: //chain lightning
                    switch (rank)
                    {
                        case 1:

                            break;
                        case 2:

                            break;
                        case 3:

                            break;
                    }
                    break;
                case 24: //force bolt
                    switch (rank)
                    {
                        case 1:

                            break;
                        case 2:

                            break;
                        case 3:

                            break;
                    }
                    break;
                case 25: //magic missile
                    switch (rank)
                    {
                        case 1:

                            break;
                        case 2:

                            break;
                        case 3:

                            break;
                    }
                    break;
                case 26: //push
                    switch (rank)
                    {
                        case 1:

                            break;
                        case 2:

                            break;
                        case 3:

                            break;
                    }
                    break;
                case 27: //magic missiles
                    switch (rank)
                    {
                        case 1:

                            break;
                        case 2:

                            break;
                        case 3:

                            break;
                    }
                    break;
                case 28: //barrier
                    switch (rank)
                    {
                        case 1:

                            break;
                        case 2:

                            break;
                        case 3:

                            break;
                    }
                    break;
                case 29: //sunder mind
                    switch (rank)
                    {
                        case 1:

                            break;
                        case 2:

                            break;
                        case 3:

                            break;
                    }
                    break;
                case 30: //flask
                    switch (rank)
                    {
                        case 1:

                            break;
                        case 2:

                            break;
                        case 3:

                            break;
                    }
                    break;
                case 31: //alchemize
                    switch (rank)
                    {
                        case 1:

                            break;
                        case 2:

                            break;
                        case 3:

                            break;
                    }
                    break;
                case 32: //deconstruct
                    switch (rank)
                    {
                        case 1:

                            break;
                        case 2:

                            break;
                        case 3:

                            break;
                    }
                    break;
                case 33: //golden missile
                    switch (rank)
                    {
                        case 1:

                            break;
                        case 2:

                            break;
                        case 3:

                            break;
                    }
                    break;
                case 34: //philosophora
                    switch (rank)
                    {
                        case 1:

                            break;
                        case 2:

                            break;
                        case 3:

                            break;
                    }
                    break;
                case 35: //ray of light
                    switch (rank)
                    {
                        case 1:
                            size = 10f;
                            cooldown = 0.5f;
                            damage = 2f;
                            lifespan = 0.5f;
                            knockback = 1f;
                            break;
                        case 2:
                            cooldown = 0.4f;
                            damage = 4f;
                            size = 15f;
                            lifespan = 0.5f;
                            knockback = 1f;

                            break;
                        case 3:
                            size = 20f;
                            cooldown = 0.1f;
                            damage = 10f;
                            lifespan = 0.5f;
                            knockback = 1f;

                            break;
                    }
                    break;
                case 36: //purify
                    switch (rank)
                    {
                        case 1:

                            break;
                        case 2:

                            break;
                        case 3:

                            break;
                    }
                    break;
                case 37: //smite
                    switch (rank)
                    {
                        case 1:

                            break;
                        case 2:

                            break;
                        case 3:

                            break;
                    }
                    break;
                case 38: //lightbringer
                    switch (rank)
                    {
                        case 1:

                            break;
                        case 2:

                            break;
                        case 3:

                            break;
                    }
                    break;
                case 39: //protection
                    switch (rank)
                    {
                        case 1:

                            break;
                        case 2:

                            break;
                        case 3:

                            break;
                    }
                    break;
                case 40: //consecrate
                    switch (rank)
                    {
                        case 1:

                            break;
                        case 2:

                            break;
                        case 3:

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
