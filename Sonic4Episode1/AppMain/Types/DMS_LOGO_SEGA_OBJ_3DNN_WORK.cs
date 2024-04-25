public partial class AppMain
{
    public class DMS_LOGO_SEGA_OBJ_3DNN_WORK : IOBS_OBJECT_WORK
    {
        public readonly OBS_ACTION3D_NN_WORK obj_3d = new OBS_ACTION3D_NN_WORK();
        public readonly OBS_DATA_WORK data_work = new OBS_DATA_WORK();
        public readonly OBS_OBJECT_WORK obj_work;

        public OBS_OBJECT_WORK Cast()
        {
            return this.obj_work;
        }

        public static explicit operator OBS_OBJECT_WORK(
          DMS_LOGO_SEGA_OBJ_3DNN_WORK work)
        {
            return work.obj_work;
        }

        public static explicit operator DMS_LOGO_SEGA_OBJ_3DNN_WORK(
          OBS_OBJECT_WORK work)
        {
            return (DMS_LOGO_SEGA_OBJ_3DNN_WORK)work.holder;
        }

        public DMS_LOGO_SEGA_OBJ_3DNN_WORK()
        {
            this.obj_work = OBS_OBJECT_WORK.Create(this);
        }
    }
}
