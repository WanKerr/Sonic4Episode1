using System;

public partial class AppMain
{
    private static void ObjDispSRand(uint seed)
    {
        _obj_disp_rand = seed;
    }

    private static ushort ObjDispRand()
    {
        _obj_disp_rand = (uint)(1663525 * (int)_obj_disp_rand + 1013904223);
        return (ushort)(_obj_disp_rand >> 16);
    }

    private static int ObjSpdUpSet(int lSpd, int sSpd, int sMaxSpd)
    {
        lSpd += FX_Mul(sSpd, g_obj.speed);
        if (sMaxSpd == 0)
            return lSpd;
        if (sSpd >= 0)
        {
            if (lSpd > sMaxSpd)
                lSpd = sMaxSpd;
        }
        else if (lSpd < -sMaxSpd)
            lSpd = -sMaxSpd;
        return lSpd;
    }

    private static int ObjSpdDownSet(int lSpd, int sSpd)
    {
        if (lSpd > 0)
        {
            lSpd -= FX_Mul(sSpd, g_obj.speed);
            if (lSpd < 0)
                lSpd = 0;
        }
        else
        {
            lSpd += FX_Mul(sSpd, g_obj.speed);
            if (lSpd > 0)
                lSpd = 0;
        }
        return lSpd;
    }

    private static int ObjShiftSet(int lPos, int sTag, ushort usShift, int usMax, int usMin)
    {
        if (lPos == sTag)
            return lPos;
        if (usMin == 0)
            usMin = 1;
        int num = sTag - lPos >> usShift;
        if (usMax != 0)
        {
            if (num > usMax)
                num = usMax;
            if (num < -usMax)
                num = -usMax;
        }
        if (usMin != 0)
        {
            if (num > 0)
            {
                if (num < usMin)
                    num = usMin;
            }
            else if (num < 0)
            {
                if (num > -usMin)
                    num = -usMin;
            }
            else
            {
                if (sTag - lPos > 0 && num < usMin)
                    num = usMin;
                if (sTag - lPos < 0 && num > -usMin)
                    num = -usMin;
            }
        }
        lPos += num;
        if (num > 0)
        {
            if (lPos > sTag)
                lPos = sTag;
        }
        else if (num < 0 && lPos < sTag)
            lPos = sTag;
        return lPos;
    }

    private static int ObjDiffSet(
      int lPos,
      int sTag,
      int sSrc,
      ushort usShift,
      int usMax,
      int usMin)
    {
        if (lPos == sTag)
            return lPos;
        if (usMin == 0)
            usMin = 1;
        int num = lPos - sSrc >> usShift;
        if (sTag > sSrc && num < 0)
            num = 0;
        if (sTag < sSrc && num > 0)
            num = 0;
        if (usMax != 0)
        {
            if (num > usMax)
                num = usMax;
            if (num < -usMax)
                num = -usMax;
        }
        if (usMin != 0)
        {
            if (num > 0)
            {
                if (num < usMin)
                    num = usMin;
            }
            else if (num < 0)
            {
                if (num > -usMin)
                    num = -usMin;
            }
            else
            {
                if (sTag - lPos > 0 && num < usMin)
                    num = usMin;
                if (sTag - lPos < 0 && num > -usMin)
                    num = -usMin;
            }
        }
        lPos += num;
        if (num > 0)
        {
            if (lPos > sTag)
                lPos = sTag;
        }
        else if (num < 0 && lPos < sTag)
            lPos = sTag;
        return lPos;
    }

    private static int ObjAlphaSet(int sTag, int sSrc, ushort usAlpha)
    {
        if (usAlpha == 0)
            return sSrc;
        if (usAlpha == 4096)
            return sTag;
        int num = FX_Mul(sTag - sSrc, usAlpha);
        return sSrc + num;
    }

    private static byte ObjRoopMove8(byte ucDir, byte ucTag, sbyte cSpd)
    {
        if (ucTag == ucDir)
            return ucTag;
        if ((byte)Math.Abs(ucDir - ucTag) <= (ucDir <= ucTag ? (byte)(256U - ucTag + ucDir) : (byte)(256U - ucDir + ucTag)))
        {
            if (ucDir > ucTag)
            {
                if (ucTag > ucDir - cSpd)
                    ucDir = ucTag;
                else
                    ucDir -= (byte)cSpd;
            }
            else if (ucDir < ucTag)
            {
                if (ucTag < ucDir + cSpd)
                    ucDir = ucTag;
                else
                    ucDir += (byte)cSpd;
            }
        }
        else if (ucDir > ucTag)
        {
            if (ucTag + 256 < ucDir + cSpd)
                ucDir = ucTag;
            else
                ucDir += (byte)cSpd;
        }
        else if (ucDir < ucTag)
        {
            if (ucTag > ucDir - cSpd + 256)
                ucDir = ucTag;
            else
                ucDir -= (byte)cSpd;
        }
        return ucDir;
    }

    private static ushort ObjRoopMove16(ushort ucDir, ushort ucTag, short cSpd)
    {
        if (ucTag == ucDir)
            return ucTag;
        if ((ushort)Math.Abs(ucDir - ucTag) <= (ucDir <= ucTag ? (ushort)(65536U - ucTag + ucDir) : (ushort)(65536U - ucDir + ucTag)))
        {
            if (ucDir > ucTag)
            {
                if (ucTag > ucDir - cSpd)
                    ucDir = ucTag;
                else
                    ucDir -= (ushort)cSpd;
            }
            else if (ucDir < ucTag)
            {
                if (ucTag < ucDir + cSpd)
                    ucDir = ucTag;
                else
                    ucDir += (ushort)cSpd;
            }
        }
        else if (ucDir > ucTag)
        {
            if (ucTag + 65536 < ucDir + cSpd)
                ucDir = ucTag;
            else
                ucDir += (ushort)cSpd;
        }
        else if (ucDir < ucTag)
        {
            if (ucTag > ucDir - cSpd + 65536)
                ucDir = ucTag;
            else
                ucDir -= (ushort)cSpd;
        }
        return ucDir;
    }

    private static short ObjRoopDiff16(ushort usDir1, ushort usDir2)
    {
        if (usDir2 == usDir1)
            return 0;
        short num1 = (short)(usDir1 - usDir2);
        short num2 = usDir1 <= usDir2 ? (short)(65536 - usDir2 + usDir1) : (short)(65536 - usDir1 + usDir2);
        return Math.Abs(num1) > Math.Abs(num2) ? num2 : num1;
    }

    private static int ObjSwingEndMove(
      OBS_OBJECT_WORK pWork,
      int lSwingWork,
      int sSpdAdd,
      int sSpdDow,
      int sSpdMax)
    {
        if (Math.Abs(lSwingWork) < 4096 && Math.Abs(pWork.spd_m) < 2048)
        {
            lSwingWork = 0;
            pWork.spd_m = 0;
        }
        else
        {
            if (lSwingWork < 0)
            {
                if (pWork.spd_m > 0)
                    pWork.spd_m = ObjSpdDownSet(pWork.spd_m, sSpdDow);
                pWork.spd_m = ObjSpdUpSet(pWork.spd_m, -sSpdAdd, sSpdMax);
            }
            else
            {
                if (pWork.spd_m < 0)
                    pWork.spd_m = ObjSpdDownSet(pWork.spd_m, sSpdDow);
                pWork.spd_m = ObjSpdUpSet(pWork.spd_m, sSpdAdd, sSpdMax);
            }
            lSwingWork -= pWork.spd_m;
            pWork.dir.z = (ushort)(lSwingWork & ushort.MaxValue);
        }
        return lSwingWork;
    }

    private static void ObjNumCodeSet(ref uint pNum, uint sNum, uint ulMax)
    {
        short num1 = 0;
        uint num2 = 0;
        uint num3 = 0;
        if (sNum >= ulMax)
            sNum = ulMax != 0U ? ulMax - 1U : 0U;
        pNum = 0U;
        if (ulMax >= 100U)
        {
            ulMax = (uint)FX_DivS32((int)ulMax, 10);
            num1 = 0;
            while (true)
            {
                if (sNum >= ulMax)
                {
                    uint num4 = (uint)FX_DivS32((int)sNum, (int)ulMax);
                    pNum |= num4 << num1;
                    sNum -= num4 * ulMax;
                }
                num1 += 4;
                if (ulMax > 10U)
                    ulMax = (uint)FX_DivS32((int)ulMax, 10);
                else
                    break;
            }
        }
        pNum |= sNum << num1;
        for (short index = num1; num1 >= index >> 1; num1 -= 4)
        {
            num2 = num3 = 0U;
            uint num4 = (uint)((pNum & 15 << index - num1) >> index - num1 << num1);
            uint num5 = (uint)((pNum & 15 << num1) >> num1 << index - num1);
            pNum &= (uint)~(15 << index - num1 | 15 << num1);
            pNum |= num4 | num5;
        }
    }

    private static ushort ObjObjectTouchCheck(OBS_OBJECT_WORK pObj, ushort index)
    {
        OBS_RECT_WORK pRect = ObjObjectRectGet(pObj, index);
        return pRect != null ? ObjTouchCheck(pObj, pRect) : (ushort)0;
    }

    private static ushort ObjObjectTouchCheckPush(OBS_OBJECT_WORK pObj, ushort index)
    {
        OBS_RECT_WORK pRect = ObjObjectRectGet(pObj, index);
        return pRect != null ? ObjTouchCheckPush(pObj, pRect) : (ushort)0;
    }

    private static ushort ObjTouchCheck(OBS_OBJECT_WORK pWork, OBS_RECT_WORK pRect)
    {
        if (!amTpIsTouchOn(0))
            return 0;
        pRect.parent_obj = pWork;
        int num1;
        int num2;
        if (g_obj.camera[0][1] > g_obj.camera[1][1])
        {
            num1 = g_obj.camera[0][0] >> 12;
            num2 = g_obj.camera[0][1] >> 12;
        }
        else
        {
            num1 = g_obj.camera[1][0] >> 12;
            num2 = g_obj.camera[1][1] >> 12;
        }
        return ObjRectWorkPointCheck(pRect, (int)(_am_tp_touch[0].on[0] * OBD_LCD_X / (double)AMD_DISPLAY_WIDTH + num1), (int)(_am_tp_touch[0].on[1] * OBD_LCD_Y / (double)AMD_DISPLAY_HEIGHT + num2), 0);
    }

    private static ushort ObjTouchCheckPush(
      OBS_OBJECT_WORK pWork,
      OBS_RECT_WORK pRect)
    {
        if (!amTpIsTouchPush(0))
            return 0;
        pRect.parent_obj = pWork;
        int num1;
        int num2;
        if (g_obj.camera[0][1] > g_obj.camera[1][1])
        {
            num1 = g_obj.camera[0][0] >> 12;
            num2 = g_obj.camera[0][1] >> 12;
        }
        else
        {
            num1 = g_obj.camera[1][0] >> 12;
            num2 = g_obj.camera[1][1] >> 12;
        }
        return ObjRectWorkPointCheck(pRect, (int)(_am_tp_touch[0].on[0] * OBD_LCD_X / (double)AMD_DISPLAY_WIDTH + num1), (int)(_am_tp_touch[0].on[1] * OBD_LCD_Y / (double)AMD_DISPLAY_HEIGHT + num2), 0);
    }

    private static void ObjUtilGetRotPosXY(
      int pos_x,
      int pos_y,
      ref int dest_x,
      ref int dest_y,
      ushort dir)
    {
        int num1 = FX_Mul(pos_x, mtMathSin(dir));
        int num2 = FX_Mul(pos_x, mtMathCos(dir));
        int num3 = FX_Mul(pos_y, mtMathSin(dir));
        int num4 = FX_Mul(pos_y, mtMathCos(dir));
        dest_x = num2 - num3;
        dest_y = num1 + num4;
    }

    private static float ObjSpdUpSetF(float spd, float add_apd, float max_spd)
    {
        spd += add_apd * FXM_FX32_TO_FLOAT(g_obj.speed);
        if (amIsZerof(max_spd))
            return spd;
        if (add_apd >= 0.0)
        {
            if (spd > (double)max_spd)
                spd = max_spd;
        }
        else if (spd < -(double)max_spd)
            spd = -max_spd;
        return spd;
    }

    private static float ObjSpdDownSetF(float spd, float spd_dec)
    {
        if (spd > 0.0)
        {
            spd -= spd_dec * FXM_FX32_TO_FLOAT(g_obj.speed);
            if (spd < 0.0)
                spd = 0.0f;
        }
        else
        {
            spd += spd_dec * FXM_FX32_TO_FLOAT(g_obj.speed);
            if (spd > 0.0)
                spd = 0.0f;
        }
        return spd;
    }

    private static float ObjShiftSetF(float pos, float tag, int shift, float max, float min)
    {
        if (pos == (double)tag)
            return pos;
        if (0.0 == min)
            min = 1f;
        float num = (tag - pos) / (1 << shift);
        if (0.0 != max)
        {
            if (num > (double)max)
                num = max;
            if (num < -(double)max)
                num = -max;
        }
        if (0.0 != min)
        {
            if (num > 0.0)
            {
                if (num < (double)min)
                    num = min;
            }
            else if (num < 0.0)
            {
                if (num > -(double)min)
                    num = -min;
            }
            else
            {
                if (tag - (double)pos > 0.0 && num < (double)min)
                    num = min;
                if (tag - (double)pos < 0.0 && num > -(double)min)
                    num = -min;
            }
        }
        pos += num;
        if (num > 0.0)
        {
            if (pos > (double)tag)
                pos = tag;
        }
        else if (num < 0.0 && pos < (double)tag)
            pos = tag;
        return pos;
    }

    public static short OBD_LCD_X => g_obj.lcd_size[0];

    public static short OBD_LCD_Y => g_obj.lcd_size[1];

    public static short OBD_OBJ_CLIP_LCD_X => g_obj.clip_lcd_size[0];

    public static short OBD_OBJ_CLIP_LCD_Y => g_obj.clip_lcd_size[1];

    public static OBS_OBJECT_WORK OBM_OBJECT_TASK_DETAIL_INIT(
      ushort priority,
      byte group,
      byte pause_level,
      byte obj_pause_level,
      TaskWorkFactoryDelegate work_size,
      string name)
    {
        return ObjObjectTaskDetailInit(priority, group, pause_level, obj_pause_level, work_size, name);
    }

    public static void ObjObjectSetTexDoubleBuffer(object bank1, object bank2, object db_slot_flag)
    {
    }

    public static void ObjObjectTaskNameSet(object obj, object name)
    {
    }

    public static OBS_OBJECT g_obj
    {
        get
        {
            if (_g_obj == null)
                _g_obj = new OBS_OBJECT();
            return _g_obj;
        }
        set => _g_obj = value;
    }

    private static void ObjInit(
      byte group,
      ushort prio,
      byte pause_level,
      short lcd_size_x,
      short lcd_size_y,
      float disp_width,
      float disp_height)
    {
        if (obj_ptcb != null)
            ObjExit();
        g_obj = new OBS_OBJECT();
        ObjDispSRand(0U);
        g_obj.speed = 4096;
        g_obj.glb_scale.x = 4096;
        g_obj.glb_scale.y = 4096;
        g_obj.glb_scale.z = 4096;
        g_obj.draw_scale.x = 4096;
        g_obj.draw_scale.y = 4096;
        g_obj.draw_scale.z = 4096;
        g_obj.scale.x = 4096;
        g_obj.scale.y = 4096;
        g_obj.scale.z = 4096;
        g_obj.inv_scale.x = 4096;
        g_obj.inv_scale.y = 4096;
        g_obj.inv_scale.z = 4096;
        g_obj.inv_glb_scale.x = 4096;
        g_obj.inv_glb_scale.y = 4096;
        g_obj.inv_glb_scale.z = 4096;
        g_obj.inv_draw_scale.x = 4096;
        g_obj.inv_draw_scale.y = 4096;
        g_obj.inv_draw_scale.z = 4096;
        g_obj.depth = 4096;
        g_obj.col_through_dot = 5;
        g_obj.cam_scale_center[0][0] = (short)(lcd_size_x / 2);
        g_obj.cam_scale_center[0][1] = (short)(lcd_size_y / 2);
        g_obj.cam_scale_center[1][0] = (short)(lcd_size_x / 2);
        g_obj.cam_scale_center[1][1] = (short)(lcd_size_y / 2);
        g_obj.disp_width = disp_width;
        g_obj.disp_height = disp_height;
        g_obj.lcd_size[0] = lcd_size_x;
        g_obj.lcd_size[1] = lcd_size_y;
        g_obj.clip_lcd_size[0] = lcd_size_x;
        g_obj.clip_lcd_size[1] = lcd_size_y;
        g_obj.load_drawflag = 0U;
        g_obj.drawflag = 0U;
        g_obj.glb_camera_id = -1;
        ObjRectCheckInit();
        ObjCollisionObjectClear();
        ObjCollisionObjectClear();
        ObjDrawInit();
        ObjLoadSetInitDrawFlag(false);
        if (obj_ptcb != null)
            return;
        obj_ptcb = MTM_TASK_MAKE_TCB(new GSF_TASK_PROCEDURE(objMain), new GSF_TASK_PROCEDURE(objDestructor), 0U, pause_level, prio, group, null, "object");
    }

    private static void ObjExit()
    {
        if (obj_ptcb == null)
            return;
        for (OBS_OBJECT_WORK obj_work = ObjObjectSearchRegistObject(null, ushort.MaxValue); obj_work != null; obj_work = ObjObjectSearchRegistObject(obj_work, ushort.MaxValue))
            obj_work.flag |= 4U;
        mtTaskChangeTcbProcedure(obj_ptcb, new GSF_TASK_PROCEDURE(objExitWait));
        g_obj.flag |= 2147483648U;
    }

    private static void ObjPreExit()
    {
        ObjCameraExit();
        g_obj.glb_camera_id = -1;
    }

    private static bool ObjIsInit()
    {
        return null != obj_ptcb;
    }

    private static bool ObjIsExitWait()
    {
        return obj_ptcb != null && ((int)g_obj.flag & int.MinValue) != 0;
    }

    private static void ObjObjectPause(ushort pause_level)
    {
        g_obj.flag |= 2U;
        g_obj.pause_level = pause_level;
    }

    private static void ObjObjectPauseOut()
    {
        g_obj.flag &= 4294967293U;
        g_obj.pause_level = -1;
    }

    private static uint ObjObjectPauseCheck(uint ulFlag)
    {
        return ((int)_g_obj.flag & 1) == 0 || ((int)ulFlag & 32) != 0 ? 0U : 1U;
    }

    private static void ObjDataAlloc(int num)
    {
        if (obj_data_work_save != null)
        {
            g_obj.pData = obj_data_work_save;
            g_obj.data_max = obj_data_max_save;
            obj_data_work_save = null;
            obj_data_max_save = 0;
        }
        else
        {
            g_obj.data_max = num;
            g_obj.pData = New<OBS_DATA_WORK>(num);
        }
    }

    private static OBS_DATA_WORK ObjDataGet(int index)
    {
        return g_obj.data_max <= index ? null : g_obj.pData[index];
    }

    private static void ObjDataFree()
    {
        g_obj.data_max = 0;
        if (g_obj.pData == null)
            return;
        g_obj.pData = null;
    }

    private static void ObjObjectClipLCDSet(short size_x, short size_y)
    {
        g_obj.clip_lcd_size[0] = size_x;
        g_obj.clip_lcd_size[1] = size_y;
    }

    private static void ObjObjectOffsetSet(short sX, short sY)
    {
        g_obj.offset[0] = sX;
        g_obj.offset[1] = sY;
    }

    private static void ObjObjectSpeedSet(int sSpd)
    {
        g_obj.speed = sSpd;
    }

    private static int ObjObjectSpeedGet()
    {
        return g_obj.speed;
    }

    private void ObjObjectScrollSet(int spd_x, int spd_y)
    {
        g_obj.scroll[0] = spd_x;
        g_obj.scroll[1] = spd_y;
    }

    private static int ObjObjectScrollGetX()
    {
        return g_obj.scroll[0];
    }

    private static int ObjObjectScrollGetY()
    {
        return g_obj.scroll[1];
    }

    private static void ObjObjectBeltSetDepth(int depth)
    {
        g_obj.depth = depth;
    }

    private static int ObjObjectBeltGetDepth()
    {
        return g_obj.depth;
    }

    public static void ObjObjectCameraSet(int x1, int y1, int x2, int y2)
    {
        g_obj.camera[0][0] = x1;
        g_obj.camera[0][1] = y1;
        g_obj.camera[1][0] = x2;
        g_obj.camera[1][1] = y2;
    }

    private static void ObjObjectClipCameraSet(int x, int y)
    {
        g_obj.clip_camera[0] = x;
        g_obj.clip_camera[1] = y;
    }

    private static void ObjObjectCameraZSet(int z)
    {
        if (z > 0)
        {
            g_obj.glb_scale.x = 4096 - (z >> 1);
            g_obj.glb_scale.y = 4096 - (z >> 1);
        }
        else
        {
            g_obj.glb_scale.x = 4096 - z;
            g_obj.glb_scale.y = 4096 - z;
        }
        g_obj.glb_scale.z = 4096;
    }

    private static OBS_OBJECT_WORK ObjObjectTaskInit()
    {
        return OBM_OBJECT_TASK_DETAIL_INIT(4096, 1, 0, 0, () => OBS_OBJECT_WORK.Create(), "object");
    }

    private static OBS_OBJECT_WORK ObjObjectTaskDetailInit(
      ushort prio,
      byte group,
      byte pause_level,
      byte obj_pause_level,
      TaskWorkFactoryDelegate work_size,
      string name)
    {
        MTS_TASK_TCB tcb = MTM_TASK_MAKE_TCB(_ObjObjectMain, _ObjObjectExit, 0U, pause_level, prio, group, work_size, name == null ? "" : name);
        OBS_OBJECT_WORK tcbWork = mtTaskGetTcbWork(tcb);
        tcbWork.tcb = tcb;
        tcbWork.pause_level = obj_pause_level;
        tcbWork.scale.x = 4096;
        tcbWork.scale.y = 4096;
        tcbWork.scale.z = 4096;
        if (((int)g_obj.flag & 8192) == 0)
        {
            if (((int)g_obj.flag & 16384) != 0)
                tcbWork.flag |= 1048576U;
            else
                tcbWork.flag |= 2097152U;
        }
        tcbWork.ppViewCheck = _ObjObjectViewOutCheck;
        if (((int)g_obj.flag & 65536) != 0)
            tcbWork.flag |= 16U;
        tcbWork.field_ajst_w_db_f = 2;
        tcbWork.field_ajst_w_db_b = 4;
        tcbWork.field_ajst_w_dl_f = 2;
        tcbWork.field_ajst_w_dl_b = 4;
        tcbWork.field_ajst_w_dt_f = 2;
        tcbWork.field_ajst_w_dt_b = 4;
        tcbWork.field_ajst_w_dr_f = 2;
        tcbWork.field_ajst_w_dr_b = 4;
        tcbWork.field_ajst_h_db_r = 1;
        tcbWork.field_ajst_h_db_l = 1;
        tcbWork.field_ajst_h_dl_r = 1;
        tcbWork.field_ajst_h_dl_l = 1;
        tcbWork.field_ajst_h_dt_r = 1;
        tcbWork.field_ajst_h_dt_l = 1;
        tcbWork.field_ajst_h_dr_r = 2;
        tcbWork.field_ajst_h_dr_l = 2;
        ObjObjectRegistObject(tcbWork);
        return tcbWork;
    }

    public static void ObjObjectRegistObject(OBS_OBJECT_WORK pWork)
    {
        pWork.prev = g_obj.obj_list_tail;
        pWork.next = null;
        if (pWork.prev != null)
            pWork.prev.next = pWork;
        else
            g_obj.obj_list_head = pWork;
        g_obj.obj_list_tail = pWork;
    }

    private static void ObjObjectRevokeObject(OBS_OBJECT_WORK pWork)
    {
        if (pWork.prev != null)
            pWork.prev.next = pWork.next;
        else
            g_obj.obj_list_head = pWork.next;
        if (pWork.next != null)
            pWork.next.prev = pWork.prev;
        else
            g_obj.obj_list_tail = pWork.prev;
    }

    private static void ObjObjectClearAllObject()
    {
        OBS_OBJECT_WORK next;
        for (OBS_OBJECT_WORK obsObjectWork = g_obj.obj_list_head; obsObjectWork != null; obsObjectWork = next)
        {
            next = obsObjectWork.next;
            obsObjectWork.flag |= 4U;
        }
    }

    private static bool ObjObjectCheckClearAllObject()
    {
        return g_obj.obj_list_head == null;
    }

    private static OBS_OBJECT_WORK ObjObjectSearchRegistObject(
      OBS_OBJECT_WORK obj_work,
      ushort obj_type)
    {
        OBS_OBJECT_WORK obsObjectWork = obj_work != null ? obj_work.next : g_obj.obj_list_head;
        while (obsObjectWork != null && (obsObjectWork.obj_type != obj_type && obj_type != ushort.MaxValue))
            obsObjectWork = obsObjectWork.next;
        return obsObjectWork;
    }

    private static void ObjObjectTypeSet(OBS_OBJECT_WORK pObj, ushort usType)
    {
        pObj.obj_type = usType;
    }

    private static void ObjObjectParentSet(
      OBS_OBJECT_WORK pObj,
      OBS_OBJECT_WORK pParent,
      uint ulFlag)
    {
        pObj.parent_obj = pParent;
        pObj.flag &= 4294963711U;
        pObj.flag |= ulFlag & 3584U;
    }

    private static object ObjObjecExWorkAlloc(OBS_OBJECT_WORK pObj, uint ulSize)
    {
        if (pObj.ex_work != null)
            pObj.ex_work = null;
        if (ulSize != 0U)
        {
            pObj.ex_work = (new byte[(int)ulSize]);
            pObj.flag |= 8388608U;
        }
        return pObj.ex_work;
    }

    public static void ObjObjectMain(MTS_TASK_TCB tcb)
    {
        OBS_OBJECT_WORK tcbWork = mtTaskGetTcbWork(tcb);
        if (((int)tcbWork.flag & 4) != 0)
            objObjectExitDataRelease(tcb);
        else if (((int)tcbWork.flag & 8) != 0)
            tcbWork.flag |= 4U;
        else if (((int)tcbWork.flag & 16) == 0 && tcbWork.ppViewCheck != null && tcbWork.ppViewCheck(tcbWork) != 0)
        {
            tcbWork.flag |= 4U;
        }
        else
        {
            if (objObjectParent(tcbWork) != 0)
                return;
            if (tcbWork.obj_3d != null)
            {
                if (!ObjAction3dNNModelLoadCheck(tcbWork.obj_3d) && ((int)tcbWork.flag & 256) == 0)
                    return;
            }
            else if (tcbWork.obj_2d != null && !ObjAction2dAMALoadCheck(tcbWork.obj_2d) && ((int)tcbWork.flag & 256) == 0)
                return;
            if (_g_obj.ppObjPre != null)
                _g_obj.ppObjPre(tcbWork);
            objObjectColRideTouchCheck(tcbWork);
            tcbWork.pos.x -= tcbWork.prev_temp_ofst.x;
            tcbWork.pos.y -= tcbWork.prev_temp_ofst.y;
            tcbWork.pos.z -= tcbWork.prev_temp_ofst.z;
            uint num = ObjObjectPauseCheck(tcbWork.flag);
            if ((num == 0U || ((int)tcbWork.flag & 64) != 0) && tcbWork.ppIn != null)
                tcbWork.ppIn(tcbWork);
            if (((int)tcbWork.disp_flag & 536870912) != 0 && ((int)tcbWork.flag & 1073741824) != 0)
            {
                tcbWork.flag |= 2147483648U;
                tcbWork.disp_flag |= 1073741824U;
            }
            else
            {
                tcbWork.flag &= int.MaxValue;
                tcbWork.disp_flag &= 3221225471U;
            }
            if (num == 0U)
            {
                if (tcbWork.vib_timer != 0)
                    tcbWork.vib_timer = ObjTimeCountDown(tcbWork.vib_timer);
                if (tcbWork.hitstop_timer != 0)
                {
                    tcbWork.hitstop_timer = ObjTimeCountDown(tcbWork.hitstop_timer);
                    if (((int)tcbWork.flag & 8192) != 0)
                        tcbWork.hitstop_timer = 0;
                }
                if (((int)_g_obj.flag & 32768) == 0 || tcbWork.hitstop_timer == 0)
                {
                    if (tcbWork.invincible_timer != 0)
                        tcbWork.invincible_timer = ObjTimeCountDown(tcbWork.invincible_timer);
                    if (((int)tcbWork.flag & -2147483520) == 0 && tcbWork.ppFunc != null)
                        tcbWork.ppFunc(tcbWork);
                }
                if (((int)tcbWork.flag & int.MinValue) == 0)
                {
                    if (((int)tcbWork.move_flag & 8192) == 0 && tcbWork.ppMove != null)
                        tcbWork.ppMove(tcbWork);
                    if (((int)_g_obj.flag & 48) != 0 && ((int)tcbWork.move_flag & 256) == 0 && (((int)_g_obj.flag & 2097152) == 0 || tcbWork.hitstop_timer == 0))
                    {
                        if (tcbWork.ppCol != null)
                            tcbWork.ppCol(tcbWork);
                        else if (_g_obj.ppCollision != null)
                            _g_obj.ppCollision(tcbWork);
                    }
                }
            }
            tcbWork.pos.x += tcbWork.temp_ofst.x;
            tcbWork.pos.y += tcbWork.temp_ofst.y;
            tcbWork.pos.z += tcbWork.temp_ofst.z;
            tcbWork.prev_temp_ofst.x = tcbWork.temp_ofst.x;
            tcbWork.prev_temp_ofst.y = tcbWork.temp_ofst.y;
            tcbWork.prev_temp_ofst.z = tcbWork.temp_ofst.z;
            if (((int)tcbWork.flag & int.MinValue) == 0)
            {
                if (num == 0U && tcbWork.vib_timer != 0 && ((int)tcbWork.flag & 16384) == 0)
                {
                    tcbWork.ofst.x += g_object_vib_tbl[tcbWork.vib_timer >> 13 & 15];
                    tcbWork.ofst.y += g_object_vib_tbl[(tcbWork.vib_timer >> 13) + 1 & 15];
                }
                if ((num == 0U || ((int)tcbWork.flag & 65536) != 0) && ((int)_g_obj.flag & 64) != 0)
                {
                    if (tcbWork.ppRec != null)
                        tcbWork.ppRec(tcbWork);
                    if (_g_obj.ppRegRecAuto != null)
                        _g_obj.ppRegRecAuto(tcbWork);
                }
                if ((num == 0U || ((int)tcbWork.flag & 262144) != 0) && tcbWork.ppLast != null)
                    tcbWork.ppLast(tcbWork);
            }
            if (_g_obj.ppObjPost == null)
                return;
            _g_obj.ppObjPost(tcbWork);
        }
    }

    private static void ObjObjectExit(MTS_TASK_TCB pTcb)
    {
        OBS_OBJECT_WORK tcbWork = mtTaskGetTcbWork(pTcb);
        if (tcbWork.obj_3d != null)
        {
            ObjAction3dNNMotionRelease(tcbWork.obj_3d);
            if (((int)tcbWork.flag & 536870912) == 0)
            {
                if (tcbWork.obj_3d._object != null)
                    tcbWork.obj_3d._object = null;
                if (tcbWork.obj_3d.texlistbuf != null)
                    tcbWork.obj_3d.texlistbuf = null;
            }
        }
        if (tcbWork.obj_3des != null)
        {
            if (tcbWork.obj_3des._object != null)
                tcbWork.obj_3des._object = null;
            if (tcbWork.obj_3des.model_data_work != null)
            {
                ObjDataRelease(tcbWork.obj_3des.model_data_work);
                tcbWork.obj_3des.model_data_work = null;
            }
            else if (tcbWork.obj_3des.model != null)
            {
                int num1 = (int)tcbWork.obj_3des.flag & 262144;
            }
            tcbWork.obj_3des.model = null;
            if (tcbWork.obj_3des.texlistbuf != null)
                tcbWork.obj_3des.texlistbuf = null;
            if (tcbWork.obj_3des.ambtex_data_work != null)
            {
                ObjDataRelease(tcbWork.obj_3des.ambtex_data_work);
                tcbWork.obj_3des.ambtex_data_work = null;
            }
            else if (tcbWork.obj_3des.ambtex != null)
            {
                int num2 = (int)tcbWork.obj_3des.flag & 131072;
            }
            tcbWork.obj_3des.ambtex = null;
            if (tcbWork.obj_3des.ecb != null)
            {
                amEffectDelete(tcbWork.obj_3des.ecb);
                tcbWork.obj_3des.ecb = null;
            }
            if (tcbWork.obj_3des.eff_data_work != null)
            {
                ObjDataRelease(tcbWork.obj_3des.eff_data_work);
                tcbWork.obj_3des.eff_data_work = null;
            }
            else if (tcbWork.obj_3des.eff != null)
            {
                int num3 = (int)tcbWork.obj_3des.flag & 65536;
            }
            tcbWork.obj_3des.eff = null;
        }
        if (tcbWork.obj_2d != null && tcbWork.obj_2d.act != null)
        {
            AoActDelete(tcbWork.obj_2d.act);
            tcbWork.obj_2d.act = null;
        }
        if (tcbWork.col_work != null)
        {
            if (tcbWork.col_work.diff_data_work != null)
                ObjDataRelease(tcbWork.col_work.diff_data_work);
            else if (tcbWork.col_work.obj_col.diff_data != null)
            {
                int num1 = (int)tcbWork.col_work.obj_col.flag & 134217728;
            }
            if (tcbWork.col_work.dir_data_work != null)
                ObjDataRelease(tcbWork.col_work.dir_data_work);
            else if (tcbWork.col_work.obj_col.dir_data != null)
            {
                int num2 = (int)tcbWork.col_work.obj_col.flag & 268435456;
            }
            if (tcbWork.col_work.attr_data_work != null)
                ObjDataRelease(tcbWork.col_work.attr_data_work);
            else if (tcbWork.col_work.obj_col.attr_data != null)
            {
                int num3 = (int)tcbWork.col_work.obj_col.flag & 536870912;
            }
        }
        if (((int)tcbWork.flag & 520093696) != 0)
        {
            if (((int)tcbWork.flag & 134217728) != 0)
            {
                OBS_ACTION3D_NN_WORK obj3d = tcbWork.obj_3d;
            }
            if (((int)tcbWork.flag & 268435456) != 0)
            {
                OBS_ACTION3D_ES_WORK obj3des = tcbWork.obj_3des;
            }
            if (((int)tcbWork.flag & 67108864) != 0)
            {
                OBS_ACTION2D_AMA_WORK obj2d = tcbWork.obj_2d;
            }
            if (((int)tcbWork.flag & 16777216) != 0)
            {
                OBS_COLLISION_WORK colWork = tcbWork.col_work;
            }
            if (((int)tcbWork.flag & 33554432) != 0)
            {
                int num = null != tcbWork.rect_work ? 1 : 0;
            }
        }
        if (tcbWork.ex_work != null)
        {
            int num4 = (int)tcbWork.flag & 8388608;
        }
        ObjObjectRevokeObject(tcbWork);
    }

    private static int ObjObjectViewOutCheck(OBS_OBJECT_WORK pWork)
    {
        return ObjViewOutCheck(pWork.pos.x, pWork.pos.y, pWork.view_out_ofst, pWork.view_out_ofst_plus[0], pWork.view_out_ofst_plus[1], pWork.view_out_ofst_plus[2], pWork.view_out_ofst_plus[3]);
    }

    private static void ObjObjectRectRegist(OBS_OBJECT_WORK pWork, OBS_RECT_WORK pRec)
    {
        if (((int)pWork.flag & 12) != 0 || ((int)g_obj.flag & 64) == 0 || ((int)pWork.flag & 2) != 0)
            return;
        pRec.parent_obj = pWork;
        pRec.flag &= 4294967292U;
        if (ObjObjectDirFallReverseCheck(pWork.dir_fall) != 0U)
        {
            pRec.flag ^= 2U;
            pRec.flag ^= 1U;
        }
        ObjRectRegist(pRec);
    }

    private static void ObjObjectGetRectBuf(
      OBS_OBJECT_WORK pWork,
      ArrayPointer<OBS_RECT_WORK> rect_work,
      ushort rect_num)
    {
        if ((ushort)(rect_num - 1U) > 31)
            return;
        if (null != pWork.rect_work)
        {
            if (((int)pWork.flag & 33554432) == 0)
                return;
            ObjObjectReleaseRectBuf(pWork);
        }
        if (rect_work == null)
        {
            pWork.rect_work = (new AppMain.OBS_RECT_WORK[(int)rect_num]);
            pWork.flag |= 33554432U;
            pWork.rect_num = rect_num;
        }
        else
        {
            pWork.rect_num = rect_num;
            pWork.rect_work = rect_work;
        }
    }

    private static void ObjObjectReleaseRectBuf(OBS_OBJECT_WORK pWork)
    {
        if (((int)pWork.flag & 33554432) == 0 || !(null != pWork.rect_work))
            return;
        pWork.rect_work = null;
        pWork.flag &= 4261412863U;
    }

    private static void ObjObjectSetRectWork(
      OBS_OBJECT_WORK pWork,
      OBS_RECT_WORK rect_work)
    {
        rect_work.parent_obj = pWork;
        rect_work.group_no = 0;
        rect_work.target_g_flag = 1;
        rect_work.hit_power = 64;
        rect_work.def_power = 63;
        rect_work.hit_flag = 2;
        rect_work.def_flag = 1;
    }

    private static OBS_RECT_WORK ObjObjectRectGet(
      OBS_OBJECT_WORK pWork,
      ushort usIndex)
    {
        return pWork.rect_work[usIndex];
    }

    private static int ObjViewOutCheck(
      int lPosX,
      int lPosY,
      short sOfst,
      short sLeft,
      short sTop,
      short sRight,
      short sBottom)
    {
        short num1 = g_obj.clip_lcd_size[0];
        short num2 = _g_obj.clip_lcd_size[1];
        if (_g_obj.glb_scale.x != 4096)
            num1 = (short)FX_Mul(num1, 8192 - _g_obj.glb_scale.x);
        if (_g_obj.glb_scale.y != 4096)
            num2 = (short)FX_Mul(num2, 8192 - _g_obj.glb_scale.y);
        if (((int)_g_obj.flag & 8) == 0)
            return 0;
        int num3 = (_g_obj.clip_camera[0] >> 12) - sOfst;
        int num4 = (_g_obj.clip_camera[1] >> 12) - sOfst;
        int num5 = num1 + (sOfst << 1);
        int num6 = num2 + (sOfst << 1);
        int num7 = num3 + sLeft;
        int num8 = num4 + sTop;
        int num9 = num6 + (-sTop + sBottom);
        int num10 = num5 + (-sLeft + sRight);
        return num7 <= lPosX >> 12 && num7 + num10 >= lPosX >> 12 && (num8 <= lPosY >> 12 && num8 + num9 >= lPosY >> 12) ? 0 : 1;
    }

    private static void ObjObjectSpdDirFall(ref int sSpdX, ref int sSpdY, ushort ucDirFall)
    {
        int num1 = sSpdX;
        int num2 = sSpdY;
        float num3 = nnSin(ucDirFall);
        float num4 = nnCos(ucDirFall);
        float num5 = num1 * num3;
        float num6 = num1 * num4;
        float num7 = num2 * num3;
        float num8 = num2 * num4;
        sSpdX = (int)nnRoundOff(num6 - num7);
        sSpdY = (int)nnRoundOff(num5 + num8);
    }

    private static uint ObjObjectDirFallReverseCheck(ushort ucDirFall)
    {
        return ucDirFall > 24576 && ucDirFall < 40960 ? 1U : 0U;
    }

    private static int ObjTimeCountGet(int count)
    {
        return g_obj.speed != 4096 ? FX_Mul(count, g_obj.speed) : count;
    }

    private static int ObjTimeCountDown(int timer)
    {
        if (g_obj.speed == 4096)
            timer -= 4096;
        else
            timer -= FX_Mul(4096, g_obj.speed);
        if (timer < 0)
            timer = 0;
        return timer;
    }

    private static int ObjTimeCountUp(int timer)
    {
        if (g_obj.speed == 4096)
            timer += 4096;
        else
            timer += FX_Mul(4096, g_obj.speed);
        if (timer < 0)
            timer = 0;
        return timer;
    }

    private static float ObjTimeCountDownF(float timer)
    {
        if (g_obj.speed == 4096)
            --timer;
        else
            timer -= (float)(1.0 * g_obj.speed / 4096.0);
        if (timer < 0.0)
            timer = 0.0f;
        return timer;
    }

    private static float ObjTimeCountUpF(float timer)
    {
        if (g_obj.speed == 4096)
            ++timer;
        else
            timer += (float)(1.0 * g_obj.speed / 4096.0);
        if (timer < 0.0)
            timer = 0.0f;
        return timer;
    }

    private static int ObjObjectMapOutCheck(OBS_OBJECT_WORK pWork)
    {
        return ObjMapOutCheck(pWork.pos.x, pWork.pos.y, pWork.view_out_ofst, pWork.view_out_ofst_plus[0], pWork.view_out_ofst_plus[1], pWork.view_out_ofst_plus[2], pWork.view_out_ofst_plus[3]);
    }

    private static int ObjMapOutCheck(
      int lPosX,
      int lPosY,
      short sOfst,
      short sLeft,
      short sTop,
      short sRight,
      short sBottom)
    {
        int num1;
        int num2;
        int num3;
        int num4;
        if (((int)g_obj.flag & 16) != 0)
        {
            OBS_BLOCK_COLLISION blockCollision = ObjGetBlockCollision();
            if (blockCollision == null)
                return 0;
            num1 = blockCollision.left - sOfst;
            num2 = blockCollision.top - sOfst;
            num3 = blockCollision.right - blockCollision.left + (sOfst << 1);
            num4 = blockCollision.bottom - blockCollision.top + (sOfst << 1);
        }
        else
        {
            OBS_DIFF_COLLISION diffCollision = ObjGetDiffCollision();
            if (diffCollision == null)
                return 0;
            num1 = diffCollision.left - sOfst;
            num2 = diffCollision.top - sOfst;
            num3 = diffCollision.right - diffCollision.left + (sOfst << 1);
            num4 = diffCollision.bottom - diffCollision.top + (sOfst << 1);
        }
        return num1 <= lPosX >> 12 && num1 + num3 >= lPosX >> 12 && (num2 <= lPosY >> 12 && num2 + num4 >= lPosY >> 12) ? 0 : 1;
    }

    private static void objMain(MTS_TASK_TCB tcb)
    {
        g_obj.scale.x = FX_Mul(g_obj.glb_scale.x, g_obj.draw_scale.x);
        g_obj.scale.y = FX_Mul(g_obj.glb_scale.y, g_obj.draw_scale.y);
        g_obj.scale.z = FX_Mul(g_obj.glb_scale.z, g_obj.draw_scale.z);
        g_obj.inv_scale.x = FX_Div(4096, g_obj.scale.x);
        g_obj.inv_scale.y = FX_Div(4096, g_obj.scale.y);
        g_obj.inv_scale.z = FX_Div(4096, g_obj.scale.z);
        if (g_obj.ppPre != null)
            g_obj.ppPre();
        if (((int)g_obj.flag & 64) != 0)
            ObjRectCheckAllGroup();
        if (g_obj.glb_camera_id >= 0)
        {
            ObjDraw3DNNSetCameraEx(g_obj.glb_camera_id, g_obj.glb_camera_type, 15U);
            ObjDraw3DNNSetCameraEx(g_obj.glb_camera_id, g_obj.glb_camera_type, 0U);
        }
        if (g_obj.ppDrawSort != null)
        {
            g_obj.ppDrawSort();
            for (OBS_OBJECT_WORK pWork = g_obj.obj_draw_list_head; pWork != null; pWork = pWork.draw_next)
                objObjectDraw(pWork);
        }
        else
        {
            for (OBS_OBJECT_WORK pWork = g_obj.obj_list_head; pWork != null; pWork = pWork.next)
                objObjectDraw(pWork);
            GmGmkPulleyDrawServerMain();
            GmTvxExecuteDraw();
            gmDecoDrawServerMain(null);
        }
        ObjDrawAction2DAMADrawStart();
        ObjDrawNNStart();
        if (((int)g_obj.flag & 1) == 0)
            ObjCollisionObjectClear();
        g_obj.timer_fx += ObjTimeCountGet(4096);
        g_obj.flag |= 4096U;
        if ((int)g_obj.timer == g_obj.timer_fx >> 12)
            g_obj.flag &= 4294963199U;
        g_obj.timer = (uint)(g_obj.timer_fx >> 12);
        if (((int)g_obj.flag & 2) != 0)
            g_obj.flag |= 1U;
        else
            g_obj.flag &= 4294967294U;
        if (g_obj.ppPost == null)
            return;
        g_obj.ppPost();
    }

    private static void objObjectDraw(OBS_OBJECT_WORK pWork)
    {
        uint num1 = 0;
        uint num2 = ObjObjectPauseCheck(pWork.flag);
        if (((int)pWork.flag & 4) != 0)
            return;
        if (pWork.hitstop_timer != 0 && ((int)pWork.flag & 8192) == 0 || num2 != 0U)
        {
            num1 = pWork.disp_flag & 16U;
            pWork.disp_flag |= 16U;
        }
        if (pWork.ppOut != null)
            pWork.ppOut(pWork);
        if (pWork.ppOutSub != null)
            pWork.ppOutSub(pWork);
        if (pWork.hitstop_timer != 0 && ((int)pWork.flag & 8192) == 0 || num2 != 0U)
        {
            pWork.disp_flag &= 4294967279U;
            pWork.disp_flag |= num1;
        }
        if (num2 != 0U)
            return;
        pWork.prev_ofst.x = pWork.ofst.x;
        pWork.prev_ofst.y = pWork.ofst.y;
        pWork.prev_ofst.z = pWork.ofst.z;
        pWork.ofst.x = 0;
        pWork.ofst.y = 0;
        pWork.ofst.z = 0;
    }

    private static void objDestructor(MTS_TASK_TCB pTcb)
    {
        obj_ptcb = null;
        ObjSetBlockCollision(null);
        ObjSetDiffCollision(null);
        if (((int)g_obj.flag & 1073741824) != 0)
        {
            obj_data_max_save = g_obj.data_max;
            obj_data_work_save = g_obj.pData;
            g_obj.data_max = 0;
            g_obj.pData = null;
        }
        else
            ObjDataFree();
    }

    private static void objExitWait(MTS_TASK_TCB pTcb)
    {
        OBS_OBJECT_WORK obj_work = ObjObjectSearchRegistObject(null, ushort.MaxValue);
        if (obj_work == null)
        {
            mtTaskClearTcb(obj_ptcb);
            g_obj = new OBS_OBJECT();
        }
        else
        {
            for (; obj_work != null; obj_work = ObjObjectSearchRegistObject(obj_work, ushort.MaxValue))
                obj_work.flag |= 4U;
        }
    }

    private static void objObjectExitDataRelease(MTS_TASK_TCB tcb)
    {
        bool flag = false;
        OBS_OBJECT_WORK tcbWork = mtTaskGetTcbWork(tcb);
        if (tcbWork.ppUserRelease != null && tcbWork.ppUserRelease(tcbWork))
            flag = true;
        if (tcbWork.obj_3d != null && ((int)tcbWork.flag & 536870912) == 0)
        {
            ObjAction3dNNModelRelease(tcbWork.obj_3d);
            flag = true;
        }
        if (tcbWork.obj_3des != null)
        {
            if (tcbWork.obj_3des.texlist != null)
            {
                ObjAction3dESTextureRelease(tcbWork.obj_3des);
                flag = true;
            }
            if (tcbWork.obj_3des.model != null)
            {
                ObjAction3dESModelRelease(tcbWork.obj_3des);
                flag = true;
            }
        }
        if (tcbWork.obj_2d != null && tcbWork.obj_2d.ao_tex.texlist != null)
        {
            AoTexRelease(tcbWork.obj_2d.ao_tex);
            flag = true;
        }
        if (flag)
            mtTaskChangeTcbProcedure(tcb, _objObjectDataReleaseCheck);
        else
            mtTaskClearTcb(tcb);
    }

    private static void objObjectDataReleaseCheck(MTS_TASK_TCB tcb)
    {
        bool flag1 = true;
        bool flag2 = true;
        bool flag3 = true;
        bool flag4 = true;
        OBS_OBJECT_WORK tcbWork = mtTaskGetTcbWork(tcb);
        if (tcbWork.ppUserReleaseWait != null && tcbWork.ppUserReleaseWait(tcbWork))
            flag1 = false;
        if (tcbWork.obj_3d != null && ((int)tcbWork.flag & 536870912) == 0)
        {
            if (ObjAction3dNNModelReleaseCheck(tcbWork.obj_3d))
            {
                flag2 = true;
                tcbWork.obj_3d.reg_index = -1;
            }
            else
                flag2 = false;
        }
        if (tcbWork.obj_3des != null)
        {
            flag3 = true;
            if (tcbWork.obj_3des.ecb != null)
                ObjAction3dESEffectRelease(tcbWork.obj_3des);
            if (!ObjAction3dESModelReleaseCheck(tcbWork.obj_3des))
                flag3 = false;
            if (!ObjAction3dESTextureReleaseCheck(tcbWork.obj_3des))
                flag3 = false;
        }
        if (tcbWork.obj_2d != null && tcbWork.obj_2d.ao_tex.texlist != null && !AoTexIsReleased(tcbWork.obj_2d.ao_tex))
            flag4 = false;
        if (!flag2 || !flag3 || (!flag1 || !flag4))
            return;
        mtTaskClearTcb(tcb);
    }

    private static ushort objObjectParent(OBS_OBJECT_WORK pWork)
    {
        OBS_OBJECT_WORK parentObj = pWork.parent_obj;
        if (parentObj != null)
        {
            if (((int)parentObj.flag & 4) != 0)
            {
                if (((int)pWork.flag & 512) == 0)
                {
                    pWork.flag |= 4U;
                    pWork.parent_obj = null;
                    return 1;
                }
                pWork.parent_obj = null;
            }
            if (ObjObjectPauseCheck(pWork.flag) == 0U)
            {
                if (((int)pWork.flag & 1024) != 0)
                {
                    if (((int)pWork.flag & 131072) == 0)
                    {
                        pWork.disp_flag &= 4294967292U;
                        pWork.disp_flag |= parentObj.disp_flag & 3U;
                    }
                    if (((int)pWork.flag & 524288) == 0)
                    {
                        pWork.disp_flag &= 4294967263U;
                        pWork.disp_flag |= parentObj.disp_flag & 32U;
                    }
                    pWork.pos.x = parentObj.pos.x + pWork.parent_ofst.x;
                    pWork.pos.y = parentObj.pos.y + pWork.parent_ofst.y;
                    pWork.pos.z = parentObj.pos.z + pWork.parent_ofst.z;
                    if (((int)pWork.disp_flag & 1) != 0)
                        pWork.pos.x = parentObj.pos.x - pWork.parent_ofst.x;
                    if (((int)pWork.disp_flag & 2) != 0)
                        pWork.pos.y = parentObj.pos.y - pWork.parent_ofst.y;
                    pWork.ofst.x = parentObj.prev_ofst.x;
                    pWork.ofst.y = parentObj.prev_ofst.y;
                    pWork.ofst.z = parentObj.prev_ofst.z;
                    if (parentObj.hitstop_timer != 0)
                    {
                        pWork.hitstop_timer = parentObj.hitstop_timer;
                        pWork.hitstop_timer += ObjTimeCountGet(4096);
                    }
                }
                if (((int)pWork.flag & 2048) != 0)
                {
                    pWork.disp_flag &= 4294967235U;
                    pWork.disp_flag |= parentObj.disp_flag & 60U;
                }
            }
        }
        return 0;
    }

    private static void ObjObjectCollision(OBS_OBJECT_WORK pWork)
    {
        int x1 = pWork.pos.x;
        int y1 = pWork.pos.y;
        int x2 = pWork.pos.x;
        int y2 = pWork.pos.y;
        int x3 = pWork.prev_pos.x;
        int y3 = pWork.prev_pos.y;
        ushort num1 = 32768;
        uint num2 = 0;
        uint num3 = 0;
        pWork.col_flag_prev = pWork.col_flag;
        pWork.col_flag = 0U;
        if (((int)pWork.move_flag & 256) == 0)
        {
            pWork.move_flag &= 4290772991U;
            if (((int)pWork.move_flag & 1) != 0)
                pWork.move_flag |= 4194304U;
            pWork.move_flag &= 4294967280U;
            if (((int)pWork.move_flag & 1) == 0)
                num1 >>= 1;
            if (Math.Abs(pWork.pos.x - pWork.prev_pos.x) > num1 || Math.Abs(pWork.pos.y - pWork.prev_pos.y) > num1)
            {
                pWork.pos.x = pWork.prev_pos.x;
                pWork.pos.y = pWork.prev_pos.y;
                while (true)
                {
                    int y4;
                    do
                    {
                        if (Math.Abs(pWork.pos.x - x1) > num1)
                        {
                            pWork.prev_pos.x = pWork.pos.x;
                            pWork.pos.x = x1 <= pWork.prev_pos.x ? pWork.prev_pos.x - num1 : pWork.prev_pos.x + num1;
                        }
                        else
                            pWork.pos.x = x1;
                        if (Math.Abs(pWork.pos.y - y1) > num1)
                        {
                            pWork.prev_pos.y = pWork.pos.y;
                            pWork.pos.y = y1 <= pWork.prev_pos.y ? pWork.prev_pos.y - num1 : pWork.prev_pos.y + num1;
                        }
                        else
                            pWork.pos.y = y1;
                        if (pWork.pos.x != x1 || pWork.pos.y != y1)
                        {
                            int x4 = pWork.pos.x;
                            y4 = pWork.pos.y;
                            ObjDiffCollisionEarthCheck(pWork);
                            num2 |= pWork.col_flag;
                            num3 |= pWork.move_flag & 15U;
                            if (x4 != pWork.pos.x)
                                x1 = pWork.pos.x;
                        }
                        else
                            goto label_18;
                    }
                    while (y4 == pWork.pos.y);
                    y1 = pWork.pos.y;
                }
            }
        label_18:
            ObjDiffCollisionEarthCheck(pWork);
            uint num4 = num2 | pWork.col_flag;
            pWork.col_flag = num4;
            pWork.move_flag |= num3;
            if (((int)pWork.move_flag & 32) != 0 && ((int)pWork.col_flag & 2) == 0)
                pWork.move_flag &= 4294967263U;
        }
        pWork.prev_pos.x = x3;
        pWork.prev_pos.y = y3;
    }

    private static void ObjObjectMove(OBS_OBJECT_WORK pWork)
    {
        int num1 = 0;
        int num2 = 0;
        int num3 = 0;
        pWork.prev_pos.x = pWork.pos.x;
        pWork.prev_pos.y = pWork.pos.y;
        pWork.prev_pos.z = pWork.pos.z;
        if (((int)pWork.move_flag & 134217728) != 0)
        {
            pWork.flow.x = 0;
            pWork.flow.y = 0;
            pWork.flow.z = 0;
        }
        int x = pWork.flow.x;
        int y = pWork.flow.y;
        if ((x != 0 || y != 0) && pWork.dir_fall != 0)
            ObjObjectSpdDirFall(ref x, ref y, pWork.dir_fall);
        if (pWork.hitstop_timer != 0)
        {
            pWork.move.x = FX_Mul(x, _g_obj.speed);
            pWork.move.y = FX_Mul(y, _g_obj.speed);
            pWork.move.z = FX_Mul(pWork.flow.z, _g_obj.speed);
        }
        else
        {
            if (((int)pWork.move_flag & 1) == 0)
            {
                if (((int)pWork.move_flag & 128) != 0 && ((int)pWork.move_flag & 1) == 0)
                    pWork.spd.y += FX_Mul(pWork.spd_fall, _g_obj.speed);
                if (((int)pWork.move_flag & 128) != 0 && pWork.spd.y > pWork.spd_fall_max)
                    pWork.spd.y = pWork.spd_fall_max;
            }
            if (((int)pWork.move_flag & 64) != 0)
            {
                if (((int)pWork.move_flag & 131072) != 0 && (pWork.spd_m != 0 || ((int)pWork.move_flag & 262144) == 0))
                {
                    int sSpd = FX_Mul(pWork.spd_slope, mtMathSin(pWork.dir.z));
                    if (sSpd != 0)
                        pWork.spd_m = ObjSpdUpSet(pWork.spd_m, sSpd, pWork.spd_slope_max);
                    else if (pWork.spd_m > 0)
                    {
                        if (pWork.spd_m > pWork.spd_slope_max)
                            pWork.spd_m = pWork.spd_slope_max;
                    }
                    else if (pWork.spd_m < -pWork.spd_slope_max)
                        pWork.spd_m = -pWork.spd_slope_max;
                }
                if (((int)pWork.move_flag & 32768) == 0)
                {
                    num1 = FX_Mul(pWork.spd_m, mtMathCos(pWork.dir.z));
                    num2 = FX_Mul(pWork.spd_m, mtMathSin(pWork.dir.z));
                }
            }
            if (((int)pWork.move_flag & 67108864) != 0)
            {
                pWork.move.x = FX_Mul(pWork.spd.x + num1 + x, _g_obj.speed);
                pWork.move.y = FX_Mul(pWork.spd.y + num2 + y, _g_obj.speed);
            }
            else
            {
                pWork.move.x = FX_Mul(pWork.spd.x + num1 + x + _g_obj.scroll[0], _g_obj.speed);
                pWork.move.y = FX_Mul(pWork.spd.y + num2 + y + _g_obj.scroll[1], _g_obj.speed);
            }
            pWork.move.z = FX_Mul(pWork.spd.z + num3 + pWork.flow.z, _g_obj.speed);
            ObjObjectSpdDirFall(ref pWork.move.x, ref pWork.move.y, pWork.dir_fall);
        }
        pWork.pos.x += pWork.move.x;
        pWork.pos.y += pWork.move.y;
        pWork.pos.z += pWork.move.z;
        pWork.spd.x += pWork.spd_add.x;
        pWork.spd.y += pWork.spd_add.y;
        pWork.spd.z += pWork.spd_add.z;
        pWork.flow.x = 0;
        pWork.flow.y = 0;
        pWork.flow.z = 0;
    }

    private static void objObjectColRideTouchCheck(OBS_OBJECT_WORK pWork)
    {
        if (pWork.col_work != null)
        {
            if (pWork.col_work.obj_col.rider_obj != null && ((int)pWork.col_work.obj_col.rider_obj.flag & 4) != 0)
                pWork.col_work.obj_col.rider_obj = null;
            if (pWork.col_work.obj_col.toucher_obj != null && pWork.col_work.obj_col.toucher_obj != pWork.col_work.obj_col.rider_obj && pWork.col_work.obj_col.toucher_obj != null)
            {
                if (ObjObjectPauseCheck(pWork.flag) == 0U && pWork.col_work.obj_col.toucher_obj != pWork.col_work.obj_col.rider_obj && (((int)pWork.col_work.obj_col.toucher_obj.move_flag & 16777216) != 0 && ((int)pWork.move_flag & 33554432) != 0))
                {
                    int num = MTM_MATH_CLIP(MTM_MATH_CLIP(pWork.col_work.obj_col.toucher_obj.move.x, -pWork.col_work.obj_col.toucher_obj.push_max, pWork.col_work.obj_col.toucher_obj.push_max), -pWork.push_max, pWork.push_max);
                    pWork.flow.x += num;
                }
                if (((int)pWork.col_work.obj_col.toucher_obj.flag & 4) != 0)
                    pWork.col_work.obj_col.toucher_obj = null;
            }
        }
        if (pWork.touch_obj != null)
        {
            if (ObjObjectPauseCheck(pWork.flag) == 0U && ((int)pWork.touch_obj.move_flag & 33554432) != 0 && ((int)pWork.move_flag & 16777216) != 0)
            {
                pWork.flow.x += (short)(pWork.touch_obj.pos.x - pWork.touch_obj.prev_pos.x);
                if ((pWork.touch_obj.move.x & 4095) != 0)
                {
                    if (pWork.touch_obj.move.x > 0)
                        pWork.flow.x += 4096 - (pWork.touch_obj.move.x & 4095);
                    else
                        pWork.flow.x -= 4096 + (pWork.touch_obj.move.x & 4095);
                }
            }
            if (((int)pWork.touch_obj.flag & 4) != 0)
                pWork.touch_obj = null;
        }
        if (pWork.ride_obj == null)
            return;
        if (((int)pWork.ride_obj.flag & 4) != 0)
        {
            pWork.ride_obj = null;
        }
        else
        {
            if (ObjObjectPauseCheck(pWork.flag) != 0U)
                return;
            if (((int)pWork.ride_obj.move_flag & 256) != 0)
            {
                pWork.flow.x += pWork.ride_obj.move.x;
                pWork.flow.y += pWork.ride_obj.move.y;
                pWork.flow.z += pWork.ride_obj.move.z;
            }
            else
            {
                pWork.flow.x += (short)(pWork.ride_obj.pos.x - pWork.ride_obj.prev_pos.x);
                pWork.flow.y += (short)(pWork.ride_obj.pos.y - pWork.ride_obj.prev_pos.y);
                pWork.flow.z += (short)(pWork.ride_obj.pos.z - pWork.ride_obj.prev_pos.z);
            }
            if ((pWork.ride_obj.move.y & 4095) == 0)
                return;
            pWork.flow.y += 4096 - (pWork.ride_obj.move.y & 4095);
        }
    }

    private static void ObjObjectFieldRectSet(
      OBS_OBJECT_WORK pObj,
      short cLeft,
      short cTop,
      short cRight,
      short cBottom)
    {
        pObj.field_rect[0] = cLeft;
        pObj.field_rect[1] = cTop;
        pObj.field_rect[2] = cRight;
        pObj.field_rect[3] = cBottom;
        pObj.move_flag &= 4294967039U;
    }

    private static void ObjObjectFallSet(OBS_OBJECT_WORK pObj, int fSpdFall, int fSpdFallMax)
    {
        pObj.spd_fall = fSpdFall;
        pObj.spd_fall_max = fSpdFallMax;
        pObj.move_flag |= 128U;
    }

}