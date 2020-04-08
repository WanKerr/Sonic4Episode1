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
    public class GMS_GMK_PULLEY_MANAGER
    {
        public readonly AppMain.GMS_GMK_PULLEY_REGISTER[] reg = AppMain.New<AppMain.GMS_GMK_PULLEY_REGISTER>(32);
        public uint tex_id;
        public AppMain.NNS_TEXLIST texlist;
        public uint num;
        public uint rsv;
    }
}
