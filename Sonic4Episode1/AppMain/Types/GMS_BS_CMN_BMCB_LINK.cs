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
    public class GMS_BS_CMN_BMCB_LINK : AppMain.IClearable
    {
        public AppMain.GMS_BS_CMN_BMCB_LINK next;
        public AppMain.GMS_BS_CMN_BMCB_LINK prev;
        public AppMain.MPP_VOID_MOTION_NSSOBJECT_OBJECT bmcb_func;
        public object bmcb_param;

        public void Clear()
        {
            this.next = this.prev = (AppMain.GMS_BS_CMN_BMCB_LINK)null;
            this.bmcb_func = (AppMain.MPP_VOID_MOTION_NSSOBJECT_OBJECT)null;
            this.bmcb_param = (object)null;
        }
    }
}
