public partial class AppMain
{
    public class GMS_BOSS1_BODY_WORK : IOBS_OBJECT_WORK
    {
        public readonly ushort[] reserved = new ushort[1];
        public readonly GMS_BS_CMN_BMCB_MGR bmcb_mgr = new GMS_BS_CMN_BMCB_MGR();
        public readonly GMS_BS_CMN_SNM_WORK snm_work = new GMS_BS_CMN_SNM_WORK();
        public readonly GMS_BS_CMN_CNM_MGR_WORK cnm_mgr_work = new GMS_BS_CMN_CNM_MGR_WORK();
        public readonly GMS_BS_CMN_DMG_FLICKER_WORK flk_work = new GMS_BS_CMN_DMG_FLICKER_WORK();
        public readonly GMS_BOSS1_1SHOT_TIMER se_timer = new GMS_BOSS1_1SHOT_TIMER();
        public readonly GMS_BOSS1_EFF_BOMB_WORK bomb_work = new GMS_BOSS1_EFF_BOMB_WORK();
        public readonly OBS_OBJECT_WORK[] parts_objs = new OBS_OBJECT_WORK[3];
        public readonly GMS_BOSS1_MTN_SUSPEND_WORK[] mtn_suspend = New<GMS_BOSS1_MTN_SUSPEND_WORK>(3);
        public readonly GMS_ENEMY_3D_WORK ene_3d;
        public int state;
        public int prev_state;
        public GMS_BOSS1_MGR_WORK mgr_work;
        public MPP_VOID_GMS_BOSS1_BODY_WORK proc_update;
        public uint flag;
        public int whole_act_id;
        public ushort egg_revert_mtn_id;
        public uint wait_timer;
        public int chain_snm_reg_id;
        public int egg_snm_reg_id;
        public int body_snm_reg_id;
        public int chaintop_snm_reg_id;
        public int chaintop_cnm_reg_id;
        public uint se_cnt;
        public uint no_hit_timer;
        public int move_time;
        public int move_cnt;
        public short cur_angle;
        public short orig_angle;
        public int turn_angle;
        public int turn_amount;
        public int turn_spd;
        public int turn_gen_var;
        public int turn_gen_factor;
        public int drift_pivot_x;
        public int drift_angle;
        public int drift_ang_spd;
        public int drift_timer;
        public int atk_nml_alt;
        public VecFx32 bash_targ_pos;
        public VecFx32 bash_ret_pos;
        public VecFx32 bash_orig_pos;
        public int bash_homing_deg;

        public GMS_BOSS1_BODY_WORK()
        {
            this.ene_3d = new GMS_ENEMY_3D_WORK(this);
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.ene_3d.ene_com.obj_work;
        }

        public static explicit operator GMS_ENEMY_COM_WORK(GMS_BOSS1_BODY_WORK p)
        {
            return p.ene_3d.ene_com;
        }
    }
}
