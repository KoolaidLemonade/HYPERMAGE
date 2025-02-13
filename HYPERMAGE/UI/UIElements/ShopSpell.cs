using HYPERMAGE.Helpers;
using HYPERMAGE.Managers;
using HYPERMAGE.Models;
using HYPERMAGE.Particles;
using HYPERMAGE.Spells;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HYPERMAGE.UI.UIElements
{
    public class ShopSpell : Button
    {
        public Spell spell;

        private new Color color;

        private TextBox spellText;

        private float timer;
        private float drawOffset;

        public Vector2 originalPos;

        public float layerDepth = 0.93f;
        public ShopSpell(SpriteFont spriteFont, Spell spell, Texture2D texture, Vector2 position) : base(texture, position)
        {
            this.spell = spell;

            spellText = new TextBox(layerDepth + 0.01f, spriteFont, spell.description, InputManager.MousePosition, 85);

            spellText.text.Insert(0, " ");
            spellText.text.Insert(0, spell.name);

            color = Spellbook.GetRarityColor(spell);

            originalPos = position;
        }
        public override void Update()
        {
            UpdateButtonHitbox();

            timer += 0.015f;

            drawOffset = (float)Math.Sin(timer) * 2.5f;

            if (new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height).Contains(InputManager.MousePosition))
            {
                if (InputManager.Clicked)
                {
                    Clicked();
                }

                mouseHovering = true;
            }
            else
            {
                mouseHovering = false;
            }

            if (mouseHovering)
            {
                spellText.Update();
            }

            spellText.position = new((int)InputManager.MousePosition.X, (int)InputManager.MousePosition.Y);
        }
        public override void Draw()
        {
            if (mouseHovering)
            {
                spellText.Draw();

                Globals.SpriteBatch.Draw(Globals.GetBlankTexture(), new Rectangle((int)position.X, (int)((int)position.Y + (int)drawOffset), (int)texture.Width, (int)texture.Height), null, Color.White * 0.5f, 0f, Vector2.Zero, SpriteEffects.None, layerDepth - 0.01f);

                for (int i = 0; i < spell.spellTraits.Count; i++)
                {
                    Globals.SpriteBatch.Draw(Globals.Content.Load<Texture2D>("traiticons"), new((int)InputManager.MousePosition.X + spellText.longestString - ((i + 1) * 9), (int)InputManager.MousePosition.Y + 1), new Rectangle((spell.spellTraits[i] - 1) * 7, 0, 7, 7), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, layerDepth + 0.01f);

                    if (i == spell.spellTraits.Count - 1)
                    {
                        Globals.SpriteBatch.DrawString(Globals.GetPixelFont(), spell.cost.ToString(), new((int)InputManager.MousePosition.X + Globals.GetPixelFont().MeasureString(spell.name).X + 6, (int)InputManager.MousePosition.Y), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, layerDepth + 0.01f);
                        Globals.SpriteBatch.Draw(Globals.Content.Load<Texture2D>("mana"), new((int)InputManager.MousePosition.X + Globals.GetPixelFont().MeasureString(spell.name).X + 6 + Globals.GetPixelFont().MeasureString(spell.cost.ToString()).X, (int)InputManager.MousePosition.Y + 1), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, layerDepth + 0.01f);
                    }
                }
            }

            else
            {

            }

            Globals.SpriteBatch.Draw(spell.icon, (new Vector2((int)position.X, (int)position.Y) + Vector2.One) + new Vector2(0, (int)drawOffset), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, layerDepth - 0.01f);

            Globals.SpriteBatch.Draw(texture, new Vector2((int)position.X, (int)position.Y) + new Vector2(0, (int)drawOffset), null, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, layerDepth - 0.01f);
        }

        public override void Clicked()
        {
            if (GameManager.GetPlayer().mana >= spell.cost)
            {
                if (Spellbook.spellMemory.Count >= Spellbook.spellMemoryMax)
                {
                    return;
                }

                for (int i = 0; i < 20; i++)
                {
                    ParticleData particleData = new()
                    {
                        sizeStart = 2.5f,
                        sizeEnd = 0,
                        colorStart = color,
                        colorEnd = color,
                        velocity = new Vector2(Globals.RandomFloat(-30, 30), Globals.RandomFloat(-30, 30)),
                        lifespan = Globals.RandomFloat(0, 1f),
                        rotationSpeed = 1f,
                        resistance = 1f
                    };

                    Particle particle = new(new(position.X + Globals.RandomFloat(0, texture.Width), position.Y + Globals.RandomFloat(0, texture.Height)), particleData);
                    ParticleManager.AddParticle(particle);
                }

                GameManager.GetPlayer().mana -= spell.cost;

                ShopManager.RemoveSpell(this);

                Spellbook.AddSpellMemory(spell);

                SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("ding"), 0.5f, 0.1f, 0f);

            }

            base.Clicked();
        }
    }
}
