using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using accel;
using dbg;
using er;
using er.web;
using gs;
using gs.backup;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using mpp;
using setting;

public partial class AppMain
{
    public class GMS_SOUND_BGM_FADE_WORK
    {
        public float start_vol;
        public float end_vol;
        public float fade_spd;
        public float now_vol;
        public int frame;
        public AppMain.GSS_SND_SCB snd_scb;
        public AppMain.GMS_SOUND_BGM_FADE_WORK next;
        public AppMain.GMS_SOUND_BGM_FADE_WORK prev;
    }
}
