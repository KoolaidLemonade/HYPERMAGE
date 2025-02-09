using HYPERMAGE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HYPERMAGE.Managers
{
    public class AnimationManager
    {
        private readonly Dictionary<object, Animation> anims = [];
        private object lastKey;

        public Animation getFirstAnim()
        {
            if (anims.Count == 0)
            {
                return null;
            }

            return anims[0];
        }
        public void AddAnimation(object key, Animation animation)
        {
            anims.Add(key, animation);
            lastKey ??= key;
        }

        public void Update(object key)
        {
            if (anims.TryGetValue(key, out Animation value))
            {
                value.Start();
                anims[key].Update();
                lastKey = key;
            }
            else
            {
                anims[lastKey].Stop();
                anims[lastKey].Reset();
            }
        }

        public void Draw(Vector2 pos, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects spriteEffect, float layerDepth)
        {
            anims[lastKey].Draw(pos, color, rotation, origin, scale, spriteEffect, layerDepth);
        }
    }
}
