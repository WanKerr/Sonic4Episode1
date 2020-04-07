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
    private static AppMain.OBS_OBJECT_WORK GmGmkSpipeInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_ENEMY_COM_WORK()), "GMK_S_PIPE");
        AppMain.GMS_ENEMY_COM_WORK gmsEnemyComWork = (AppMain.GMS_ENEMY_COM_WORK)work;
        work.move_flag |= 8480U;
        work.pos.z = -131072;
        AppMain.OBS_RECT_WORK pRec = gmsEnemyComWork.rect_work[2];
        AppMain.ObjRectGroupSet(pRec, (byte)1, (byte)1);
        AppMain.ObjRectAtkSet(pRec, (ushort)0, (short)1);
        AppMain.ObjRectDefSet(pRec, (ushort)65534, (short)0);
        AppMain.ObjRectSet(pRec.rect, (short)((int)eve_rec.left * 2), (short)((int)eve_rec.top * 2), (short)((int)(ushort)((uint)eve_rec.width * 2U) + (int)(short)((int)eve_rec.left * 2)), (short)((int)(ushort)((uint)eve_rec.height * 2U) + (int)(short)((int)eve_rec.top * 2)));
        pRec.ppDef = new AppMain.OBS_RECT_WORK_Delegate1(AppMain.gmGmkSpipeDefFunc);
        pRec.parent_obj = work;
        pRec.flag |= 192U;
        return work;
    }

    private static void gmGmkSpipeDefFunc(
      AppMain.OBS_RECT_WORK mine_rect,
      AppMain.OBS_RECT_WORK match_rect)
    {
        AppMain.GMS_ENEMY_COM_WORK parentObj1 = (AppMain.GMS_ENEMY_COM_WORK)mine_rect.parent_obj;
        AppMain.GMS_PLAYER_WORK parentObj2 = (AppMain.GMS_PLAYER_WORK)match_rect.parent_obj;
        if (parentObj1 == null || parentObj2 == null || parentObj2.obj_work.obj_type != (ushort)1)
            return;
        if (parentObj2.seq_state != 37)
            AppMain.GmPlySeqInitSpipe(parentObj2);
        parentObj2.gmk_flag |= 65536U;
    }

}