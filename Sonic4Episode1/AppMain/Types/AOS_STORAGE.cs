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
    public class AOS_STORAGE
    {
        public bool initialized;
        public int state;
        public int error;
        public bool save_success;
        public byte[] save_buf;
        public uint save_size;
        public bool load_success;
        public byte[] load_buf;
        public uint load_size;
        public bool del_success;
        public AppMain.AMS_TCB tcb;
        internal Sonic4Save save;

        public AOS_STORAGE(bool init, int state, int error)
        {
            this.initialized = init;
            this.state = state;
            this.error = error;
        }
    }
}
