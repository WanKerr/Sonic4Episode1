public partial class AppMain
{
    public class GMS_GMK_BWALL_WORK : IOBS_OBJECT_WORK
    {
        public readonly GMS_ENEMY_3D_WORK gmk_work;
        public int obj_type;
        public int wall_type;
        public int hitpass;
        public short hitcheck;
        public ushort broketype;
        public ushort vect;

        public static explicit operator GMS_ENEMY_3D_WORK(GMS_GMK_BWALL_WORK p)
        {
            return p.gmk_work;
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.gmk_work.ene_com.obj_work;
        }

        public static explicit operator OBS_OBJECT_WORK(GMS_GMK_BWALL_WORK work)
        {
            return work.gmk_work.ene_com.obj_work;
        }

        public GMS_GMK_BWALL_WORK()
        {
            this.gmk_work = new GMS_ENEMY_3D_WORK(this);
        }
    }
}
