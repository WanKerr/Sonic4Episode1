public partial class AppMain
{
    public class GMS_EFFECT_COM_WORK : IOBS_OBJECT_WORK
    {
        public OBS_RECT_WORK[] rect_work = New<OBS_RECT_WORK>(2);
        public OBS_OBJECT_WORK obj_work;
        public readonly object holder;

        public void Clear()
        {
            this.obj_work.Clear();
            for (int index = 0; index < 2; ++index)
                this.rect_work[index].Clear();
        }

        public static explicit operator GMS_BOSS1_FLASH_SCREEN_WORK(
          GMS_EFFECT_COM_WORK p)
        {
            return (GMS_BOSS1_FLASH_SCREEN_WORK)p.holder;
        }

        public static explicit operator GMS_BS_CMN_NODE_CTRL_OBJECT(
          GMS_EFFECT_COM_WORK p)
        {
            return (GMS_BS_CMN_NODE_CTRL_OBJECT)p.holder;
        }

        public static explicit operator GMS_EFFECT_COM_WORK(OBS_OBJECT_WORK work)
        {
            return work.holder is GMS_EFFECT_COM_WORK ? (GMS_EFFECT_COM_WORK)work.holder : ((GMS_EFFECT_3DES_WORK)work.holder).efct_com;
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.obj_work;
        }

        public static explicit operator OBS_OBJECT_WORK(GMS_EFFECT_COM_WORK work)
        {
            return work.obj_work;
        }

        public GMS_EFFECT_COM_WORK()
        {
            this.obj_work = OBS_OBJECT_WORK.Create(this);
        }

        public GMS_EFFECT_COM_WORK(object _holder)
        {
            this.obj_work = OBS_OBJECT_WORK.Create(this);
            this.holder = _holder;
        }

        public static explicit operator GMS_GMK_POPSTEAMPARTS_WORK(
          GMS_EFFECT_COM_WORK work)
        {
            return (GMS_GMK_POPSTEAMPARTS_WORK)(GMS_EFFECT_3DNN_WORK)work.holder;
        }

        public static explicit operator GMS_BOSS5_FLASH_SCREEN_WORK(
          GMS_EFFECT_COM_WORK work)
        {
            return (GMS_BOSS5_FLASH_SCREEN_WORK)work.holder;
        }

        public static explicit operator GMS_EFFECT_3DES_WORK(
          GMS_EFFECT_COM_WORK work)
        {
            return (GMS_EFFECT_3DES_WORK)work.holder;
        }
    }
}
