public partial class AppMain
{
    public class GMS_COCKPIT_COM_WORK
    {
        public readonly OBS_OBJECT_WORK obj_work;
        public readonly object holder;

        public GMS_COCKPIT_COM_WORK(object holder)
        {
            this.holder = holder;
            this.obj_work = OBS_OBJECT_WORK.Create(this);
        }

        public GMS_COCKPIT_COM_WORK()
        {
            this.obj_work = OBS_OBJECT_WORK.Create(this);
        }
    }
}
