using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman
{
    enum Sound
    {
        Start,
        Hurt,
        Explosion,
        Drop,
        Pickup
    }

    class Audio
    {
        private readonly Dictionary<Sound, SoundEffect> sounds;

        public Audio()
        {
            sounds = new Dictionary<Sound, SoundEffect>();
        }

        public void Register(Sound sound, SoundEffect soundEffect)
        {
            sounds[sound] = soundEffect;
        }

        public void Play(Sound sound)
        {
            sounds[sound].Play();
        }
    }
}
