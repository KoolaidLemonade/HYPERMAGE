using HYPERMAGE.Helpers;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HYPERMAGE.Managers
{
    public static class SoundManager
    {
        public static float musicVolume = 1f;
        public static float soundVolume = 1f;
        public static float globalVolume = 0.01f;

        public static List<SoundEffectInstance> sounds = [];
        public static void PlaySound(SoundEffect effect, float volume, float pitch, float pan)
        {
            SoundEffectInstance sound = effect.CreateInstance();

            sound.Volume = soundVolume * globalVolume * volume;
            sound.Pitch = pitch;
            sound.Pan = pan;

            sounds.Add(sound);
            sound.Play();
        }

        public static void ClearSounds()
        {
            PlaySound(Globals.Content.Load<SoundEffect>("hit"), 0f, 0f, 0f);

            foreach (var sound in sounds)
            {
                sound.Stop();
                sound.Dispose();
            }    

            sounds.Clear();
        }
        public static void PlaySong(Song song, float volume)
        {
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = musicVolume * globalVolume * volume;
            MediaPlayer.Play(song);
        }

        public static void ClearSong()
        {
            MediaPlayer.Stop();
        }
    }
}
