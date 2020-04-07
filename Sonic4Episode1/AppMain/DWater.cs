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
    private static int dwaterInit()
    {
        if (AppMain._dmap_water == null)
            AppMain._dmap_water = new AppMain.DMAP_WATER();
        AppMain._dmap_water.repeat_u = 1f;
        AppMain._dmap_water.repeat_v = 1f;
        AppMain._dmap_water.speed_u = 1f / 1000f;
        AppMain._dmap_water.speed_v = -1f / 1000f;
        AppMain._dmap_water.speed_surface = 1f / 1000f;
        AppMain._dmap_water.regist_index = -1;
        AppMain._dmap_water.repeat_pos_x = 280f;
        AppMain.dwaterSetColor(0.65f, 1f, 0.75f);
        return AppMain._dmap_water.regist_index;
    }

    private static void dwaterExit()
    {
        if (AppMain._dmap_water != null)
        {
            for (int index = 0; index < 2; ++index)
            {
                if (AppMain._dmap_water._object[index]._object != null)
                    AppMain._dmap_water._object[index]._object = (AppMain.NNS_OBJECT)null;
                if (AppMain._dmap_water._object[index].texlistbuf != null)
                    AppMain._dmap_water._object[index].texlistbuf = (object)null;
                if (AppMain._dmap_water._object[index].motion != null)
                    AppMain.amMotionDelete(AppMain._dmap_water._object[index].motion);
            }
        }
        AppMain._dmap_water = (AppMain.DMAP_WATER)null;
    }

    private static void dwaterSetObjectAMB(
      AppMain.AMS_AMB_HEADER amb_obj,
      AppMain.AMS_AMB_HEADER amb_tex)
    {
        AppMain._dmap_water.amb_object = amb_obj;
        AppMain._dmap_water.amb_texture = amb_tex;
    }

    private static int dwaterLoadObject(uint objflag)
    {
        int num1 = 1;
        AppMain.ArrayPointer<AppMain.DMAP_WATER_OBJ> arrayPointer = new AppMain.ArrayPointer<AppMain.DMAP_WATER_OBJ>(AppMain._dmap_water._object);
        if (AppMain._dmap_water.regist_index != -1)
        {
            if (!AppMain.amDrawIsRegistComplete(AppMain._dmap_water.regist_index))
                return 0;
            AppMain._dmap_water.regist_index = -1;
            if (((AppMain.DMAP_WATER_OBJ)~arrayPointer)._object != null)
            {
                int num2 = 0;
                while (num2 < 2)
                {
                    object file = AppMain._dmap_water.amb_object.buf[2 + num2];
                    ((AppMain.DMAP_WATER_OBJ)~arrayPointer).motion = AppMain.amMotionCreate(((AppMain.DMAP_WATER_OBJ)~arrayPointer)._object);
                    AppMain.amMotionMaterialRegistFile(((AppMain.DMAP_WATER_OBJ)~arrayPointer).motion, 0, file);
                    AppMain.amMotionMaterialSet(((AppMain.DMAP_WATER_OBJ)~arrayPointer).motion, 0);
                    AppMain.amMotionMaterialSetFrame(((AppMain.DMAP_WATER_OBJ)~arrayPointer).motion, AppMain.amMotionMaterialGetStartFrame(((AppMain.DMAP_WATER_OBJ)~arrayPointer).motion, 0));
                    ++num2;
                    ++arrayPointer;
                }
                return 1;
            }
        }
        int index = 0;
        while (index < 2)
        {
            object buf = AppMain.amBindGet(AppMain._dmap_water.amb_object, index);
            AppMain._dmap_water.regist_index = AppMain.amObjectLoad(out ((AppMain.DMAP_WATER_OBJ)~arrayPointer)._object, out ((AppMain.DMAP_WATER_OBJ)~arrayPointer).texlist, out ((AppMain.DMAP_WATER_OBJ)~arrayPointer).texlistbuf, buf, objflag, (string)null, AppMain._dmap_water.amb_texture);
            num1 = 0;
            ++index;
            ++arrayPointer;
        }
        return num1;
    }

    private static int dwaterLoadTexture(
      object water_image,
      int water_size,
      object color_image,
      int color_size)
    {
        return 0;
    }

    private static int dwaterRelease()
    {
        int num = -1;
        if (AppMain._dmap_water != null)
        {
            for (int index = 0; index < 2; ++index)
            {
                if (AppMain._dmap_water._object[index]._object != null)
                    num = AppMain.amObjectRelease(AppMain._dmap_water._object[index]._object, AppMain._dmap_water._object[index].texlist);
            }
        }
        return num;
    }

    private static void dwaterSetColor(float r, float g, float b)
    {
    }

    private static void dwaterUpdate(
      float speed,
      float pos_x,
      float pos_y,
      float dy,
      int rot_z,
      float scale)
    {
        float num1 = 0.006666667f;
        AppMain._dmap_water.speed_surface += 0.0005f;
        if ((double)AppMain._dmap_water.speed_surface > 1.0)
            --AppMain._dmap_water.speed_surface;
        AppMain._dmap_water.pos_x = pos_x;
        AppMain._dmap_water.pos_y = pos_y;
        AppMain._dmap_water.pos_dy = dy;
        AppMain._dmap_water.rot_z = rot_z;
        AppMain._dmap_water.scale = scale;
        float f1 = AppMain._dmap_water.ofst_u + AppMain._dmap_water.speed_u * speed;
        float num2 = (double)f1 < 0.0 ? (float)(1.0 - (-(double)f1 - (double)AppMain.floorf(-f1))) : f1 - AppMain.floorf(f1);
        AppMain._dmap_water.ofst_u = num2;
        float f2 = AppMain._dmap_water.ofst_v + AppMain._dmap_water.speed_v * speed;
        float num3 = (double)f2 < 0.0 ? (float)(1.0 - (-(double)f2 - (double)AppMain.floorf(-f2))) : f2 - AppMain.floorf(f2);
        AppMain._dmap_water.ofst_v = num3;
        float num4 = (float)((double)num1 * (double)AppMain._dmap_water.repeat_u / 1.14999997615814);
        float f3 = AppMain._dmap_water.ofst_u + pos_x * num4;
        float num5 = (double)f3 < 0.0 ? (float)(1.0 - (-(double)f3 - (double)AppMain.floorf(-f3))) : f3 - AppMain.floorf(f3);
        AppMain._dmap_water.draw_u = num5;
        AppMain._dmap_water.draw_v = AppMain._dmap_water.ofst_v;
        AppMain.ArrayPointer<AppMain.DMAP_WATER_OBJ> arrayPointer = new AppMain.ArrayPointer<AppMain.DMAP_WATER_OBJ>(AppMain._dmap_water._object);
        int num6 = 0;
        while (num6 < 2)
        {
            AppMain.AMS_MOTION motion = ((AppMain.DMAP_WATER_OBJ)~arrayPointer).motion;
            float num7 = ((AppMain.DMAP_WATER_OBJ)~arrayPointer).frame + speed;
            float startFrame = AppMain.amMotionMaterialGetStartFrame(motion, 0);
            float endFrame = AppMain.amMotionMaterialGetEndFrame(motion, 0);
            while ((double)num7 >= (double)endFrame)
                num7 = startFrame + (num7 - endFrame);
            ((AppMain.DMAP_WATER_OBJ)~arrayPointer).frame = num7;
            ++num6;
            ++arrayPointer;
        }
    }

    private static void dwaterGetParam(AppMain.DMAP_PARAM_WATER param)
    {
        param.frame[0] = AppMain._dmap_water._object[0].frame;
        param.frame[1] = AppMain._dmap_water._object[1].frame;
        param.draw_u = AppMain._dmap_water.draw_u;
        param.draw_v = AppMain._dmap_water.draw_v;
        param.scale = AppMain._dmap_water.scale;
        param.pos_x = AppMain._dmap_water.pos_x;
        param.pos_y = AppMain._dmap_water.pos_y;
        param.pos_dy = AppMain._dmap_water.pos_dy;
        param.repeat_u = AppMain._dmap_water.repeat_u;
        param.repeat_v = AppMain._dmap_water.repeat_v;
        param.rot_z = AppMain._dmap_water.rot_z;
        param.color = AppMain._dmap_water.color;
    }

    private static void dwaterSetParam()
    {
        AppMain.DMAP_PARAM_WATER dmapParamWater = AppMain.amDrawAlloc_DMAP_PARAM_WATER();
        AppMain.dwaterGetParam(dmapParamWater);
        AppMain.amDrawMakeTask(new AppMain.TaskProc(AppMain._dwaterSetParam), (ushort)0, (object)dmapParamWater);
    }

    private static void dwaterDrawReflection(uint state, uint drawflag)
    {
        AppMain.amMatrixPush();
        AppMain.NNS_MATRIX current = AppMain.amMatrixGetCurrent();
        AppMain.DMAP_WATER_OBJ dmapWaterObj = AppMain._dmap_water._object[1];
        AppMain.AMS_MOTION motion = dmapWaterObj.motion;
        AppMain.amMotionMaterialSetFrame(motion, AppMain._dmap_water._object[1].frame);
        AppMain.amMotionMaterialCalc(motion);
        float x = AppMain.floorf(AppMain._dmap_water.pos_x / (AppMain._dmap_water.repeat_pos_x * AppMain._dmap_water.scale) - 0.5f) * (AppMain._dmap_water.repeat_pos_x * AppMain._dmap_water.scale);
        float y = AppMain._dmap_water.pos_y + AppMain._dmap_water.pos_dy;
        for (int index = 0; index < 2; ++index)
        {
            AppMain.nnMakeTranslateMatrix(current, x, y, AppMain.FX_FX32_TO_F32(-1179648));
            AppMain.nnScaleMatrix(current, current, AppMain._dmap_water.scale, AppMain._dmap_water.scale, 1f);
            AppMain.ObjDraw3DNNMotionMaterialMotion(motion, dmapWaterObj.texlist, drawflag, 0U, (AppMain.MPP_VOID_OBJECT_DELEGATE)null, (object)null, state, (AppMain.MPP_VOID_ARRAYNNSMATRIX_NNSOBJECT_OBJECT)null, (object)null, (AppMain.MPP_BOOL_NNSDRAWCALLBACKVAL_OBJECT_DELEGATE)null, (object)null, (AppMain.AMS_DRAWSTATE)null, 1U);
            x += AppMain._dmap_water.repeat_pos_x * AppMain._dmap_water.scale;
        }
        AppMain.amMatrixPop();
    }

    private static void dwaterDrawSurface(uint state, uint drawflag)
    {
        AppMain.amMatrixPush();
        AppMain.NNS_MATRIX current = AppMain.amMatrixGetCurrent();
        AppMain.DMAP_WATER_OBJ dmapWaterObj = AppMain._dmap_water._object[0];
        AppMain.AMS_MOTION motion = dmapWaterObj.motion;
        AppMain.amMotionMaterialSetFrame(motion, AppMain._dmap_water._object[0].frame);
        AppMain.amMotionMaterialCalc(motion);
        float x = AppMain.floorf(AppMain._dmap_water.pos_x / (AppMain._dmap_water.repeat_pos_x * AppMain._dmap_water.scale) - 0.5f) * (AppMain._dmap_water.repeat_pos_x * AppMain._dmap_water.scale);
        float y = AppMain._dmap_water.pos_y + AppMain._dmap_water.pos_dy;
        for (int index = 0; index < 2; ++index)
        {
            AppMain.nnMakeTranslateMatrix(current, x, y, AppMain.FX_FX32_TO_F32(917504));
            AppMain.nnScaleMatrix(current, current, AppMain._dmap_water.scale, AppMain._dmap_water.scale * 2f, 1f);
            AppMain.ObjDraw3DNNMotionMaterialMotion(motion, dmapWaterObj.texlist, drawflag, 0U, (AppMain.MPP_VOID_OBJECT_DELEGATE)null, (object)null, state, (AppMain.MPP_VOID_ARRAYNNSMATRIX_NNSOBJECT_OBJECT)null, (object)null, (AppMain.MPP_BOOL_NNSDRAWCALLBACKVAL_OBJECT_DELEGATE)null, (object)null, (AppMain.AMS_DRAWSTATE)null, 1U);
            x += AppMain._dmap_water.repeat_pos_x * AppMain._dmap_water.scale;
        }
        AppMain.amMatrixPop();
    }

    private static void dwaterDrawWater(AppMain.AMS_RENDER_TARGET texture)
    {
        AppMain.DMAP_PARAM_WATER drawParam = AppMain._dmap_water.draw_param;
        AppMain.amDrawPushState();
        AppMain.NNS_VECTOR nnsVector = new AppMain.NNS_VECTOR(0.0f, drawParam.pos_dy, -0.5f);
        AppMain.nnTransformVector(nnsVector, AppMain.amDrawGetProjectionMatrix(), nnsVector);
        double drawU = (double)drawParam.draw_u;
        double drawV = (double)drawParam.draw_v;
        double repeatU = (double)drawParam.repeat_u;
        double scale1 = (double)drawParam.scale;
        double repeatV = (double)drawParam.repeat_v;
        double scale2 = (double)drawParam.scale;
        double y = (double)nnsVector.y;
        int rotZ = drawParam.rot_z;
        int color = (int)drawParam.color;
        AppMain.amDrawPopState();
    }

    private static void _dwaterSetParam(AppMain.AMS_TCB tcbp)
    {
        AppMain.DMAP_PARAM_WATER work = (AppMain.DMAP_PARAM_WATER)AppMain.amTaskGetWork(tcbp);
        AppMain._dmap_water.draw_param = work;
    }

}