using HYPERMAGE.Helpers;
using HYPERMAGE.Managers;
using HYPERMAGE.Particles;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HYPERMAGE.UI.UIElements
{
    public class Upgrade : Button
    {
        public int id;
        public float opacity;
        public float layerDepth = 1f;

        public List<string> description = [];
        public List<string> name = [];

        public static List<int> usedIDs = [];
        public static bool choiceMade = false;

        public bool active = false;
        public Upgrade(Vector2 position, Texture2D texture) : base(texture, position)
        {
            choiceMade = false;

            this.texture = texture;
            this.position = position;

            Reroll();
        }

        public void Reroll()
        {
            usedIDs.Remove(id);

            id = Globals.Random.Next(1, UpgradeManager.totalUpgrades);

            if (usedIDs.Contains(id) || UpgradeManager.usedUpgrades.Contains(id))
            {
                Reroll();
                return;
            }

            usedIDs.Add(id);

            description = Globals.LineBreakText(UpgradeManager.GetUpgradeDescription(id), Globals.GetPixelFont(), 60, true);
            name = Globals.LineBreakText(UpgradeManager.GetUpgradeName(id), Globals.GetPixelFont(), 60, true);
        }

        public override void Update()
        {
            if (opacity >= 1f)
            {
                active = true;
            }

            base.Update();
        }
        public override void Draw()
        {
            for (int i = 0; i < name.Count; i++)
            {
                Globals.SpriteBatch.DrawString(Globals.GetPixelFont(), name[i], position + new Vector2(4, 8 + i * 10), Color.White * opacity, 0f, Vector2.Zero, 1f, SpriteEffects.None, layerDepth);
            }

            for (int i = 0; i < description.Count; i++)
            {
                Globals.SpriteBatch.DrawString(Globals.GetPixelFont(), description[i], position + new Vector2(4, 16 + (i + name.Count) * 10), Color.White * opacity, 0f, Vector2.Zero, 1f, SpriteEffects.None, layerDepth);
            }

            Globals.SpriteBatch.Draw(texture, position, null, Color.White * opacity, 0f, Vector2.Zero, 1f, SpriteEffects.None, layerDepth);

            if (mouseHovering)
            {
                Globals.SpriteBatch.Draw(Globals.GetBlankTexture(), new Rectangle((int)position.X - 1, (int)position.Y - 1, (int)texture.Width + 2, (int)texture.Height + 2), Color.White * opacity);
            }
        }

        public override void Clicked()
        {
            if (choiceMade || !active) return;

            choiceMade = true;

            for (int i = 0; i < 50; i++)
            {
                Vector2 pos = position + new Vector2(Globals.RandomFloat(0, 70), Globals.RandomFloat(0, 120));

                ParticleData spawnParticleData = new()
                {
                    opacityStart = 1f,
                    opacityEnd = 1f,
                    sizeStart = 6 - Globals.Random.Next(6),
                    sizeEnd = 0,
                    colorStart = Color.White,
                    colorEnd = Color.White,
                    velocity = new Vector2(position.X + texture.Width / 2, position.Y + texture.Height / 2).DirectionTo(pos) * 200f,
                    lifespan = Globals.RandomFloat(0.2f, 0.8f),
                    rotationSpeed = Globals.RandomFloat(-0.5f, 0.5f)
                };

                Particle spawnParticle = new(pos, spawnParticleData);
                ParticleManager.AddParticle(spawnParticle);
            }

            GameManager.AddScreenShake(0.2f, 15f);
            GameManager.AddAbberationPowerForce(500, 50);

            SoundManager.PlaySound(Globals.Content.Load<SoundEffect>("select"), 1f, 0f, 0f);

            UpgradeManager.AddUpgrade(id);
            base.Clicked();
        }
    }
}
