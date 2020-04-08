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
    public class GMS_GDBUILD_BUILD_MDL_WORK : AppMain.IClearable
    {
        public AppMain.GME_GAME_DBUILD_MDL_STATE build_state;
        public AppMain.OBS_ACTION3D_NN_WORK[] obj_3d_list;
        public int num;
        public int reg_num;
        public AppMain.AMS_AMB_HEADER mdl_amb;
        public AppMain.AMS_AMB_HEADER tex_amb;
        public uint draw_flag;
        public AppMain.TXB_HEADER txb;

        public void Clear()
        {
            this.build_state = AppMain.GME_GAME_DBUILD_MDL_STATE.GME_GAME_DBUILD_MDL_STATE_REG_WAIT;
            this.obj_3d_list = (AppMain.OBS_ACTION3D_NN_WORK[])null;
            this.num = this.reg_num = 0;
            this.mdl_amb = this.tex_amb = (AppMain.AMS_AMB_HEADER)null;
            this.draw_flag = 0U;
            this.txb = (AppMain.TXB_HEADER)null;
        }
    }
}
