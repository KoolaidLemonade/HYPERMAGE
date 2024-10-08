using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HYPERMAGE.Spells
{
    public static class Spellbook
    {
        public static int spellMemoryMax = 10;
        public static int spellCountPrimary = 1;
        public static int spellCountSecondary = 1;
        public static List<Spell> spellsPrimary = [];
        public static List<Spell> spellsSecondary = [];
        public static List<Spell> spellMemory = [];

        public static void Update()
        {
            
        }

        public static void AddSpellPrimary(Spell spell)
        {
            spellsPrimary.Add(spell);
        }
        public static void RemoveSpellPrimary(Spell spell)
        {
            spellsPrimary.Remove(spell);
        }
        public static void AddSpellSecondary(Spell spell)
        {
            spellsSecondary.Add(spell);
        }
        public static void RemoveSpellSecondary(Spell spell)
        {
            spellsSecondary.Remove(spell);
        }
        public static void AddSpellMemory(Spell spell)
        {
            spellMemory.Add(spell);
        }
        public static void RemoveSpellMemory(Spell spell)
        {
            spellMemory.Remove(spell);
        }
    }
}
