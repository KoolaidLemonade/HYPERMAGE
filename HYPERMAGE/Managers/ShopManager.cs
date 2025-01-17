using HYPERMAGE.Helpers;
using HYPERMAGE.Spells;
using HYPERMAGE.UI.UIElements;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HYPERMAGE.Managers
{
    public static class ShopManager
    {
        private static int shopSpellCount = 5;

        private static bool reroll = false;

        public static int rerollCost = 2;
        public static int xpCost = 5;

        public static List<ShopSpell> shopSpells = [];

        public static bool locked = false;

        public static List<Vector2> shopWeights = [];

        public static void Update()
        {
            if (reroll)
            {

                for (int i = 0; i < shopSpellCount; i++)
                {
                    switch (Globals.GetWeightedRandomInt(shopWeights))
                    {
                        case 1:
                            {
                                shopSpells.Add(new ShopSpell(Globals.GetPixelFont(), Spellbook.GetRandomSpell(1), Globals.Content.Load<Texture2D>("ui2"), new Vector2(160 - 16 / 2, 75) + new Vector2(-30 - shopSpellCount * 2, 0).RotatedBy(MathHelper.ToRadians(MathHelper.Lerp(0 + shopSpellCount * 2, -180 - shopSpellCount * 2, i / (shopSpellCount - 1f))))));
                                break;
                            }
                        case 2:
                            {
                                shopSpells.Add(new ShopSpell(Globals.GetPixelFont(), Spellbook.GetRandomSpell(2), Globals.Content.Load<Texture2D>("ui2"), new Vector2(160 - 16 / 2, 75) + new Vector2(-30 - shopSpellCount * 2, 0).RotatedBy(MathHelper.ToRadians(MathHelper.Lerp(0 + shopSpellCount * 2, -180 - shopSpellCount * 2, i / (shopSpellCount - 1f))))));
                                break;
                            }
                        case 3:
                            {
                                shopSpells.Add(new ShopSpell(Globals.GetPixelFont(), Spellbook.GetRandomSpell(3), Globals.Content.Load<Texture2D>("ui2"), new Vector2(160 - 16 / 2, 75) + new Vector2(-30 - shopSpellCount * 2, 0).RotatedBy(MathHelper.ToRadians(MathHelper.Lerp(0 + shopSpellCount * 2, -180 - shopSpellCount * 2, i / (shopSpellCount - 1f))))));
                                break;
                            }
                        case 4:
                            {
                                shopSpells.Add(new ShopSpell(Globals.GetPixelFont(), Spellbook.GetRandomSpell(4), Globals.Content.Load<Texture2D>("ui2"), new Vector2(160 - 16 / 2, 75) + new Vector2(-30 - shopSpellCount * 2, 0).RotatedBy(MathHelper.ToRadians(MathHelper.Lerp(0 + shopSpellCount * 2, -180 - shopSpellCount * 2, i / (shopSpellCount - 1f))))));
                                break;
                            }
                        case 5:
                            {
                                shopSpells.Add(new ShopSpell(Globals.GetPixelFont(), Spellbook.GetRandomSpell(5), Globals.Content.Load<Texture2D>("ui2"), new Vector2(160 - 16 / 2, 75) + new Vector2(-30 - shopSpellCount * 2, 0).RotatedBy(MathHelper.ToRadians(MathHelper.Lerp(0 + shopSpellCount * 2, -180 - shopSpellCount * 2, i / (shopSpellCount - 1f))))));
                                break;
                            }
                    }

                    reroll = false;

                    UIManager.AddElement(shopSpells[i]);
                }
            }

            if (SpellbookUI.open || SpellbookUI.closing)
            {
                for (int i = 0; i < shopSpells.Count; i++)
                {
                    shopSpells[i].position = new(Globals.NonLerp(shopSpells[i].originalPos.X, 215, SpellbookUI.openingTimer), Globals.NonLerp(shopSpells[i].originalPos.Y, 26 + i * 20, SpellbookUI.openingTimer));
                }
            }
        }
        public static void Reroll()
        {
            reroll = true;

            for (int i = 0; i < shopWeights.Count; i++)
            {
                Debug.WriteLine(shopWeights[i]);
            }

            if (shopSpells.Count != 0)
            {
                for (int i = 0; i < shopSpells.Count; i++)
                {
                    UIManager.RemoveElement(shopSpells[i]);
                }
            }

            shopSpells.Clear();

            switch (GameManager.GetPlayer().level)
            {
                case 1:
                    {
                        shopWeights.Clear();
                        shopWeights.Add(new(1, 100));
                        shopWeights.Add(new(2, 0));
                        shopWeights.Add(new(3, 0));
                        shopWeights.Add(new(4, 0));
                        shopWeights.Add(new(5, 0));

                        break;
                    }
                case 2:
                    {
                        shopWeights.Clear();
                        shopWeights.Add(new(1, 75));
                        shopWeights.Add(new(2, 25));
                        shopWeights.Add(new(3, 0));
                        shopWeights.Add(new(4, 0));
                        shopWeights.Add(new(5, 0));

                        break;
                    }
                case 3:
                    {
                        shopWeights.Clear();
                        shopWeights.Add(new(1, 60));
                        shopWeights.Add(new(2, 30));
                        shopWeights.Add(new(3, 10));
                        shopWeights.Add(new(4, 0));
                        shopWeights.Add(new(5, 0));

                        break;
                    }
                case 4:
                    {
                        shopWeights.Clear();
                        shopWeights.Add(new(1, 50));
                        shopWeights.Add(new(2, 35));
                        shopWeights.Add(new(3, 15));
                        shopWeights.Add(new(4, 0));
                        shopWeights.Add(new(5, 0));

                        break;
                    }
                case 5:
                    {
                        shopWeights.Clear();
                        shopWeights.Add(new(1, 40));
                        shopWeights.Add(new(2, 38));
                        shopWeights.Add(new(3, 20));
                        shopWeights.Add(new(4, 2));
                        shopWeights.Add(new(5, 0));

                        break;
                    }
                case 6:
                    {
                        shopWeights.Clear();
                        shopWeights.Add(new(1, 27));
                        shopWeights.Add(new(2, 42));
                        shopWeights.Add(new(3, 25));
                        shopWeights.Add(new(4, 5));
                        shopWeights.Add(new(5, 1));

                        break;
                    }
                case 7:
                    {
                        shopWeights.Clear();
                        shopWeights.Add(new(1, 20));
                        shopWeights.Add(new(2, 37));
                        shopWeights.Add(new(3, 32));
                        shopWeights.Add(new(4, 9));
                        shopWeights.Add(new(5, 2));

                        break;
                    }
                case 8:
                    {
                        shopWeights.Clear();
                        shopWeights.Add(new(1, 12));
                        shopWeights.Add(new(2, 30));
                        shopWeights.Add(new(3, 40));
                        shopWeights.Add(new(4, 13));
                        shopWeights.Add(new(5, 5));

                        break;
                    }
                case 9:
                    {
                        shopWeights.Clear();
                        shopWeights.Add(new(1, 7));
                        shopWeights.Add(new(2, 20));
                        shopWeights.Add(new(3, 43));
                        shopWeights.Add(new(4, 20));
                        shopWeights.Add(new(5, 10));

                        break;
                    }

                case 10:
                    {
                        shopWeights.Clear();
                        shopWeights.Add(new(1, 5));
                        shopWeights.Add(new(2, 10));
                        shopWeights.Add(new(3, 33));
                        shopWeights.Add(new(4, 30));
                        shopWeights.Add(new(5, 22));

                        break;
                    }
            }
        }

        public static void RemoveSpell(ShopSpell spell)
        {
            shopSpells.Remove(spell);
            UIManager.RemoveElement(spell);
        }
    }
}
