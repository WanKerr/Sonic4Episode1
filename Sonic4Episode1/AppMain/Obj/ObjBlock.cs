using System;
using System.Collections.Generic;
using System.Text;

partial class AppMain
{
    private static void ObjSetBlockCollision(AppMain.OBS_BLOCK_COLLISION pCol)
    {
        AppMain._obj_bcol = pCol;
    }

    private static AppMain.OBS_BLOCK_COLLISION ObjGetBlockCollision()
    {
        return AppMain._obj_bcol;
    }

    private static int ObjBlockCollisionDet(
      int lPosX,
      int lPosY,
      ushort usFlag,
      ushort usVec,
      ushort[] pDir,
      uint[] pAttr)
    {
        AppMain.OBS_COL_CHK_DATA pData = AppMain.GlobalPool<AppMain.OBS_COL_CHK_DATA>.Alloc();
        pData.pos_x = lPosX;
        pData.pos_y = lPosY;
        pData.dir = pDir;
        pData.attr = pAttr;
        pData.flag = usFlag;
        pData.vec = usVec;
        int num = AppMain.ObjBlockCollision(pData);
        AppMain.GlobalPool<AppMain.OBS_COL_CHK_DATA>.Release(pData);
        return num;
    }

    private static int ObjBlockCollision(AppMain.OBS_COL_CHK_DATA pData)
    {
        int a = 0;
        short num1 = 0;
        short num2 = 0;
        if (AppMain._obj_bcol.pData[0] == null)
        {
            switch (pData.vec)
            {
                case 0:
                    a = AppMain._obj_bcol.right - pData.pos_x;
                    break;
                case 1:
                    a = pData.pos_x - AppMain._obj_bcol.left;
                    break;
                case 2:
                    a = AppMain._obj_bcol.bottom - pData.pos_y;
                    break;
                case 3:
                    a = pData.pos_y - AppMain._obj_bcol.top;
                    break;
            }
            return AppMain.MTM_MATH_CLIP(a, -31, 31);
        }
        if (pData.dir != null)
        {
            int num3 = (int)pData.dir[0];
        }
        if (pData.attr != null)
        {
            int num4 = (int)pData.attr[0];
        }
        int num5 = AppMain.objGetBlockColData(pData);
        if (((int)pData.vec & 2) != 0)
        {
            short num6 = (short)(pData.pos_y & 15);
            switch (((int)pData.vec & 1) == 0 ? (short)((int)num6 + (int)(short)num5) : (short)((int)num6 - (int)(short)num5))
            {
                case 0:
                    num2 = (short)-16;
                    break;
                case 15:
                    num2 = (short)16;
                    break;
            }
        }
        else
        {
            short num6 = (short)(pData.pos_x & 15);
            switch (((int)pData.vec & 1) == 0 ? (short)((int)num6 + (int)(short)num5) : (short)((int)num6 - (int)(short)num5))
            {
                case 0:
                    num1 = (short)-16;
                    break;
                case 15:
                    num1 = (short)16;
                    break;
            }
        }
        if (num1 != (short)0 || num2 != (short)0)
        {
            uint num6 = 0;
            ushort num7 = 0;
            if (pData.dir != null)
                num7 = pData.dir[0];
            if (pData.attr != null)
                num6 = pData.attr[0];
            pData.pos_x += (int)num1;
            pData.pos_y += (int)num2;
            int blockColData = AppMain.objGetBlockColData(pData);
            pData.pos_x -= (int)num1;
            pData.pos_y -= (int)num2;
            if (blockColData >= 0)
            {
                if (pData.dir != null)
                    pData.dir[0] = num7;
                if (pData.attr != null)
                    pData.attr[0] = num6;
            }
            if (num1 < (short)0)
                ++num1;
            if (num1 > (short)0)
                --num1;
            if (num2 < (short)0)
                ++num2;
            if (num2 > (short)0)
                --num2;
            num5 = ((int)pData.vec & 1) == 0 ? blockColData + ((int)num1 + (int)num2) : blockColData - ((int)num1 + (int)num2);
        }
        return num5;
    }

    private static int objGetBlockColData(AppMain.OBS_COL_CHK_DATA pData)
    {
        int num1 = 0;
        int num2 = 0;
        if (((int)pData.flag & 64) != 0)
        {
            int num3 = AppMain.objBlockColLimit(pData);
            if (num3 < 0)
                return num3;
        }
        else
        {
            num1 = AppMain.MTM_MATH_CLIP(pData.pos_x, AppMain._obj_bcol.left, AppMain._obj_bcol.right - 1);
            num2 = AppMain.MTM_MATH_CLIP(pData.pos_y, AppMain._obj_bcol.top, AppMain._obj_bcol.bottom - 1);
        }
        uint num4 = (uint)((ulong)(num2 >> 4) * (ulong)AppMain._obj_bcol.width + (ulong)(num1 >> 4));
        ushort num5 = (ushort)AppMain._obj_bcol.pData[(int)pData.flag & 1][(int)num4];
        return AppMain._obj_block_collision_func[(int)num5](pData);
    }

    private static int objBlockColLimit(AppMain.OBS_COL_CHK_DATA pData)
    {
        switch (pData.vec)
        {
            case 0:
                if (AppMain._obj_bcol.right - 1 < pData.pos_x)
                    return AppMain._obj_bcol.right - 1 - pData.pos_x;
                goto case 1;
            case 1:
                if (AppMain._obj_bcol.left > pData.pos_x)
                    return pData.pos_x - AppMain._obj_bcol.left;
                break;
            case 2:
                if (AppMain._obj_bcol.bottom - 1 < pData.pos_y)
                    return AppMain._obj_bcol.bottom - 1 - pData.pos_y;
                goto case 3;
            case 3:
                if (AppMain._obj_bcol.top > pData.pos_y)
                    return pData.pos_y - AppMain._obj_bcol.top;
                goto case 0;
        }
        return 1;
    }

    private static int objBlockCalcEmpty(AppMain.OBS_COL_CHK_DATA pData)
    {
        switch (pData.vec)
        {
            case 0:
                return 15 - (pData.pos_x & 15);
            case 1:
                return pData.pos_x & 15;
            case 2:
                return 15 - (pData.pos_y & 15);
            case 3:
                return pData.pos_y & 15;
            default:
                return 15;
        }
    }

    private static int objBlockCalcFill(AppMain.OBS_COL_CHK_DATA pData)
    {
        switch (pData.vec)
        {
            case 0:
                return -(pData.pos_x & 15);
            case 1:
                return -(15 - (pData.pos_x & 15));
            case 2:
                return -(pData.pos_y & 15);
            case 3:
                return -(15 - (pData.pos_y & 15));
            default:
                return 15;
        }
    }

    private static int objBlockColEmpty(AppMain.OBS_COL_CHK_DATA pData)
    {
        if (pData.dir != null)
            pData.dir[0] = (ushort)0;
        return AppMain.objBlockCalcEmpty(pData);
    }

    private static int objBlockColBlockFill(AppMain.OBS_COL_CHK_DATA pData)
    {
        if (pData.dir != null)
            pData.dir[0] = (ushort)0;
        return AppMain.objBlockCalcFill(pData);
    }

    private static int objBlockColBlockFillThrough(AppMain.OBS_COL_CHK_DATA pData)
    {
        if (pData.dir != null)
            pData.dir[0] = (ushort)0;
        if (pData.attr != null)
            pData.attr[0] |= 2U;
        return ((int)pData.flag & 128) != 0 ? AppMain.objBlockCalcEmpty(pData) : AppMain.objBlockCalcFill(pData);
    }

}
