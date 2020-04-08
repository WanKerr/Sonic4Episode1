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
    public class GMS_SMSG_MGR_WORK : AppMain.IClearable
    {
        public AppMain.GMS_SMSG_2D_OBJ_WORK[] ama_2d_work = new AppMain.GMS_SMSG_2D_OBJ_WORK[AppMain.GMD_SMSG_AMA_ACT_MAX];
        public AppMain.GMS_SMSG_2D_OBJ_WORK[] ama_2d_work_act = new AppMain.GMS_SMSG_2D_OBJ_WORK[AppMain.GMD_SMSG_AMA_ACT_ACTION_MAX];
        public uint flag;
        public int timer;
        public int msg_type;
        public AppMain.pfnGMS_SMSG_MGR_WORK func;
        public float win_per;

        public void Clear()
        {
            this.flag = 0U;
            this.timer = 0;
            this.msg_type = 0;
            this.func = (AppMain.pfnGMS_SMSG_MGR_WORK)null;
            Array.Clear((Array)this.ama_2d_work, 0, this.ama_2d_work.Length);
            Array.Clear((Array)this.ama_2d_work_act, 0, this.ama_2d_work_act.Length);
        }
    }
}
