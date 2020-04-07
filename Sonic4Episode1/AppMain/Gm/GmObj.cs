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
    private static void GmObjCollision(AppMain.OBS_OBJECT_WORK obj_work)
    {
        obj_work.ride_obj = (AppMain.OBS_OBJECT_WORK)null;
        obj_work.touch_obj = (AppMain.OBS_OBJECT_WORK)null;
        AppMain.ObjObjectCollision(obj_work);
    }

    private static void GmObjPreFunc()
    {
    }

    private static void GmObjRegistRectAuto(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (obj_work.rect_work.array != null && obj_work.rect_num > 0U)
        {
            int offset = obj_work.rect_work.offset;
            int num = offset + (int)obj_work.rect_num;
            for (int index = offset; index < num; ++index)
                AppMain.ObjObjectRectRegist(obj_work, obj_work.rect_work.array[index]);
        }
        if (obj_work.col_work == null || obj_work.col_work.obj_col.obj == null || AppMain.ObjObjectViewOutCheck(obj_work) != 0)
            return;
        AppMain.ObjCollisionObjectRegist(obj_work.col_work.obj_col);
    }

    private static void GmObjObjPreFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
    }

    private static void GmObjObjPostFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
    }

    private static bool GmObjCheckMapLeftLimit(AppMain.OBS_OBJECT_WORK obj_work, int ofst)
    {
        return ((int)obj_work.move_flag & 12) != 0 && obj_work.pos.x >> 12 <= AppMain.g_gm_main_system.map_fcol.left + ofst;
    }

    private static bool GmObjCheckMapRightLimit(AppMain.OBS_OBJECT_WORK obj_work, int ofst)
    {
        return ((int)obj_work.move_flag & 12) != 0 && obj_work.pos.x >> 12 >= AppMain.g_gm_main_system.map_fcol.right - ofst;
    }

    private static void GmObjSetClip(
      AppMain.OBS_OBJECT_WORK obj_work,
      short out_ofst,
      short plus_left,
      short plus_top,
      short plus_right,
      short plus_bottom)
    {
        obj_work.ppViewCheck = new AppMain.OBS_OBJECT_WORK_Delegate3(AppMain.ObjObjectViewOutCheck);
        obj_work.view_out_ofst = out_ofst;
        obj_work.view_out_ofst_plus[0] = plus_left;
        obj_work.view_out_ofst_plus[1] = plus_top;
        obj_work.view_out_ofst_plus[2] = plus_right;
        obj_work.view_out_ofst_plus[3] = plus_bottom;
        obj_work.flag &= 4294967279U;
    }

    private static void GmObjGetRotPosXY(
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

    private static void GmObjSetAllObjectNoDisp()
    {
        for (AppMain.OBS_OBJECT_WORK obj_work = AppMain.ObjObjectSearchRegistObject((AppMain.OBS_OBJECT_WORK)null, ushort.MaxValue); obj_work != null; obj_work = AppMain.ObjObjectSearchRegistObject(obj_work, ushort.MaxValue))
        {
            if (((int)obj_work.flag & 12) == 0)
                obj_work.disp_flag |= 32U;
        }
    }

    private static void GmObjSetObjectNoFunc(uint obj_type_flag)
    {
        uint num = obj_type_flag & (uint)int.MaxValue;
        for (ushort obj_type = 0; obj_type < (ushort)31 && num != 0U; num >>= 1)
        {
            if (((int)num & 1) != 0)
            {
                for (AppMain.OBS_OBJECT_WORK obj_work = AppMain.ObjObjectSearchRegistObject((AppMain.OBS_OBJECT_WORK)null, obj_type); obj_work != null; obj_work = AppMain.ObjObjectSearchRegistObject(obj_work, obj_type))
                {
                    if (((int)obj_work.flag & 12) == 0)
                    {
                        obj_work.flag |= 130U;
                        obj_work.move_flag |= 8448U;
                    }
                }
            }
            ++obj_type;
        }
    }
}