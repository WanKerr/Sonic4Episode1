using System;

public partial class AppMain
{
    public class OBS_CAMERA : IClearable
    {
        public readonly NNS_VECTOR disp_pos = GlobalPool<NNS_VECTOR>.Alloc();
        public readonly NNS_VECTOR prev_disp_pos = GlobalPool<NNS_VECTOR>.Alloc();
        public readonly NNS_VECTOR pos = GlobalPool<NNS_VECTOR>.Alloc();
        public readonly NNS_VECTOR prev_pos = GlobalPool<NNS_VECTOR>.Alloc();
        public readonly NNS_VECTOR ofst = GlobalPool<NNS_VECTOR>.Alloc();
        public readonly NNS_VECTOR disp_ofst = GlobalPool<NNS_VECTOR>.Alloc();
        public readonly NNS_VECTOR target_ofst = GlobalPool<NNS_VECTOR>.Alloc();
        public readonly NNS_VECTOR play_ofst_max = GlobalPool<NNS_VECTOR>.Alloc();
        public readonly NNS_VECTOR allow = GlobalPool<NNS_VECTOR>.Alloc();
        public readonly NNS_VECTOR allow_limit = GlobalPool<NNS_VECTOR>.Alloc();
        public readonly NNS_VECTOR target_pos = GlobalPool<NNS_VECTOR>.Alloc();
        public readonly NNS_VECTOR camup_pos = GlobalPool<NNS_VECTOR>.Alloc();
        public readonly NNS_VECTOR spd = GlobalPool<NNS_VECTOR>.Alloc();
        public readonly NNS_VECTOR spd_add = GlobalPool<NNS_VECTOR>.Alloc();
        public readonly NNS_VECTOR spd_max = GlobalPool<NNS_VECTOR>.Alloc();
        public readonly int[] roll_hist = new int[16];
        public readonly NNS_VECTOR work = GlobalPool<NNS_VECTOR>.Alloc();
        public readonly int[] limit = new int[6];
        public readonly NNS_MATRIX prj_pers_mtx = GlobalPool<NNS_MATRIX>.Alloc();
        public readonly NNS_MATRIX prj_ortho_mtx = GlobalPool<NNS_MATRIX>.Alloc();
        public readonly NNS_MATRIX view_mtx = GlobalPool<NNS_MATRIX>.Alloc();
        public readonly NNS_VECTOR up_vec = GlobalPool<NNS_VECTOR>.Alloc();
        public int camera_id;
        public OBS_OBJECT_WORK target_obj;
        public OBS_OBJECT_WORK camup_obj;
        public int roll;
        public ushort roll_ptr;
        public ushort shift;
        public ushort index;
        public uint command_state;
        public OBJF_CAMERA_USER_FUNC user_func;
        public object user_work;
        public uint flag;
        public OBE_CAMERA_TYPE camera_type;
        public int fovy;
        public float scale;
        public float left;
        public float right;
        public float bottom;
        public float top;
        public float znear;
        public float zfar;
        public float aspect;

        public void Clear()
        {
            this.camera_id = 0;
            this.target_obj = this.camup_obj = null;
            this.disp_pos.Clear();
            this.prev_disp_pos.Clear();
            this.pos.Clear();
            this.prev_pos.Clear();
            this.ofst.Clear();
            this.disp_ofst.Clear();
            this.target_ofst.Clear();
            this.play_ofst_max.Clear();
            this.allow.Clear();
            this.allow_limit.Clear();
            this.target_pos.Clear();
            this.camup_pos.Clear();
            this.spd.Clear();
            this.spd_add.Clear();
            this.spd_max.Clear();
            this.roll = 0;
            Array.Clear(roll_hist, 0, this.roll_hist.Length);
            this.roll_ptr = 0;
        }
    }
}
