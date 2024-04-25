using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Uno.Foundation;
using Uno.Foundation.Interop;
using gs;

namespace Microsoft.Xna.Framework.Media
{
    public class WasmMediaQueue
    {
        public Song ActiveSong { get; set; }
        public int ActiveSongIndex { get; set; }
    }

    public class WasmMediaPlayer
    {
        public static float Volume
        {
            get => float.Parse(WebAssemblyRuntime.InvokeJS($"window.WASM_MediaPlayer.getVolume()"));
            set => WebAssemblyRuntime.InvokeJS($"window.WASM_MediaPlayer.setVolume({value})");
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
                //Console.WriteLine(song.Duration.TotalSeconds);
                WebAssemblyRuntime.InvokeJS($"window.WASM_MediaPlayer.play(\"{WebAssemblyRuntime.EscapeJs(path)}\", {song.Duration.TotalSeconds})");
            }
            else
            {
                WebAssemblyRuntime.InvokeJS($"window.WASM_MediaPlayer.play(\"{WebAssemblyRuntime.EscapeJs(path)}\")");
            }
        }

        // public static void LoadSong(Song song)
        // {
        //     WebAssemblyRuntime.InvokeJS($"window.WASM_MediaPlayer.loadSong(\"{WebAssemblyRuntime.EscapeJs(song.EXT_FilePath)}\")");
        // }

        public static void Pause()
        {
        }

        public static void Resume()
        {
        }

        public static void Stop()
        {
            WebAssemblyRuntime.InvokeJS($"window.WASM_MediaPlayer.stop()");
        }

        public static void ChangeState(int state)
        {
            var mediaState = (MediaState) state;
            State = mediaState;
            MediaStateChanged?.Invoke(null, EventArgs.Empty);
        }
    }
}

namespace Microsoft.Xna.Framework.Input
{
    public class InputInjector
    {
        public static void Init()
        {
            WebAssemblyRuntime.InvokeJS("WASM_initInput()");
        }
    }

    public class WasmMouse
    {
        private static MouseState _mouseState
            = new();

        public static void InjectMouseUp(float x, float y, int button)
        {
            _mouseState = new MouseState(
                (int) x,
                (int) y,
                _mouseState.ScrollWheelValue,
                button == 0 ? ButtonState.Released : _mouseState.LeftButton,
                button == 1 ? ButtonState.Released : _mouseState.MiddleButton,
                button == 2 ? ButtonState.Released : _mouseState.RightButton,
                ButtonState.Released,
                ButtonState.Released);
        }

        public static void InjectMouseDown(float x, float y, int button)
        {
            _mouseState = new MouseState(
                (int) x,
                (int) y,
                _mouseState.ScrollWheelValue,
                button == 0 ? ButtonState.Pressed : _mouseState.LeftButton,
                button == 1 ? ButtonState.Pressed : _mouseState.MiddleButton,
                button == 2 ? ButtonState.Pressed : _mouseState.RightButton,
                ButtonState.Released,
                ButtonState.Released);
        }

        public static void InjectMouseMove(float x, float y)
        {
            _mouseState = new MouseState(
                (int) x,
                (int) y,
                _mouseState.ScrollWheelValue,
                _mouseState.LeftButton,
                _mouseState.MiddleButton,
                _mouseState.RightButton,
                ButtonState.Released,
                ButtonState.Released);
        }

        public static MouseState GetState() => _mouseState;
    }

    public class WasmKeyboard
    {
        private static HashSet<Keys> _keys = new();
        private static KeyboardState _keybordState = new();

        public static void InjectKeyDown(int jsKeyCode)
        {
            var key = AdaptKeyCode(jsKeyCode);
            _keys.Add(key);

            _keybordState = new KeyboardState(_keys);
        }

        public static void InjectKeyUp(int jsKeyCode)
        {
            var key = AdaptKeyCode(jsKeyCode);
            _keys.Remove(key);

            _keybordState = new KeyboardState(_keys);
        }

        private static Keys AdaptKeyCode(int jsKeyCode)
        {
            return (Keys) jsKeyCode; // should just work?
        }

        public static KeyboardState GetState() => _keybordState;
    }
}