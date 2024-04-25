public partial class AppMain
{
    public class GMS_GMK_SHUTTER_WORK : IOBS_OBJECT_WORK
    {
        public readonly OBS_ACTION3D_NN_WORK obj_3d_parts = new OBS_ACTION3D_NN_WORK();
        public readonly GMS_ENEMY_3D_WORK gimmick_work;
        public GMS_EFFECT_3DES_WORK effect_work;

        public GMS_GMK_SHUTTER_WORK()
        {
            this.gimmick_work = new GMS_ENEMY_3D_WORK(this);
        }

        public static explicit operator OBS_OBJECT_WORK(GMS_GMK_SHUTTER_WORK p)
        {
            return p.gimmick_work.ene_com.obj_work;
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.gimmick_work.ene_com.obj_work;
        }
    }
}
