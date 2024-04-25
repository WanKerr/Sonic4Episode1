public partial class AppMain
{
    public class GMS_BOSS5_EFCT_GENERAL_WORK : IOBS_OBJECT_WORK
    {
        public readonly NNS_MATRIX ofst_mtx = new NNS_MATRIX();
        public readonly GMS_BOSS5_1SHOT_TIMER se_timer = new GMS_BOSS5_1SHOT_TIMER();
        public readonly GMS_EFFECT_3DES_WORK efct_3des;
        public uint flag;
        public uint user_flag;
        public uint user_work;
        public int ref_node_snm_id;
        public uint timer;
        public float ratio_timer;
        public uint se_cnt;
        public GSS_SND_SE_HANDLE se_handle;

        public GMS_BOSS5_EFCT_GENERAL_WORK()
        {
            this.efct_3des = new GMS_EFFECT_3DES_WORK(this);
        }

        public static explicit operator GMS_EFFECT_COM_WORK(
          GMS_BOSS5_EFCT_GENERAL_WORK p)
        {
            return p.efct_3des.efct_com;
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.efct_3des.efct_com.obj_work;
        }

        public static explicit operator OBS_OBJECT_WORK(
          GMS_BOSS5_EFCT_GENERAL_WORK work)
        {
            return work.efct_3des.efct_com.obj_work;
        }
    }
}
