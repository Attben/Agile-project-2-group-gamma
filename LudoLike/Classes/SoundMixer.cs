using System;
using System.Collections.Generic;
using Windows.Media.Core;
using Windows.Media.Playback;

namespace LudoLike.Classes
{
    /// <summary>
    /// Simple multi-source audio module.
    /// MediaPlayer objects only play a single audio source at a time,
    /// but more fully-featured sound libraries are much more difficult to use.
    /// This class is used as a workaround.
    /// </summary>
    public class SoundMixer
    {
        private static readonly SoundMixer SoundMixerInstance =
            new SoundMixer(Enum.GetNames(typeof(SoundChannels)).Length);
        private readonly MediaPlayer[] _audioChannels;
        private static readonly Random _sfxRng = new Random(); //Sound effects RNG, used to play random sounds.

        private SoundMixer(int channels)
        {
            _audioChannels = new MediaPlayer[channels];
            for(int n = 0; n < _audioChannels.Length; ++n)
            {
                _audioChannels[n] = new MediaPlayer()
                {
                    AutoPlay = false,
                    IsLoopingEnabled = (n == (int)SoundChannels.music) //Looping is enabled only on the music channel.
                };
            }
        }

        public static void PlaySound(MediaSource soundFile, SoundChannels soundChannel = SoundChannels.sfx1)
        {
            MediaPlayer player = SoundMixerInstance._audioChannels[(int)soundChannel];
            player.Source = soundFile;
            player.Play();
        }

        public static void PlayRandomSound(List<MediaSource> sounds, SoundChannels soundChannel = SoundChannels.sfx1)
        {
            PlaySound(sounds[_sfxRng.Next(sounds.Count)], soundChannel);
        }

        public static void StopPlaying(SoundChannels soundChannel)
        {
            SoundMixerInstance._audioChannels[(int)soundChannel].Source = null; //Why in the world doesn't this class have a less hacky way to stop playback?
        }
    }

    public enum SoundChannels
    {
        music, sfx1, sfx2
    }
}
