public partial class AppMain
{
    public class GMS_BOSS4_CHIBI_WORK : IOBS_OBJECT_WORK
    {
        public readonly GMS_BOSS4_1SHOT_TIMER timer = new GMS_BOSS4_1SHOT_TIMER();
        public readonly GMS_BOSS4_EFF_BOMB_WORK bomb = new GMS_BOSS4_EFF_BOMB_WORK();
        public readonly GMS_BOSS4_FLICKER_WORK flk_work = new GMS_BOSS4_FLICKER_WORK();
        public readonly GMS_BOSS4_DIRECTION dir = new GMS_BOSS4_DIRECTION();
        public readonly GMS_BOSS4_NODE_MATRIX node_work = new GMS_BOSS4_NODE_MATRIX();
        public readonly GMS_ENEMY_3D_WORK ene_3d;
        public int type;
        public uint flag;
        public int act_id;
        public int cap_no;
        public int wait;
        public int count;
        public int bound;
        public int bnd_xspd;
        public GMS_EFFECT_3DES_WORK boost;
        public MPP_VOID_GMS_BOSS4_CHIBI_WORK proc_update;

        public GMS_BOSS4_CHIBI_WORK()
        {
            this.ene_3d = new GMS_ENEMY_3D_WORK(this);
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.ene_3d.ene_com.obj_work;
        }

        public static explicit operator OBS_OBJECT_WORK(GMS_BOSS4_CHIBI_WORK work)
        {
            return work.ene_3d.ene_com.obj_work;
        }
    }
}
