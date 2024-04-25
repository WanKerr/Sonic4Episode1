public partial class AppMain
{
    public class GMS_GMK_PWALLCTRL_WORK : IOBS_OBJECT_WORK
    {
        public readonly GMS_ENEMY_3D_WORK gmk_work;
        public int line_top;
        public int line_bottom;
        public GMS_PLAYER_WORK ply_work;
        public int last_ply_x;
        public int last_ply_y;

        public GMS_GMK_PWALLCTRL_WORK()
        {
            this.gmk_work = new GMS_ENEMY_3D_WORK(this);
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.gmk_work.ene_com.obj_work;
        }

        public int line_left
        {
            get => this.line_top;
            set => this.line_top = value;
        }

        public int line_right
        {
            get => this.line_bottom;
            set => this.line_bottom = value;
        }
    }
}
