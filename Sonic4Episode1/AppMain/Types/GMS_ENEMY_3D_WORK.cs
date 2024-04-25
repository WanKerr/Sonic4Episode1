public partial class AppMain
{
    public class GMS_ENEMY_3D_WORK : IOBS_OBJECT_WORK
    {
        public readonly OBS_ACTION3D_NN_WORK obj_3d = new OBS_ACTION3D_NN_WORK();
        public readonly GMS_ENEMY_COM_WORK ene_com;
        public readonly object holder;

        public GMS_ENEMY_3D_WORK()
        {
            this.ene_com = new GMS_ENEMY_COM_WORK(this);
        }

        public GMS_ENEMY_3D_WORK(object _holder)
        {
            this.ene_com = new GMS_ENEMY_COM_WORK(this);
            this.holder = _holder;
        }

        public static explicit operator GMS_GMK_GEAR_WORK(GMS_ENEMY_3D_WORK p)
        {
            return (GMS_GMK_GEAR_WORK)p.holder;
        }

        public static explicit operator GMS_GMK_WATER_SLIDER_WORK(
          GMS_ENEMY_3D_WORK p)
        {
            return (GMS_GMK_WATER_SLIDER_WORK)p.holder;
        }

        public static explicit operator GMS_GMK_UPBUMPER_WORK(GMS_ENEMY_3D_WORK p)
        {
            return (GMS_GMK_UPBUMPER_WORK)p.holder;
        }

        public static explicit operator GMS_GMK_STEAMP_WORK(GMS_ENEMY_3D_WORK p)
        {
            return (GMS_GMK_STEAMP_WORK)p.holder;
        }

        public static explicit operator GMS_GMK_SEESAW_WORK(GMS_ENEMY_3D_WORK p)
        {
            return (GMS_GMK_SEESAW_WORK)p.holder;
        }

        public static explicit operator GMS_GMK_PWALL_WORK(GMS_ENEMY_3D_WORK p)
        {
            return (GMS_GMK_PWALL_WORK)p.holder;
        }

        public static explicit operator GMS_GMK_PWALLCTRL_WORK(
          GMS_ENEMY_3D_WORK p)
        {
            return (GMS_GMK_PWALLCTRL_WORK)p.holder;
        }

        public static explicit operator GMS_GMK_SHUTTER_WORK(GMS_ENEMY_3D_WORK p)
        {
            return (GMS_GMK_SHUTTER_WORK)p.holder;
        }

        public static explicit operator GMS_GMK_P_STEAM_WORK(GMS_ENEMY_3D_WORK p)
        {
            return (GMS_GMK_P_STEAM_WORK)p.holder;
        }

        public static explicit operator GMS_GMK_PISTON_WORK(GMS_ENEMY_3D_WORK p)
        {
            return (GMS_GMK_PISTON_WORK)p.holder;
        }

        public static explicit operator GMS_GMK_DRAIN_TANK_OUT_WORK(
          GMS_ENEMY_3D_WORK p)
        {
            return (GMS_GMK_DRAIN_TANK_OUT_WORK)p.holder;
        }

        public static explicit operator GMS_GMK_CANNON_WORK(GMS_ENEMY_3D_WORK p)
        {
            return (GMS_GMK_CANNON_WORK)p.holder;
        }

        public static explicit operator GMS_GMK_BOSS3_PILLAR_MAIN_WORK(
          GMS_ENEMY_3D_WORK p)
        {
            return (GMS_GMK_BOSS3_PILLAR_MAIN_WORK)p.holder;
        }

        public static explicit operator GMS_GMK_BOSS3_PILLAR_MANAGER_WORK(
          GMS_ENEMY_3D_WORK p)
        {
            return (GMS_GMK_BOSS3_PILLAR_MANAGER_WORK)p.holder;
        }

        public static explicit operator GMS_GMK_BOSS3_PILLAR_WALL_WORK(
          GMS_ENEMY_3D_WORK p)
        {
            return (GMS_GMK_BOSS3_PILLAR_WALL_WORK)p.holder;
        }

        public static explicit operator GMS_BOSS5_LAND_WORK(GMS_ENEMY_3D_WORK p)
        {
            return (GMS_BOSS5_LAND_WORK)p.holder;
        }

        public static explicit operator GMS_BOSS5_CTPLT_WORK(GMS_ENEMY_3D_WORK p)
        {
            return (GMS_BOSS5_CTPLT_WORK)p.holder;
        }

        public static explicit operator GMS_BOSS5_TURRET_WORK(GMS_ENEMY_3D_WORK p)
        {
            return (GMS_BOSS5_TURRET_WORK)p.holder;
        }

        public static explicit operator GMS_BOSS5_EGG_WORK(GMS_ENEMY_3D_WORK p)
        {
            return (GMS_BOSS5_EGG_WORK)p.holder;
        }

        public static explicit operator GMS_BOSS5_ROCKET_WORK(GMS_ENEMY_3D_WORK p)
        {
            return (GMS_BOSS5_ROCKET_WORK)p.holder;
        }

        public static explicit operator GMS_BOSS5_MGR_WORK(GMS_ENEMY_3D_WORK p)
        {
            return (GMS_BOSS5_MGR_WORK)p.holder;
        }

        public static explicit operator GMS_BOSS5_BODY_WORK(GMS_ENEMY_3D_WORK p)
        {
            return (GMS_BOSS5_BODY_WORK)p.holder;
        }

        public static explicit operator GMS_BOSS4_CHIBI_WORK(GMS_ENEMY_3D_WORK p)
        {
            return (GMS_BOSS4_CHIBI_WORK)p.holder;
        }

        public static explicit operator GMS_BOSS4_BODY_WORK(GMS_ENEMY_3D_WORK p)
        {
            return (GMS_BOSS4_BODY_WORK)p.holder;
        }

        public static explicit operator GMS_BOSS4_CAP_WORK(GMS_ENEMY_3D_WORK p)
        {
            return (GMS_BOSS4_CAP_WORK)p.holder;
        }

        public static explicit operator GMS_BOSS4_EGG_WORK(GMS_ENEMY_3D_WORK p)
        {
            return (GMS_BOSS4_EGG_WORK)p.holder;
        }

        public static explicit operator GMS_BOSS4_MGR_WORK(GMS_ENEMY_3D_WORK p)
        {
            return (GMS_BOSS4_MGR_WORK)p.holder;
        }

        public static explicit operator GMS_GMK_TRUCK_WORK(GMS_ENEMY_3D_WORK p)
        {
            return (GMS_GMK_TRUCK_WORK)p.holder;
        }

        public static explicit operator GMS_BOSS2_BALL_WORK(GMS_ENEMY_3D_WORK p)
        {
            return (GMS_BOSS2_BALL_WORK)p.holder;
        }

        public static explicit operator GMS_BOSS2_EGG_WORK(GMS_ENEMY_3D_WORK p)
        {
            return (GMS_BOSS2_EGG_WORK)p.holder;
        }

        public static explicit operator GMS_BOSS2_BODY_WORK(GMS_ENEMY_3D_WORK p)
        {
            return (GMS_BOSS2_BODY_WORK)p.holder;
        }

        public static explicit operator GMS_BOSS2_MGR_WORK(GMS_ENEMY_3D_WORK p)
        {
            return (GMS_BOSS2_MGR_WORK)p.holder;
        }

        public static explicit operator GMS_BOSS3_EGG_WORK(GMS_ENEMY_3D_WORK p)
        {
            return (GMS_BOSS3_EGG_WORK)p.holder;
        }

        public static explicit operator GMS_BOSS3_BODY_WORK(GMS_ENEMY_3D_WORK p)
        {
            return (GMS_BOSS3_BODY_WORK)p.holder;
        }

        public static explicit operator GMS_BOSS3_MGR_WORK(GMS_ENEMY_3D_WORK p)
        {
            return (GMS_BOSS3_MGR_WORK)p.holder;
        }

        public static explicit operator GMS_ENE_UNIUNI_WORK(GMS_ENEMY_3D_WORK p)
        {
            return (GMS_ENE_UNIUNI_WORK)p.holder;
        }

        public static explicit operator GMS_ENE_UNIDES_WORK(GMS_ENEMY_3D_WORK p)
        {
            return (GMS_ENE_UNIDES_WORK)p.holder;
        }

        public static explicit operator GMS_ENE_BUKU_WORK(GMS_ENEMY_3D_WORK p)
        {
            return (GMS_ENE_BUKU_WORK)p.holder;
        }

        public static explicit operator GMS_ENE_T_STAR_WORK(GMS_ENEMY_3D_WORK p)
        {
            return (GMS_ENE_T_STAR_WORK)p.holder;
        }

        public static explicit operator GMS_ENE_KANI_WORK(GMS_ENEMY_3D_WORK p)
        {
            return (GMS_ENE_KANI_WORK)p.holder;
        }

        public static explicit operator GMS_ENE_KAMA_WORK(GMS_ENEMY_3D_WORK p)
        {
            return p == null ? null : (GMS_ENE_KAMA_WORK)p.holder;
        }

        public static explicit operator GMS_ENE_MOGU_WORK(GMS_ENEMY_3D_WORK p)
        {
            return (GMS_ENE_MOGU_WORK)p.holder;
        }

        public static explicit operator GMS_GMK_SPCTPLT_WORK(GMS_ENEMY_3D_WORK p)
        {
            return (GMS_GMK_SPCTPLT_WORK)p.holder;
        }

        public static explicit operator GMS_GMK_SLOT_WORK(GMS_ENEMY_3D_WORK p)
        {
            return (GMS_GMK_SLOT_WORK)p.holder;
        }

        public static explicit operator GMS_GMK_STOPPER_WORK(GMS_ENEMY_3D_WORK p)
        {
            return (GMS_GMK_STOPPER_WORK)p.holder;
        }

        public static explicit operator GMS_ENE_HARO_WORK(GMS_ENEMY_3D_WORK p)
        {
            return (GMS_ENE_HARO_WORK)p.holder;
        }

        public static explicit operator GMS_ENE_GARDON_WORK(GMS_ENEMY_3D_WORK p)
        {
            return (GMS_ENE_GARDON_WORK)p.holder;
        }

        public static explicit operator GMS_GMK_BUMPER_WORK(GMS_ENEMY_3D_WORK p)
        {
            return (GMS_GMK_BUMPER_WORK)p.holder;
        }

        public static explicit operator GMS_BOSS1_CHAIN_WORK(GMS_ENEMY_3D_WORK p)
        {
            return (GMS_BOSS1_CHAIN_WORK)p.holder;
        }

        public static explicit operator GMS_BOSS1_MGR_WORK(GMS_ENEMY_3D_WORK p)
        {
            return (GMS_BOSS1_MGR_WORK)p.holder;
        }

        public static explicit operator GMS_BOSS1_EGG_WORK(GMS_ENEMY_3D_WORK p)
        {
            return (GMS_BOSS1_EGG_WORK)p.holder;
        }

        public static explicit operator GMS_BOSS1_BODY_WORK(GMS_ENEMY_3D_WORK p)
        {
            return (GMS_BOSS1_BODY_WORK)p.holder;
        }

        public static explicit operator GMS_GMK_PULLEY_WORK(GMS_ENEMY_3D_WORK p)
        {
            return (GMS_GMK_PULLEY_WORK)p.holder;
        }

        public static explicit operator GMS_ENE_STING_WORK(GMS_ENEMY_3D_WORK p)
        {
            return (GMS_ENE_STING_WORK)p.holder;
        }

        public static explicit operator GMS_GMK_BLAND_WORK(GMS_ENEMY_3D_WORK p)
        {
            return (GMS_GMK_BLAND_WORK)p.holder;
        }

        public static explicit operator GMS_GMK_PMARKER_WORK(GMS_ENEMY_3D_WORK p)
        {
            return (GMS_GMK_PMARKER_WORK)p.holder;
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.ene_com.obj_work;
        }

        public static explicit operator OBS_OBJECT_WORK(GMS_ENEMY_3D_WORK work)
        {
            return work.ene_com.obj_work;
        }

        public static explicit operator GMS_GMK_NEEDLE_WORK(
          GMS_ENEMY_3D_WORK work)
        {
            return (GMS_GMK_NEEDLE_WORK)work.holder;
        }

        public static explicit operator GMS_ENE_HARI_WORK(GMS_ENEMY_3D_WORK work)
        {
            return (GMS_ENE_HARI_WORK)work.holder;
        }

        public static explicit operator GMS_ENE_MOTORA_WORK(
          GMS_ENEMY_3D_WORK work)
        {
            return (GMS_ENE_MOTORA_WORK)work.holder;
        }

        public static explicit operator GMS_GMK_BOBJ_WORK(GMS_ENEMY_3D_WORK work)
        {
            return (GMS_GMK_BOBJ_WORK)work.holder;
        }

        public static explicit operator GMS_GMK_BOBJ_PARTS(GMS_ENEMY_3D_WORK work)
        {
            return (GMS_GMK_BOBJ_PARTS)work.holder;
        }

        public static explicit operator GMS_GMK_BELTC_WORK(GMS_ENEMY_3D_WORK p)
        {
            return (GMS_GMK_BELTC_WORK)p.holder;
        }

        public static explicit operator GMS_GMK_ROCK_FALL_WORK(
          GMS_ENEMY_3D_WORK work)
        {
            return (GMS_GMK_ROCK_FALL_WORK)work.holder;
        }

        public static explicit operator GMS_GMK_ROCK_FALL_MGR_WORK(
          GMS_ENEMY_3D_WORK work)
        {
            return (GMS_GMK_ROCK_FALL_MGR_WORK)work.holder;
        }

        public static explicit operator GMS_GMK_ROCK_CHASE_WORK(
          GMS_ENEMY_3D_WORK work)
        {
            return (GMS_GMK_ROCK_CHASE_WORK)work.holder;
        }
    }
}
