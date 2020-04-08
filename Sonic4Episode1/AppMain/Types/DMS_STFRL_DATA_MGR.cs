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
    public class DMS_STFRL_DATA_MGR
    {
        public readonly AppMain.AMS_AMB_HEADER[] arc_cmn_amb = new AppMain.AMS_AMB_HEADER[2];
        public AppMain.AMS_AMB_HEADER arc_font_amb;
        public AppMain.AMS_AMB_HEADER arc_scr_amb;
        public AppMain.AMS_AMB_HEADER arc_end_amb;
        public AppMain.AMS_AMB_HEADER arc_end_jp_amb;
        public AppMain.YSDS_HEADER stf_list_ysd;

        public void Clear()
        {
            this.arc_font_amb = (AppMain.AMS_AMB_HEADER)null;
            this.arc_scr_amb = (AppMain.AMS_AMB_HEADER)null;
            this.arc_end_amb = (AppMain.AMS_AMB_HEADER)null;
            this.arc_end_jp_amb = (AppMain.AMS_AMB_HEADER)null;
            Array.Clear((Array)this.arc_cmn_amb, 0, 2);
            this.stf_list_ysd = (AppMain.YSDS_HEADER)null;
        }
    }
}
