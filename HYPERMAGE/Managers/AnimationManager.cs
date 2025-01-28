/*
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

        public void Draw(Vector2 position)
        {
            anims[lastKey].Draw(position);
        }
    }
}
*/