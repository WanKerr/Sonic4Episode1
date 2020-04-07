using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using mpp;

public partial class AppMain
{
    private static AppMain.OBS_OBJECT_WORK GmGmkEndingAnimalInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_ENEMY_3D_WORK()), "GMK_END_ANIMAL");
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work;
        uint num = (uint)eve_rec.flag & 7U;
        work.user_work = num;
        AppMain.gmGmkAnimalObjSet(work, gmsEnemy3DWork.obj_3d);
        work.user_flag = ((int)eve_rec.flag & 16) == 0 ? 0U : 2U;
        work.disp_flag |= 4259840U;
        work.move_flag &= 4294952703U;
        work.move_flag |= 1680U;
        work.spd.y = -AppMain.g_gm_gmk_animal_speed_param[(int)work.user_work].jump;
        work.spd_fall = AppMain.g_gm_gmk_animal_speed_param[(int)work.user_work].gravity;
        work.pos.z = -131072;
        work.flag |= 512U;
        work.flag |= 2U;
        work.flag &= 4294967279U;
        work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkEndingAnimalMove);
        return work;
    }
}