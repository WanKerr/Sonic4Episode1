public partial class AppMain
{
    public class GMS_BOSS4_BODY_WORK : IOBS_OBJECT_WORK
    {
        public readonly uint[] flag = new uint[1];
        public readonly ushort[] reserved = new ushort[1];
        public readonly GMS_BOSS4_NODE_MATRIX node_work = new GMS_BOSS4_NODE_MATRIX();
        public readonly GMS_BS_CMN_CNM_MGR_WORK cnm_mgr_work = new GMS_BS_CMN_CNM_MGR_WORK();
        public readonly GMS_BS_CMN_DMG_FLICKER_WORK flk_work = new GMS_BS_CMN_DMG_FLICKER_WORK();
        public readonly GMS_BOSS4_1SHOT_TIMER se_timer = new GMS_BOSS4_1SHOT_TIMER();
        public readonly GMS_BOSS4_MOVE move_work = new GMS_BOSS4_MOVE();
        public readonly GMS_BOSS4_DIRECTION dir = new GMS_BOSS4_DIRECTION();
        public readonly GMS_BOSS4_EFF_BOMB_WORK bomb_work = new GMS_BOSS4_EFF_BOMB_WORK();
        public readonly GMS_BOSS4_EFF_BOMB_WORK bomb_work2 = new GMS_BOSS4_EFF_BOMB_WORK();
        public readonly OBS_OBJECT_WORK[] parts_objs = new OBS_OBJECT_WORK[2];
        public readonly GMS_BOSS4_MTN_SUSPEND_WORK[] mtn_suspend = New<GMS_BOSS4_MTN_SUSPEND_WORK>(2);
        public readonly GMS_BOSS4_NOHIT_TIMER nohit_work = new GMS_BOSS4_NOHIT_TIMER();
        public readonly GMS_CMN_FLASH_SCR_WORK flash_work = new GMS_CMN_FLASH_SCR_WORK();
        public readonly GMS_ENEMY_3D_WORK ene_3d;
        public int state;
        public int prev_state;
        public GMS_BOSS4_MGR_WORK mgr_work;
        public MPP_VOID_GMS_BOSS4_BODY_WORK proc_update;
        public int whole_act_id;
        public int egg_revert_mtn_id;
        public uint wait_timer;
        public uint wait_timer2;
        public int chaintop_cnm_reg_id;
        public uint se_cnt;
        public int move_time;
        public int move_cnt;
        public int drift_pivot_x;
        public int drift_angle;
        public int drift_ang_spd;
        public int drift_timer;
        public int atk_nml_alt;
        public VecFx32 bash_targ_pos;
        public VecFx32 bash_ret_pos;
        public VecFx32 bash_orig_pos;
        public int bash_homing_deg;
        public int damage_timer;
        public int avoid_timer;
        public int avoid_yspd;

        public GMS_BOSS4_BODY_WORK()
        {
            this.ene_3d = new GMS_ENEMY_3D_WORK(this);
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.ene_3d.ene_com.obj_work;
        }

        public static explicit operator OBS_OBJECT_WORK(GMS_BOSS4_BODY_WORK work)
        {
            return work?.ene_3d.ene_com.obj_work;
        }

        public static explicit operator GMS_ENEMY_COM_WORK(
          GMS_BOSS4_BODY_WORK work)
        {
            return work.ene_3d.ene_com;
        }

        public static explicit operator GMS_ENEMY_3D_WORK(
          GMS_BOSS4_BODY_WORK work)
        {
            return work.ene_3d;
        }
    }
}
