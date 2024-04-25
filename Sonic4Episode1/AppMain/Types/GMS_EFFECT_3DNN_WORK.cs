public partial class AppMain
{
    public class GMS_EFFECT_3DNN_WORK : IOBS_OBJECT_WORK
    {
        public OBS_ACTION3D_NN_WORK obj_3d = new OBS_ACTION3D_NN_WORK();
        public GMS_EFFECT_COM_WORK efct_com;
        public readonly object holder;

        public GMS_EFFECT_3DNN_WORK()
        {
            this.efct_com = new GMS_EFFECT_COM_WORK(this);
        }

        public GMS_EFFECT_3DNN_WORK(object _holder)
        {
            this.efct_com = new GMS_EFFECT_COM_WORK(this);
            this.holder = _holder;
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.efct_com.obj_work;
        }

        public static explicit operator GMS_GMK_PRESSWALL_PARTS(
          GMS_EFFECT_3DNN_WORK work)
        {
            return (GMS_GMK_PRESSWALL_PARTS)work.holder;
        }

        public static explicit operator GMS_GMK_SEESAWPARTS_WORK(
          GMS_EFFECT_3DNN_WORK work)
        {
            return (GMS_GMK_SEESAWPARTS_WORK)work.holder;
        }

        public static explicit operator GMS_GMK_POPSTEAMPARTS_WORK(
          GMS_EFFECT_3DNN_WORK work)
        {
            return (GMS_GMK_POPSTEAMPARTS_WORK)work.holder;
        }

        public static explicit operator GMS_GMK_PISTONROD_WORK(
          GMS_EFFECT_3DNN_WORK work)
        {
            return (GMS_GMK_PISTONROD_WORK)work.holder;
        }

        public static explicit operator GMS_GMK_CANNONPARTS_WORK(
          GMS_EFFECT_3DNN_WORK work)
        {
            return (GMS_GMK_CANNONPARTS_WORK)work.holder;
        }

        public static explicit operator GMS_BOSS5_LDPART_WORK(
          GMS_EFFECT_3DNN_WORK work)
        {
            return (GMS_BOSS5_LDPART_WORK)work.holder;
        }

        public static explicit operator GMS_GMK_SLOTPARTS_WORK(
          GMS_EFFECT_3DNN_WORK work)
        {
            return (GMS_GMK_SLOTPARTS_WORK)work.holder;
        }

        public static explicit operator GMS_GMK_BWALL_PARTS(
          GMS_EFFECT_3DNN_WORK work)
        {
            return (GMS_GMK_BWALL_PARTS)work.holder;
        }

        public static explicit operator GMS_GMK_BOBJ_PARTS(
          GMS_EFFECT_3DNN_WORK work)
        {
            return (GMS_GMK_BOBJ_PARTS)work.holder;
        }
    }
}
