using HYPERMAGE.Helpers;
using HYPERMAGE.Managers;
using HYPERMAGE.UI.UIElements;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace HYPERMAGE.Spells
{
    public static class Spellbook
    {
        public static List<Spell> allSpells = [];

        public static List<Spell> allSpellsCost1 = [];
        public static List<Spell> allSpellsCost2 = [];
        public static List<Spell> allSpellsCost3 = [];
        public static List<Spell> allSpellsCost4 = [];
        public static List<Spell> allSpellsCost5 = [];

        public static int spellMemoryMax = 10;
        public static int spellCountPrimary = 1;
        public static int spellCountSecondary = 1;

        public static List<Spell> spellsPrimary = [];
        public static List<Spell> spellsSecondary = [];
        public static List<Spell> spellMemory = [];

        public static List<Spell> totalSpellList = [];

        public static int spellCounterPrimary = 0;
        public static int spellCounterSecondary = 0;

        public static float spellCooldown = 0;

        public static float spellsRechargePrimary = 0;
        public static float spellsRechargeSecondary = 0;

        public static float spellsRechargePrimaryTime = 3f;
        public static float spellsRechargeSecondaryTime = 3f;

        public static List<int> traitsPrimary = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
        public static List<int> traitsSecondary = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
        public static List<int> traitsMemory = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];

        public static void Init()
        {
            for (int i = 0; i < Spell.totalSpellTypes; i++)
            {
                allSpells.Add(new Spell(i + 1, 1));
            }

            for (int i = 0; i < allSpells.Count; i++)
            {
                if (allSpells[i].cost == 1)
                {
                    allSpellsCost1.Add(allSpells[i]);
                }

                if (allSpells[i].cost == 2)
                {
                    allSpellsCost2.Add(allSpells[i]);
                }

                if (allSpells[i].cost == 3)
                {
                    allSpellsCost3.Add(allSpells[i]);
                }

                if (allSpells[i].cost == 4)
                {
                    allSpellsCost4.Add(allSpells[i]);
                }

                if (allSpells[i].cost == 5)
                {
                    allSpellsCost5.Add(allSpells[i]);
                }
            }
        }
        public static void Update()
        {
            spellsRechargePrimary -= Globals.TotalSeconds;
            spellsRechargeSecondary -= Globals.TotalSeconds;
            spellCooldown -= Globals.TotalSeconds;

            CheckRankUp();
        }

        public static void CheckTraits(int index)
        {
            List<int> usedSpells = [];

            if (index == 1)
            {
                traitsPrimary = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];

                foreach (Spell spell in spellsPrimary)
                {
                    if (!usedSpells.Contains(spell.spellType))
                    {
                        usedSpells.Add(spell.spellType);

                        for (int i = 0; i < spell.spellTraits.Count; i++)
                        {
                            traitsPrimary[spell.spellTraits[i] - 1]++;
                        }
                    }
                }
            }

            if (index == 2)
            {
                traitsSecondary = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];

                foreach (Spell spell in spellsSecondary)
                {
                    if (!usedSpells.Contains(spell.spellType))
                    {
                        usedSpells.Add(spell.spellType);

                        for (int i = 0; i < spell.spellTraits.Count; i++)
                        {
                            traitsSecondary[spell.spellTraits[i] - 1]++;
                        }
                    }
                }
            }

            if (index == 3)
            {
                traitsMemory = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];

                foreach (Spell spell in spellMemory)
                {
                    if (!usedSpells.Contains(spell.spellType))
                    {
                        usedSpells.Add(spell.spellType);

                        for (int i = 0; i < spell.spellTraits.Count; i++)
                        {
                            traitsMemory[spell.spellTraits[i] - 1]++;
                        }
                    }
                }
            }

            Debug.Write("( ");

            for (int i = 0; i < traitsPrimary.Count; i++)
            {
                Debug.Write(traitsPrimary[i] + " ");
            }

            Debug.Write(")");
        }
        public static void CheckRankUp()
        {
            for (int i = 0; i < totalSpellList.Count; i++)
            {
                int k = 0;

                List<Spell> types = [];

                for (int j = 0; j < totalSpellList.Count; j++)
                {
                    if (totalSpellList[i].spellType == totalSpellList[j].spellType && totalSpellList[i].rank == totalSpellList[j].rank)
                    {
                        if (k > 0)
                        {
                            types.Add(totalSpellList[j]);
                        }

                        if (k > 1)
                        {
                            RankUpSpell(totalSpellList[i]);

                            for (int l = 0; l < types.Count; l++)
                            {
                                totalSpellList.Remove(types[l]);

                                switch (types[l].index)
                                {
                                    case 1:
                                        RemoveSpellPrimary(types[l]);
                                        break;
                                    case 2:
                                        RemoveSpellSecondary(types[l]);
                                        break;
                                    case 3:
                                        RemoveSpellMemory(types[l]);
                                        break;
                                }
                            }
                        }

                        k++;
                    }
                }
            }
        }
        public static Spell GetRandomSpell(int cost)
        {
            switch (cost)
            {
                case 1:
                    return new Spell(allSpellsCost1[Globals.Random.Next(allSpellsCost1.Count)].spellType, 1);
                case 2:
                    return new Spell(allSpellsCost2[Globals.Random.Next(allSpellsCost2.Count)].spellType, 1);
                case 3:
                    return new Spell(allSpellsCost3[Globals.Random.Next(allSpellsCost3.Count)].spellType, 1);
                case 4:
                    return new Spell(allSpellsCost4[Globals.Random.Next(allSpellsCost4.Count)].spellType, 1);
                case 5:
                    return new Spell(allSpellsCost5[Globals.Random.Next(allSpellsCost5.Count)].spellType, 1);
            }

            return null; 
        }

        public static void RankUpSpell(Spell spell)
        {
            if (spell.index == 1)
            {
                foreach (Spell i in spellsPrimary)
                {
                    if (i == spell)
                    {
                        i.RankUp();
                    }
                }
            }

            if (spell.index == 2)
            {
                foreach (Spell i in spellsSecondary)
                {
                    if (i == spell)
                    {
                        i.RankUp();
                    }
                }
            }

            if (spell.index == 3)
            {
                foreach (Spell i in spellMemory)
                {
                    if (i == spell)
                    {
                        i.RankUp();
                    }
                }
            }

        }
        public static void AddSpellPrimary(Spell spell)
        {
            spell.index = 1;

            spellsPrimary.Add(spell);

            totalSpellList.Add(spell);

            SpellbookUI.AddSpell(new SpellbookSpell(SpellbookUI.GetFirstEmptyPosIndex(1), 1, spell, spell.icon, new Vector2(-100, -100)));

            CheckTraits(1);
            UpdateSpellPositions(1);
        }
        public static void RemoveSpellPrimary(Spell spell)
        {
            spellsPrimary.Remove(spell);

            totalSpellList.Remove(spell);

            SpellbookUI.RemoveSpell(spell);

            CheckTraits(1);
            UpdateSpellPositions(1);
        }
        public static void AddSpellSecondary(Spell spell)
        {
            spell.index = 2;

            spellsSecondary.Add(spell);

            totalSpellList.Add(spell);

            SpellbookUI.AddSpell(new SpellbookSpell(SpellbookUI.GetFirstEmptyPosIndex(2), 2, spell, spell.icon, new Vector2(-100, -100)));

            CheckTraits(2);
            UpdateSpellPositions(2);
        }
        public static void RemoveSpellSecondary(Spell spell)
        {
            spellsSecondary.Remove(spell);

            totalSpellList.Remove(spell);

            SpellbookUI.RemoveSpell(spell);

            CheckTraits(2);
            UpdateSpellPositions(2);
        }
        public static void AddSpellMemory(Spell spell)
        {
            spell.index = 3;

            totalSpellList.Add(spell);

            spellMemory.Add(spell);

            SpellbookUI.AddSpell(new SpellbookSpell(SpellbookUI.GetFirstEmptyPosIndex(3), 3, spell, spell.icon, new Vector2(-100, -100)));

            CheckTraits(3);
            UpdateSpellPositions(3);
        }
        public static void RemoveSpellMemory(Spell spell)
        {
            spellMemory.Remove(spell);

            totalSpellList.Remove(spell);

            SpellbookUI.RemoveSpell(spell);

            CheckTraits(3);
            UpdateSpellPositions(3);
        }

        public static Color GetRarityColor(Spell spell)
        {
            switch (spell.cost)
            {
                case 1:
                    return Color.White;
                case 2:
                    return Color.LawnGreen;
                case 3:
                    return Color.RoyalBlue;

                case 4:
                    return Color.DeepPink;
                case 5:
                    return Color.Yellow;

            }
            return Color.White;
        }

        public static void CastFrontSpellPrimary()
        {
            if (spellsPrimary.Count == 0 || spellCooldown > 0 || spellsRechargePrimary > 0)
            {
                return;
            }

            List<int> positions = [];
            int frontPos;

            foreach (Spell spell in spellsPrimary)
            {
                positions.Add(spell.position);
            }
            
            positions.Sort();
            frontPos = positions[spellCounterPrimary];

            foreach (Spell spell in spellsPrimary)
            {
                if (spell.position == frontPos)
                {
                    spell.Cast(GameManager.GetPlayer());
                    spellCooldown = spell.cooldown;
                }
            }

            spellCounterPrimary++;

            if (spellCounterPrimary > positions.Count - 1)
            {
                spellsRechargePrimary = spellsRechargePrimaryTime;
                spellCounterPrimary = 0;

                return;
            }
        }

        public static void UpdateSpellPositions(int index)
        {
            if (index == 1)
            {
                spellCounterPrimary = 0;

                for (int i = 0; i < spellsPrimary.Count; i++)
                {
                    spellsPrimary[i].position = SpellbookUI.spellsPrimary[i].posIndex;
                }
            }

            if (index == 2)
            {
                spellCounterSecondary = 0;

                for (int i = 0; i < spellsSecondary.Count; i++)
                {
                    spellsSecondary[i].position = SpellbookUI.spellsSecondary[i].posIndex;
                }
            }

            if (index == 3)
            {
                for (int i = 0; i < spellMemory.Count; i++)
                {
                    spellMemory[i].position = SpellbookUI.spellsMemory[i].posIndex;
                }
            }
        }
        public static void CastFrontSpellSecondary()
        {
            if (spellsSecondary.Count == 0 || spellCooldown > 0 || spellsRechargeSecondary > 0)
            {
                return;
            }

            List<int> positions = [];
            int frontPos;

            foreach (Spell spell in spellsSecondary)
            {
                positions.Add(spell.position);
            }

            positions.Sort();
            frontPos = positions[spellCounterSecondary];

            foreach (Spell spell in spellsSecondary)
            {
                if (spell.position == frontPos)
                {
                    spell.Cast(GameManager.GetPlayer());
                    spellCooldown = spell.cooldown;
                }
            }

            spellCounterSecondary++;

            if (spellCounterSecondary > positions.Count - 1)
            {
                spellsRechargeSecondary = spellsRechargeSecondaryTime;
                spellCounterSecondary = 0;

                return;
            }
        }
    }
}
