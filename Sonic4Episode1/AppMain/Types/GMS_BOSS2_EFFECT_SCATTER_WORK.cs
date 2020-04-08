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
    public class GMS_BOSS2_EFFECT_SCATTER_WORK : AppMain.IOBS_OBJECT_WORK
    {
        public AppMain.NNS_QUATERNION spin_quat = new AppMain.NNS_QUATERNION();
        public readonly AppMain.GMS_BS_CMN_NODE_CTRL_OBJECT control_node_work;

        public GMS_BOSS2_EFFECT_SCATTER_WORK()
        {
            this.control_node_work = new AppMain.GMS_BS_CMN_NODE_CTRL_OBJECT((object)this);
        }

        public static explicit operator AppMain.GMS_BS_CMN_NODE_CTRL_OBJECT(
          AppMain.GMS_BOSS2_EFFECT_SCATTER_WORK p)
        {
            return p.control_node_work;
        }

        public static explicit operator AppMain.OBS_OBJECT_WORK(
          AppMain.GMS_BOSS2_EFFECT_SCATTER_WORK p)
        {
            return p.control_node_work.efct_com.obj_work;
        }

        public AppMain.OBS_OBJECT_WORK Cast()
        {
            return this.control_node_work.efct_com.obj_work;
        }
    }
}
