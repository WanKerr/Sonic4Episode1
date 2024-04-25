public partial class AppMain
{
    public class GMS_GMK_ROCK_FALL_WORK : IOBS_OBJECT_WORK
    {
        public readonly GMS_ENEMY_3D_WORK enemy_work;
        public GMS_EFFECT_3DES_WORK effect_work;
        public int wait_time;
        public ushort roll;
        public ushort roll_d;
        public GMS_ENEMY_3D_WORK hook_work;

        public GMS_GMK_ROCK_FALL_WORK()
        {
            this.enemy_work = new GMS_ENEMY_3D_WORK(this);
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.enemy_work.ene_com.obj_work;
        }
    }
}
