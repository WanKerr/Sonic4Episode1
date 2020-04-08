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
    private static void ObjDispSRand(uint seed)
    {
        AppMain._obj_disp_rand = seed;
    }

    private static ushort ObjDispRand()
    {
        AppMain._obj_disp_rand = (uint)(1663525 * (int)AppMain._obj_disp_rand + 1013904223);
        return (ushort)(AppMain._obj_disp_rand >> 16);
    }

    private static int ObjSpdUpSet(int lSpd, int sSpd, int sMaxSpd)
    {
        lSpd += AppMain.FX_Mul(sSpd, AppMain.g_obj.speed);
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
            lSpd -= AppMain.FX_Mul(sSpd, AppMain.g_obj.speed);
            if (lSpd < 0)
                lSpd = 0;
        }
        else
        {
            lSpd += AppMain.FX_Mul(sSpd, AppMain.g_obj.speed);
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
        int num = sTag - lPos >> (int)usShift;
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
        int num = lPos - sSrc >> (int)usShift;
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
        if (usAlpha == (ushort)0)
            return sSrc;
        if (usAlpha == (ushort)4096)
            return sTag;
        int num = AppMain.FX_Mul(sTag - sSrc, (int)usAlpha);
        return sSrc + num;
    }

    private static byte ObjRoopMove8(byte ucDir, byte ucTag, sbyte cSpd)
    {
        if ((int)ucTag == (int)ucDir)
            return ucTag;
        if ((int)(byte)Math.Abs((int)ucDir - (int)ucTag) <= ((int)ucDir <= (int)ucTag ? (int)(byte)(256U - (uint)ucTag + (uint)ucDir) : (int)(byte)(256U - (uint)ucDir + (uint)ucTag)))
        {
            if ((int)ucDir > (int)ucTag)
            {
                if ((int)ucTag > (int)ucDir - (int)cSpd)
                    ucDir = ucTag;
                else
                    ucDir -= (byte)cSpd;
            }
            else if ((int)ucDir < (int)ucTag)
            {
                if ((int)ucTag < (int)ucDir + (int)cSpd)
                    ucDir = ucTag;
                else
                    ucDir += (byte)cSpd;
            }
        }
        else if ((int)ucDir > (int)ucTag)
        {
            if ((int)ucTag + 256 < (int)ucDir + (int)cSpd)
                ucDir = ucTag;
            else
                ucDir += (byte)cSpd;
        }
        else if ((int)ucDir < (int)ucTag)
        {
            if ((int)ucTag > (int)ucDir - (int)cSpd + 256)
                ucDir = ucTag;
            else
                ucDir -= (byte)cSpd;
        }
        return ucDir;
    }

    private static ushort ObjRoopMove16(ushort ucDir, ushort ucTag, short cSpd)
    {
        if ((int)ucTag == (int)ucDir)
            return ucTag;
        if ((int)(ushort)Math.Abs((int)ucDir - (int)ucTag) <= ((int)ucDir <= (int)ucTag ? (int)(ushort)(65536U - (uint)ucTag + (uint)ucDir) : (int)(ushort)(65536U - (uint)ucDir + (uint)ucTag)))
        {
            if ((int)ucDir > (int)ucTag)
            {
                if ((int)ucTag > (int)ucDir - (int)cSpd)
                    ucDir = ucTag;
                else
                    ucDir -= (ushort)cSpd;
            }
            else if ((int)ucDir < (int)ucTag)
            {
                if ((int)ucTag < (int)ucDir + (int)cSpd)
                    ucDir = ucTag;
                else
                    ucDir += (ushort)cSpd;
            }
        }
        else if ((int)ucDir > (int)ucTag)
        {
            if ((int)ucTag + 65536 < (int)ucDir + (int)cSpd)
                ucDir = ucTag;
            else
                ucDir += (ushort)cSpd;
        }
        else if ((int)ucDir < (int)ucTag)
        {
            if ((int)ucTag > (int)ucDir - (int)cSpd + 65536)
                ucDir = ucTag;
            else
                ucDir -= (ushort)cSpd;
        }
        return ucDir;
    }

    private static short ObjRoopDiff16(ushort usDir1, ushort usDir2)
    {
        if ((int)usDir2 == (int)usDir1)
            return 0;
        short num1 = (short)((int)usDir1 - (int)usDir2);
        short num2 = (int)usDir1 <= (int)usDir2 ? (short)(65536 - (int)usDir2 + (int)usDir1) : (short)(65536 - (int)usDir1 + (int)usDir2);
        return (int)Math.Abs(num1) > (int)Math.Abs(num2) ? num2 : num1;
    }

    private static int ObjSwingEndMove(
      AppMain.OBS_OBJECT_WORK pWork,
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
                    pWork.spd_m = AppMain.ObjSpdDownSet(pWork.spd_m, sSpdDow);
                pWork.spd_m = AppMain.ObjSpdUpSet(pWork.spd_m, -sSpdAdd, sSpdMax);
            }
            else
            {
                if (pWork.spd_m < 0)
                    pWork.spd_m = AppMain.ObjSpdDownSet(pWork.spd_m, sSpdDow);
                pWork.spd_m = AppMain.ObjSpdUpSet(pWork.spd_m, sSpdAdd, sSpdMax);
            }
            lSwingWork -= pWork.spd_m;
            pWork.dir.z = (ushort)(lSwingWork & (int)ushort.MaxValue);
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
            ulMax = (uint)AppMain.FX_DivS32((int)ulMax, 10);
            num1 = (short)0;
            while (true)
            {
                if (sNum >= ulMax)
                {
                    uint num4 = (uint)AppMain.FX_DivS32((int)sNum, (int)ulMax);
                    pNum |= num4 << (int)num1;
                    sNum -= num4 * ulMax;
                }
                num1 += (short)4;
                if (ulMax > 10U)
                    ulMax = (uint)AppMain.FX_DivS32((int)ulMax, 10);
                else
                    break;
            }
        }
        pNum |= sNum << (int)num1;
        for (short index = num1; (int)num1 >= (int)index >> 1; num1 -= (short)4)
        {
            num2 = num3 = 0U;
            uint num4 = (uint)(((long)pNum & (long)(15 << (int)index - (int)num1)) >> (int)index - (int)num1 << (int)num1);
            uint num5 = (uint)(((long)pNum & (long)(15 << (int)num1)) >> (int)num1 << (int)index - (int)num1);
            pNum &= (uint)~(15 << (int)index - (int)num1 | 15 << (int)num1);
            pNum |= num4 | num5;
        }
    }

    private static ushort ObjObjectTouchCheck(AppMain.OBS_OBJECT_WORK pObj, ushort index)
    {
        AppMain.OBS_RECT_WORK pRect = AppMain.ObjObjectRectGet(pObj, index);
        return pRect != null ? AppMain.ObjTouchCheck(pObj, pRect) : (ushort)0;
    }

    private static ushort ObjObjectTouchCheckPush(AppMain.OBS_OBJECT_WORK pObj, ushort index)
    {
        AppMain.OBS_RECT_WORK pRect = AppMain.ObjObjectRectGet(pObj, index);
        return pRect != null ? AppMain.ObjTouchCheckPush(pObj, pRect) : (ushort)0;
    }

    private static ushort ObjTouchCheck(AppMain.OBS_OBJECT_WORK pWork, AppMain.OBS_RECT_WORK pRect)
    {
        if (!AppMain.amTpIsTouchOn(0))
            return 0;
        pRect.parent_obj = pWork;
        int num1;
        int num2;
        if (AppMain.g_obj.camera[0][1] > AppMain.g_obj.camera[1][1])
        {
            num1 = AppMain.g_obj.camera[0][0] >> 12;
            num2 = AppMain.g_obj.camera[0][1] >> 12;
        }
        else
        {
            num1 = AppMain.g_obj.camera[1][0] >> 12;
            num2 = AppMain.g_obj.camera[1][1] >> 12;
        }
        return AppMain.ObjRectWorkPointCheck(pRect, (int)((double)((int)AppMain._am_tp_touch[0].on[0] * (int)AppMain.OBD_LCD_X) / (double)AppMain.AMD_DISPLAY_WIDTH + (double)num1), (int)((double)((int)AppMain._am_tp_touch[0].on[1] * (int)AppMain.OBD_LCD_Y) / (double)AppMain.AMD_DISPLAY_HEIGHT + (double)num2), 0);
    }

    private static ushort ObjTouchCheckPush(
      AppMain.OBS_OBJECT_WORK pWork,
      AppMain.OBS_RECT_WORK pRect)
    {
        if (!AppMain.amTpIsTouchPush(0))
            return 0;
        pRect.parent_obj = pWork;
        int num1;
        int num2;
        if (AppMain.g_obj.camera[0][1] > AppMain.g_obj.camera[1][1])
        {
            num1 = AppMain.g_obj.camera[0][0] >> 12;
            num2 = AppMain.g_obj.camera[0][1] >> 12;
        }
        else
        {
            num1 = AppMain.g_obj.camera[1][0] >> 12;
            num2 = AppMain.g_obj.camera[1][1] >> 12;
        }
        return AppMain.ObjRectWorkPointCheck(pRect, (int)((double)((int)AppMain._am_tp_touch[0].on[0] * (int)AppMain.OBD_LCD_X) / (double)AppMain.AMD_DISPLAY_WIDTH + (double)num1), (int)((double)((int)AppMain._am_tp_touch[0].on[1] * (int)AppMain.OBD_LCD_Y) / (double)AppMain.AMD_DISPLAY_HEIGHT + (double)num2), 0);
    }

    private static void ObjUtilGetRotPosXY(
      int pos_x,
      int pos_y,
      ref int dest_x,
      ref int dest_y,
      ushort dir)
    {
        int num1 = AppMain.FX_Mul(pos_x, AppMain.mtMathSin((int)dir));
        int num2 = AppMain.FX_Mul(pos_x, AppMain.mtMathCos((int)dir));
        int num3 = AppMain.FX_Mul(pos_y, AppMain.mtMathSin((int)dir));
        int num4 = AppMain.FX_Mul(pos_y, AppMain.mtMathCos((int)dir));
        dest_x = num2 - num3;
        dest_y = num1 + num4;
    }

    private static float ObjSpdUpSetF(float spd, float add_apd, float max_spd)
    {
        spd += add_apd * AppMain.FXM_FX32_TO_FLOAT(AppMain.g_obj.speed);
        if (AppMain.amIsZerof(max_spd))
            return spd;
        if ((double)add_apd >= 0.0)
        {
            if ((double)spd > (double)max_spd)
                spd = max_spd;
        }
        else if ((double)spd < -(double)max_spd)
            spd = -max_spd;
        return spd;
    }

    private static float ObjSpdDownSetF(float spd, float spd_dec)
    {
        if ((double)spd > 0.0)
        {
            spd -= spd_dec * AppMain.FXM_FX32_TO_FLOAT(AppMain.g_obj.speed);
            if ((double)spd < 0.0)
                spd = 0.0f;
        }
        else
        {
            spd += spd_dec * AppMain.FXM_FX32_TO_FLOAT(AppMain.g_obj.speed);
            if ((double)spd > 0.0)
                spd = 0.0f;
        }
        return spd;
    }

    private static float ObjShiftSetF(float pos, float tag, int shift, float max, float min)
    {
        if ((double)pos == (double)tag)
            return pos;
        if (0.0 == (double)min)
            min = 1f;
        float num = (tag - pos) / (float)(1 << shift);
        if (0.0 != (double)max)
        {
            if ((double)num > (double)max)
                num = max;
            if ((double)num < -(double)max)
                num = -max;
        }
        if (0.0 != (double)min)
        {
            if ((double)num > 0.0)
            {
                if ((double)num < (double)min)
                    num = min;
            }
            else if ((double)num < 0.0)
            {
                if ((double)num > -(double)min)
                    num = -min;
            }
            else
            {
                if ((double)tag - (double)pos > 0.0 && (double)num < (double)min)
                    num = min;
                if ((double)tag - (double)pos < 0.0 && (double)num > -(double)min)
                    num = -min;
            }
        }
        pos += num;
        if ((double)num > 0.0)
        {
            if ((double)pos > (double)tag)
                pos = tag;
        }
        else if ((double)num < 0.0 && (double)pos < (double)tag)
            pos = tag;
        return pos;
    }

    public static short OBD_LCD_X
    {
        get
        {
            return AppMain.g_obj.lcd_size[0];
        }
    }

    public static short OBD_LCD_Y
    {
        get
        {
            return AppMain.g_obj.lcd_size[1];
        }
    }

    public static short OBD_OBJ_CLIP_LCD_X
    {
        get
        {
            return AppMain.g_obj.clip_lcd_size[0];
        }
    }

    public static short OBD_OBJ_CLIP_LCD_Y
    {
        get
        {
            return AppMain.g_obj.clip_lcd_size[1];
        }
    }

    public static AppMain.OBS_OBJECT_WORK OBM_OBJECT_TASK_DETAIL_INIT(
      ushort priority,
      byte group,
      byte pause_level,
      byte obj_pause_level,
      AppMain.TaskWorkFactoryDelegate work_size,
      string name)
    {
        return AppMain.ObjObjectTaskDetailInit(priority, group, pause_level, obj_pause_level, work_size, name);
    }

    public static void ObjObjectSetTexDoubleBuffer(object bank1, object bank2, object db_slot_flag)
    {
    }

    public static void ObjObjectTaskNameSet(object obj, object name)
    {
    }

    public static AppMain.OBS_OBJECT g_obj
    {
        get
        {
            if (AppMain._g_obj == null)
                AppMain._g_obj = new AppMain.OBS_OBJECT();
            return AppMain._g_obj;
        }
        set
        {
            AppMain._g_obj = value;
        }
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
        if (AppMain.obj_ptcb != null)
            AppMain.ObjExit();
        AppMain.g_obj = new AppMain.OBS_OBJECT();
        AppMain.ObjDispSRand(0U);
        AppMain.g_obj.speed = 4096;
        AppMain.g_obj.glb_scale.x = 4096;
        AppMain.g_obj.glb_scale.y = 4096;
        AppMain.g_obj.glb_scale.z = 4096;
        AppMain.g_obj.draw_scale.x = 4096;
        AppMain.g_obj.draw_scale.y = 4096;
        AppMain.g_obj.draw_scale.z = 4096;
        AppMain.g_obj.scale.x = 4096;
        AppMain.g_obj.scale.y = 4096;
        AppMain.g_obj.scale.z = 4096;
        AppMain.g_obj.inv_scale.x = 4096;
        AppMain.g_obj.inv_scale.y = 4096;
        AppMain.g_obj.inv_scale.z = 4096;
        AppMain.g_obj.inv_glb_scale.x = 4096;
        AppMain.g_obj.inv_glb_scale.y = 4096;
        AppMain.g_obj.inv_glb_scale.z = 4096;
        AppMain.g_obj.inv_draw_scale.x = 4096;
        AppMain.g_obj.inv_draw_scale.y = 4096;
        AppMain.g_obj.inv_draw_scale.z = 4096;
        AppMain.g_obj.depth = 4096;
        AppMain.g_obj.col_through_dot = (sbyte)5;
        AppMain.g_obj.cam_scale_center[0][0] = (short)((int)lcd_size_x / 2);
        AppMain.g_obj.cam_scale_center[0][1] = (short)((int)lcd_size_y / 2);
        AppMain.g_obj.cam_scale_center[1][0] = (short)((int)lcd_size_x / 2);
        AppMain.g_obj.cam_scale_center[1][1] = (short)((int)lcd_size_y / 2);
        AppMain.g_obj.disp_width = disp_width;
        AppMain.g_obj.disp_height = disp_height;
        AppMain.g_obj.lcd_size[0] = lcd_size_x;
        AppMain.g_obj.lcd_size[1] = lcd_size_y;
        AppMain.g_obj.clip_lcd_size[0] = lcd_size_x;
        AppMain.g_obj.clip_lcd_size[1] = lcd_size_y;
        AppMain.g_obj.load_drawflag = 0U;
        AppMain.g_obj.drawflag = 0U;
        AppMain.g_obj.glb_camera_id = -1;
        AppMain.ObjRectCheckInit();
        AppMain.ObjCollisionObjectClear();
        AppMain.ObjCollisionObjectClear();
        AppMain.ObjDrawInit();
        AppMain.ObjLoadSetInitDrawFlag(false);
        if (AppMain.obj_ptcb != null)
            return;
        AppMain.obj_ptcb = AppMain.MTM_TASK_MAKE_TCB(new AppMain.GSF_TASK_PROCEDURE(AppMain.objMain), new AppMain.GSF_TASK_PROCEDURE(AppMain.objDestructor), 0U, (ushort)pause_level, (uint)prio, (int)group, (AppMain.TaskWorkFactoryDelegate)null, "object");
    }

    private static void ObjExit()
    {
        if (AppMain.obj_ptcb == null)
            return;
        for (AppMain.OBS_OBJECT_WORK obj_work = AppMain.ObjObjectSearchRegistObject((AppMain.OBS_OBJECT_WORK)null, ushort.MaxValue); obj_work != null; obj_work = AppMain.ObjObjectSearchRegistObject(obj_work, ushort.MaxValue))
            obj_work.flag |= 4U;
        AppMain.mtTaskChangeTcbProcedure(AppMain.obj_ptcb, new AppMain.GSF_TASK_PROCEDURE(AppMain.objExitWait));
        AppMain.g_obj.flag |= 2147483648U;
    }

    private static void ObjPreExit()
    {
        AppMain.ObjCameraExit();
        AppMain.g_obj.glb_camera_id = -1;
    }

    private static bool ObjIsInit()
    {
        return null != AppMain.obj_ptcb;
    }

    private static bool ObjIsExitWait()
    {
        return AppMain.obj_ptcb != null && ((int)AppMain.g_obj.flag & int.MinValue) != 0;
    }

    private static void ObjObjectPause(ushort pause_level)
    {
        AppMain.g_obj.flag |= 2U;
        AppMain.g_obj.pause_level = (int)pause_level;
    }

    private static void ObjObjectPauseOut()
    {
        AppMain.g_obj.flag &= 4294967293U;
        AppMain.g_obj.pause_level = -1;
    }

    private static uint ObjObjectPauseCheck(uint ulFlag)
    {
        return ((int)AppMain._g_obj.flag & 1) == 0 || ((int)ulFlag & 32) != 0 ? 0U : 1U;
    }

    private static void ObjDataAlloc(int num)
    {
        if (AppMain.obj_data_work_save != null)
        {
            AppMain.g_obj.pData = AppMain.obj_data_work_save;
            AppMain.g_obj.data_max = AppMain.obj_data_max_save;
            AppMain.obj_data_work_save = (AppMain.OBS_DATA_WORK[])null;
            AppMain.obj_data_max_save = 0;
        }
        else
        {
            AppMain.g_obj.data_max = num;
            AppMain.g_obj.pData = AppMain.New<AppMain.OBS_DATA_WORK>(num);
        }
    }

    private static AppMain.OBS_DATA_WORK ObjDataGet(int index)
    {
        return AppMain.g_obj.data_max <= index ? (AppMain.OBS_DATA_WORK)null : AppMain.g_obj.pData[index];
    }

    private static void ObjDataFree()
    {
        AppMain.g_obj.data_max = 0;
        if (AppMain.g_obj.pData == null)
            return;
        AppMain.g_obj.pData = (AppMain.OBS_DATA_WORK[])null;
    }

    private static void ObjObjectClipLCDSet(short size_x, short size_y)
    {
        AppMain.g_obj.clip_lcd_size[0] = size_x;
        AppMain.g_obj.clip_lcd_size[1] = size_y;
    }

    private static void ObjObjectOffsetSet(short sX, short sY)
    {
        AppMain.g_obj.offset[0] = sX;
        AppMain.g_obj.offset[1] = sY;
    }

    private static void ObjObjectSpeedSet(int sSpd)
    {
        AppMain.g_obj.speed = sSpd;
    }

    private static int ObjObjectSpeedGet()
    {
        return AppMain.g_obj.speed;
    }

    private void ObjObjectScrollSet(int spd_x, int spd_y)
    {
        AppMain.g_obj.scroll[0] = spd_x;
        AppMain.g_obj.scroll[1] = spd_y;
    }

    private static int ObjObjectScrollGetX()
    {
        return AppMain.g_obj.scroll[0];
    }

    private static int ObjObjectScrollGetY()
    {
        return AppMain.g_obj.scroll[1];
    }

    private static void ObjObjectBeltSetDepth(int depth)
    {
        AppMain.g_obj.depth = depth;
    }

    private static int ObjObjectBeltGetDepth()
    {
        return AppMain.g_obj.depth;
    }

    public static void ObjObjectCameraSet(int x1, int y1, int x2, int y2)
    {
        AppMain.g_obj.camera[0][0] = x1;
        AppMain.g_obj.camera[0][1] = y1;
        AppMain.g_obj.camera[1][0] = x2;
        AppMain.g_obj.camera[1][1] = y2;
    }

    private static void ObjObjectClipCameraSet(int x, int y)
    {
        AppMain.g_obj.clip_camera[0] = x;
        AppMain.g_obj.clip_camera[1] = y;
    }

    private static void ObjObjectCameraZSet(int z)
    {
        if (z > 0)
        {
            AppMain.g_obj.glb_scale.x = 4096 - (z >> 1);
            AppMain.g_obj.glb_scale.y = 4096 - (z >> 1);
        }
        else
        {
            AppMain.g_obj.glb_scale.x = 4096 - z;
            AppMain.g_obj.glb_scale.y = 4096 - z;
        }
        AppMain.g_obj.glb_scale.z = 4096;
    }

    private static AppMain.OBS_OBJECT_WORK ObjObjectTaskInit()
    {
        return AppMain.OBM_OBJECT_TASK_DETAIL_INIT((ushort)4096, (byte)1, (byte)0, (byte)0, (AppMain.TaskWorkFactoryDelegate)(() => (object)AppMain.OBS_OBJECT_WORK.Create()), "object");
    }

    private static AppMain.OBS_OBJECT_WORK ObjObjectTaskDetailInit(
      ushort prio,
      byte group,
      byte pause_level,
      byte obj_pause_level,
      AppMain.TaskWorkFactoryDelegate work_size,
      string name)
    {
        AppMain.MTS_TASK_TCB tcb = AppMain.MTM_TASK_MAKE_TCB(AppMain._ObjObjectMain, AppMain._ObjObjectExit, 0U, (ushort)pause_level, (uint)prio, (int)group, work_size, name == null ? "" : name);
        AppMain.OBS_OBJECT_WORK tcbWork = AppMain.mtTaskGetTcbWork(tcb);
        tcbWork.tcb = tcb;
        tcbWork.pause_level = (int)obj_pause_level;
        tcbWork.scale.x = 4096;
        tcbWork.scale.y = 4096;
        tcbWork.scale.z = 4096;
        if (((int)AppMain.g_obj.flag & 8192) == 0)
        {
            if (((int)AppMain.g_obj.flag & 16384) != 0)
                tcbWork.flag |= 1048576U;
            else
                tcbWork.flag |= 2097152U;
        }
        tcbWork.ppViewCheck = AppMain._ObjObjectViewOutCheck;
        if (((int)AppMain.g_obj.flag & 65536) != 0)
            tcbWork.flag |= 16U;
        tcbWork.field_ajst_w_db_f = (sbyte)2;
        tcbWork.field_ajst_w_db_b = (sbyte)4;
        tcbWork.field_ajst_w_dl_f = (sbyte)2;
        tcbWork.field_ajst_w_dl_b = (sbyte)4;
        tcbWork.field_ajst_w_dt_f = (sbyte)2;
        tcbWork.field_ajst_w_dt_b = (sbyte)4;
        tcbWork.field_ajst_w_dr_f = (sbyte)2;
        tcbWork.field_ajst_w_dr_b = (sbyte)4;
        tcbWork.field_ajst_h_db_r = (sbyte)1;
        tcbWork.field_ajst_h_db_l = (sbyte)1;
        tcbWork.field_ajst_h_dl_r = (sbyte)1;
        tcbWork.field_ajst_h_dl_l = (sbyte)1;
        tcbWork.field_ajst_h_dt_r = (sbyte)1;
        tcbWork.field_ajst_h_dt_l = (sbyte)1;
        tcbWork.field_ajst_h_dr_r = (sbyte)2;
        tcbWork.field_ajst_h_dr_l = (sbyte)2;
        AppMain.ObjObjectRegistObject(tcbWork);
        return tcbWork;
    }

    public static void ObjObjectRegistObject(AppMain.OBS_OBJECT_WORK pWork)
    {
        pWork.prev = AppMain.g_obj.obj_list_tail;
        pWork.next = (AppMain.OBS_OBJECT_WORK)null;
        if (pWork.prev != null)
            pWork.prev.next = pWork;
        else
            AppMain.g_obj.obj_list_head = pWork;
        AppMain.g_obj.obj_list_tail = pWork;
    }

    private static void ObjObjectRevokeObject(AppMain.OBS_OBJECT_WORK pWork)
    {
        if (pWork.prev != null)
            pWork.prev.next = pWork.next;
        else
            AppMain.g_obj.obj_list_head = pWork.next;
        if (pWork.next != null)
            pWork.next.prev = pWork.prev;
        else
            AppMain.g_obj.obj_list_tail = pWork.prev;
    }

    private static void ObjObjectClearAllObject()
    {
        AppMain.OBS_OBJECT_WORK next;
        for (AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.g_obj.obj_list_head; obsObjectWork != null; obsObjectWork = next)
        {
            next = obsObjectWork.next;
            obsObjectWork.flag |= 4U;
        }
    }

    private static bool ObjObjectCheckClearAllObject()
    {
        return AppMain.g_obj.obj_list_head == null;
    }

    private static AppMain.OBS_OBJECT_WORK ObjObjectSearchRegistObject(
      AppMain.OBS_OBJECT_WORK obj_work,
      ushort obj_type)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = obj_work != null ? obj_work.next : AppMain.g_obj.obj_list_head;
        while (obsObjectWork != null && ((int)obsObjectWork.obj_type != (int)obj_type && obj_type != ushort.MaxValue))
            obsObjectWork = obsObjectWork.next;
        return obsObjectWork;
    }

    private static void ObjObjectTypeSet(AppMain.OBS_OBJECT_WORK pObj, ushort usType)
    {
        pObj.obj_type = usType;
    }

    private static void ObjObjectParentSet(
      AppMain.OBS_OBJECT_WORK pObj,
      AppMain.OBS_OBJECT_WORK pParent,
      uint ulFlag)
    {
        pObj.parent_obj = pParent;
        pObj.flag &= 4294963711U;
        pObj.flag |= ulFlag & 3584U;
    }

    private static object ObjObjecExWorkAlloc(AppMain.OBS_OBJECT_WORK pObj, uint ulSize)
    {
        if (pObj.ex_work != null)
            pObj.ex_work = (object)null;
        if (ulSize != 0U)
        {
            pObj.ex_work = (object)new byte[(int)ulSize];
            pObj.flag |= 8388608U;
        }
        return pObj.ex_work;
    }

    public static void ObjObjectMain(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.OBS_OBJECT_WORK tcbWork = AppMain.mtTaskGetTcbWork(tcb);
        if (((int)tcbWork.flag & 4) != 0)
            AppMain.objObjectExitDataRelease(tcb);
        else if (((int)tcbWork.flag & 8) != 0)
            tcbWork.flag |= 4U;
        else if (((int)tcbWork.flag & 16) == 0 && tcbWork.ppViewCheck != null && tcbWork.ppViewCheck(tcbWork) != 0)
        {
            tcbWork.flag |= 4U;
        }
        else
        {
            if (AppMain.objObjectParent(tcbWork) != (ushort)0)
                return;
            if (tcbWork.obj_3d != null)
            {
                if (!AppMain.ObjAction3dNNModelLoadCheck(tcbWork.obj_3d) && ((int)tcbWork.flag & 256) == 0)
                    return;
            }
            else if (tcbWork.obj_2d != null && !AppMain.ObjAction2dAMALoadCheck(tcbWork.obj_2d) && ((int)tcbWork.flag & 256) == 0)
                return;
            if (AppMain._g_obj.ppObjPre != null)
                AppMain._g_obj.ppObjPre(tcbWork);
            AppMain.objObjectColRideTouchCheck(tcbWork);
            tcbWork.pos.x -= tcbWork.prev_temp_ofst.x;
            tcbWork.pos.y -= tcbWork.prev_temp_ofst.y;
            tcbWork.pos.z -= tcbWork.prev_temp_ofst.z;
            uint num = AppMain.ObjObjectPauseCheck(tcbWork.flag);
            if ((num == 0U || ((int)tcbWork.flag & 64) != 0) && tcbWork.ppIn != null)
                tcbWork.ppIn(tcbWork);
            if (((int)tcbWork.disp_flag & 536870912) != 0 && ((int)tcbWork.flag & 1073741824) != 0)
            {
                tcbWork.flag |= 2147483648U;
                tcbWork.disp_flag |= 1073741824U;
            }
            else
            {
                tcbWork.flag &= (uint)int.MaxValue;
                tcbWork.disp_flag &= 3221225471U;
            }
            if (num == 0U)
            {
                if (tcbWork.vib_timer != 0)
                    tcbWork.vib_timer = AppMain.ObjTimeCountDown(tcbWork.vib_timer);
                if (tcbWork.hitstop_timer != 0)
                {
                    tcbWork.hitstop_timer = AppMain.ObjTimeCountDown(tcbWork.hitstop_timer);
                    if (((int)tcbWork.flag & 8192) != 0)
                        tcbWork.hitstop_timer = 0;
                }
                if (((int)AppMain._g_obj.flag & 32768) == 0 || tcbWork.hitstop_timer == 0)
                {
                    if (tcbWork.invincible_timer != 0)
                        tcbWork.invincible_timer = AppMain.ObjTimeCountDown(tcbWork.invincible_timer);
                    if (((int)tcbWork.flag & -2147483520) == 0 && tcbWork.ppFunc != null)
                        tcbWork.ppFunc(tcbWork);
                }
                if (((int)tcbWork.flag & int.MinValue) == 0)
                {
                    if (((int)tcbWork.move_flag & 8192) == 0 && tcbWork.ppMove != null)
                        tcbWork.ppMove(tcbWork);
                    if (((int)AppMain._g_obj.flag & 48) != 0 && ((int)tcbWork.move_flag & 256) == 0 && (((int)AppMain._g_obj.flag & 2097152) == 0 || tcbWork.hitstop_timer == 0))
                    {
                        if (tcbWork.ppCol != null)
                            tcbWork.ppCol(tcbWork);
                        else if (AppMain._g_obj.ppCollision != null)
                            AppMain._g_obj.ppCollision(tcbWork);
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
                    tcbWork.ofst.x += (int)AppMain.g_object_vib_tbl[tcbWork.vib_timer >> 13 & 15];
                    tcbWork.ofst.y += (int)AppMain.g_object_vib_tbl[(tcbWork.vib_timer >> 13) + 1 & 15];
                }
                if ((num == 0U || ((int)tcbWork.flag & 65536) != 0) && ((int)AppMain._g_obj.flag & 64) != 0)
                {
                    if (tcbWork.ppRec != null)
                        tcbWork.ppRec(tcbWork);
                    if (AppMain._g_obj.ppRegRecAuto != null)
                        AppMain._g_obj.ppRegRecAuto(tcbWork);
                }
                if ((num == 0U || ((int)tcbWork.flag & 262144) != 0) && tcbWork.ppLast != null)
                    tcbWork.ppLast(tcbWork);
            }
            if (AppMain._g_obj.ppObjPost == null)
                return;
            AppMain._g_obj.ppObjPost(tcbWork);
        }
    }

    private static void ObjObjectExit(AppMain.MTS_TASK_TCB pTcb)
    {
        AppMain.OBS_OBJECT_WORK tcbWork = AppMain.mtTaskGetTcbWork(pTcb);
        if (tcbWork.obj_3d != null)
        {
            AppMain.ObjAction3dNNMotionRelease(tcbWork.obj_3d);
            if (((int)tcbWork.flag & 536870912) == 0)
            {
                if (tcbWork.obj_3d._object != null)
                    tcbWork.obj_3d._object = (AppMain.NNS_OBJECT)null;
                if (tcbWork.obj_3d.texlistbuf != null)
                    tcbWork.obj_3d.texlistbuf = (object)null;
            }
        }
        if (tcbWork.obj_3des != null)
        {
            if (tcbWork.obj_3des._object != null)
                tcbWork.obj_3des._object = (AppMain.NNS_OBJECT)null;
            if (tcbWork.obj_3des.model_data_work != null)
            {
                AppMain.ObjDataRelease(tcbWork.obj_3des.model_data_work);
                tcbWork.obj_3des.model_data_work = (AppMain.OBS_DATA_WORK)null;
            }
            else if (tcbWork.obj_3des.model != null)
            {
                int num1 = (int)tcbWork.obj_3des.flag & 262144;
            }
            tcbWork.obj_3des.model = (object)null;
            if (tcbWork.obj_3des.texlistbuf != null)
                tcbWork.obj_3des.texlistbuf = (object)null;
            if (tcbWork.obj_3des.ambtex_data_work != null)
            {
                AppMain.ObjDataRelease(tcbWork.obj_3des.ambtex_data_work);
                tcbWork.obj_3des.ambtex_data_work = (AppMain.OBS_DATA_WORK)null;
            }
            else if (tcbWork.obj_3des.ambtex != null)
            {
                int num2 = (int)tcbWork.obj_3des.flag & 131072;
            }
            tcbWork.obj_3des.ambtex = (object)null;
            if (tcbWork.obj_3des.ecb != null)
            {
                AppMain.amEffectDelete(tcbWork.obj_3des.ecb);
                tcbWork.obj_3des.ecb = (AppMain.AMS_AME_ECB)null;
            }
            if (tcbWork.obj_3des.eff_data_work != null)
            {
                AppMain.ObjDataRelease(tcbWork.obj_3des.eff_data_work);
                tcbWork.obj_3des.eff_data_work = (AppMain.OBS_DATA_WORK)null;
            }
            else if (tcbWork.obj_3des.eff != null)
            {
                int num3 = (int)tcbWork.obj_3des.flag & 65536;
            }
            tcbWork.obj_3des.eff = (object)null;
        }
        if (tcbWork.obj_2d != null && tcbWork.obj_2d.act != null)
        {
            AppMain.AoActDelete(tcbWork.obj_2d.act);
            tcbWork.obj_2d.act = (AppMain.AOS_ACTION)null;
        }
        if (tcbWork.col_work != null)
        {
            if (tcbWork.col_work.diff_data_work != null)
                AppMain.ObjDataRelease(tcbWork.col_work.diff_data_work);
            else if (tcbWork.col_work.obj_col.diff_data != null)
            {
                int num1 = (int)tcbWork.col_work.obj_col.flag & 134217728;
            }
            if (tcbWork.col_work.dir_data_work != null)
                AppMain.ObjDataRelease(tcbWork.col_work.dir_data_work);
            else if (tcbWork.col_work.obj_col.dir_data != null)
            {
                int num2 = (int)tcbWork.col_work.obj_col.flag & 268435456;
            }
            if (tcbWork.col_work.attr_data_work != null)
                AppMain.ObjDataRelease(tcbWork.col_work.attr_data_work);
            else if (tcbWork.col_work.obj_col.attr_data != null)
            {
                int num3 = (int)tcbWork.col_work.obj_col.flag & 536870912;
            }
        }
        if (((int)tcbWork.flag & 520093696) != 0)
        {
            if (((int)tcbWork.flag & 134217728) != 0)
            {
                AppMain.OBS_ACTION3D_NN_WORK obj3d = tcbWork.obj_3d;
            }
            if (((int)tcbWork.flag & 268435456) != 0)
            {
                AppMain.OBS_ACTION3D_ES_WORK obj3des = tcbWork.obj_3des;
            }
            if (((int)tcbWork.flag & 67108864) != 0)
            {
                AppMain.OBS_ACTION2D_AMA_WORK obj2d = tcbWork.obj_2d;
            }
            if (((int)tcbWork.flag & 16777216) != 0)
            {
                AppMain.OBS_COLLISION_WORK colWork = tcbWork.col_work;
            }
            if (((int)tcbWork.flag & 33554432) != 0)
            {
                int num = (AppMain.ArrayPointer<AppMain.OBS_RECT_WORK>)(AppMain.OBS_RECT_WORK[])null != tcbWork.rect_work ? 1 : 0;
            }
        }
        if (tcbWork.ex_work != null)
        {
            int num4 = (int)tcbWork.flag & 8388608;
        }
        AppMain.ObjObjectRevokeObject(tcbWork);
    }

    private static int ObjObjectViewOutCheck(AppMain.OBS_OBJECT_WORK pWork)
    {
        return AppMain.ObjViewOutCheck(pWork.pos.x, pWork.pos.y, pWork.view_out_ofst, pWork.view_out_ofst_plus[0], pWork.view_out_ofst_plus[1], pWork.view_out_ofst_plus[2], pWork.view_out_ofst_plus[3]);
    }

    private static void ObjObjectRectRegist(AppMain.OBS_OBJECT_WORK pWork, AppMain.OBS_RECT_WORK pRec)
    {
        if (((int)pWork.flag & 12) != 0 || ((int)AppMain.g_obj.flag & 64) == 0 || ((int)pWork.flag & 2) != 0)
            return;
        pRec.parent_obj = pWork;
        pRec.flag &= 4294967292U;
        if (AppMain.ObjObjectDirFallReverseCheck(pWork.dir_fall) != 0U)
        {
            pRec.flag ^= 2U;
            pRec.flag ^= 1U;
        }
        AppMain.ObjRectRegist(pRec);
    }

    private static void ObjObjectGetRectBuf(
      AppMain.OBS_OBJECT_WORK pWork,
      AppMain.ArrayPointer<AppMain.OBS_RECT_WORK> rect_work,
      ushort rect_num)
    {
        if ((ushort)((uint)rect_num - 1U) > (ushort)31)
            return;
        if ((AppMain.ArrayPointer<AppMain.OBS_RECT_WORK>)(AppMain.OBS_RECT_WORK[])null != pWork.rect_work)
        {
            if (((int)pWork.flag & 33554432) == 0)
                return;
            AppMain.ObjObjectReleaseRectBuf(pWork);
        }
        if (rect_work == (AppMain.ArrayPointer<AppMain.OBS_RECT_WORK>)(AppMain.OBS_RECT_WORK[])null)
        {
            pWork.rect_work = (AppMain.ArrayPointer<AppMain.OBS_RECT_WORK>)new AppMain.OBS_RECT_WORK[(int)rect_num];
            pWork.flag |= 33554432U;
            pWork.rect_num = (uint)rect_num;
        }
        else
        {
            pWork.rect_num = (uint)rect_num;
            pWork.rect_work = rect_work;
        }
    }

    private static void ObjObjectReleaseRectBuf(AppMain.OBS_OBJECT_WORK pWork)
    {
        if (((int)pWork.flag & 33554432) == 0 || !((AppMain.ArrayPointer<AppMain.OBS_RECT_WORK>)(AppMain.OBS_RECT_WORK[])null != pWork.rect_work))
            return;
        pWork.rect_work = (AppMain.ArrayPointer<AppMain.OBS_RECT_WORK>)(AppMain.OBS_RECT_WORK[])null;
        pWork.flag &= 4261412863U;
    }

    private static void ObjObjectSetRectWork(
      AppMain.OBS_OBJECT_WORK pWork,
      AppMain.OBS_RECT_WORK rect_work)
    {
        rect_work.parent_obj = pWork;
        rect_work.group_no = (byte)0;
        rect_work.target_g_flag = (byte)1;
        rect_work.hit_power = (short)64;
        rect_work.def_power = (short)63;
        rect_work.hit_flag = (ushort)2;
        rect_work.def_flag = (ushort)1;
    }

    private static AppMain.OBS_RECT_WORK ObjObjectRectGet(
      AppMain.OBS_OBJECT_WORK pWork,
      ushort usIndex)
    {
        return pWork.rect_work[(int)usIndex];
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
        short num1 = AppMain.g_obj.clip_lcd_size[0];
        short num2 = AppMain._g_obj.clip_lcd_size[1];
        if (AppMain._g_obj.glb_scale.x != 4096)
            num1 = (short)AppMain.FX_Mul((int)num1, 8192 - AppMain._g_obj.glb_scale.x);
        if (AppMain._g_obj.glb_scale.y != 4096)
            num2 = (short)AppMain.FX_Mul((int)num2, 8192 - AppMain._g_obj.glb_scale.y);
        if (((int)AppMain._g_obj.flag & 8) == 0)
            return 0;
        int num3 = (AppMain._g_obj.clip_camera[0] >> 12) - (int)sOfst;
        int num4 = (AppMain._g_obj.clip_camera[1] >> 12) - (int)sOfst;
        int num5 = (int)num1 + ((int)sOfst << 1);
        int num6 = (int)num2 + ((int)sOfst << 1);
        int num7 = num3 + (int)sLeft;
        int num8 = num4 + (int)sTop;
        int num9 = num6 + ((int)-sTop + (int)sBottom);
        int num10 = num5 + ((int)-sLeft + (int)sRight);
        return num7 <= lPosX >> 12 && num7 + num10 >= lPosX >> 12 && (num8 <= lPosY >> 12 && num8 + num9 >= lPosY >> 12) ? 0 : 1;
    }

    private static void ObjObjectSpdDirFall(ref int sSpdX, ref int sSpdY, ushort ucDirFall)
    {
        int num1 = sSpdX;
        int num2 = sSpdY;
        float num3 = AppMain.nnSin((int)ucDirFall);
        float num4 = AppMain.nnCos((int)ucDirFall);
        float num5 = (float)num1 * num3;
        float num6 = (float)num1 * num4;
        float num7 = (float)num2 * num3;
        float num8 = (float)num2 * num4;
        sSpdX = (int)AppMain.nnRoundOff(num6 - num7);
        sSpdY = (int)AppMain.nnRoundOff(num5 + num8);
    }

    private static uint ObjObjectDirFallReverseCheck(ushort ucDirFall)
    {
        return ucDirFall > (ushort)24576 && ucDirFall < (ushort)40960 ? 1U : 0U;
    }

    private static int ObjTimeCountGet(int count)
    {
        return AppMain.g_obj.speed != 4096 ? AppMain.FX_Mul(count, AppMain.g_obj.speed) : count;
    }

    private static int ObjTimeCountDown(int timer)
    {
        if (AppMain.g_obj.speed == 4096)
            timer -= 4096;
        else
            timer -= AppMain.FX_Mul(4096, AppMain.g_obj.speed);
        if (timer < 0)
            timer = 0;
        return timer;
    }

    private static int ObjTimeCountUp(int timer)
    {
        if (AppMain.g_obj.speed == 4096)
            timer += 4096;
        else
            timer += AppMain.FX_Mul(4096, AppMain.g_obj.speed);
        if (timer < 0)
            timer = 0;
        return timer;
    }

    private static float ObjTimeCountDownF(float timer)
    {
        if (AppMain.g_obj.speed == 4096)
            --timer;
        else
            timer -= (float)(1.0 * (double)AppMain.g_obj.speed / 4096.0);
        if ((double)timer < 0.0)
            timer = 0.0f;
        return timer;
    }

    private static float ObjTimeCountUpF(float timer)
    {
        if (AppMain.g_obj.speed == 4096)
            ++timer;
        else
            timer += (float)(1.0 * (double)AppMain.g_obj.speed / 4096.0);
        if ((double)timer < 0.0)
            timer = 0.0f;
        return timer;
    }

    private static int ObjObjectMapOutCheck(AppMain.OBS_OBJECT_WORK pWork)
    {
        return AppMain.ObjMapOutCheck(pWork.pos.x, pWork.pos.y, pWork.view_out_ofst, pWork.view_out_ofst_plus[0], pWork.view_out_ofst_plus[1], pWork.view_out_ofst_plus[2], pWork.view_out_ofst_plus[3]);
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
        if (((int)AppMain.g_obj.flag & 16) != 0)
        {
            AppMain.OBS_BLOCK_COLLISION blockCollision = AppMain.ObjGetBlockCollision();
            if (blockCollision == null)
                return 0;
            num1 = blockCollision.left - (int)sOfst;
            num2 = blockCollision.top - (int)sOfst;
            num3 = blockCollision.right - blockCollision.left + ((int)sOfst << 1);
            num4 = blockCollision.bottom - blockCollision.top + ((int)sOfst << 1);
        }
        else
        {
            AppMain.OBS_DIFF_COLLISION diffCollision = AppMain.ObjGetDiffCollision();
            if (diffCollision == null)
                return 0;
            num1 = diffCollision.left - (int)sOfst;
            num2 = diffCollision.top - (int)sOfst;
            num3 = diffCollision.right - diffCollision.left + ((int)sOfst << 1);
            num4 = diffCollision.bottom - diffCollision.top + ((int)sOfst << 1);
        }
        return num1 <= lPosX >> 12 && num1 + num3 >= lPosX >> 12 && (num2 <= lPosY >> 12 && num2 + num4 >= lPosY >> 12) ? 0 : 1;
    }

    private static void objMain(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.g_obj.scale.x = AppMain.FX_Mul(AppMain.g_obj.glb_scale.x, AppMain.g_obj.draw_scale.x);
        AppMain.g_obj.scale.y = AppMain.FX_Mul(AppMain.g_obj.glb_scale.y, AppMain.g_obj.draw_scale.y);
        AppMain.g_obj.scale.z = AppMain.FX_Mul(AppMain.g_obj.glb_scale.z, AppMain.g_obj.draw_scale.z);
        AppMain.g_obj.inv_scale.x = AppMain.FX_Div(4096, AppMain.g_obj.scale.x);
        AppMain.g_obj.inv_scale.y = AppMain.FX_Div(4096, AppMain.g_obj.scale.y);
        AppMain.g_obj.inv_scale.z = AppMain.FX_Div(4096, AppMain.g_obj.scale.z);
        if (AppMain.g_obj.ppPre != null)
            AppMain.g_obj.ppPre();
        if (((int)AppMain.g_obj.flag & 64) != 0)
            AppMain.ObjRectCheckAllGroup();
        if (AppMain.g_obj.glb_camera_id >= 0)
        {
            AppMain.ObjDraw3DNNSetCameraEx(AppMain.g_obj.glb_camera_id, AppMain.g_obj.glb_camera_type, 15U);
            AppMain.ObjDraw3DNNSetCameraEx(AppMain.g_obj.glb_camera_id, AppMain.g_obj.glb_camera_type, 0U);
        }
        if (AppMain.g_obj.ppDrawSort != null)
        {
            AppMain.g_obj.ppDrawSort();
            for (AppMain.OBS_OBJECT_WORK pWork = AppMain.g_obj.obj_draw_list_head; pWork != null; pWork = pWork.draw_next)
                AppMain.objObjectDraw(pWork);
        }
        else
        {
            for (AppMain.OBS_OBJECT_WORK pWork = AppMain.g_obj.obj_list_head; pWork != null; pWork = pWork.next)
                AppMain.objObjectDraw(pWork);
            AppMain.GmGmkPulleyDrawServerMain();
            AppMain.GmTvxExecuteDraw();
            AppMain.gmDecoDrawServerMain((AppMain.MTS_TASK_TCB)null);
        }
        AppMain.ObjDrawAction2DAMADrawStart();
        AppMain.ObjDrawNNStart();
        if (((int)AppMain.g_obj.flag & 1) == 0)
            AppMain.ObjCollisionObjectClear();
        AppMain.g_obj.timer_fx += AppMain.ObjTimeCountGet(4096);
        AppMain.g_obj.flag |= 4096U;
        if ((int)AppMain.g_obj.timer == AppMain.g_obj.timer_fx >> 12)
            AppMain.g_obj.flag &= 4294963199U;
        AppMain.g_obj.timer = (uint)(AppMain.g_obj.timer_fx >> 12);
        if (((int)AppMain.g_obj.flag & 2) != 0)
            AppMain.g_obj.flag |= 1U;
        else
            AppMain.g_obj.flag &= 4294967294U;
        if (AppMain.g_obj.ppPost == null)
            return;
        AppMain.g_obj.ppPost();
    }

    private static void objObjectDraw(AppMain.OBS_OBJECT_WORK pWork)
    {
        uint num1 = 0;
        uint num2 = AppMain.ObjObjectPauseCheck(pWork.flag);
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

    private static void objDestructor(AppMain.MTS_TASK_TCB pTcb)
    {
        AppMain.obj_ptcb = (AppMain.MTS_TASK_TCB)null;
        AppMain.ObjSetBlockCollision((AppMain.OBS_BLOCK_COLLISION)null);
        AppMain.ObjSetDiffCollision((AppMain.OBS_DIFF_COLLISION)null);
        if (((int)AppMain.g_obj.flag & 1073741824) != 0)
        {
            AppMain.obj_data_max_save = AppMain.g_obj.data_max;
            AppMain.obj_data_work_save = AppMain.g_obj.pData;
            AppMain.g_obj.data_max = 0;
            AppMain.g_obj.pData = (AppMain.OBS_DATA_WORK[])null;
        }
        else
            AppMain.ObjDataFree();
    }

    private static void objExitWait(AppMain.MTS_TASK_TCB pTcb)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.ObjObjectSearchRegistObject((AppMain.OBS_OBJECT_WORK)null, ushort.MaxValue);
        if (obj_work == null)
        {
            AppMain.mtTaskClearTcb(AppMain.obj_ptcb);
            AppMain.g_obj = new AppMain.OBS_OBJECT();
        }
        else
        {
            for (; obj_work != null; obj_work = AppMain.ObjObjectSearchRegistObject(obj_work, ushort.MaxValue))
                obj_work.flag |= 4U;
        }
    }

    private static void objObjectExitDataRelease(AppMain.MTS_TASK_TCB tcb)
    {
        bool flag = false;
        AppMain.OBS_OBJECT_WORK tcbWork = AppMain.mtTaskGetTcbWork(tcb);
        if (tcbWork.ppUserRelease != null && tcbWork.ppUserRelease(tcbWork))
            flag = true;
        if (tcbWork.obj_3d != null && ((int)tcbWork.flag & 536870912) == 0)
        {
            AppMain.ObjAction3dNNModelRelease(tcbWork.obj_3d);
            flag = true;
        }
        if (tcbWork.obj_3des != null)
        {
            if (tcbWork.obj_3des.texlist != null)
            {
                AppMain.ObjAction3dESTextureRelease(tcbWork.obj_3des);
                flag = true;
            }
            if (tcbWork.obj_3des.model != null)
            {
                AppMain.ObjAction3dESModelRelease(tcbWork.obj_3des);
                flag = true;
            }
        }
        if (tcbWork.obj_2d != null && tcbWork.obj_2d.ao_tex.texlist != null)
        {
            AppMain.AoTexRelease(tcbWork.obj_2d.ao_tex);
            flag = true;
        }
        if (flag)
            AppMain.mtTaskChangeTcbProcedure(tcb, AppMain._objObjectDataReleaseCheck);
        else
            AppMain.mtTaskClearTcb(tcb);
    }

    private static void objObjectDataReleaseCheck(AppMain.MTS_TASK_TCB tcb)
    {
        bool flag1 = true;
        bool flag2 = true;
        bool flag3 = true;
        bool flag4 = true;
        AppMain.OBS_OBJECT_WORK tcbWork = AppMain.mtTaskGetTcbWork(tcb);
        if (tcbWork.ppUserReleaseWait != null && tcbWork.ppUserReleaseWait(tcbWork))
            flag1 = false;
        if (tcbWork.obj_3d != null && ((int)tcbWork.flag & 536870912) == 0)
        {
            if (AppMain.ObjAction3dNNModelReleaseCheck(tcbWork.obj_3d))
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
                AppMain.ObjAction3dESEffectRelease(tcbWork.obj_3des);
            if (!AppMain.ObjAction3dESModelReleaseCheck(tcbWork.obj_3des))
                flag3 = false;
            if (!AppMain.ObjAction3dESTextureReleaseCheck(tcbWork.obj_3des))
                flag3 = false;
        }
        if (tcbWork.obj_2d != null && tcbWork.obj_2d.ao_tex.texlist != null && !AppMain.AoTexIsReleased(tcbWork.obj_2d.ao_tex))
            flag4 = false;
        if (!flag2 || !flag3 || (!flag1 || !flag4))
            return;
        AppMain.mtTaskClearTcb(tcb);
    }

    private static ushort objObjectParent(AppMain.OBS_OBJECT_WORK pWork)
    {
        AppMain.OBS_OBJECT_WORK parentObj = pWork.parent_obj;
        if (parentObj != null)
        {
            if (((int)parentObj.flag & 4) != 0)
            {
                if (((int)pWork.flag & 512) == 0)
                {
                    pWork.flag |= 4U;
                    pWork.parent_obj = (AppMain.OBS_OBJECT_WORK)null;
                    return 1;
                }
                pWork.parent_obj = (AppMain.OBS_OBJECT_WORK)null;
            }
            if (AppMain.ObjObjectPauseCheck(pWork.flag) == 0U)
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
                        pWork.hitstop_timer += AppMain.ObjTimeCountGet(4096);
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

    private static void ObjObjectCollision(AppMain.OBS_OBJECT_WORK pWork)
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
            if (Math.Abs(pWork.pos.x - pWork.prev_pos.x) > (int)num1 || Math.Abs(pWork.pos.y - pWork.prev_pos.y) > (int)num1)
            {
                pWork.pos.x = pWork.prev_pos.x;
                pWork.pos.y = pWork.prev_pos.y;
                while (true)
                {
                    int y4;
                    do
                    {
                        if (Math.Abs(pWork.pos.x - x1) > (int)num1)
                        {
                            pWork.prev_pos.x = pWork.pos.x;
                            pWork.pos.x = x1 <= pWork.prev_pos.x ? pWork.prev_pos.x - (int)num1 : pWork.prev_pos.x + (int)num1;
                        }
                        else
                            pWork.pos.x = x1;
                        if (Math.Abs(pWork.pos.y - y1) > (int)num1)
                        {
                            pWork.prev_pos.y = pWork.pos.y;
                            pWork.pos.y = y1 <= pWork.prev_pos.y ? pWork.prev_pos.y - (int)num1 : pWork.prev_pos.y + (int)num1;
                        }
                        else
                            pWork.pos.y = y1;
                        if (pWork.pos.x != x1 || pWork.pos.y != y1)
                        {
                            int x4 = pWork.pos.x;
                            y4 = pWork.pos.y;
                            AppMain.ObjDiffCollisionEarthCheck(pWork);
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
            AppMain.ObjDiffCollisionEarthCheck(pWork);
            uint num4 = num2 | pWork.col_flag;
            pWork.col_flag = num4;
            pWork.move_flag |= num3;
            if (((int)pWork.move_flag & 32) != 0 && ((int)pWork.col_flag & 2) == 0)
                pWork.move_flag &= 4294967263U;
        }
        pWork.prev_pos.x = x3;
        pWork.prev_pos.y = y3;
    }

    private static void ObjObjectMove(AppMain.OBS_OBJECT_WORK pWork)
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
        if ((x != 0 || y != 0) && pWork.dir_fall != (ushort)0)
            AppMain.ObjObjectSpdDirFall(ref x, ref y, pWork.dir_fall);
        if (pWork.hitstop_timer != 0)
        {
            pWork.move.x = AppMain.FX_Mul(x, AppMain._g_obj.speed);
            pWork.move.y = AppMain.FX_Mul(y, AppMain._g_obj.speed);
            pWork.move.z = AppMain.FX_Mul(pWork.flow.z, AppMain._g_obj.speed);
        }
        else
        {
            if (((int)pWork.move_flag & 1) == 0)
            {
                if (((int)pWork.move_flag & 128) != 0 && ((int)pWork.move_flag & 1) == 0)
                    pWork.spd.y += AppMain.FX_Mul(pWork.spd_fall, AppMain._g_obj.speed);
                if (((int)pWork.move_flag & 128) != 0 && pWork.spd.y > pWork.spd_fall_max)
                    pWork.spd.y = pWork.spd_fall_max;
            }
            if (((int)pWork.move_flag & 64) != 0)
            {
                if (((int)pWork.move_flag & 131072) != 0 && (pWork.spd_m != 0 || ((int)pWork.move_flag & 262144) == 0))
                {
                    int sSpd = AppMain.FX_Mul(pWork.spd_slope, AppMain.mtMathSin((int)pWork.dir.z));
                    if (sSpd != 0)
                        pWork.spd_m = AppMain.ObjSpdUpSet(pWork.spd_m, sSpd, pWork.spd_slope_max);
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
                    num1 = AppMain.FX_Mul(pWork.spd_m, AppMain.mtMathCos((int)pWork.dir.z));
                    num2 = AppMain.FX_Mul(pWork.spd_m, AppMain.mtMathSin((int)pWork.dir.z));
                }
            }
            if (((int)pWork.move_flag & 67108864) != 0)
            {
                pWork.move.x = AppMain.FX_Mul(pWork.spd.x + num1 + x, AppMain._g_obj.speed);
                pWork.move.y = AppMain.FX_Mul(pWork.spd.y + num2 + y, AppMain._g_obj.speed);
            }
            else
            {
                pWork.move.x = AppMain.FX_Mul(pWork.spd.x + num1 + x + AppMain._g_obj.scroll[0], AppMain._g_obj.speed);
                pWork.move.y = AppMain.FX_Mul(pWork.spd.y + num2 + y + AppMain._g_obj.scroll[1], AppMain._g_obj.speed);
            }
            pWork.move.z = AppMain.FX_Mul(pWork.spd.z + num3 + pWork.flow.z, AppMain._g_obj.speed);
            AppMain.ObjObjectSpdDirFall(ref pWork.move.x, ref pWork.move.y, pWork.dir_fall);
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

    private static void objObjectColRideTouchCheck(AppMain.OBS_OBJECT_WORK pWork)
    {
        if (pWork.col_work != null)
        {
            if (pWork.col_work.obj_col.rider_obj != null && ((int)pWork.col_work.obj_col.rider_obj.flag & 4) != 0)
                pWork.col_work.obj_col.rider_obj = (AppMain.OBS_OBJECT_WORK)null;
            if (pWork.col_work.obj_col.toucher_obj != null && pWork.col_work.obj_col.toucher_obj != pWork.col_work.obj_col.rider_obj && pWork.col_work.obj_col.toucher_obj != null)
            {
                if (AppMain.ObjObjectPauseCheck(pWork.flag) == 0U && pWork.col_work.obj_col.toucher_obj != pWork.col_work.obj_col.rider_obj && (((int)pWork.col_work.obj_col.toucher_obj.move_flag & 16777216) != 0 && ((int)pWork.move_flag & 33554432) != 0))
                {
                    int num = AppMain.MTM_MATH_CLIP(AppMain.MTM_MATH_CLIP(pWork.col_work.obj_col.toucher_obj.move.x, -pWork.col_work.obj_col.toucher_obj.push_max, pWork.col_work.obj_col.toucher_obj.push_max), -pWork.push_max, pWork.push_max);
                    pWork.flow.x += num;
                }
                if (((int)pWork.col_work.obj_col.toucher_obj.flag & 4) != 0)
                    pWork.col_work.obj_col.toucher_obj = (AppMain.OBS_OBJECT_WORK)null;
            }
        }
        if (pWork.touch_obj != null)
        {
            if (AppMain.ObjObjectPauseCheck(pWork.flag) == 0U && ((int)pWork.touch_obj.move_flag & 33554432) != 0 && ((int)pWork.move_flag & 16777216) != 0)
            {
                pWork.flow.x += (int)(short)(pWork.touch_obj.pos.x - pWork.touch_obj.prev_pos.x);
                if ((pWork.touch_obj.move.x & 4095) != 0)
                {
                    if (pWork.touch_obj.move.x > 0)
                        pWork.flow.x += 4096 - (pWork.touch_obj.move.x & 4095);
                    else
                        pWork.flow.x -= 4096 + (pWork.touch_obj.move.x & 4095);
                }
            }
            if (((int)pWork.touch_obj.flag & 4) != 0)
                pWork.touch_obj = (AppMain.OBS_OBJECT_WORK)null;
        }
        if (pWork.ride_obj == null)
            return;
        if (((int)pWork.ride_obj.flag & 4) != 0)
        {
            pWork.ride_obj = (AppMain.OBS_OBJECT_WORK)null;
        }
        else
        {
            if (AppMain.ObjObjectPauseCheck(pWork.flag) != 0U)
                return;
            if (((int)pWork.ride_obj.move_flag & 256) != 0)
            {
                pWork.flow.x += pWork.ride_obj.move.x;
                pWork.flow.y += pWork.ride_obj.move.y;
                pWork.flow.z += pWork.ride_obj.move.z;
            }
            else
            {
                pWork.flow.x += (int)(short)(pWork.ride_obj.pos.x - pWork.ride_obj.prev_pos.x);
                pWork.flow.y += (int)(short)(pWork.ride_obj.pos.y - pWork.ride_obj.prev_pos.y);
                pWork.flow.z += (int)(short)(pWork.ride_obj.pos.z - pWork.ride_obj.prev_pos.z);
            }
            if ((pWork.ride_obj.move.y & 4095) == 0)
                return;
            pWork.flow.y += 4096 - (pWork.ride_obj.move.y & 4095);
        }
    }

    private static void ObjObjectFieldRectSet(
      AppMain.OBS_OBJECT_WORK pObj,
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

    private static void ObjObjectFallSet(AppMain.OBS_OBJECT_WORK pObj, int fSpdFall, int fSpdFallMax)
    {
        pObj.spd_fall = fSpdFall;
        pObj.spd_fall_max = fSpdFallMax;
        pObj.move_flag |= 128U;
    }

}