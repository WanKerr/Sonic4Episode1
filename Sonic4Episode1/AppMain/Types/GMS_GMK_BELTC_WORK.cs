public partial class AppMain
{
    public class GMS_GMK_BELTC_WORK : IOBS_OBJECT_WORK
    {
        public readonly GMS_ENEMY_3D_WORK gmk_work;
        public bool last_under;
        public ushort vect;
        public short width;
        public int diradd;
        public int rolldir;
        public int speed;
        public int roller;
        public float tex_u;

        public GMS_GMK_BELTC_WORK()
        {
            this.gmk_work = new GMS_ENEMY_3D_WORK(this);
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.gmk_work.ene_com.obj_work;
        }

        public static explicit operator OBS_OBJECT_WORK(GMS_GMK_BELTC_WORK work)
        {
            return work.gmk_work.ene_com.obj_work;
        }

        public static explicit operator GMS_ENEMY_3D_WORK(GMS_GMK_BELTC_WORK work)
        {
            return work.gmk_work;
        }
    }
}
