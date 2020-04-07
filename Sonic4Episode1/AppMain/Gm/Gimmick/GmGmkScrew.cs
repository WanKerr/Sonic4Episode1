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
    private static AppMain.OBS_OBJECT_WORK GmGmkScrewInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_ENEMY_COM_WORK()), "GMK_SCREW");
        AppMain.GMS_ENEMY_COM_WORK gmsEnemyComWork = (AppMain.GMS_ENEMY_COM_WORK)work;
        work.move_flag |= 8480U;
        gmsEnemyComWork.rect_work[0].flag &= 4294967291U;
        gmsEnemyComWork.rect_work[1].flag &= 4294967291U;
        AppMain.OBS_RECT_WORK pRec = gmsEnemyComWork.rect_work[2];
        pRec.ppHit = (AppMain.OBS_RECT_WORK_Delegate1)null;
        pRec.ppDef = new AppMain.OBS_RECT_WORK_Delegate1(AppMain.gmGmkScrewDefFunc);
        AppMain.ObjRectAtkSet(pRec, (ushort)0, (short)0);
        AppMain.ObjRectDefSet(pRec, (ushort)65534, (short)0);
        if (((int)eve_rec.flag & AppMain.GMD_GMK_SCREW_EVE_FLAG_LEFT) != 0)
            AppMain.ObjRectWorkSet(pRec, (short)-4, (short)-8, (short)-16, (short)0);
        else
            AppMain.ObjRectWorkSet(pRec, (short)4, (short)-8, (short)16, (short)0);
        work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkScrewMain);
        return work;
    }

    private static void gmGmkScrewMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_PLAYER_WORK gmsPlayerWork = AppMain.g_gm_main_system.ply_work[0];
        if (gmsPlayerWork.seq_state == 38 || gmsPlayerWork.gmk_obj == obj_work)
            return;
        obj_work.flag &= 4294967279U;
    }

    private static void gmGmkScrewDefFunc(
      AppMain.OBS_RECT_WORK mine_rect,
      AppMain.OBS_RECT_WORK match_rect)
    {
        AppMain.GMS_ENEMY_COM_WORK parentObj1 = (AppMain.GMS_ENEMY_COM_WORK)mine_rect.parent_obj;
        AppMain.GMS_PLAYER_WORK parentObj2 = (AppMain.GMS_PLAYER_WORK)match_rect.parent_obj;
        if (parentObj1 == null || parentObj2 == null || (parentObj2.obj_work.obj_type != (ushort)1 || parentObj2.seq_state == 38))
            return;
        int spd3 = parentObj2.spd3;
        ushort flag = parentObj1.eve_rec.flag;
        if ((parentObj2.obj_work.spd_m < spd3 || ((int)parentObj1.eve_rec.flag & AppMain.GMD_GMK_SCREW_EVE_FLAG_LEFT) != 0) && (parentObj2.obj_work.spd_m > -spd3 || ((int)parentObj1.eve_rec.flag & AppMain.GMD_GMK_SCREW_EVE_FLAG_LEFT) == 0) || ((int)parentObj2.obj_work.move_flag & 1) == 0)
            return;
        AppMain.GmPlySeqInitScrew(parentObj2, parentObj1, parentObj1.obj_work.pos.x, parentObj1.obj_work.pos.y, flag);
        parentObj1.obj_work.flag |= 16U;
    }

}