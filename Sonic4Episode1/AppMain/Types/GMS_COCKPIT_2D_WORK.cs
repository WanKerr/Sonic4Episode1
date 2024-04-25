public partial class AppMain
{
    public class GMS_COCKPIT_2D_WORK : IOBS_OBJECT_WORK
    {
        public readonly OBS_ACTION2D_AMA_WORK obj_2d = new OBS_ACTION2D_AMA_WORK();
        public readonly GMS_COCKPIT_COM_WORK cpit_com;

        public GMS_COCKPIT_2D_WORK()
        {
            this.cpit_com = new GMS_COCKPIT_COM_WORK(this);
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.cpit_com.obj_work;
        }

        public static explicit operator OBS_OBJECT_WORK(GMS_COCKPIT_2D_WORK work)
        {
            return work.cpit_com.obj_work;
        }

        public static explicit operator GMS_COCKPIT_2D_WORK(OBS_OBJECT_WORK work)
        {
            return (GMS_COCKPIT_2D_WORK)((GMS_COCKPIT_COM_WORK)work.holder).holder;
        }
    }
}
