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
    public class GMS_EFFECT_CREATE_PARAM
    {
        public int ame_idx;
        public uint pos_type;
        public uint init_flag;
        public readonly AppMain.NNS_VECTOR disp_ofst;
        public AppMain.NNS_ROTATE_A16 disp_rot;
        public float scale;
        public AppMain.MPP_VOID_OBS_OBJECT_WORK main_func;
        public int model_idx;

        public GMS_EFFECT_CREATE_PARAM(
          int ame_idx,
          uint pos_type,
          uint init_flag,
          AppMain.NNS_VECTOR disp_ofst,
          AppMain.NNS_ROTATE_A16 disp_rot,
          float scale,
          AppMain.MPP_VOID_OBS_OBJECT_WORK main_func,
          int model_idx)
        {
            this.ame_idx = ame_idx;
            this.pos_type = pos_type;
            this.init_flag = init_flag;
            this.disp_ofst = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
            this.disp_ofst.Assign(disp_ofst);
            this.disp_rot = disp_rot;
            this.scale = scale;
            this.main_func = main_func;
            this.model_idx = model_idx;
        }

        public GMS_EFFECT_CREATE_PARAM()
        {
            this.disp_ofst = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
            this.disp_rot = new AppMain.NNS_ROTATE_A16();
        }

        public void Assign(AppMain.GMS_EFFECT_CREATE_PARAM param)
        {
            this.ame_idx = param.ame_idx;
            this.pos_type = param.pos_type;
            this.init_flag = param.init_flag;
            this.disp_ofst.Assign(param.disp_ofst);
            this.disp_rot = param.disp_rot;
            this.scale = param.scale;
            this.main_func = param.main_func;
            this.model_idx = param.model_idx;
        }
    }
}
