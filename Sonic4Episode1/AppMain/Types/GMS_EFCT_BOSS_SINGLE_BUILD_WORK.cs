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
    public class GMS_EFCT_BOSS_SINGLE_BUILD_WORK
    {
        public int tex_reg_id;
        public int model_reg_id;
        public AppMain.OBS_DATA_WORK ambtex_dwork;
        public AppMain.OBS_DATA_WORK texlist_dwork;
        public AppMain.OBS_DATA_WORK model_dwork;
        public AppMain.OBS_DATA_WORK object_dwork;
    }
}
