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
    public class GMS_CMN_FLASH_SCR_WORK : AppMain.IClearable
    {
        public AppMain.GMS_FADE_OBJ_WORK fade_obj_work;
        public uint active_flag;
        public float duration_frame;
        public float fi_frame;
        public float duration_timer;

        public void Clear()
        {
            this.fade_obj_work = (AppMain.GMS_FADE_OBJ_WORK)null;
            this.active_flag = 0U;
            this.duration_frame = this.fi_frame = this.duration_timer = 0.0f;
        }
    }
}
