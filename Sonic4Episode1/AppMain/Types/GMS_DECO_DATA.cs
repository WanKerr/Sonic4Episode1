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
    public class GMS_DECO_DATA
    {
        public readonly AppMain.AOS_TEXTURE tvx_tex = new AppMain.AOS_TEXTURE();
        public AppMain.AMS_AMB_HEADER amb_header;
        public AppMain.OBS_ACTION3D_NN_WORK obj_3d_list;
        public AppMain.OBS_ACTION3D_NN_WORK[] obj_3d_list_fall;
        public AppMain.AMS_AMB_HEADER tvx_model;
        public AppMain.TVX_FILE[] tvx_model_data;

        public void Clear()
        {
            this.tvx_tex.Clear();
            this.amb_header = (AppMain.AMS_AMB_HEADER)null;
            this.obj_3d_list = (AppMain.OBS_ACTION3D_NN_WORK)null;
            this.obj_3d_list_fall = (AppMain.OBS_ACTION3D_NN_WORK[])null;
            this.tvx_model = (AppMain.AMS_AMB_HEADER)null;
        }
    }
}
