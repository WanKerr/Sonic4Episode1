public partial class AppMain
{
    public class GMS_SMSG_2D_OBJ_WORK : IOBS_OBJECT_WORK
    {
        public readonly OBS_ACTION2D_AMA_WORK obj_2d = new OBS_ACTION2D_AMA_WORK();
        public readonly OBS_OBJECT_WORK obj_work;

        public GMS_SMSG_2D_OBJ_WORK()
        {
            this.obj_work = OBS_OBJECT_WORK.Create(this);
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.obj_work;
        }
    }
}
