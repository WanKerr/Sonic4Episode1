public partial class AppMain
{
    public class GMS_EFFECT_3DES_WORK : IOBS_OBJECT_WORK
    {
        public readonly GMS_EFFECT_COM_WORK efct_com = new GMS_EFFECT_COM_WORK();
        public readonly OBS_ACTION3D_ES_WORK obj_3des = new OBS_ACTION3D_ES_WORK();
        public uint saved_pos_type;
        public uint saved_init_flag;
        public object m_pHolder;

        public void Clear()
        {
            this.efct_com.Clear();
            this.obj_3des.Clear();
            this.saved_pos_type = 0U;
            this.saved_init_flag = 0U;
        }

        public static explicit operator GMS_EFFECT_COM_WORK(
          GMS_EFFECT_3DES_WORK work)
        {
            return work.efct_com;
        }

        public static explicit operator GMS_BOSS5_EFCT_GENERAL_WORK(
          GMS_EFFECT_3DES_WORK work)
        {
            return (GMS_BOSS5_EFCT_GENERAL_WORK)work.m_pHolder;
        }

        public static explicit operator GMS_BOSS4_EFF_COMMON_WORK(
          GMS_EFFECT_3DES_WORK work)
        {
            return (GMS_BOSS4_EFF_COMMON_WORK)work.m_pHolder;
        }

        public static explicit operator GMS_BOSS1_EFF_SHOCKWAVE_SUB_WORK(
          GMS_EFFECT_3DES_WORK work)
        {
            return (GMS_BOSS1_EFF_SHOCKWAVE_SUB_WORK)work.m_pHolder;
        }

        public static explicit operator GMS_BOSS1_EFF_SHOCKWAVE_WORK(
          GMS_EFFECT_3DES_WORK work)
        {
            return (GMS_BOSS1_EFF_SHOCKWAVE_WORK)work.m_pHolder;
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.efct_com.obj_work;
        }

        public static explicit operator OBS_OBJECT_WORK(GMS_EFFECT_3DES_WORK work)
        {
            return work.efct_com.obj_work;
        }

        public GMS_EFFECT_3DES_WORK()
        {
            this.efct_com = new GMS_EFFECT_COM_WORK(this);
        }

        public GMS_EFFECT_3DES_WORK(object pHolder)
          : this()
        {
            this.m_pHolder = pHolder;
        }
    }
}
