public partial class AppMain
{
    public class GMS_GMK_P_STEAM_WORK : IOBS_OBJECT_WORK
    {
        public OBS_OBJECT_WORK[] opt_steam_int = new OBS_OBJECT_WORK[3];
        public const int GMD_GMK_PSTEAM_STAT_HIT = 1;
        public const int GMD_GMK_PSTEAM_STAT_TRUE = 2;
        public readonly GMS_ENEMY_3D_WORK gmk_work;
        public int obj_type;
        public short timer;
        public ushort steamvect;
        public short steamsize;
        public short steamwait;
        public int steampower;
        public ushort status;
        public GMS_PLAYER_WORK ply_work;
        public OBS_OBJECT_WORK opt_timer;
        public OBS_OBJECT_WORK opt_steam;
        public int pos_x;
        public int pos_y;

        public GMS_GMK_P_STEAM_WORK()
        {
            this.gmk_work = new GMS_ENEMY_3D_WORK(this);
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.gmk_work.ene_com.obj_work;
        }
    }
}
