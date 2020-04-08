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
    public class DMS_STFRL_FS_DATA_MGR
    {
        public readonly AppMain.AMS_AMB_HEADER[] arc_cmn_amb_fs = new AppMain.AMS_AMB_HEADER[2];
        public AppMain.AMS_AMB_HEADER arc_list_font_amb_fs;
        public AppMain.AMS_AMB_HEADER arc_scr_amb_fs;
        public AppMain.AMS_AMB_HEADER arc_end_amb_fs;
        public AppMain.AMS_AMB_HEADER arc_end_jp_amb_fs;
        public AppMain.AMS_FS staff_list_fs;

        public void Clear()
        {
            this.arc_list_font_amb_fs = (AppMain.AMS_AMB_HEADER)null;
            this.arc_scr_amb_fs = (AppMain.AMS_AMB_HEADER)null;
            this.arc_end_amb_fs = (AppMain.AMS_AMB_HEADER)null;
            this.arc_end_jp_amb_fs = (AppMain.AMS_AMB_HEADER)null;
            Array.Clear((Array)this.arc_cmn_amb_fs, 0, this.arc_cmn_amb_fs.Length);
            this.staff_list_fs = (AppMain.AMS_FS)null;
        }
    }
}
