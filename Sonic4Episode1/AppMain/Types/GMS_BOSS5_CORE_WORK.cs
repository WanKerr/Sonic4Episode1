public partial class AppMain
{
    public class GMS_BOSS5_CORE_WORK : IOBS_OBJECT_WORK
    {
        public readonly GMS_ENEMY_COM_WORK ene_com;
        public MPP_VOID_GMS_BOSS5_CORE_WORK proc_update;

        public GMS_BOSS5_CORE_WORK()
        {
            this.ene_com = new GMS_ENEMY_COM_WORK(this);
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.ene_com.obj_work;
        }
    }
}
