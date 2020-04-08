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
    public class OBS_TBL_WORK
    {
        public readonly short[] key_timer = new short[4];
        public readonly short[] move_timer = new short[4];
        public AppMain.VecFx32 spd = new AppMain.VecFx32();
        public AppMain.VecFx32 scale = new AppMain.VecFx32();
        public AppMain.VecU16 dir = new AppMain.VecU16();
        public readonly AppMain.OBS_DATA_WORK[] data_work = new AppMain.OBS_DATA_WORK[4];
        public readonly short[] repeat_point = new short[4];
        public uint flag;
        public AppMain.OBS_ACT_TBL act_tbl;
        public AppMain.OBS_MOVE_TBL move_tbl;
        public AppMain.OBS_SCALE_TBL scale_tbl;
        public AppMain.OBS_DIR_TBL dir_tbl;

        public OBS_TBL_WORK()
        {
        }

        public OBS_TBL_WORK(AppMain.OBS_TBL_WORK tblWork)
        {
            Array.Copy((Array)tblWork.key_timer, (Array)this.key_timer, this.key_timer.Length);
            Array.Copy((Array)tblWork.move_timer, (Array)this.move_timer, this.move_timer.Length);
            this.flag = tblWork.flag;
            this.spd.Assign(tblWork.spd);
            this.scale.Assign(tblWork.scale);
            this.dir.Assign(tblWork.dir);
            this.act_tbl = tblWork.act_tbl;
            this.move_tbl = tblWork.move_tbl;
            this.scale_tbl = tblWork.scale_tbl;
            this.dir_tbl = tblWork.dir_tbl;
            Array.Copy((Array)tblWork.data_work, (Array)this.data_work, this.data_work.Length);
            Array.Copy((Array)tblWork.repeat_point, (Array)this.repeat_point, this.repeat_point.Length);
        }

        public AppMain.OBS_TBL_WORK Assign(AppMain.OBS_TBL_WORK tblWork)
        {
            if (this != tblWork)
            {
                Array.Copy((Array)tblWork.key_timer, (Array)this.key_timer, this.key_timer.Length);
                Array.Copy((Array)tblWork.move_timer, (Array)this.move_timer, this.move_timer.Length);
                this.flag = tblWork.flag;
                this.spd.Assign(tblWork.spd);
                this.scale.Assign(tblWork.scale);
                this.dir.Assign(tblWork.dir);
                this.act_tbl = tblWork.act_tbl;
                this.move_tbl = tblWork.move_tbl;
                this.scale_tbl = tblWork.scale_tbl;
                this.dir_tbl = tblWork.dir_tbl;
                Array.Copy((Array)tblWork.data_work, (Array)this.data_work, this.data_work.Length);
                Array.Copy((Array)tblWork.repeat_point, (Array)this.repeat_point, this.repeat_point.Length);
            }
            return this;
        }
    }
}
