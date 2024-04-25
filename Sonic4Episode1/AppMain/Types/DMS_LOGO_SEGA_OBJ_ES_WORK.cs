public partial class AppMain
{
    public class DMS_LOGO_SEGA_OBJ_ES_WORK : IOBS_OBJECT_WORK
    {
        public readonly OBS_ACTION3D_ES_WORK obj_3des = new OBS_ACTION3D_ES_WORK();
        public readonly OBS_DATA_WORK data_work_texamb = new OBS_DATA_WORK();
        public readonly OBS_DATA_WORK data_work_texlist = new OBS_DATA_WORK();
        public readonly OBS_DATA_WORK data_work_model = new OBS_DATA_WORK();
        public readonly OBS_OBJECT_WORK obj_work;

        public OBS_OBJECT_WORK Cast()
        {
            return this.obj_work;
        }

        public static explicit operator DMS_LOGO_SEGA_OBJ_ES_WORK(
          OBS_OBJECT_WORK work)
        {
            return (DMS_LOGO_SEGA_OBJ_ES_WORK)work.holder;
        }

        public DMS_LOGO_SEGA_OBJ_ES_WORK()
        {
            this.obj_work = OBS_OBJECT_WORK.Create(this);
        }
    }
}
