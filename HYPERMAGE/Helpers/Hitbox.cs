using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HYPERMAGE.Helpers
{
    public class Hitbox
    {
        public Vector2 origin;
        public Vector2 position;
        public Vector2 offset;
        public float size;
        public Hitbox(Vector2 pos, float size, Vector2 offset, Vector2 origin)
        {
            this.position = pos;
            this.size = size;
            this.offset = offset;
            this.origin = origin;
        }

        public bool Intersects(Hitbox other)
        {
            if (position.Distance(other.position) < size + other.size)
            {
                return true;
            }

            return false;
        }

        public void SetRotation(float angle)
        {
            position = position + offset;

            float s = (float)Math.Sin(angle);
            float c = (float)Math.Cos(angle);

            position.X -= origin.X;
            position.Y -= origin.Y;

            float xnew = position.X * c - position.Y * s;
            float ynew = position.X * s + position.Y * c;

            position.X = xnew + origin.X;
            position.Y = ynew + origin.Y;
        }
    }
}
