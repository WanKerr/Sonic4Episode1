public partial class AppMain
{
    public class GMS_DECO_SUBMODEL_WORK : IClearable, IOBS_OBJECT_WORK
    {
        public readonly OBS_ACTION3D_NN_WORK obj_3d_sub = new OBS_ACTION3D_NN_WORK();
        public readonly GMS_DECO_WORK deco_work;
        public int sub_model_index;

        public void Clear()
        {
            this.deco_work.Clear();
            this.obj_3d_sub.Clear();
            this.sub_model_index = 0;
        }

        public static explicit operator GMS_DECO_WORK(GMS_DECO_SUBMODEL_WORK work)
        {
            return work.deco_work;
        }

        public GMS_DECO_SUBMODEL_WORK()
        {
            this.deco_work = new GMS_DECO_WORK(this);
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.deco_work.obj_work;
        }
    }
}
