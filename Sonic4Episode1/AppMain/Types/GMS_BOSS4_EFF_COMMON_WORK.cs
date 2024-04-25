public partial class AppMain
{
    public class GMS_BOSS4_EFF_COMMON_WORK : IOBS_OBJECT_WORK
    {
        public readonly NNS_VECTOR ofs = new NNS_VECTOR();
        public readonly NNS_VECTOR rot = new NNS_VECTOR();
        public readonly GMS_EFFECT_3DES_WORK eff_3des;
        public GMS_BOSS4_NODE_MATRIX node_work;
        public int link;
        public uint[] lookflag;
        public uint lookmask;

        public GMS_BOSS4_EFF_COMMON_WORK()
        {
            this.eff_3des = new GMS_EFFECT_3DES_WORK(this);
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.eff_3des.efct_com.obj_work;
        }
    }
}
