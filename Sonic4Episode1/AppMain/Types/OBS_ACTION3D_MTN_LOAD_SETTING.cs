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
    public class OBS_ACTION3D_MTN_LOAD_SETTING
    {
        public bool enable;
        public bool marge;
        public AppMain.OBS_DATA_WORK data_work;
        public string filename;
        public int index;
        public AppMain.AMS_AMB_HEADER archive;

        public void Clear()
        {
            this.enable = false;
            this.marge = false;
            this.data_work = (AppMain.OBS_DATA_WORK)null;
            this.filename = "";
            this.index = 0;
            this.archive = (AppMain.AMS_AMB_HEADER)null;
        }
    }
}
