public partial class AppMain
{
    public class GMS_GMK_CAM_SCR_LIMIT_WORK : IOBS_OBJECT_WORK
    {
        public readonly GMS_GMK_CAM_SCR_LIMIT_SETTING limit_setting = new GMS_GMK_CAM_SCR_LIMIT_SETTING();
        public readonly GMS_ENEMY_COM_WORK gmk_work;

        public GMS_GMK_CAM_SCR_LIMIT_WORK()
        {
            this.gmk_work = new GMS_ENEMY_COM_WORK(this);
        }

        public static explicit operator GMS_ENEMY_COM_WORK(
          GMS_GMK_CAM_SCR_LIMIT_WORK work)
        {
            return work.gmk_work;
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.gmk_work.obj_work;
        }
    }
}
