public partial class AppMain
{
    public class GMS_DECO_WORK : IClearable, IOBS_OBJECT_WORK
    {
        public readonly OBS_ACTION3D_NN_WORK obj_3d = new OBS_ACTION3D_NN_WORK();
        public readonly OBS_RECT_WORK[] rect_work = New<OBS_RECT_WORK>(1);
        public readonly OBS_OBJECT_WORK obj_work;
        public GMS_EVE_RECORD_DECORATE event_record;
        public byte event_x;
        public AOS_TEXTURE model_tex;
        public int model_index;
        public readonly object holder;

        public static explicit operator GMS_DECO_SUBMODEL_WORK(GMS_DECO_WORK p)
        {
            return (GMS_DECO_SUBMODEL_WORK)p.holder;
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.obj_work;
        }

        public static explicit operator OBS_OBJECT_WORK(GMS_DECO_WORK work)
        {
            return work.obj_work;
        }

        public void Clear()
        {
            this.obj_work.Clear();
            this.obj_3d.Clear();
            ClearArray(this.rect_work);
            this.event_record = null;
            this.event_x = 0;
            this.model_tex = null;
            this.model_index = 0;
        }

        public GMS_DECO_WORK()
        {
            this.obj_work = OBS_OBJECT_WORK.Create(this);
        }

        public GMS_DECO_WORK(object _holder)
        {
            this.holder = _holder;
            this.obj_work = OBS_OBJECT_WORK.Create(this);
        }
    }
}
