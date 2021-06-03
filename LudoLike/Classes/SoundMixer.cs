using System;
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

        private SoundMixer(int channels)
        {
            _audioChannels = new MediaPlayer[channels];
            for(int n = 0; n < _audioChannels.Length; ++n)
            {
                _audioChannels[n] = new MediaPlayer()
                {
                    AutoPlay = false
                };
            }
        }

        public static void PlaySound(MediaSource soundFile, SoundChannels soundChannel = SoundChannels.sfx1)
        {
            MediaPlayer player = SoundMixerInstance._audioChannels[(int)soundChannel];
            player.Source = soundFile;
            player.Play();
        }
    }

    public enum SoundChannels
    {
        music, sfx1, sfx2
    }
}
