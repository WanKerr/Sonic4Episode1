public partial class AppMain
{
    public class GMS_GMK_DRAIN_TANK_OUT_WORK : IOBS_OBJECT_WORK
    {
        public readonly GMS_ENEMY_3D_WORK enemy_work;
        public bool flag_dir_left;
        public int base_pos_x;
        public int base_pos_y;
        public int player_offset_x;
        public int player_offset_y;
        public int camera_roll;
        public int counter_roll_key;

        public GMS_GMK_DRAIN_TANK_OUT_WORK()
        {
            this.enemy_work = new GMS_ENEMY_3D_WORK(this);
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.enemy_work.ene_com.obj_work;
        }
    }
}
