using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman
{
    // typ zvukového efektu
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
        // map typ efektu:resource. z ktorého sa dá prehrávať
        private readonly Dictionary<Sound, SoundEffect> sounds;

        public Audio()
        {
            sounds = new Dictionary<Sound, SoundEffect>();
        }

        // pridanie zvuku do mapy
        public void Register(Sound sound, SoundEffect soundEffect)
        {
            sounds[sound] = soundEffect;
        }

        // prehratie zvuku z mapy poďla typu    
        public void Play(Sound sound)
        {
            sounds[sound].Play();
        }
    }
}
