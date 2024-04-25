public partial class AppMain
{
    public class GMS_GMK_BOSS5_LAND_PLACE_WORK : IOBS_OBJECT_WORK
    {
        public readonly GMS_ENEMY_3D_WORK ene_3d;

        public GMS_GMK_BOSS5_LAND_PLACE_WORK()
        {
            this.ene_3d = new GMS_ENEMY_3D_WORK(this);
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.ene_3d.ene_com.obj_work;
        }
    }
}
