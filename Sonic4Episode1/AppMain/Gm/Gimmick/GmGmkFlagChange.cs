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
    private static AppMain.OBS_OBJECT_WORK GmGmkFlagChangeInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_ENEMY_COM_WORK()), "GMK_FLAG_CNG");
        AppMain.OBS_RECT_WORK pRec = ((AppMain.GMS_ENEMY_COM_WORK)work).rect_work[2];
        AppMain.ObjRectGroupSet(pRec, (byte)1, (byte)1);
        AppMain.ObjRectAtkSet(pRec, (ushort)0, (short)1);
        AppMain.ObjRectDefSet(pRec, (ushort)65534, (short)0);
        AppMain.ObjRectSet(pRec.rect, (short)eve_rec.left, (short)eve_rec.top, (short)((int)eve_rec.width + (int)eve_rec.left), (short)((int)eve_rec.height + (int)eve_rec.top));
        pRec.ppDef = new AppMain.OBS_RECT_WORK_Delegate1(AppMain.gmGmkFlagChangeDefFunc);
        pRec.parent_obj = work;
        pRec.flag |= 192U;
        work.move_flag |= 8480U;
        return work;
    }

    private static void gmGmkFlagChangeDefFunc(
      AppMain.OBS_RECT_WORK mine_rect,
      AppMain.OBS_RECT_WORK match_rect)
    {
        if (match_rect.parent_obj == null || match_rect.parent_obj.obj_type != (ushort)1)
            return;
        AppMain.GMS_ENEMY_COM_WORK parentObj1 = (AppMain.GMS_ENEMY_COM_WORK)mine_rect.parent_obj;
        AppMain.GMS_PLAYER_WORK parentObj2 = (AppMain.GMS_PLAYER_WORK)match_rect.parent_obj;
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)parentObj2;
        switch (parentObj1.eve_rec.id)
        {
            case 60:
                if (((int)parentObj1.eve_rec.flag & 128) != 0)
                {
                    if (parentObj2.seq_state != 29 && parentObj2.seq_state != 49 && (parentObj2.seq_state != 50 && parentObj2.seq_state != 42))
                        break;
                    parentObj2.gmk_flag |= 2048U;
                }
                if (obsObjectWork.obj_type != (ushort)1 || ((int)obsObjectWork.move_flag & 16) == 0)
                    break;
                parentObj2.gmk_flag |= 1U;
                if (((int)parentObj1.eve_rec.flag & 16) != 0)
                {
                    parentObj2.gmk_flag |= 2U;
                    break;
                }
                if (((int)parentObj1.eve_rec.flag & 2) != 0)
                {
                    if (((int)parentObj2.obj_work.disp_flag & 1) != 0)
                        break;
                    parentObj2.gmk_flag |= 33554432U;
                    break;
                }
                if (((int)parentObj1.eve_rec.flag & 1) == 0 || ((int)parentObj2.obj_work.disp_flag & 1) == 0)
                    break;
                parentObj2.gmk_flag |= 33554432U;
                break;
            case 61:
                obsObjectWork.flag &= 4294967294U;
                if (obsObjectWork.obj_type != (ushort)1)
                    break;
                parentObj2.graind_prev_ride = (byte)0;
                break;
            case 62:
                obsObjectWork.flag |= 1U;
                if (obsObjectWork.obj_type != (ushort)1)
                    break;
                parentObj2.graind_prev_ride = (byte)0;
                break;
            case 99:
                if (obsObjectWork.obj_type != (ushort)1)
                    break;
                AppMain.GmPlySeqChangeDeath(parentObj2);
                break;
            case 131:
                if (obsObjectWork.obj_type != (ushort)1)
                    break;
                ushort flag = parentObj1.eve_rec.flag;
                short num1 = (short)((int)flag & 7);
                if (((int)flag & 8) != 0)
                    num1 = (short)-num1;
                parentObj2.gmk_camera_center_ofst_x = (short)((int)num1 << 3);
                short num2 = (short)(((int)flag & 112) >> 4);
                if (((int)flag & 128) != 0)
                    num2 = (short)-num2;
                parentObj2.gmk_camera_center_ofst_y = (short)((int)num2 << 3);
                break;
            case 162:
                if (obsObjectWork.obj_type != (ushort)1)
                    break;
                AppMain.ObjCameraGet(0).flag |= 2147483648U;
                break;
            case 195:
                if (obsObjectWork.obj_type != (ushort)1)
                    break;
                AppMain.GmGmkSsOnewayThrough((uint)parentObj1.eve_rec.flag);
                break;
            case 276:
                AppMain.GmEndingPlyNopSet();
                break;
            case 277:
                AppMain.GmEndingPlyBrakeSet();
                break;
            case 283:
                AppMain.GmMainDatLoadBossBattleStart((int)parentObj1.eve_rec.flag);
                parentObj1.enemy_flag |= 65536U;
                parentObj1.obj_work.flag |= 10U;
                break;
            case 286:
                AppMain.GmGmkPressPillarStartup((uint)parentObj1.eve_rec.flag);
                break;
        }
    }
}