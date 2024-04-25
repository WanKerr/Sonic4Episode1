public partial class AppMain
{
    public class DMS_TITLEOP_OBJ_3DNN_WORK : IOBS_OBJECT_WORK
    {
        public OBS_ACTION3D_NN_WORK obj_3d = new OBS_ACTION3D_NN_WORK();
        public readonly OBS_OBJECT_WORK obj_work;
        public DMS_TITLEOP_ROCK_SETTING[] rock_setting;
        public int rock_setting_num;
        public float sky_rot;

        public OBS_OBJECT_WORK Cast()
        {
            return this.obj_work;
        }

        public static explicit operator OBS_OBJECT_WORK(
          DMS_TITLEOP_OBJ_3DNN_WORK work)
        {
            return work.obj_work;
        }

        public static explicit operator DMS_TITLEOP_OBJ_3DNN_WORK(
          OBS_OBJECT_WORK work)
        {
            return (DMS_TITLEOP_OBJ_3DNN_WORK)work.holder;
        }

        public DMS_TITLEOP_OBJ_3DNN_WORK()
        {
            this.obj_work = OBS_OBJECT_WORK.Create(this);
        }
    }
}
