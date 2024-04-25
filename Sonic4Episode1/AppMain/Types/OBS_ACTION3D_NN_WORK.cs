using System;

public partial class AppMain
{
    public class OBS_ACTION3D_NN_WORK
    {
        public readonly object[] mtn = new object[4];
        public readonly OBS_DATA_WORK[] mtn_data_work = new OBS_DATA_WORK[4];
        public readonly object[] mat_mtn = new object[4];
        public readonly OBS_DATA_WORK[] mat_mtn_data_work = new OBS_DATA_WORK[4];
        public readonly int[] act_id = new int[2];
        public readonly float[] frame = new float[2];
        public readonly float[] speed = new float[2];
        public readonly NNS_MATRIX user_obj_mtx = GlobalPool<NNS_MATRIX>.Alloc();
        public readonly NNS_MATRIX user_obj_mtx_r = GlobalPool<NNS_MATRIX>.Alloc();
        public readonly AMS_DRAWSTATE draw_state = new AMS_DRAWSTATE();
        public readonly OBS_ACTION3D_MTN_LOAD_SETTING[] mtn_load_setting = new OBS_ACTION3D_MTN_LOAD_SETTING[4];
        public readonly OBS_ACTION3D_MTN_LOAD_SETTING[] mat_mtn_load_setting = new OBS_ACTION3D_MTN_LOAD_SETTING[4];
        public NNS_OBJECT _object;
        public NNS_TEXLIST texlist;
        public object texlistbuf;
        public AMS_MOTION motion;
        public object model;
        public OBS_DATA_WORK model_data_work;
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
        public MPP_VOID_OBJECT_DELEGATE user_func;
        public object user_param;
        public MPP_VOID_ARRAYNNSMATRIX_NNSOBJECT_OBJECT mplt_cb_func;
        public object mplt_cb_param;
        public mtn_cb_func_delegate mtn_cb_func;
        public object mtn_cb_param;
        public MPP_BOOL_NNSDRAWCALLBACKVAL_OBJECT_DELEGATE material_cb_func;
        public object material_cb_param;
        public int reg_index;

        public OBS_ACTION3D_NN_WORK()
        {
            for (int index = 0; index < 4; ++index)
            {
                this.mtn_load_setting[index] = new OBS_ACTION3D_MTN_LOAD_SETTING();
                this.mat_mtn_load_setting[index] = new OBS_ACTION3D_MTN_LOAD_SETTING();
            }
        }

        public void Clear()
        {
            this._object = null;
            this.texlist = null;
            this.texlistbuf = null;
            this.motion = null;
            this.model = null;
            this.model_data_work = null;
            Array.Clear(mtn, 0, this.mtn.Length);
            Array.Clear(mtn_data_work, 0, this.mtn_data_work.Length);
            Array.Clear(mat_mtn, 0, this.mat_mtn.Length);
            Array.Clear(mat_mtn_data_work, 0, this.mat_mtn_data_work.Length);
            this.command_state = 0U;
            this.flag = 0U;
            this.marge = 0.0f;
            this.per = 0.0f;
            Array.Clear(act_id, 0, this.act_id.Length);
            Array.Clear(frame, 0, this.frame.Length);
            Array.Clear(speed, 0, this.speed.Length);
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
            this.user_func = null;
            this.user_param = null;
            this.mplt_cb_func = null;
            this.mplt_cb_param = null;
            this.mtn_cb_func = null;
            this.mtn_cb_param = null;
            this.material_cb_func = null;
            this.material_cb_param = null;
            this.reg_index = 0;
            for (int index = 0; index < 4; ++index)
            {
                this.mtn_load_setting[index].Clear();
                this.mat_mtn_load_setting[index].Clear();
            }
        }

        public class CMPLT_Wrapper
        {
            public GMS_BS_CMN_CNM_NODE_INFO[] m_pInfos;
            public ushort reg_node_cnt;
        }
    }
}
