public partial class AppMain
{
    private static OBS_OBJECT_WORK GmGmkFlagChangeInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        OBS_OBJECT_WORK work = GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_ENEMY_COM_WORK(), "GMK_FLAG_CNG");
        OBS_RECT_WORK pRec = ((GMS_ENEMY_COM_WORK)work).rect_work[2];
        ObjRectGroupSet(pRec, 1, 1);
        ObjRectAtkSet(pRec, 0, 1);
        ObjRectDefSet(pRec, 65534, 0);
        ObjRectSet(pRec.rect, eve_rec.left, eve_rec.top, (short)(eve_rec.width + eve_rec.left), (short)(eve_rec.height + eve_rec.top));
        pRec.ppDef = new OBS_RECT_WORK_Delegate1(gmGmkFlagChangeDefFunc);
        pRec.parent_obj = work;
        pRec.flag |= 192U;
        work.move_flag |= 8480U;
        return work;
    }

    private static void gmGmkFlagChangeDefFunc(
      OBS_RECT_WORK mine_rect,
      OBS_RECT_WORK match_rect)
    {
        if (match_rect.parent_obj == null || match_rect.parent_obj.obj_type != 1)
            return;
        GMS_ENEMY_COM_WORK parentObj1 = (GMS_ENEMY_COM_WORK)mine_rect.parent_obj;
        GMS_PLAYER_WORK parentObj2 = (GMS_PLAYER_WORK)match_rect.parent_obj;
        OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)parentObj2;
        switch (parentObj1.eve_rec.id)
        {
            case 60:
                if ((parentObj1.eve_rec.flag & 128) != 0)
                {
                    if (parentObj2.seq_state != 29 && parentObj2.seq_state != 49 && (parentObj2.seq_state != 50 && parentObj2.seq_state != 42))
                        break;
                    parentObj2.gmk_flag |= 2048U;
                }
                if (obsObjectWork.obj_type != 1 || ((int)obsObjectWork.move_flag & 16) == 0)
                    break;
                parentObj2.gmk_flag |= 1U;
                if ((parentObj1.eve_rec.flag & 16) != 0)
                {
                    parentObj2.gmk_flag |= 2U;
                    break;
                }
                if ((parentObj1.eve_rec.flag & 2) != 0)
                {
                    if (((int)parentObj2.obj_work.disp_flag & 1) != 0)
                        break;
                    parentObj2.gmk_flag |= 33554432U;
                    break;
                }
                if ((parentObj1.eve_rec.flag & 1) == 0 || ((int)parentObj2.obj_work.disp_flag & 1) == 0)
                    break;
                parentObj2.gmk_flag |= 33554432U;
                break;
            case 61:
                obsObjectWork.flag &= 4294967294U;
                if (obsObjectWork.obj_type != 1)
                    break;
                parentObj2.graind_prev_ride = 0;
                break;
            case 62:
                obsObjectWork.flag |= 1U;
                if (obsObjectWork.obj_type != 1)
                    break;
                parentObj2.graind_prev_ride = 0;
                break;
            case 99:
                if (obsObjectWork.obj_type != 1)
                    break;
                GmPlySeqChangeDeath(parentObj2);
                break;
            case 131:
                if (obsObjectWork.obj_type != 1)
                    break;
                ushort flag = parentObj1.eve_rec.flag;
                short num1 = (short)(flag & 7);
                if ((flag & 8) != 0)
                    num1 = (short)-num1;
                parentObj2.gmk_camera_center_ofst_x = (short)(num1 << 3);
                short num2 = (short)((flag & 112) >> 4);
                if ((flag & 128) != 0)
                    num2 = (short)-num2;
                parentObj2.gmk_camera_center_ofst_y = (short)(num2 << 3);
                break;
            case 162:
                if (obsObjectWork.obj_type != 1)
                    break;
                ObjCameraGet(0).flag |= 2147483648U;
                break;
            case 195:
                if (obsObjectWork.obj_type != 1)
                    break;
                GmGmkSsOnewayThrough(parentObj1.eve_rec.flag);
                break;
            case 276:
                GmEndingPlyNopSet();
                break;
            case 277:
                GmEndingPlyBrakeSet();
                break;
            case 283:
                GmMainDatLoadBossBattleStart(parentObj1.eve_rec.flag);
                parentObj1.enemy_flag |= 65536U;
                parentObj1.obj_work.flag |= 10U;
                break;
            case 286:
                GmGmkPressPillarStartup(parentObj1.eve_rec.flag);
                break;
        }
    }
}