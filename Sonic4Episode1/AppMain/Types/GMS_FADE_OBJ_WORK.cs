public partial class AppMain
{
    public class GMS_FADE_OBJ_WORK : IOBS_OBJECT_WORK
    {
        public readonly IZS_FADE_WORK fade_work = new IZS_FADE_WORK();
        public object m_pHolder;
        public readonly OBS_OBJECT_WORK obj_work;

        public GMS_FADE_OBJ_WORK(object _obj)
          : this()
        {
            this.m_pHolder = _obj;
        }

        public GMS_FADE_OBJ_WORK()
        {
            this.obj_work = OBS_OBJECT_WORK.Create(this);
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.obj_work;
        }

        public static explicit operator OBS_OBJECT_WORK(GMS_FADE_OBJ_WORK work)
        {
            return work.obj_work;
        }

        public static explicit operator GMS_BOSS5_ALARM_FADE_WORK(
          GMS_FADE_OBJ_WORK p)
        {
            return (GMS_BOSS5_ALARM_FADE_WORK)p.m_pHolder;
        }
    }
}
