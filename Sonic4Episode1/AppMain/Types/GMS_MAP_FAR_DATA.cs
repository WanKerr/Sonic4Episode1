using System;

public partial class AppMain
{
    public class GMS_MAP_FAR_DATA
    {
        public readonly OBS_OBJECT_WORK[] obj_work = new OBS_OBJECT_WORK[16];
        public readonly NNS_VECTOR pos = GlobalPool<NNS_VECTOR>.Alloc();
        public AMS_AMB_HEADER amb_header;
        public OBS_ACTION3D_NN_WORK[] obj_3d_list;
        public OBS_ACTION3D_NN_WORK[] obj_3d_list_render;
        public OBS_ACTION3D_NN_WORK nn_work;
        public int nn_work_num;
        public int nn_regist_num;
        public MP_HEADER mp_header;
        public MD_HEADER md_header;
        public float degSky;
        public float degSky2;

        internal void Clear()
        {
            this.amb_header = null;
            this.obj_3d_list = null;
            this.obj_3d_list_render = null;
            Array.Clear(obj_work, 0, 16);
            this.nn_work = null;
            this.nn_work_num = 0;
            this.nn_regist_num = 0;
            this.pos.Clear();
            this.mp_header = null;
            this.md_header = null;
            this.degSky = 0.0f;
            this.degSky2 = 0.0f;
        }
    }
}
