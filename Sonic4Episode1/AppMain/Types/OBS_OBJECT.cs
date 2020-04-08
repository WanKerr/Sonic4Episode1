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
    public class OBS_OBJECT
    {
        public AppMain.VecFx32 glb_scale = new AppMain.VecFx32();
        public AppMain.VecFx32 draw_scale = new AppMain.VecFx32();
        public readonly short[] offset = new short[2];
        public readonly int[] scroll = new int[2];
        public readonly short[] lcd_size = new short[2];
        public readonly short[] clip_lcd_size = new short[2];
        public readonly int[][] camera = AppMain.New<int>(2, 2);
        public readonly int[] clip_camera = new int[2];
        public readonly short[][] cam_scale_center = AppMain.New<short>(2, 2);
        public AppMain.NNS_RGB ambient_color = new AppMain.NNS_RGB();
        public readonly AppMain.OBS_LIGHT[] light = AppMain.New<AppMain.OBS_LIGHT>(AppMain.NNE_LIGHT_MAX);
        public AppMain.VecFx32 scale = new AppMain.VecFx32();
        public AppMain.VecFx32 inv_scale = new AppMain.VecFx32();
        public AppMain.VecFx32 inv_glb_scale = new AppMain.VecFx32();
        public AppMain.VecFx32 inv_draw_scale = new AppMain.VecFx32();
        public int speed;
        public int depth;
        public uint timer;
        public int timer_fx;
        public uint flag;
        public int pause_level;
        public float disp_width;
        public float disp_height;
        public AppMain.OBJECT_Cam_Delegate pp3dCam;
        public int glb_camera_id;
        public int glb_camera_type;
        public uint load_drawflag;
        public uint drawflag;
        public uint def_user_light_flag;
        public sbyte col_through_dot;
        public AppMain.OBS_DATA_WORK[] pData;
        public int data_max;
        public AppMain.OBS_OBJECT_WORK obj_list_head;
        public AppMain.OBS_OBJECT_WORK obj_list_tail;
        public AppMain.OBS_OBJECT_WORK obj_draw_list_head;
        public AppMain.OBS_OBJECT_WORK obj_draw_list_tail;
        public AppMain.OBJECT_Delegate ppPre;
        public AppMain.OBJECT_Delegate ppPost;
        public AppMain.OBJECT_Delegate ppDrawSort;
        public AppMain.OBJECT_WORK_Delegate ppCollision;
        public AppMain.OBJECT_WORK_Delegate ppObjPre;
        public AppMain.OBJECT_WORK_Delegate ppObjPost;
        public AppMain.OBJECT_WORK_Delegate ppRegRecAuto;
    }
}
