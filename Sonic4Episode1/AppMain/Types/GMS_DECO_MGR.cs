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
    public class GMS_DECO_MGR : AppMain.IClearable
    {
        public int[] common_frame_motion = new int[3];
        public int[] motion_frame_loop = new int[12];
        public AppMain.MTS_TASK_TCB tcb_post;
        public bool flag_render_front;
        public bool flag_render_back;
        public AppMain.AMS_RENDER_TARGET render_target_front;
        public AppMain.AMS_RENDER_TARGET render_target_back;
        public int state_loop;
        public AppMain.GSS_SND_SE_HANDLE se_handle;

        public void Clear()
        {
            this.tcb_post = (AppMain.MTS_TASK_TCB)null;
            this.flag_render_front = this.flag_render_back = false;
            this.render_target_front = this.render_target_back = (AppMain.AMS_RENDER_TARGET)null;
            this.state_loop = 0;
            this.se_handle = (AppMain.GSS_SND_SE_HANDLE)null;
            for (int index = 0; index < this.motion_frame_loop.Length; ++index)
                this.motion_frame_loop[index] = 0;
        }
    }
}
