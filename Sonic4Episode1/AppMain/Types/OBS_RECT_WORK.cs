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
    public class OBS_RECT_WORK : AppMain.IClearable
    {
        public readonly AppMain.OBS_RECT rect = new AppMain.OBS_RECT();
        public uint flag;
        public AppMain.OBS_OBJECT_WORK parent_obj;
        public AppMain.OBS_RECT_WORK_Delegate1 ppHit;
        public AppMain.OBS_RECT_WORK_Delegate1 ppDef;
        public AppMain.OBS_RECT_WORK_Delegate2 ppCheck;
        public short hit_power;
        public short def_power;
        public ushort hit_flag;
        public ushort def_flag;
        public byte group_no;
        public byte target_g_flag;
        public uint attr_flag;
        public uint user_data;
        public object pDataWork;

        public void Clear()
        {
            this.rect.Clear();
            this.flag = 0U;
            this.parent_obj = (AppMain.OBS_OBJECT_WORK)null;
            this.ppHit = this.ppDef = (AppMain.OBS_RECT_WORK_Delegate1)null;
            this.ppCheck = (AppMain.OBS_RECT_WORK_Delegate2)null;
            this.hit_power = this.def_power = (short)0;
            this.hit_flag = this.def_flag = (ushort)0;
            this.group_no = this.target_g_flag = (byte)0;
            this.attr_flag = this.user_data = 0U;
            this.pDataWork = (object)null;
        }
    }
}
