public partial class AppMain
{
    public class GMS_SCORE_DISP_WORK : IOBS_OBJECT_WORK
    {
        public readonly GMS_EFFECT_3DES_WORK[] efct_work = new GMS_EFFECT_3DES_WORK[5];
        public readonly OBS_OBJECT_WORK obj_work;
        public int vib_level;
        public int scale;
        public VecFx32 base_pos;
        public int rise_dist;
        public int rise_spd;
        public int rise_dec;
        public int vib_timer;
        public int timer;

        public static explicit operator OBS_OBJECT_WORK(GMS_SCORE_DISP_WORK work)
        {
            return work.obj_work;
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.obj_work;
        }

        public GMS_SCORE_DISP_WORK()
        {
            this.obj_work = OBS_OBJECT_WORK.Create(this);
        }
    }
}
