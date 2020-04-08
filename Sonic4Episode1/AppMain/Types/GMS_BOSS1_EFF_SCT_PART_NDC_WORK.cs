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
    public class GMS_BOSS1_EFF_SCT_PART_NDC_WORK : AppMain.IOBS_OBJECT_WORK
    {
        public readonly AppMain.GMS_BS_CMN_NODE_CTRL_OBJECT ncd_obj = new AppMain.GMS_BS_CMN_NODE_CTRL_OBJECT();
        public AppMain.NNS_QUATERNION spin_quat;
        public bool is_ironball;

        public GMS_BOSS1_EFF_SCT_PART_NDC_WORK()
        {
            this.ncd_obj = new AppMain.GMS_BS_CMN_NODE_CTRL_OBJECT((object)this);
        }

        public AppMain.OBS_OBJECT_WORK Cast()
        {
            return this.ncd_obj.efct_com.obj_work;
        }

        public static explicit operator AppMain.GMS_BS_CMN_NODE_CTRL_OBJECT(
          AppMain.GMS_BOSS1_EFF_SCT_PART_NDC_WORK work)
        {
            return work.ncd_obj;
        }
    }
}
