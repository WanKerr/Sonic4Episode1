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
    public class OBS_ACTION3D_ES_WORK
    {
        public AppMain.VecU16 disp_rot = new AppMain.VecU16();
        public readonly AppMain.NNS_VECTOR4D disp_ofst = new AppMain.NNS_VECTOR4D();
        public readonly AppMain.NNS_VECTOR4D dup_draw_ofst = new AppMain.NNS_VECTOR4D();
        public AppMain.NNS_QUATERNION user_dir_quat = new AppMain.NNS_QUATERNION();
        public AppMain.AMS_AME_ECB ecb;
        public AppMain.NNS_TEXLIST texlist;
        public object texlistbuf;
        public AppMain.OBS_DATA_WORK texlist_data_work;
        public AppMain.NNS_OBJECT _object;
        public AppMain.OBS_DATA_WORK object_data_work;
        public object eff;
        public AppMain.OBS_DATA_WORK eff_data_work;
        public object ambtex;
        public AppMain.OBS_DATA_WORK ambtex_data_work;
        public object model;
        public AppMain.OBS_DATA_WORK model_data_work;
        public uint flag;
        public uint command_state;
        public int user_attr;
        public int tex_reg_index;
        public int model_reg_index;
        public float speed;

        public void Clear()
        {
            this.ecb = (AppMain.AMS_AME_ECB)null;
            this.texlist = (AppMain.NNS_TEXLIST)null;
            this.texlistbuf = (object)null;
            this.texlist_data_work = (AppMain.OBS_DATA_WORK)null;
            this._object = (AppMain.NNS_OBJECT)null;
            this.object_data_work = (AppMain.OBS_DATA_WORK)null;
            this.eff = (object)null;
            this.eff_data_work = (AppMain.OBS_DATA_WORK)null;
            this.ambtex = (object)null;
            this.ambtex_data_work = (AppMain.OBS_DATA_WORK)null;
            this.model = (object)null;
            this.model_data_work = (AppMain.OBS_DATA_WORK)null;
            this.flag = 0U;
            this.command_state = 0U;
            this.disp_rot.Clear();
            this.disp_ofst.Clear();
            this.dup_draw_ofst.Clear();
            this.user_dir_quat.Clear();
            this.user_attr = 0;
            this.tex_reg_index = 0;
            this.model_reg_index = 0;
            this.speed = 0.0f;
        }
    }
}
