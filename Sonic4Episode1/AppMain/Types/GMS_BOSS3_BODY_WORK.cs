public partial class AppMain
{
    public class GMS_BOSS3_BODY_WORK : IOBS_OBJECT_WORK
    {
        public OBS_OBJECT_WORK[] parts_objs = new OBS_OBJECT_WORK[2];
        public VecFx32 start_pos = new VecFx32();
        public VecFx32 end_pos = new VecFx32();
        public readonly GMS_BS_CMN_BMCB_MGR bmcb_mgr = new GMS_BS_CMN_BMCB_MGR();
        public readonly GMS_BS_CMN_SNM_WORK snm_work = new GMS_BS_CMN_SNM_WORK();
        public int[] snm_reg_id = new int[1];
        public readonly GMS_BS_CMN_DMG_FLICKER_WORK flk_work = new GMS_BS_CMN_DMG_FLICKER_WORK();
        public readonly GMS_CMN_FLASH_SCR_WORK flash_work = new GMS_CMN_FLASH_SCR_WORK();
        public readonly GMS_BOSS3_EFF_BOMB_WORK bomb_work = new GMS_BOSS3_EFF_BOMB_WORK();
        public readonly GMS_ENEMY_3D_WORK ene_3d;
        public int state;
        public int prev_state;
        public GMF_BOSS3_BODY_STATE_FUNC proc_update;
        public uint flag;
        public int action_id;
        public int pattern_no;
        public short angle_current;
        public float move_counter;
        public float move_frame;
        public short turn_start;
        public int turn_amount;
        public float turn_counter;
        public float turn_frame;
        public int is_move;
        public uint counter_no_hit;
        public uint counter_invincible;

        public GMS_BOSS3_BODY_WORK()
        {
            this.ene_3d = new GMS_ENEMY_3D_WORK(this);
        }

        public static explicit operator OBS_OBJECT_WORK(GMS_BOSS3_BODY_WORK p)
        {
            return p.ene_3d.ene_com.obj_work;
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.ene_3d.ene_com.obj_work;
        }
    }
}
