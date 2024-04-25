using System;
using System.Collections.Generic;
using System.Text;

internal class GmPlayer
{
    public static void SpdParameterSet(AppMain.GMS_PLAYER_WORK ply_work)
    {
        var g_gm_player_parameter = AppMain.g_gm_player_parameter;
        var ply_params = g_gm_player_parameter[ply_work.char_id];
        ply_work.spd_add = ply_params.spd_add;
        ply_work.spd_max = ply_params.spd_max;
        ply_work.spd1 = (int)(ply_work.spd_max * 0.15f);
        ply_work.spd2 = (int)(ply_work.spd_max * 0.3f);
        ply_work.spd3 = (int)(ply_work.spd_max * 0.5f);
        ply_work.spd4 = (int)(ply_work.spd_max * 0.7f);
        ply_work.spd5 = (int)(ply_work.spd_max * 0.9f);
        ply_work.spd_dec = ply_params.spd_dec;
        ply_work.spd_spin = ply_params.spd_spin;
        ply_work.spd_add_spin = ply_params.spd_add_spin;
        ply_work.spd_max_spin = ply_params.spd_max_spin;
        ply_work.spd_dec_spin = ply_params.spd_dec_spin;
        ply_work.spd_max_add_slope = ply_params.spd_max_add_slope;
        ply_work.spd_jump = ply_params.spd_jump;
        ply_work.time_air = ply_params.time_air;
        ply_work.time_air <<= 12;
        ply_work.time_damage = ply_params.time_damage;
        ply_work.time_damage <<= 12;
        ply_work.fall_timer = ply_params.fall_wait_time;
        ply_work.fall_timer <<= 12;
        ply_work.spd_jump_add = ply_params.spd_jump_add;
        ply_work.spd_jump_max = ply_params.spd_jump_max;
        ply_work.spd_jump_dec = ply_params.spd_jump_dec;
        ply_work.spd_add_spin_pinball = ply_params.spd_add_spin_pinball;
        ply_work.spd_max_spin_pinball = ply_params.spd_max_spin_pinball;
        ply_work.spd_dec_spin_pinball = ply_params.spd_dec_spin_pinball;
        ply_work.spd_max_add_slope_spin_pinball =
            ply_params.spd_max_add_slope_spin_pinball;

        ply_work.obj_work.dir_slope = AppMain.GSM_MAIN_STAGE_IS_SPSTAGE()
            ? (ushort)1
            : ((ply_work.player_flag & AppMain.GMD_PLF_TRUCK_RIDE) == 0 ? (ushort)192 : (ushort)512);

        ply_work.obj_work.spd_slope = ply_params.spd_slope;
        ply_work.obj_work.spd_slope_max = ply_params.spd_slope_max;
        ply_work.obj_work.spd_fall = ply_params.spd_fall;
        ply_work.obj_work.spd_fall_max = ply_params.spd_fall_max;
        ply_work.obj_work.push_max = ply_params.push_max;

        if ((ply_work.player_flag & AppMain.GMD_PLF_WATER) != 0)
        {
            AppMain.GMD_PLAYER_WATERJUMP_SET(ref ply_work.spd_jump);
            AppMain.GMD_PLAYER_WATER_SET(ref ply_work.obj_work.spd_fall);
        }

        if (ply_work.hi_speed_timer == 0)
            return;
        ply_work.spd_add <<= 1;
        if (ply_work.spd_add > 61440)
            ply_work.spd_add = 61440;
        ply_work.spd_max <<= 1;
        if (ply_work.spd_max > 61440)
            ply_work.spd_max = 61440;
        ply_work.spd_dec <<= 1;
        ply_work.spd_spin <<= 1;
        ply_work.spd_add_spin <<= 1;
        ply_work.spd_max_spin <<= 1;
        if (ply_work.spd_max_spin > 61440)
            ply_work.spd_max_spin = 61440;
        ply_work.spd_dec_spin <<= 1;
        ply_work.spd_max_add_slope <<= 1;
        ply_work.spd_jump_add <<= 1;
        ply_work.spd_jump_max <<= 1;
        if (ply_work.spd_jump_max > 61440)
            ply_work.spd_jump_max = 61440;
        ply_work.spd_jump_dec <<= 1;
    }
}
