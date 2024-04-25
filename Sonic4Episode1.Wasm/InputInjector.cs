using System;
using System.IO;
using System.Runtime.InteropServices.JavaScript;

namespace Microsoft.Xna.Framework.Media
{
    public class WasmMediaQueue
    {
        public Song ActiveSong { get; set; }
        public int ActiveSongIndex { get; set; }
    }

    public partial class WasmMediaPlayer
    {
        [JSImport("MediaPlayer_GetVolume", "main.js")]
        internal static partial float MediaPlayer_GetVolume();
        [JSImport("MediaPlayer_SetVolume", "main.js")]
        internal static partial void MediaPlayer_SetVolume([JSMarshalAs<JSType.Number>] float value);

        [JSImport("MediaPlayer_Play", "main.js")]
        internal static partial void MediaPlayer_Play([JSMarshalAs<JSType.String>] string path);
        [JSImport("MediaPlayer_PlayLooped", "main.js")]
        internal static partial void MediaPlayer_PlayLooped([JSMarshalAs<JSType.String>] string path, [JSMarshalAs<JSType.Number>] double loopPos);

        [JSImport("MediaPlayer_LoadSong", "main.js")]
        internal static partial void MediaPlayer_LoadSong([JSMarshalAs<JSType.String>] string path);

        [JSImport("MediaPlayer_Stop", "main.js")]
        internal static partial void MediaPlayer_Stop();

        public static float Volume
        {
            get => MediaPlayer_GetVolume();
            set => MediaPlayer_SetVolume(value);
        }

        public static bool IsRepeating { get; set; }

        public static WasmMediaQueue Queue { get; private set; } = new();

        public static event EventHandler<EventArgs> MediaStateChanged;
        public static event EventHandler<EventArgs> ActiveSongChanged;

        public static MediaState State { get; private set; }

        public static void Play(Song song, Song loop)
        {
            var path = Path.GetDirectoryName(song.EXT_FilePath) + ".ogg";
            if (loop != null)
            {
                MediaPlayer_PlayLooped(path, song.Duration.TotalSeconds);
            }
            else
            {
                MediaPlayer_Play(path);
            }
        }

        public static void LoadSong(Song song)
        {
            var path = Path.GetDirectoryName(song.EXT_FilePath) + ".ogg";
            MediaPlayer_LoadSong(path);
        }

        public static void Pause()
        {
        }

        public static void Resume()
        {
        }

        public static void Stop()
        {
            MediaPlayer_Stop();
        }

        [JSExport]
        public static void ChangeState(int state)
        {
            var mediaState = (MediaState)state;
            State = mediaState;
            MediaStateChanged?.Invoke(null, EventArgs.Empty);
        }
    }
}