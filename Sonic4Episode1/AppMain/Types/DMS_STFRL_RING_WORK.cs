public partial class AppMain
{
    public class DMS_STFRL_RING_WORK : IOBS_OBJECT_WORK
    {
        public readonly VecFx32[] pos = new VecFx32[6];
        public readonly int[] spd_x = new int[6];
        public readonly int[] spd_y = new int[6];
        public readonly OBS_OBJECT_WORK obj_work;
        public VecFx32 start_pos;
        public VecFx32 scale;
        public _proc_efct_ proc_efct;
        public int efct_start_time;
        public int timer;
        public int efct_timer;
        public uint flag;
        public float alpha;
        public float alpha_spd;
        public int disp_ring_pos_no;
        public int disp_efct_pos_no;

        public DMS_STFRL_RING_WORK()
        {
            this.obj_work = OBS_OBJECT_WORK.Create(this);
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.obj_work;
        }

        public static explicit operator OBS_OBJECT_WORK(DMS_STFRL_RING_WORK work)
        {
            return work.obj_work;
        }

        public delegate void _proc_efct_(OBS_OBJECT_WORK work);
    }
}
