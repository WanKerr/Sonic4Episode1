public partial class AppMain
{
    public class GMS_MAP_FAR_OBJ_WORK : IOBS_OBJECT_WORK
    {
        public readonly OBS_ACTION3D_NN_WORK obj_3d = new OBS_ACTION3D_NN_WORK();
        public readonly OBS_OBJECT_WORK obj_work;
        public readonly object holder;

        public static explicit operator GMS_MAP_FAR_OBJ_WORK(OBS_OBJECT_WORK ob)
        {
            return (GMS_MAP_FAR_OBJ_WORK)ob.holder;
        }

        public static explicit operator OBS_OBJECT_WORK(GMS_MAP_FAR_OBJ_WORK work)
        {
            return work.obj_work;
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.obj_work;
        }

        public GMS_MAP_FAR_OBJ_WORK()
        {
            this.obj_work = OBS_OBJECT_WORK.Create(this);
        }
    }
}
