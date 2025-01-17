using HYPERMAGE.Helpers;
using HYPERMAGE.Managers;
using HYPERMAGE.Spells;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace HYPERMAGE.UI.UIElements
{
    public class SpellbookUI : Button
    {
        public static bool open;

        public static bool closing;
        public static float openingTimer;
        public static float openTime = 0.5f;

        private Vector2 originalPosition;

        public static List<SpellbookSpell> spellsPrimary = [];
        public static List<SpellbookSpell> spellsSecondary = [];
        public static List<SpellbookSpell> spellsMemory = [];

        public static List<SpellbookSpellBorder> spellsPrimaryBorders = [];
        public static List<SpellbookSpellBorder> spellsSecondaryBorders = [];
        public static List<SpellbookSpellBorder> spellsMemoryBorders = [];
        public SpellbookUI(Vector2 position, Texture2D texture) : base(texture, position)
        {
            originalPosition = position;


            for (int i = 0; i < Spellbook.spellsPrimary.Count; i++)
            {
                spellsPrimary.Add(new SpellbookSpell(i, 1, Spellbook.spellsPrimary[i], Spellbook.spellsPrimary[i].icon, new Vector2(-100, -100)));
            }

            for (int i = 0; i < Spellbook.spellsSecondary.Count; i++)
            {
                spellsSecondary.Add(new SpellbookSpell(i, 2, Spellbook.spellsSecondary[i], Spellbook.spellsSecondary[i].icon, new Vector2(-100, -100)));
            }

            for (int i = 0; i < Spellbook.spellMemory.Count; i++)
            {
                spellsMemory.Add(new SpellbookSpell(i, 3, Spellbook.spellMemory[i], Spellbook.spellMemory[i].icon, new Vector2(-100, -100)));
            }

            for (int i = 0; i < spellsMemory.Count; i++)
            {
                UIManager.AddElement(spellsMemory[i]);
            }

            for (int i = 0; i < spellsPrimary.Count; i++)
            {
                UIManager.AddElement(spellsPrimary[i]);
            }

            for (int i = 0; i < spellsSecondary.Count; i++)
            {
                UIManager.AddElement(spellsSecondary[i]);
            }

            //

            for (int i = 0; i < Spellbook.spellCountPrimary; i++)
            {
                spellsPrimaryBorders.Add(new SpellbookSpellBorder(i, 1, Globals.Content.Load<Texture2D>("ui2"), new Vector2(-100, -100)));
            }

            for (int i = 0; i < Spellbook.spellCountSecondary; i++)
            {
                spellsSecondaryBorders.Add(new SpellbookSpellBorder(i, 2, Globals.Content.Load<Texture2D>("ui2"), new Vector2(-100, -100)));
            }

            for (int i = 0; i < Spellbook.spellMemoryMax; i++)
            {
                spellsMemoryBorders.Add(new SpellbookSpellBorder(i, 3, Globals.Content.Load<Texture2D>("ui2"), new Vector2(-100, -100)));
            }

            for (int i = 0; i < spellsMemoryBorders.Count; i++)
            {
                UIManager.AddElement(spellsMemoryBorders[i]);
            }

            for (int i = 0; i < spellsPrimaryBorders.Count; i++)
            {
                UIManager.AddElement(spellsPrimaryBorders[i]);
            }

            for (int i = 0; i < spellsSecondaryBorders.Count; i++)
            {
                UIManager.AddElement(spellsSecondaryBorders[i]);
            }
        }

        public override void Update()
        {
            UpdateButtonHitbox();

            if (buttonHitbox.Contains(InputManager.MousePosition))
            {
                mouseHovering = true;

                if (InputManager.Clicked)
                {
                    Clicked();
                }
            }

            else
            {
                mouseHovering = false;
            }

            if (open)
            {
                if (openingTimer <= 1 - Globals.TotalSeconds)
                {
                    openingTimer += Globals.TotalSeconds * 1 / openTime;
                }
            }

            else
            {
                if (openingTimer > 0)
                {
                    openingTimer -= Globals.TotalSeconds * 1 / openTime;
                }

                if (openingTimer <= 0)
                {
                    closing = false;
                }
            }

            openingTimer /= 1;

            for (int i = 0; i < spellsPrimary.Count; i++)
            {
                spellsPrimary[i].position = new Vector2(1 + 8 + spellsPrimary[i].posIndex * 20, Globals.NonLerp(-48, 16 + 1, openingTimer));
            }

            for (int i = 0; i < spellsSecondary.Count; i++)
            {
                spellsSecondary[i].position = new Vector2(1 + 8 + spellsSecondary[i].posIndex * 20, Globals.NonLerp(-32, (16 * 4) + 1, openingTimer));
            }

            for (int i = 0; i < spellsMemory.Count; i++)
            {
                spellsMemory[i].position = new Vector2(1 + 8 + spellsMemory[i].posIndex * 20, Globals.NonLerp(-16, (16 * 7) + 1, openingTimer));
            }

            //

            for (int i = 0; i < spellsPrimaryBorders.Count; i++)
            {
                spellsPrimaryBorders[i].position = new Vector2(8 + spellsPrimaryBorders[i].posIndex * 20, Globals.NonLerp(-48, 16, openingTimer));
            }

            for (int i = 0; i < spellsSecondaryBorders.Count; i++)
            {
                spellsSecondaryBorders[i].position = new Vector2(8 + spellsSecondaryBorders[i].posIndex * 20, Globals.NonLerp(-32, (16 * 4), openingTimer));
            }

            for (int i = 0; i < spellsMemoryBorders.Count; i++)
            {
                spellsMemoryBorders[i].position = new Vector2(8 + spellsMemoryBorders[i].posIndex * 20, Globals.NonLerp(-16, (16 * 7), openingTimer));
            }

            position.Y = Globals.NonLerp(originalPosition.Y, 165, openingTimer);
        }

        public override void Clicked()
        {
            if (!open)
            {
                open = true;
            }

            else
            {
                closing = true;
                open = false;
            }

            base.Clicked();
        }
        public override void Draw()
        {
            if (open || closing)
            {
                int k = 0;

                for (int i = 0; i < Spellbook.traitsPrimary.Count; i++)
                {
                    if (Spellbook.traitsPrimary[i] > 0)
                    {
                        Globals.SpriteBatch.DrawString(Globals.GetPixelFont(), Spellbook.traitsPrimary[i].ToString(), new(8 + k * 20, Globals.NonLerp(-48, 38, openingTimer )), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.92f);
                        Globals.SpriteBatch.Draw(Globals.Content.Load<Texture2D>("traiticons"), new(13 + k * 20, Globals.NonLerp(-48, 39, openingTimer )), new Rectangle(i * 7, 0, 7, 7), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.92f);

                        k++;
                    }
                }

                int j = 0;

                for (int i = 0; i < Spellbook.traitsSecondary.Count; i++)
                {
                    if (Spellbook.traitsSecondary[i] > 0)
                    {
                        Globals.SpriteBatch.DrawString(Globals.GetPixelFont(), Spellbook.traitsSecondary[i].ToString(), new(8 + j * 20, Globals.NonLerp(-48, 86, openingTimer )), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.92f);
                        Globals.SpriteBatch.Draw(Globals.Content.Load<Texture2D>("traiticons"), new(13 + j * 20, Globals.NonLerp(-32, 87, openingTimer )), new Rectangle(i * 7, 0, 7, 7), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.92f);

                        j++;
                    }
                }

                int l = 0;

                for (int i = 0; i < Spellbook.traitsMemory.Count; i++)
                {
                    if (Spellbook.traitsMemory[i] > 0)
                    {
                        Globals.SpriteBatch.DrawString(Globals.GetPixelFont(), Spellbook.traitsMemory[i].ToString(), new(8 + l * 20, Globals.NonLerp(-16, 134, openingTimer )), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.92f);
                        Globals.SpriteBatch.Draw(Globals.Content.Load<Texture2D>("traiticons"), new(13 + l * 20, Globals.NonLerp(-16, 135, openingTimer )), new Rectangle(i * 7, 0, 7, 7), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.92f);

                        l++;
                    }
                }


                Globals.SpriteBatch.DrawString(Globals.GetPixelFont(), "PRIMARY", new(8, Globals.NonLerp(-48, 6, openingTimer )), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.92f);

                Globals.SpriteBatch.DrawString(Globals.GetPixelFont(), "SECONDARY", new(8, Globals.NonLerp(-32, 54, openingTimer )), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.92f);

                Globals.SpriteBatch.DrawString(Globals.GetPixelFont(), "MEMORY", new(8, Globals.NonLerp(-16, 102, openingTimer )), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.92f);

                Globals.SpriteBatch.Draw(Globals.GetBlankTexture(), new Rectangle(0, 0, 320, (int)Globals.NonLerp(0, 160, openingTimer )), null, Color.Black * 0.85f, 0f, Vector2.Zero, SpriteEffects.None, 0.91f);

                Globals.SpriteBatch.Draw(Globals.GetBlankTexture(), new Rectangle(0, (int)Globals.NonLerp(0, 160, openingTimer ), 320, 1), Color.White);
            }

            base.Draw();
        }

        public static void AddSpell(SpellbookSpell spell)
        {
            if (spell.index == 1)
            {
                spellsPrimary.Add(spell);
                UIManager.AddElement(spell);
            }

            if (spell.index == 2)
            {
                spellsSecondary.Add(spell);
                UIManager.AddElement(spell);
            }

            if (spell.index == 3)
            {
                spellsMemory.Add(spell);
                UIManager.AddElement(spell);
            }
        }


        public static void RemoveSpell(Spell spell)
        {
            if (spell.index == 1)
            {
                foreach (SpellbookSpell i in spellsPrimary.ToList())
                {
                    if (i.spell == spell)
                    {
                        spellsPrimary.Remove(i);
                        UIManager.RemoveElement(i);
                    }
                }
            }

            if (spell.index == 2)
            {
                foreach (SpellbookSpell i in spellsSecondary.ToList())
                {
                    if (i.spell == spell)
                    {
                        spellsSecondary.Remove(i);
                        UIManager.RemoveElement(i);
                    }
                }
            }

            if (spell.index == 3)
            {
                foreach (SpellbookSpell i in spellsMemory.ToList())
                {
                    if (i.spell == spell)
                    {
                        spellsMemory.Remove(i);
                        UIManager.RemoveElement(i);
                    }
                }
            }
        }

        public static int GetFirstEmptyPosIndex(int index)
        {
            List<int> position = [];

            if (index == 1)
            {
                for (int i = 0; i < Spellbook.spellCountPrimary; i++)
                {
                    position.Add(i);
                }

                for (int i = 0; i < spellsPrimary.Count; i++)
                {
                    position.Remove(spellsPrimary[i].posIndex);
                }
            }

            if (index == 2)
            {
                for (int i = 0; i < Spellbook.spellCountSecondary; i++)
                {
                    position.Add(i);
                }

                for (int i = 0; i < spellsSecondary.Count; i++)
                {
                    position.Remove(spellsSecondary[i].posIndex);
                }
            }

            if (index == 3)
            {
                for (int i = 0; i < Spellbook.spellMemoryMax; i++)
                {
                    position.Add(i);
                }

                for (int i = 0; i < spellsMemory.Count; i++)
                {
                    position.Remove(spellsMemory[i].posIndex);
                }
            }

            if (position.Count != 0)
            {
                return position.Min();
            }    

            return 0;
        }

        public static void Level()
        {
            spellsPrimaryBorders.Add(new SpellbookSpellBorder(spellsPrimaryBorders.Count, 1, Globals.Content.Load<Texture2D>("ui2"), new(-100, -100)));
            UIManager.AddElement(spellsPrimaryBorders[spellsPrimaryBorders.Count - 1]);

            spellsSecondaryBorders.Add(new SpellbookSpellBorder(spellsSecondaryBorders.Count, 1, Globals.Content.Load<Texture2D>("ui2"), new(-100, -100)));
            UIManager.AddElement(spellsSecondaryBorders[spellsSecondaryBorders.Count - 1]);
        }
    }

    public class SpellbookSpell : Button
    {
        private TextBox spellText;
        public Spell spell;

        private bool grabbed;
        private Vector2 grabOffset;

        public int index;
        public int posIndex;
        public SpellbookSpell(int posIndex, int index, Spell spell, Texture2D texture, Vector2 position) : base(texture, position)
        {
            this.posIndex = posIndex;
            this.index = index;
            this.spell = spell;

            spellText = new TextBox(0.992f, Globals.GetPixelFont(), spell.description, InputManager.MousePosition, 85);

            spellText.text.Insert(0, " ");
            spellText.text.Insert(0, spell.name);
        }

        public override void Update()
        {
            UpdateButtonHitbox();

            if (new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height).Contains(InputManager.MousePosition) && !grabbed)
            {
                mouseHovering = true;

                if (InputManager.Clicked)
                {
                    Clicked();
                }
            }
            else
            {
                mouseHovering = false;
            }

            if (mouseHovering)
            {
                spellText.Update();
            }

            if (grabbed && InputManager.LeftMouseDown)
            {
                position = new Vector2((int)InputManager.MousePosition.X, (int)InputManager.MousePosition.Y) - new Vector2((int)grabOffset.X, (int)grabOffset.Y);
            }

            else if (grabbed && !InputManager.LeftMouseDown)
            {
                position = InputManager.MousePosition - grabOffset;

                for (int i = 0; i < SpellbookUI.spellsMemoryBorders.Count; i++)
                {
                    if (new Rectangle((int)SpellbookUI.spellsMemoryBorders[i].position.X, (int)SpellbookUI.spellsMemoryBorders[i].position.Y, SpellbookUI.spellsMemoryBorders[i].texture.Width, SpellbookUI.spellsMemoryBorders[i].texture.Height).Contains(InputManager.MousePosition))
                    {
                        if (index == 1)
                        {
                            for (int j = 0; j < SpellbookUI.spellsMemory.Count; j++)
                            {
                                if (SpellbookUI.spellsMemoryBorders[i].posIndex == SpellbookUI.spellsMemory[j].posIndex)
                                {
                                    SpellbookUI.spellsMemory[j].index = index;
                                    SpellbookUI.spellsMemory[j].posIndex = posIndex;

                                    Spellbook.AddSpellPrimary(SpellbookUI.spellsMemory[j].spell);
                                    Spellbook.RemoveSpellMemory(SpellbookUI.spellsMemory[j].spell);

                                    SpellbookUI.spellsPrimary.Add(SpellbookUI.spellsMemory[j]);
                                    SpellbookUI.spellsMemory.Remove(SpellbookUI.spellsMemory[j]);
                                }
                            }

                            posIndex = SpellbookUI.spellsMemoryBorders[i].posIndex;

                            index = 3;

                            Spellbook.AddSpellMemory(spell);
                            Spellbook.RemoveSpellPrimary(spell);

                            SpellbookUI.spellsMemory.Add(this);
                            SpellbookUI.spellsPrimary.Remove(this);
                        }

                        if (index == 2)
                        {

                            for (int j = 0; j < SpellbookUI.spellsMemory.Count; j++)
                            {
                                if (SpellbookUI.spellsMemoryBorders[i].posIndex == SpellbookUI.spellsMemory[j].posIndex)
                                {
                                    SpellbookUI.spellsMemory[j].index = index;
                                    SpellbookUI.spellsMemory[j].posIndex = posIndex;

                                    Spellbook.AddSpellSecondary(SpellbookUI.spellsMemory[j].spell);
                                    Spellbook.RemoveSpellMemory(SpellbookUI.spellsMemory[j].spell);

                                    SpellbookUI.spellsSecondary.Add(SpellbookUI.spellsMemory[j]);
                                    SpellbookUI.spellsMemory.Remove(SpellbookUI.spellsMemory[j]);
                                }
                            }

                            posIndex = SpellbookUI.spellsMemoryBorders[i].posIndex;

                            index = 3;

                            Spellbook.AddSpellMemory(spell);
                            Spellbook.RemoveSpellSecondary(spell);

                            SpellbookUI.spellsMemory.Add(this);
                            SpellbookUI.spellsSecondary.Remove(this);
                        }

                        if (index == 3)
                        {
                            for (int j = 0; j < SpellbookUI.spellsMemory.Count; j++)
                            {
                                if (SpellbookUI.spellsMemoryBorders[i].posIndex == SpellbookUI.spellsMemory[j].posIndex)
                                {
                                    SpellbookUI.spellsMemory[j].posIndex = posIndex;
                                }
                            }

                            posIndex = SpellbookUI.spellsMemoryBorders[i].posIndex;
                        }
                    }
                }

                for (int i = 0; i < SpellbookUI.spellsPrimaryBorders.Count; i++)
                {
                    if (new Rectangle((int)SpellbookUI.spellsPrimaryBorders[i].position.X, (int)SpellbookUI.spellsPrimaryBorders[i].position.Y, SpellbookUI.spellsPrimaryBorders[i].texture.Width, SpellbookUI.spellsPrimaryBorders[i].texture.Height).Contains(InputManager.MousePosition))
                    {
                        if (index == 1)
                        {
                            for (int j = 0; j < SpellbookUI.spellsPrimary.Count; j++)
                            {
                                if (SpellbookUI.spellsPrimaryBorders[i].posIndex == SpellbookUI.spellsPrimary[j].posIndex)
                                {
                                    SpellbookUI.spellsPrimary[j].posIndex = posIndex;
                                }
                            }

                            posIndex = SpellbookUI.spellsPrimaryBorders[i].posIndex;

                            Spellbook.UpdateSpellPositions(1);
                        }

                        if (index == 2)
                        {
                            for (int j = 0; j < SpellbookUI.spellsPrimary.Count; j++)
                            {
                                if (SpellbookUI.spellsPrimaryBorders[i].posIndex == SpellbookUI.spellsPrimary[j].posIndex)
                                {
                                    SpellbookUI.spellsPrimary[j].index = index;
                                    SpellbookUI.spellsPrimary[j].posIndex = posIndex;

                                    Spellbook.AddSpellSecondary(SpellbookUI.spellsPrimary[j].spell);
                                    Spellbook.RemoveSpellPrimary(SpellbookUI.spellsPrimary[j].spell);

                                    SpellbookUI.spellsSecondary.Add(SpellbookUI.spellsPrimary[j]);
                                    SpellbookUI.spellsPrimary.Remove(SpellbookUI.spellsPrimary[j]);
                                }
                            }

                            posIndex = SpellbookUI.spellsPrimaryBorders[i].posIndex;

                            index = 1;

                            Spellbook.AddSpellPrimary(spell);
                            Spellbook.RemoveSpellSecondary(spell);

                            SpellbookUI.spellsPrimary.Add(this);
                            SpellbookUI.spellsSecondary.Remove(this);
                        }

                        if (index == 3)
                        {
                            for (int j = 0; j < SpellbookUI.spellsPrimary.Count; j++)
                            {
                                if (SpellbookUI.spellsPrimaryBorders[i].posIndex == SpellbookUI.spellsPrimary[j].posIndex)
                                {
                                    SpellbookUI.spellsPrimary[j].index = index;
                                    SpellbookUI.spellsPrimary[j].posIndex = posIndex;

                                    Spellbook.AddSpellMemory(SpellbookUI.spellsPrimary[j].spell);
                                    Spellbook.RemoveSpellPrimary(SpellbookUI.spellsPrimary[j].spell);

                                    SpellbookUI.spellsMemory.Add(SpellbookUI.spellsPrimary[j]);
                                    SpellbookUI.spellsPrimary.Remove(SpellbookUI.spellsPrimary[j]);
                                }
                            }

                            posIndex = SpellbookUI.spellsPrimaryBorders[i].posIndex;

                            index = 1;

                            Spellbook.AddSpellPrimary(spell);
                            Spellbook.RemoveSpellMemory(spell);

                            SpellbookUI.spellsPrimary.Add(this);
                            SpellbookUI.spellsMemory.Remove(this);
                        }
                    }
                }

                for (int i = 0; i < SpellbookUI.spellsSecondaryBorders.Count; i++)
                {
                    if (new Rectangle((int)SpellbookUI.spellsSecondaryBorders[i].position.X, (int)SpellbookUI.spellsSecondaryBorders[i].position.Y, SpellbookUI.spellsSecondaryBorders[i].texture.Width, SpellbookUI.spellsSecondaryBorders[i].texture.Height).Contains(InputManager.MousePosition))
                    {
                        if (index == 1)
                        {
                            for (int j = 0; j < SpellbookUI.spellsSecondary.Count; j++)
                            {
                                if (SpellbookUI.spellsSecondaryBorders[i].posIndex == SpellbookUI.spellsSecondary[j].posIndex)
                                {
                                    SpellbookUI.spellsSecondary[j].index = index;
                                    SpellbookUI.spellsSecondary[j].posIndex = posIndex;

                                    Spellbook.AddSpellPrimary(SpellbookUI.spellsSecondary[j].spell);
                                    Spellbook.RemoveSpellSecondary(SpellbookUI.spellsSecondary[j].spell);

                                    SpellbookUI.spellsPrimary.Add(SpellbookUI.spellsSecondary[j]);
                                    SpellbookUI.spellsSecondary.Remove(SpellbookUI.spellsSecondary[j]);
                                }
                            }

                            posIndex = SpellbookUI.spellsSecondaryBorders[i].posIndex;

                            index = 2;

                            Spellbook.AddSpellSecondary(spell);
                            Spellbook.RemoveSpellPrimary(spell);

                            SpellbookUI.spellsSecondary.Add(this);
                            SpellbookUI.spellsPrimary.Remove(this);
                        }

                        if (index == 2)
                        {
                            for (int j = 0; j < SpellbookUI.spellsSecondary.Count; j++)
                            {
                                if (SpellbookUI.spellsSecondaryBorders[i].posIndex == SpellbookUI.spellsSecondary[j].posIndex)
                                {
                                    SpellbookUI.spellsSecondary[j].posIndex = posIndex;
                                }
                            }

                            posIndex = SpellbookUI.spellsSecondaryBorders[i].posIndex;

                            Spellbook.UpdateSpellPositions(2);
                        }

                        if (index == 3)
                        {
                            for (int j = 0; j < SpellbookUI.spellsSecondary.Count; j++)
                            {
                                if (SpellbookUI.spellsSecondaryBorders[i].posIndex == SpellbookUI.spellsSecondary[j].posIndex)
                                {
                                    SpellbookUI.spellsSecondary[j].index = index;
                                    SpellbookUI.spellsSecondary[j].posIndex = posIndex;

                                    Spellbook.AddSpellMemory(SpellbookUI.spellsSecondary[j].spell);
                                    Spellbook.RemoveSpellSecondary(SpellbookUI.spellsSecondary[j].spell);

                                    SpellbookUI.spellsMemory.Add(SpellbookUI.spellsSecondary[j]);
                                    SpellbookUI.spellsSecondary.Remove(SpellbookUI.spellsSecondary[j]);
                                }
                            }

                            posIndex = SpellbookUI.spellsSecondaryBorders[i].posIndex;

                            index = 2;

                            Spellbook.AddSpellSecondary(spell);
                            Spellbook.RemoveSpellMemory(spell);

                            SpellbookUI.spellsSecondary.Add(this);
                            SpellbookUI.spellsMemory.Remove(this);
                        }
                    }
                }

                grabbed = false;
            }

            spellText.position = new((int)InputManager.MousePosition.X, (int)InputManager.MousePosition.Y);
        }
        public override void Draw()
        {
            if (mouseHovering)
            {
                spellText.Draw();

                Globals.SpriteBatch.Draw(Globals.GetBlankTexture(), new Rectangle((int)position.X, (int)position.Y, (int)texture.Width, (int)texture.Height), null, Color.White * 0.5f, 0f, Vector2.Zero, SpriteEffects.None, 0.9925f);

                for (int i = 0; i < spell.spellTraits.Count; i++)
                {
                    Globals.SpriteBatch.Draw(Globals.Content.Load<Texture2D>("traiticons"), new((int)InputManager.MousePosition.X + spellText.longestString - ((i + 1) * 9), (int)InputManager.MousePosition.Y + 1), new Rectangle((spell.spellTraits[i] - 1) * 7, 0, 7, 7), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.993f);

                    if (i == spell.spellTraits.Count - 1)
                    {
                        string text = "";
                        
                        for (int j = 0; j < spell.rank; j++)
                        {
                            text += "*";
                        }
                        Globals.SpriteBatch.DrawString(Globals.GetPixelFont(), text, new((int)InputManager.MousePosition.X + Globals.GetPixelFont().MeasureString(spell.name).X + 3, (int)InputManager.MousePosition.Y + 1), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.993f);
                    }
                }
            }

            if (grabbed)
            {
                for (int i = 0; i < SpellbookUI.spellsPrimaryBorders.Count; i++)
                {
                    if (new Rectangle((int)SpellbookUI.spellsPrimaryBorders[i].position.X, (int)SpellbookUI.spellsPrimaryBorders[i].position.Y, SpellbookUI.spellsPrimaryBorders[i].texture.Width, SpellbookUI.spellsPrimaryBorders[i].texture.Height).Contains(InputManager.MousePosition))
                    {
                        DrawHoverGlow(1, i);
                    }
                }

                for (int i = 0; i < SpellbookUI.spellsSecondaryBorders.Count; i++)
                {
                    if (new Rectangle((int)SpellbookUI.spellsSecondaryBorders[i].position.X, (int)SpellbookUI.spellsSecondaryBorders[i].position.Y, SpellbookUI.spellsSecondaryBorders[i].texture.Width, SpellbookUI.spellsSecondaryBorders[i].texture.Height).Contains(InputManager.MousePosition))
                    {
                        DrawHoverGlow(2, i);
                    }
                }

                for (int i = 0; i < SpellbookUI.spellsMemoryBorders.Count; i++)
                {
                    if (new Rectangle((int)SpellbookUI.spellsMemoryBorders[i].position.X, (int)SpellbookUI.spellsMemoryBorders[i].position.Y, SpellbookUI.spellsMemoryBorders[i].texture.Width, SpellbookUI.spellsMemoryBorders[i].texture.Height).Contains(InputManager.MousePosition))
                    {
                        DrawHoverGlow(3, i);
                    }
                }
            }

            Globals.SpriteBatch.Draw(texture, position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.991f);
        }

        public override void Clicked()
        {
            grabOffset = InputManager.MousePosition - position;

            grabbed = true;

            base.Clicked();
        }

        private void DrawHoverGlow(int index, int posIndex)
        {
            Globals.SpriteBatch.Draw(Globals.GetBlankTexture(), new Rectangle((int)8 + posIndex * 20, index == 1 ? 16 : index == 2 ? 16 * 4 : 16 * 7, 16, 16), null, Color.White * 0.5f, 0f, Vector2.Zero, SpriteEffects.None, 0.9925f);
        }
    }

    public class SpellbookSpellBorder : UIElement
    {
        public int index;
        public int posIndex;
        public SpellbookSpellBorder(int posIndex, int index, Texture2D texture, Vector2 position) : base(texture, position)
        {
            this.posIndex = posIndex;
            this.index = index;
        }

        public override void Draw()
        {
            Globals.SpriteBatch.Draw(texture, position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.991f);
        }
    }
}
