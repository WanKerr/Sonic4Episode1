public partial class AppMain
{
    public class GMS_ENE_KAMA_WORK : IOBS_OBJECT_WORK
    {
        public readonly GMS_ENE_NODE_MATRIX node_work = new GMS_ENE_NODE_MATRIX();
        public readonly GMS_ENEMY_3D_WORK ene_3d_work;
        public int spd_dec;
        public int spd_dec_dist;
        public int hand;
        public int attack;
        public int timer;
        public int rot_z;
        public int rot_z_add;
        public int atk_wait;
        public int walk_s;
        public int ata_futa;
        public GMS_ENE_KAMA_FADE_ANIME anime_data;
        public uint anime_pat_no;
        public int anime_frame;

        public OBS_OBJECT_WORK Cast()
        {
            return this.ene_3d_work.ene_com.obj_work;
        }

        public static explicit operator OBS_OBJECT_WORK(GMS_ENE_KAMA_WORK work)
        {
            return work.ene_3d_work.ene_com.obj_work;
        }

        public GMS_ENE_KAMA_WORK()
        {
            this.ene_3d_work = new GMS_ENEMY_3D_WORK(this);
        }
    }
}
