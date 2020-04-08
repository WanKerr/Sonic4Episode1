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
    public class OBS_ACTION3D_NN_WORK
    {
        public readonly object[] mtn = new object[4];
        public readonly AppMain.OBS_DATA_WORK[] mtn_data_work = new AppMain.OBS_DATA_WORK[4];
        public readonly object[] mat_mtn = new object[4];
        public readonly AppMain.OBS_DATA_WORK[] mat_mtn_data_work = new AppMain.OBS_DATA_WORK[4];
        public readonly int[] act_id = new int[2];
        public readonly float[] frame = new float[2];
        public readonly float[] speed = new float[2];
        public readonly AppMain.NNS_MATRIX user_obj_mtx = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        public readonly AppMain.NNS_MATRIX user_obj_mtx_r = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        public readonly AppMain.AMS_DRAWSTATE draw_state = new AppMain.AMS_DRAWSTATE();
        public readonly AppMain.OBS_ACTION3D_MTN_LOAD_SETTING[] mtn_load_setting = new AppMain.OBS_ACTION3D_MTN_LOAD_SETTING[4];
        public readonly AppMain.OBS_ACTION3D_MTN_LOAD_SETTING[] mat_mtn_load_setting = new AppMain.OBS_ACTION3D_MTN_LOAD_SETTING[4];
        public AppMain.NNS_OBJECT _object;
        public AppMain.NNS_TEXLIST texlist;
        public object texlistbuf;
        public AppMain.AMS_MOTION motion;
        public object model;
        public AppMain.OBS_DATA_WORK model_data_work;
        public uint command_state;
        public uint flag;
        public float marge;
        public float per;
        public int mat_act_id;
        public float mat_frame;
        public float mat_speed;
        public float blend_spd;
        public uint sub_obj_type;
        public uint drawflag;
        public uint use_light_flag;
        public AppMain.MPP_VOID_OBJECT_DELEGATE user_func;
        public object user_param;
        public AppMain.MPP_VOID_ARRAYNNSMATRIX_NNSOBJECT_OBJECT mplt_cb_func;
        public object mplt_cb_param;
        public AppMain.mtn_cb_func_delegate mtn_cb_func;
        public object mtn_cb_param;
        public AppMain.MPP_BOOL_NNSDRAWCALLBACKVAL_OBJECT_DELEGATE material_cb_func;
        public object material_cb_param;
        public int reg_index;

        public OBS_ACTION3D_NN_WORK()
        {
            for (int index = 0; index < 4; ++index)
            {
                this.mtn_load_setting[index] = new AppMain.OBS_ACTION3D_MTN_LOAD_SETTING();
                this.mat_mtn_load_setting[index] = new AppMain.OBS_ACTION3D_MTN_LOAD_SETTING();
            }
        }

        public void Clear()
        {
            this._object = (AppMain.NNS_OBJECT)null;
            this.texlist = (AppMain.NNS_TEXLIST)null;
            this.texlistbuf = (object)null;
            this.motion = (AppMain.AMS_MOTION)null;
            this.model = (object)null;
            this.model_data_work = (AppMain.OBS_DATA_WORK)null;
            Array.Clear((Array)this.mtn, 0, this.mtn.Length);
            Array.Clear((Array)this.mtn_data_work, 0, this.mtn_data_work.Length);
            Array.Clear((Array)this.mat_mtn, 0, this.mat_mtn.Length);
            Array.Clear((Array)this.mat_mtn_data_work, 0, this.mat_mtn_data_work.Length);
            this.command_state = 0U;
            this.flag = 0U;
            this.marge = 0.0f;
            this.per = 0.0f;
            Array.Clear((Array)this.act_id, 0, this.act_id.Length);
            Array.Clear((Array)this.frame, 0, this.frame.Length);
            Array.Clear((Array)this.speed, 0, this.speed.Length);
            this.mat_act_id = 0;
            this.mat_frame = 0.0f;
            this.mat_speed = 0.0f;
            this.user_obj_mtx.Clear();
            this.user_obj_mtx_r.Clear();
            this.blend_spd = 0.0f;
            this.sub_obj_type = 0U;
            this.drawflag = 0U;
            this.draw_state.Clear();
            this.use_light_flag = 0U;
            this.user_func = (AppMain.MPP_VOID_OBJECT_DELEGATE)null;
            this.user_param = (object)null;
            this.mplt_cb_func = (AppMain.MPP_VOID_ARRAYNNSMATRIX_NNSOBJECT_OBJECT)null;
            this.mplt_cb_param = (object)null;
            this.mtn_cb_func = (AppMain.mtn_cb_func_delegate)null;
            this.mtn_cb_param = (object)null;
            this.material_cb_func = (AppMain.MPP_BOOL_NNSDRAWCALLBACKVAL_OBJECT_DELEGATE)null;
            this.material_cb_param = (object)null;
            this.reg_index = 0;
            for (int index = 0; index < 4; ++index)
            {
                this.mtn_load_setting[index].Clear();
                this.mat_mtn_load_setting[index].Clear();
            }
        }

        public class CMPLT_Wrapper
        {
            public AppMain.GMS_BS_CMN_CNM_NODE_INFO[] m_pInfos;
            public ushort reg_node_cnt;
        }
    }
}
