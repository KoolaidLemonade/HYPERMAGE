using HYPERMAGE.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HYPERMAGE.Particles
{
    public class TextPopup
    {
        private readonly TextPopupData data;

        private string text;
        private Vector2 position;
        private Vector2 velocity;
        private float lifespanLeft;
        private float lifespanAmount;
        private Color color;
        private float opacity;
        public bool isFinished = false;
        private float scale;
        private Vector2 origin;
        private float resistance;
        private float rotation;
        private float rotationSpeed;
        private float rotationResistance;

        private Rectangle hitbox;
        private int width;
        private int height;

        public TextPopup(Vector2 pos, TextPopupData data)
        {
            this.data = data;
            this.lifespanLeft = data.lifespan;
            lifespanAmount = 1f;
            this.position = pos;
            this.color = data.colorStart;
            this.opacity = data.opacityStart;
            this.resistance = data.resistance;
            this.velocity = data.velocity;
            this.rotation = data.rotation;
            this.rotationSpeed = data.rotationSpeed;
            this.text = data.text;
            this.rotationResistance = data.rotationResistance;

            origin = new(Globals.GetPixelFont().MeasureString(text).X / 2, Globals.GetPixelFont().MeasureString(text).Y / 2);

            width = (int)Globals.GetPixelFont().MeasureString(text).X;
            height = (int)Globals.GetPixelFont().MeasureString(text).Y;

        }
        public void Update()
        {
            hitbox = new Rectangle((int)position.X, (int)position.Y, width, height);

            lifespanLeft -= Globals.TotalSeconds;
            if (lifespanLeft <= 0f)
            {
                isFinished = true;
                return;
            }

            lifespanAmount = MathHelper.Clamp(lifespanLeft / data.lifespan, 0, 1);
            color = Color.Lerp(data.colorEnd, data.colorStart, lifespanAmount);
            opacity = MathHelper.Clamp(MathHelper.Lerp(data.opacityEnd, data.opacityStart, lifespanAmount), 0, 1);
            scale = MathHelper.Lerp(data.sizeEnd, data.sizeStart, lifespanAmount);
            position += velocity * Globals.TotalSeconds;

            velocity /= resistance;
            rotationSpeed /= rotationResistance;

            rotation += rotationSpeed;
        }

        public void Draw()
        {
            Globals.SpriteBatch.DrawString(Globals.GetPixelFont(), text, position, color * opacity, rotation, origin, scale, SpriteEffects.None, 0.7f);
        }
    }
}
