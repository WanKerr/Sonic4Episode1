using System;

public partial class AppMain
{
    private static void objDiffAttrSet(OBS_OBJECT_WORK pWork, uint ulAttr)
    {
        if (((int)ulAttr & 2) != 0)
            pWork.col_flag |= 1U;
        if (((int)ulAttr & 4) == 0)
            return;
        pWork.col_flag |= 4U;
    }

    private static int objCollision(OBS_COL_CHK_DATA pData)
    {
        return ((int)g_obj.flag & 16) != 0 ? ObjBlockCollision(pData) : ObjDiffCollision(pData);
    }

    private static int objCollisionFast(OBS_COL_CHK_DATA pData)
    {
        return ((int)g_obj.flag & 16) != 0 ? ObjBlockCollision(pData) : ObjDiffCollisionFast(pData);
    }

    private static int ObjCollisionUnion(
      OBS_OBJECT_WORK pWork,
      OBS_COL_CHK_DATA pData)
    {
        int num1 = 32;
        if (((int)pWork.move_flag & 4096) == 0)
            num1 = objCollision(pData);
        if (((int)pWork.move_flag & 512) == 0)
        {
            int num2 = ObjCollisionObjectCheck(pWork, pData);
            if (num1 > num2)
                num1 = num2;
        }
        return num1;
    }

    private static int ObjCollisionFastUnion(OBS_COL_CHK_DATA pData)
    {
        int num1 = objCollisionFast(pData);
        int num2 = ObjCollisionObjectFastCheck(pData);
        if (num1 > num2)
            num1 = num2;
        return num1;
    }

    private static void ObjDiffCollisionEarthCheck(OBS_OBJECT_WORK pWork)
    {
        objDiffCollisionDirCheck(pWork);
    }

    private static void objDiffCollisionDirCheck(OBS_OBJECT_WORK pWork)
    {
        pWork.col_flag = 0U;
        int sSpd = (pWork.dir_fall + 8192 & 16384) == 0 ? pWork.move.x : pWork.move.y;
        if (((int)pWork.move_flag & 1024) == 0)
            objDiffCollisionDirWidthCheck(pWork, 0, sSpd);
        if (((int)pWork.move_flag & 2048) == 0)
            objDiffCollisionDirHeightCheck(pWork);
        if (((int)pWork.move_flag & 1) != 0 && (pWork.dir.z == 0 || pWork.dir.z == 32768))
        {
            if ((pWork.dir_fall + 8192 & 16384) != 0)
                pWork.pos.x &= -4096;
            else
                pWork.pos.y &= -4096;
        }
        if (((int)pWork.move_flag & 1024) != 0)
            return;
        objDiffCollisionDirWidthCheck(pWork, 1, sSpd);
    }

    private static ushort objDiffSufSet(OBS_OBJECT_WORK pWork)
    {
        ushort num = 0;
        if (((int)pWork.move_flag & 32) != 0)
            num |= 128;
        if (((int)pWork.move_flag & 1048576) == 0)
            num |= 128;
        if (((int)pWork.flag & 1) != 0)
            num |= 1;
        if (((int)pWork.move_flag & 524288) != 0)
            num |= 64;
        return num;
    }

    private static void objDiffCollisionDirWidthCheck(
      OBS_OBJECT_WORK pWork,
      byte ucWall,
      int sSpd)
    {
        OBS_COL_CHK_DATA pData = GlobalPool<OBS_COL_CHK_DATA>.Alloc();
        short num1 = 0;
        short num2 = 0;
        short num3 = 0;
        short num4 = 0;
        byte num5 = 0;
        byte num6 = 0;
        pData.flag = objDiffSufSet(pWork);
        pData.flag |= 128;
        int lPosX = pWork.pos.x >> 12;
        int lPosY = pWork.pos.y >> 12;
        ushort num7 = 16384;
        if ((pWork.dir.z + 8192 & 49152) >> 14 == 2)
            num7 += 32768;
        int num8 = sSpd;
        if (((int)pWork.disp_flag & 1) == 0)
        {
            num7 = (ushort)-num7;
            num5 = 1;
        }
        ushort num9 = (ushort)(num7 + (uint)pWork.dir_fall);
        switch (pWork.dir_fall + 8192 >> 14 & 3)
        {
            case 2:
                num5 ^= 1;
                num9 -= 32768;
                break;
        }
        if ((pWork.dir.z + 8192 & 49152) >> 14 == 2)
            num5 ^= 1;
        if (((int)pWork.move_flag & 16) == 0)
            num9 += pWork.dir.z;
        sbyte num10 = objDiffCollisionSimpleOverCheck(pWork);
        sbyte num11 = num10 <= -4 ? (sbyte)(-num10 + 1) : (sbyte)4;
        if (num11 >= (sbyte)(pWork.field_rect[3] - pWork.field_rect[1]))
            num11 = (sbyte)(pWork.field_rect[3] - pWork.field_rect[1] - 1);
        switch ((num9 + 8192 & 49152) >> 14)
        {
            case 0:
                num2 = (short)(pWork.field_rect[2] + pWork.field_ajst_w_db_f);
                num4 = num2;
                num1 = (short)(pWork.field_rect[3] - pWork.field_ajst_w_db_b);
                num3 = (short)(pWork.field_rect[1] + num11);
                if (num5 != 0)
                {
                    num1 = (short)-num1;
                    num3 = (short)-num3;
                }
                pData.vec = 2;
                break;
            case 1:
                num1 = (short)(pWork.field_rect[0] - pWork.field_ajst_w_dl_f);
                num3 = num1;
                num2 = (short)(pWork.field_rect[3] - pWork.field_ajst_w_dl_b);
                num4 = (short)(pWork.field_rect[1] + num11);
                if (num5 != 0)
                {
                    num2 = (short)-num2;
                    num4 = (short)-num4;
                }
                pData.vec = 1;
                break;
            case 2:
                num2 = (short)(pWork.field_rect[0] - pWork.field_ajst_w_dt_f);
                num4 = num2;
                num1 = (short)-(pWork.field_rect[3] - pWork.field_ajst_w_dt_b);
                num3 = (short)-(pWork.field_rect[1] + num11);
                pData.vec = 3;
                if (num5 != 0)
                {
                    num1 = (short)-num1;
                    num3 = (short)-num3;
                    break;
                }
                break;
            case 3:
                num1 = (short)(pWork.field_rect[2] + pWork.field_ajst_w_dr_f);
                num3 = num1;
                num2 = (short)-(pWork.field_rect[3] - pWork.field_ajst_w_dr_b);
                num4 = (short)-(pWork.field_rect[1] + num11);
                pData.vec = 0;
                if (num5 != 0)
                {
                    num2 = (short)-num2;
                    num4 = (short)-num4;
                    break;
                }
                break;
        }
        pData.pos_x = lPosX + num1;
        pData.pos_y = lPosY + num2;
        sbyte cDelta1 = (sbyte)ObjCollisionUnion(pWork, pData);
        pData.pos_x = lPosX + num3;
        pData.pos_y = lPosY + num4;
        sbyte num12 = (sbyte)ObjCollisionUnion(pWork, pData);
        if (cDelta1 >= num12)
            cDelta1 = num12;
        sbyte num13 = cDelta1;
        if (cDelta1 <= 0)
        {
            num6 |= 1;
            if (ucWall == 0)
            {
                objDiffColDirMove(ref lPosX, ref lPosY, cDelta1, pData.vec);
            }
            else
            {
                pWork.move_flag |= 4U;
                if (((int)pWork.move_flag & 16384) == 0)
                {
                    if ((pWork.dir_fall + 8192 & 16384) != 0)
                    {
                        if (pData.vec == 3 && num8 < 0)
                        {
                            pWork.spd_m = 0;
                            if (((int)pWork.move_flag & 536870912) == 0)
                                pWork.spd.x = 0;
                        }
                        if (pData.vec == 2 && num8 > 0)
                        {
                            pWork.spd_m = 0;
                            if (((int)pWork.move_flag & 536870912) == 0)
                                pWork.spd.x = 0;
                        }
                    }
                    else
                    {
                        if (pData.vec == 1 && num8 < 0)
                        {
                            pWork.spd_m = 0;
                            if (((int)pWork.move_flag & 536870912) == 0)
                                pWork.spd.x = 0;
                        }
                        if (pData.vec == 0 && num8 > 0)
                        {
                            pWork.spd_m = 0;
                            if (((int)pWork.move_flag & 536870912) == 0)
                                pWork.spd.x = 0;
                        }
                    }
                }
                if ((pData.vec & 2) != 0 && ((int)pWork.move_flag & 16384) == 0 && !GSM_MAIN_STAGE_IS_SPSTAGE_NOT_RETRY())
                    pWork.spd_m = 0;
            }
        }
        switch (((ushort)(num9 + 32768U) + 8192 & 49152) >> 14)
        {
            case 0:
                num2 = (short)(pWork.field_rect[2] + pWork.field_ajst_w_db_f);
                num4 = num2;
                num1 = (short)-(pWork.field_rect[3] - pWork.field_ajst_w_db_b);
                num3 = (short)-(pWork.field_rect[1] + num11);
                pData.vec = 2;
                if (num5 != 0)
                {
                    num1 = (short)-num1;
                    num3 = (short)-num3;
                    break;
                }
                break;
            case 1:
                num1 = (short)(pWork.field_rect[0] - pWork.field_ajst_w_dl_f);
                num3 = num1;
                num2 = (short)-(pWork.field_rect[3] - pWork.field_ajst_w_dl_b);
                num4 = (short)-(pWork.field_rect[1] + num11);
                pData.vec = 1;
                if (num5 != 0)
                {
                    num2 = (short)-num2;
                    num4 = (short)-num4;
                    break;
                }
                break;
            case 2:
                num2 = (short)(pWork.field_rect[0] - pWork.field_ajst_w_dt_f);
                num4 = num2;
                num1 = (short)(pWork.field_rect[3] - pWork.field_ajst_w_dt_b);
                num3 = (short)(pWork.field_rect[1] + num11);
                pData.vec = 3;
                if (num5 != 0)
                {
                    num1 = (short)-num1;
                    num3 = (short)-num3;
                    break;
                }
                break;
            case 3:
                num1 = (short)(pWork.field_rect[2] + pWork.field_ajst_w_dr_f);
                num3 = num1;
                num2 = (short)(pWork.field_rect[3] - pWork.field_ajst_w_dr_b);
                num4 = (short)(pWork.field_rect[1] + num11);
                pData.vec = 0;
                if (num5 != 0)
                {
                    num2 = (short)-num2;
                    num4 = (short)-num4;
                    break;
                }
                break;
        }
        pData.pos_x = lPosX + num1;
        pData.pos_y = lPosY + num2;
        sbyte cDelta2 = (sbyte)ObjCollisionUnion(pWork, pData);
        pData.pos_x = lPosX + num3;
        pData.pos_y = lPosY + num4;
        sbyte num14 = (sbyte)ObjCollisionUnion(pWork, pData);
        if (cDelta2 >= num14)
            cDelta2 = num14;
        if (cDelta2 <= 0)
        {
            num6 |= 2;
            if (ucWall == 0)
            {
                objDiffColDirMove(ref lPosX, ref lPosY, cDelta2, pData.vec);
            }
            else
            {
                if (((int)pWork.move_flag & 4) == 0 || num13 < 0 || cDelta2 < 0)
                    pWork.move_flag |= 8U;
                if (((int)pWork.move_flag & 16384) == 0)
                {
                    if ((pWork.dir_fall + 8192 & 16384) != 0)
                    {
                        if (pData.vec == 3 && sSpd < 0)
                        {
                            pWork.spd_m = 0;
                            if (((int)pWork.move_flag & 536870912) == 0)
                                pWork.spd.x = 0;
                        }
                        if (pData.vec == 2 && sSpd > 0)
                        {
                            pWork.spd_m = 0;
                            if (((int)pWork.move_flag & 536870912) == 0)
                                pWork.spd.x = 0;
                        }
                    }
                    else
                    {
                        if (pData.vec == 1 && sSpd < 0)
                        {
                            pWork.spd_m = 0;
                            if (((int)pWork.move_flag & 536870912) == 0)
                                pWork.spd.x = 0;
                        }
                        if (pData.vec == 0 && sSpd > 0)
                        {
                            pWork.spd_m = 0;
                            if (((int)pWork.move_flag & 536870912) == 0)
                                pWork.spd.x = 0;
                        }
                    }
                }
            }
        }
        if (ucWall == 0)
        {
            pWork.pos.x -= (pWork.pos.x >> 12) - lPosX << 12;
            pWork.pos.y -= (pWork.pos.y >> 12) - lPosY << 12;
            if ((num6 & 3) != 0 && sSpd != 0)
            {
                bool flag1 = (num6 & 2) == 2;
                bool flag2 = ((int)pWork.disp_flag & 1) == 1;
                bool flag3 = sSpd > 0;
                if (pWork.dir_fall == 49152)
                    flag3 = sSpd < 0;
                if (flag1 ^ flag2 ^ flag3)
                {
                    int num15 = (pData.vec & 2) == 0 ? pWork.pos.x : pWork.pos.y;
                    if (sSpd > 0)
                    {
                        if ((num15 & 4095L) > 2048L)
                            num15 = num15 & -4096 | 2048;
                    }
                    else if ((num15 & 4095L) < 2048L)
                        num15 = num15 & -4096 | 2048;
                    if ((pData.vec & 2) != 0)
                        pWork.pos.y = num15;
                    else
                        pWork.pos.x = num15;
                }
            }
        }
        GlobalPool<OBS_COL_CHK_DATA>.Release(pData);
    }

    private static sbyte objDiffCollisionSimpleOverCheck(OBS_OBJECT_WORK pWork)
    {
        OBS_COL_CHK_DATA pData = GlobalPool<OBS_COL_CHK_DATA>.Alloc();
        pData.flag = objDiffSufSet(pWork);
        ushort num1 = ((int)pWork.move_flag & 16) == 0 ? (ushort)((pWork.dir.z + 8192 & 49152) >> 14) : (ushort)0;
        if (pWork.dir_fall != 0)
            num1 = (ushort)((ushort)(num1 + (uint)(ushort)((pWork.dir_fall + 8192 & 49152) >> 14)) & 3U);
        short num2;
        short num3;
        short num4;
        short num5;
        switch (num1)
        {
            case 1:
                num4 = (short)(pWork.field_rect[0] + 2);
                num5 = (short)(pWork.field_rect[2] - 2);
                num2 = (short)-pWork.field_rect[1];
                num3 = (short)-pWork.field_rect[1];
                pData.vec = 0;
                break;
            case 2:
                num2 = (short)(pWork.field_rect[2] - 2);
                num3 = (short)(pWork.field_rect[0] + 2);
                num4 = (short)-pWork.field_rect[1];
                num5 = (short)-pWork.field_rect[1];
                pData.vec = 2;
                break;
            case 3:
                num4 = (short)(pWork.field_rect[0] + 2);
                num5 = (short)(pWork.field_rect[2] - 2);
                num2 = pWork.field_rect[1];
                num3 = pWork.field_rect[1];
                pData.vec = 1;
                break;
            default:
                num2 = (short)(pWork.field_rect[2] - 2);
                num3 = (short)(pWork.field_rect[0] + 2);
                num4 = pWork.field_rect[1];
                num5 = pWork.field_rect[1];
                pData.vec = 3;
                break;
        }
        pData.pos_x = (pWork.pos.x >> 12) + num2;
        pData.pos_y = (pWork.pos.y >> 12) + num4;
        sbyte num6 = (sbyte)ObjCollisionUnion(pWork, pData);
        pData.pos_x = (pWork.pos.x >> 12) + num3;
        pData.pos_y = (pWork.pos.y >> 12) + num5;
        sbyte num7 = (sbyte)ObjCollisionUnion(pWork, pData);
        GlobalPool<OBS_COL_CHK_DATA>.Release(pData);
        sbyte num8 = num6 >= num7 ? num7 : num6;
        return num8 <= 0 ? num8 : (sbyte)0;
    }

    private static void objDiffCollisionDirHeightCheck(OBS_OBJECT_WORK pWork)
    {
        OBS_COL_CHK_DATA pData = GlobalPool<OBS_COL_CHK_DATA>.Alloc();
        OBS_OBJECT_WORK rideObj = pWork.ride_obj;
        uint[] heightCheckUlAttr = objDiffCollisionDirHeightCheck_ulAttr;
        heightCheckUlAttr[0] = 0U;
        ushort[] numArray = objDiffCollisionDirHeightCheck_usDir1;
        numArray[0] = 0;
        ushort[] heightCheckUsDir2 = objDiffCollisionDirHeightCheck_usDir2;
        heightCheckUsDir2[0] = 0;
        ushort[] heightCheckUsDir3 = objDiffCollisionDirHeightCheck_usDir3;
        heightCheckUsDir3[0] = 0;
        bool flag1 = false;
        pData.flag = objDiffSufSet(pWork);
        int lPosX = pWork.pos.x >> 12;
        int lPosY = pWork.pos.y >> 12;
        numArray[0] = heightCheckUsDir2[0] = pWork.dir.z;
        ushort num1 = ((int)pWork.move_flag & 16) == 0 ? (ushort)((pWork.dir.z + 8192 & 49152) >> 14) : (ushort)0;
        if (pWork.dir_fall != 0)
            num1 = (ushort)((ushort)(num1 + (uint)(ushort)((pWork.dir_fall + 8192 & 49152) >> 14)) & 3U);
        short num2;
        short num3;
        short num4;
        short num5;
        short num6;
        short num7;
        int num8;
        int num9;
        switch (num1)
        {
            case 1:
                num2 = (short)-pWork.field_rect[3];
                num3 = (short)(pWork.field_rect[0] - pWork.field_ajst_h_dl_l);
                num4 = (short)-pWork.field_rect[3];
                num5 = (short)(pWork.field_rect[2] + pWork.field_ajst_h_dl_r);
                num6 = (short)(pWork.field_rect[3] - Math.Abs(pWork.field_rect[2]));
                num7 = 0;
                pData.vec = 1;
                num9 = -pWork.move.x;
                num8 = -pWork.move.y;
                break;
            case 2:
                num2 = (short)(pWork.field_rect[2] + pWork.field_ajst_h_dt_r);
                num3 = (short)-pWork.field_rect[3];
                num4 = (short)(pWork.field_rect[0] - pWork.field_ajst_h_dt_l);
                num5 = (short)-pWork.field_rect[3];
                num6 = 0;
                num7 = g_obj.col_through_dot;
                pData.vec = 3;
                num8 = -pWork.move.x;
                num9 = -pWork.move.y;
                break;
            case 3:
                num2 = pWork.field_rect[3];
                num3 = (short)(pWork.field_rect[0] - pWork.field_ajst_h_dr_l);
                num4 = pWork.field_rect[3];
                num5 = (short)(pWork.field_rect[2] + pWork.field_ajst_h_dr_r);
                num6 = (short)-(pWork.field_rect[3] - Math.Abs(pWork.field_rect[0]));
                num7 = 0;
                pData.vec = 0;
                num9 = pWork.move.x;
                num8 = pWork.move.y;
                break;
            default:
                num2 = (short)(pWork.field_rect[2] + pWork.field_ajst_h_db_r);
                num3 = pWork.field_rect[3];
                num4 = (short)(pWork.field_rect[0] - pWork.field_ajst_h_db_l);
                num5 = pWork.field_rect[3];
                num6 = 0;
                num7 = (short)-g_obj.col_through_dot;
                pData.vec = 2;
                num8 = pWork.move.x;
                num9 = pWork.move.y;
                break;
        }
        if (((int)pWork.move_flag & 32) == 0)
        {
            if (((int)pWork.move_flag & 4194304) == 0)
            {
                ushort flag2 = pData.flag;
                pData.flag &= 65407;
                pData.pos_x = lPosX + (num2 + num6);
                pData.pos_y = lPosY + (num3 + num7);
                sbyte num10 = (sbyte)ObjCollisionUnion(pWork, pData);
                pData.pos_x = lPosX + (num4 + num6);
                pData.pos_y = lPosY + (num5 + num7);
                sbyte num11 = (sbyte)ObjCollisionUnion(pWork, pData);
                pData.flag = flag2;
                pWork.ride_obj = rideObj;
                if ((num10 >= num11 ? num11 : num10) >= 0)
                    pWork.move_flag |= 1048576U;
                else
                    pWork.move_flag &= 4293918719U;
            }
            else if ((pData.vec & 2) != 0 || ((int)pWork.col_flag_prev & 4) != 0)
                pWork.move_flag |= 1048576U;
            else
                pWork.move_flag &= 4293918719U;
        }
        if (((int)pWork.move_flag & 1048576) == 0)
            pData.flag |= 128;
        pData.attr = heightCheckUlAttr;
        pData.dir = numArray;
        pData.pos_x = lPosX + num2;
        pData.pos_y = lPosY + num3;
        sbyte num12 = (sbyte)ObjCollisionUnion(pWork, pData);
        objDiffAttrSet(pWork, heightCheckUlAttr[0]);
        pData.dir = heightCheckUsDir2;
        pData.pos_x = lPosX + num4;
        pData.pos_y = lPosY + num5;
        sbyte num13 = (sbyte)ObjCollisionUnion(pWork, pData);
        objDiffAttrSet(pWork, heightCheckUlAttr[0]);
        pData.dir = heightCheckUsDir3;
        pData.attr = null;
        pData.pos_y = lPosY + num3;
        if (((int)pWork.move_flag & 2097152) != 0 && ((int)pWork.move_flag & 512) == 0)
        {
            short num10;
            int num11;
            if (num2 > num4)
            {
                num10 = (short)(num2 - num4 - 1);
                num11 = lPosX + num4;
            }
            else
            {
                num10 = (short)(num4 - num2 - 1);
                num11 = lPosX + num2;
            }
            for (byte index = 1; index < num10; ++index)
            {
                pData.pos_x = num11 + index;
                sbyte num14 = (sbyte)ObjCollisionObjectCheck(pWork, pData);
                if (num14 < num12)
                {
                    num12 = num14;
                    numArray = heightCheckUsDir3;
                }
            }
        }
        pData.dir = null;
        pData.attr = heightCheckUlAttr;
        if ((num1 & 1) != 0)
        {
            pData.pos_x = lPosX + num2;
            pData.pos_y = num3 >= num5 ? lPosY + num5 + (Math.Abs(num3) + Math.Abs(num5) >> 1) : lPosY + num3 + (Math.Abs(num3) + Math.Abs(num5) >> 1);
        }
        else
        {
            pData.pos_y = lPosY + num5;
            pData.pos_x = num2 >= num4 ? lPosX + num4 + (Math.Abs(num2) + Math.Abs(num4) >> 1) : lPosX + num2 + (Math.Abs(num2) + Math.Abs(num4) >> 1);
        }
        ObjCollisionUnion(pWork, pData);
        objDiffAttrSet(pWork, heightCheckUlAttr[0]);
        if (((int)pWork.col_flag & 4) != 0 && ((int)pWork.move_flag & 16) != 0 && (num9 > 0 && ((int)pWork.col_flag & 1) == 0) && ((ushort)(numArray[0] + (uint)pWork.dir_fall) >= 16384 && (ushort)(numArray[0] + (uint)pWork.dir_fall) <= 49152 || (ushort)(heightCheckUsDir2[0] + (uint)pWork.dir_fall) >= 16384 && (ushort)(heightCheckUsDir2[0] + (uint)pWork.dir_fall) <= 49152))
        {
            num12 = 24;
            num13 = 24;
        }
        sbyte cDelta1 = num12 >= num13 ? num13 : num12;
        if (cDelta1 != 0)
        {
            if (cDelta1 < 0)
            {
                if ((((int)pWork.move_flag & 16) == 0 || num9 >= 0) && ((int)pWork.sys_flag & 17 << num1) == 0)
                    pWork.move_flag |= 1U;
                if (((int)pWork.move_flag & 1073741824) == 0 && cDelta1 >= -14 && ((int)pWork.move_flag & 1) != 0 || ((int)pWork.move_flag & 1073741824) != 0 && cDelta1 >= -28)
                {
                    if (cDelta1 < -16 && ((int)pWork.move_flag & 1073741824) != 0)
                        flag1 = true;
                    objDiffColDirMove(ref lPosX, ref lPosY, cDelta1, pData.vec);
                    pWork.move_flag &= 4294901759U;
                }
            }
            else
            {
                if (cDelta1 == 1)
                    pWork.move_flag |= 65536U;
                if (((int)pWork.move_flag & 16) == 0)
                {
                    sbyte num10 = (num1 & 1) == 0 ? (sbyte)((Math.Abs(num8) >> 12) + 3) : (sbyte)((Math.Abs(num9) >> 12) + 3);
                    if (num10 > 11)
                        num10 = 11;
                    if (cDelta1 <= num10 && ((int)pWork.sys_flag & 17 << num1) == 0)
                    {
                        pWork.move_flag |= 1U;
                        objDiffColDirMove(ref lPosX, ref lPosY, cDelta1, pData.vec);
                        if (((int)pWork.move_flag & 512) == 0 && ((int)pWork.move_flag & 64) == 0 && pWork.touch_obj == null)
                        {
                            pData.attr = null;
                            pData.dir = null;
                            pData.pos_x = lPosX + num2;
                            pData.pos_y = lPosY + num3;
                            ObjCollisionObjectCheck(pWork, pData);
                            pData.pos_x = lPosX + num4;
                            pData.pos_y = lPosY + num5;
                            ObjCollisionObjectCheck(pWork, pData);
                        }
                    }
                    else
                        pWork.move_flag &= 4294967294U;
                }
            }
        }
        else if ((((int)pWork.move_flag & 16) == 0 || pWork.spd.y >= 0) && ((int)pWork.sys_flag & 17 << num1) == 0)
            pWork.move_flag |= 1U;
        if (((int)pWork.move_flag & 1) != 0)
        {
            if (((int)pWork.move_flag & 268435456) == 0)
            {
                if (((int)pWork.col_flag & 1) == 0 && ((int)pWork.move_flag & 64) != 0)
                {
                    if (num12 >= num13)
                    {
                        if (num12 > num13)
                            numArray = heightCheckUsDir2;
                        else if (Math.Abs((ushort)(pWork.dir.z + (uint)pWork.dir_fall) - numArray[0]) > Math.Abs((ushort)(pWork.dir.z + (uint)pWork.dir_fall) - heightCheckUsDir2[0]))
                            numArray = heightCheckUsDir2;
                    }
                    pWork.dir.z = ((int)pWork.move_flag & 8388608) == 0 ? (ushort)(numArray[0] - (uint)pWork.dir_fall) : ObjRoopMove16(pWork.dir.z, (ushort)(numArray[0] - (uint)pWork.dir_fall), 256);
                }
                else if (GSM_MAIN_STAGE_IS_SPSTAGE_NOT_RETRY() && ((int)pWork.move_flag & 4194304) == 0)
                    pWork.dir.z = (ushort)-((g_gm_main_system.pseudofall_dir + 8192 & 16383) - 8192);
            }
            if (((int)pWork.move_flag & 16384) == 0 && ((int)pWork.move_flag & 536870912) == 0)
            {
                switch (((int)pWork.move_flag & 16) == 0 ? (pWork.dir.z + 8192 & 49152) >> 14 : 0)
                {
                    case 1:
                        if (pWork.spd.x < 0)
                        {
                            pWork.spd.x = 0;
                            break;
                        }
                        break;
                    case 2:
                        if (pWork.spd.y < 0)
                        {
                            pWork.spd.y = 0;
                            break;
                        }
                        break;
                    case 3:
                        if (pWork.spd.x > 0)
                        {
                            pWork.spd.x = 0;
                            break;
                        }
                        break;
                    default:
                        if (pWork.spd.y > 0)
                        {
                            if (((int)pWork.move_flag & 131072) != 0)
                                pWork.spd.x += pWork.spd.y * mtMathSin((ushort)(pWork.dir.z - (uint)g_gm_main_system.pseudofall_dir + pWork.dir_fall)) >> 12;
                            pWork.spd.y = 0;
                            break;
                        }
                        break;
                }
            }
        }
        else
            pWork.ride_obj = rideObj;
        if (((int)pWork.move_flag & 1073741824) != 0 || num9 < 256)
        {
            pData.flag |= 128;
            switch (pData.vec)
            {
                case 0:
                    num2 = (short)(pWork.field_rect[0] + 2);
                    num4 = (short)(pWork.field_rect[0] + 2);
                    pData.vec = 1;
                    break;
                case 1:
                    num2 = (short)(pWork.field_rect[2] - 2);
                    num4 = (short)(pWork.field_rect[2] - 2);
                    pData.vec = 0;
                    break;
                case 3:
                    num3 = (short)(pWork.field_rect[3] - 2);
                    num5 = (short)(pWork.field_rect[3] - 2);
                    pData.vec = 2;
                    break;
                default:
                    num3 = (short)(pWork.field_rect[1] + 2);
                    num5 = (short)(pWork.field_rect[1] + 2);
                    pData.vec = 3;
                    break;
            }
            pData.attr = null;
            pData.dir = null;
            pData.pos_x = lPosX + num2;
            pData.pos_y = lPosY + num3;
            sbyte num10 = (sbyte)ObjCollisionUnion(pWork, pData);
            pData.pos_x = lPosX + num4;
            pData.pos_y = lPosY + num5;
            sbyte num11 = (sbyte)ObjCollisionUnion(pWork, pData);
            sbyte cDelta2 = num10 >= num11 ? num11 : num10;
            if (cDelta2 <= 0)
            {
                pWork.move_flag |= 2U;
                if (((int)pWork.move_flag & 1073741824) == 0 && cDelta2 >= -14 || ((int)pWork.move_flag & 1073741824) != 0 && cDelta2 >= -28)
                {
                    if (flag1 && ((int)pWork.move_flag & 1) != 0)
                    {
                        if (cDelta1 > cDelta2)
                            objDiffColDirMove(ref lPosX, ref lPosY, (sbyte)(cDelta2 - cDelta1), pData.vec);
                    }
                    else
                        objDiffColDirMove(ref lPosX, ref lPosY, cDelta2, pData.vec);
                    if (g_gm_main_system.pseudofall_dir == 0)
                    {
                        if (((int)pWork.move_flag & 16384) == 0 && ((int)pWork.move_flag & 536870912) == 0 && num9 < 0)
                        {
                            if ((pData.vec & 2) != 0)
                                pWork.spd.y = 0;
                            else
                                pWork.spd.x = 0;
                        }
                    }
                    else if (((int)pWork.move_flag & 16384) == 0)
                    {
                        pWork.spd_m = 0;
                        if (((int)pWork.move_flag & 536870912) == 0)
                        {
                            pWork.spd.x = 0;
                            pWork.spd.y = 0;
                        }
                    }
                }
            }
        }
        pWork.pos.x -= (pWork.pos.x >> 12) - lPosX << 12;
        pWork.pos.y -= (pWork.pos.y >> 12) - lPosY << 12;
        if (((int)pWork.move_flag & 1) != 0)
        {
            int num10 = (pData.vec & 2) == 0 ? pWork.pos.x : pWork.pos.y;
            if (num9 > 0)
            {
                if ((num10 & 4095L) > 2048L)
                    num10 = num10 & -4096 | 2048;
            }
            else if ((num10 & 4095L) < 2048L)
                num10 = num10 & -4096 | 2048;
            if ((pData.vec & 2) != 0)
                pWork.pos.y = num10;
            else
                pWork.pos.x = num10;
        }
        GlobalPool<OBS_COL_CHK_DATA>.Release(pData);
    }

    private static void objDiffColDirMove(
      ref int lPosX,
      ref int lPosY,
      sbyte cDelta,
      ushort usColFlag)
    {
        switch (usColFlag)
        {
            case 0:
                lPosX += cDelta;
                break;
            case 1:
                lPosX -= cDelta;
                break;
            case 3:
                lPosY -= cDelta;
                break;
            default:
                lPosY += cDelta;
                break;
        }
    }

    private static void ObjSetDiffCollision(OBS_DIFF_COLLISION pFat)
    {
        _obj_fcol = pFat;
    }

    private static OBS_DIFF_COLLISION ObjGetDiffCollision()
    {
        return _obj_fcol;
    }

    private static int ObjDiffCollisionDetFast(
      int lPosX,
      int lPosY,
      ushort usFlag,
      ushort usVec,
      ushort[] pDir,
      uint[] pAttr)
    {
        OBS_COL_CHK_DATA pData = GlobalPool<OBS_COL_CHK_DATA>.Alloc();
        pData.pos_x = lPosX;
        pData.pos_y = lPosY;
        pData.dir = pDir;
        pData.attr = pAttr;
        pData.flag = usFlag;
        pData.vec = usVec;
        int num = ObjDiffCollisionFast(pData);
        GlobalPool<OBS_COL_CHK_DATA>.Release(pData);
        return num;
    }

    private static int ObjDiffCollisionFast(OBS_COL_CHK_DATA pData)
    {
        int a = 0;
        ushort num1 = 0;
        uint num2 = 0;
        sbyte sDelta = 8;
        if (_obj_fcol.cl_diff_datap == null)
        {
            switch (pData.vec)
            {
                case 0:
                    a = _obj_fcol.right - pData.pos_x;
                    break;
                case 1:
                    a = pData.pos_x - _obj_fcol.left;
                    break;
                case 2:
                    a = _obj_fcol.bottom - pData.pos_y;
                    break;
                case 3:
                    a = pData.pos_y - _obj_fcol.top;
                    break;
            }
            return MTM_MATH_CLIP(a, -31, 31);
        }
        if (pData.dir != null)
            num1 = pData.dir[0];
        if (pData.attr != null)
            num2 = pData.attr[0];
        if ((pData.vec & 1) != 0)
            sDelta = -8;
        int lCol;
        sbyte sPix;
        if ((pData.vec & 2) != 0)
        {
            lCol = objGetColDataY(pData.pos_x, pData.pos_y, pData.flag, pData.dir, pData.attr);
            sPix = (sbyte)(pData.pos_y & 7);
        }
        else
        {
            lCol = objGetColDataX(pData.pos_x, pData.pos_y, pData.flag, pData.dir, pData.attr);
            sPix = (sbyte)(pData.pos_x & 7);
        }
        switch (lCol)
        {
            case 0:
                if (pData.dir != null)
                    pData.dir[0] = num1;
                if (pData.attr != null)
                    pData.attr[0] = num2;
                return field_objMapGetForward(sPix, sDelta);
            case 8:
                return field_objMapGetBack(sPix, sDelta);
            default:
                return field_objMapGetDiff(lCol, sPix, sDelta);
        }
    }

    private static int ObjDiffCollisionDet(
      int lPosX,
      int lPosY,
      ushort usFlag,
      ushort usVec,
      ushort[] pDir,
      uint[] pAttr)
    {
        OBS_COL_CHK_DATA pData = GlobalPool<OBS_COL_CHK_DATA>.Alloc();
        pData.pos_x = lPosX;
        pData.pos_y = lPosY;
        pData.dir = pDir;
        pData.attr = pAttr;
        pData.flag = usFlag;
        pData.vec = usVec;
        int num = ObjDiffCollision(pData);
        GlobalPool<OBS_COL_CHK_DATA>.Release(pData);
        return num;
    }

    private static int ObjDiffCollision(OBS_COL_CHK_DATA pData)
    {
        int a = 0;
        int num1 = 0;
        int num2 = 0;
        ushort num3 = 0;
        uint num4 = 0;
        sbyte num5 = 0;
        sbyte num6 = 0;
        if (_obj_fcol.cl_diff_datap == null)
        {
            switch (pData.vec)
            {
                case 0:
                    a = _obj_fcol.right - pData.pos_x;
                    break;
                case 1:
                    a = pData.pos_x - _obj_fcol.left;
                    break;
                case 2:
                    a = _obj_fcol.bottom - pData.pos_y;
                    break;
                case 3:
                    a = pData.pos_y - _obj_fcol.top;
                    break;
            }
            return MTM_MATH_CLIP(a, -31, 31);
        }
        if (pData.dir != null)
            num3 = pData.dir[0];
        if (pData.attr != null)
            num4 = pData.attr[0];
        if ((pData.vec & 2) != 0)
        {
            num6 = 8;
            if ((pData.vec & 1) != 0)
                num6 = -8;
        }
        else
        {
            num5 = 8;
            if ((pData.vec & 1) != 0)
                num5 = -8;
        }
        sbyte sPix;
        bool flag;
        int lCol1;
        if ((pData.vec & 2) != 0)
        {
            sPix = (sbyte)(pData.pos_y & 7);
            flag = true;
            lCol1 = objGetColDataY(pData.pos_x, pData.pos_y, pData.flag, pData.dir, pData.attr);
        }
        else
        {
            sPix = (sbyte)(pData.pos_x & 7);
            flag = false;
            lCol1 = objGetColDataX(pData.pos_x, pData.pos_y, pData.flag, pData.dir, pData.attr);
        }
        switch (lCol1)
        {
            case 0:
                int num7 = num1 + num5;
                int num8 = num2 + num6;
                int lCol2 = !flag ? objGetColDataX(pData.pos_x + num7, pData.pos_y + num8, pData.flag, pData.dir, pData.attr) : objGetColDataY(pData.pos_x + num7, pData.pos_y + num8, pData.flag, pData.dir, pData.attr);
                switch (lCol2)
                {
                    case 0:
                        int num9 = num7 + num5;
                        int num10 = num8 + num6;
                        int lCol3 = !flag ? objGetColDataX(pData.pos_x + num9, pData.pos_y + num10, pData.flag, pData.dir, pData.attr) : objGetColDataY(pData.pos_x + num9, pData.pos_y + num10, pData.flag, pData.dir, pData.attr);
                        switch (lCol3)
                        {
                            case 0:
                                if (pData.dir != null)
                                    pData.dir[0] = num3;
                                if (pData.attr != null)
                                    pData.attr[0] = num4;
                                return field_objMapGetForward(sPix, (sbyte)(num5 + num6)) + 16;
                            case 8:
                                return field_objMapGetBack(sPix, (sbyte)(num5 + num6)) + 16;
                            default:
                                return field_objMapGetDiff(lCol3, sPix, (sbyte)(num5 + num6)) + 16;
                        }
                    case 8:
                        return field_objMapGetBack(sPix, (sbyte)(num5 + num6)) + 8;
                    default:
                        return field_objMapGetDiff(lCol2, sPix, (sbyte)(num5 + num6)) + 8;
                }
            case 8:
                if (pData.dir != null)
                    num3 = pData.dir[0];
                if (pData.attr != null)
                    num4 = pData.attr[0];
                int num11 = num1 - num5;
                int num12 = num2 - num6;
                int lCol4 = !flag ? objGetColDataX(pData.pos_x + num11, pData.pos_y + num12, pData.flag, pData.dir, pData.attr) : objGetColDataY(pData.pos_x + num11, pData.pos_y + num12, pData.flag, pData.dir, pData.attr);
                switch (lCol4)
                {
                    case 0:
                        if (pData.dir != null)
                            pData.dir[0] = num3;
                        if (pData.attr != null)
                            pData.attr[0] = num4;
                        return field_objMapGetForwardRev(sPix, (sbyte)(num5 + num6)) - 8;
                    case 8:
                        if (pData.dir != null)
                            num3 = pData.dir[0];
                        if (pData.attr != null)
                            num4 = pData.attr[0];
                        int num13 = num11 - num5;
                        int num14 = num12 - num6;
                        int lCol5 = !flag ? objGetColDataX(pData.pos_x + num13, pData.pos_y + num14, pData.flag, pData.dir, pData.attr) : objGetColDataY(pData.pos_x + num13, pData.pos_y + num14, pData.flag, pData.dir, pData.attr);
                        switch (lCol5)
                        {
                            case 0:
                                if (pData.dir != null)
                                    pData.dir[0] = num3;
                                if (pData.attr != null)
                                    pData.attr[0] = num4;
                                return field_objMapGetForwardRev(sPix, (sbyte)(num5 + num6)) - 16;
                            case 8:
                                return field_objMapGetBack(sPix, (sbyte)(num5 + num6)) - 16;
                            default:
                                return field_objMapGetDiff(lCol5, sPix, (sbyte)(num5 + num6)) - 16;
                        }
                    default:
                        return field_objMapGetDiff(lCol4, sPix, (sbyte)(num5 + num6)) - 8;
                }
            default:
                return field_objMapGetDiff(lCol1, sPix, (sbyte)(num5 + num6));
        }
    }

    private static MP_BLOCK objGetMapBlockData(int pos_x, int pos_y, ushort suf)
    {
        int num1 = pos_x >> 3;
        int num2 = pos_y >> 3;
        int num3 = num1 >> 3;
        int num4 = num2 >> 3;
        int index = _obj_fcol.map_block_num_x * num4 + num3;
        if (index < 0)
            index = 0;
        return _obj_fcol.block_map_datap[suf][index];
    }

    private static byte[] objGetDiffCharData(int pos_x, int pos_y, ushort suf)
    {
        int num1 = pos_x >> 3;
        int num2 = pos_y >> 3;
        int num3 = num1 >> 3;
        int num4 = num2 >> 3;
        int pos_x1 = num1 - num3 * 8;
        int pos_y1 = num2 - num4 * 8;
        int index = _obj_fcol.map_block_num_x * num4 + num3;
        if (index < 0)
            index = 0;
        MP_BLOCK mp_block = _obj_fcol.block_map_datap[suf][index];
        int id = mp_block.id;
        int conv_pos_x;
        int conv_pos_y;
        objGetConv88Pos(pos_x1, pos_y1, ref mp_block, out conv_pos_x, out conv_pos_y);
        DF_BLOCK.DF_CELL dfCell = _obj_fcol.cl_diff_datap[id].df[conv_pos_y, conv_pos_x];
        byte[] diffChar = _objGetDiffCharData.diff_char;
        Buffer.BlockCopy(dfCell.Data, dfCell.Offset, diffChar, 0, 8);
        return diffChar;
    }

    private static sbyte objGetXDiffData(int pos_x, int pos_y, ushort suf)
    {
        byte[] diffCharData = objGetDiffCharData(pos_x, pos_y, suf);
        int conv_pos_x = pos_x & 7;
        int conv_pos_y = pos_y & 7;
        MP_BLOCK mapBlockData = objGetMapBlockData(pos_x, pos_y, suf);
        objGetConv88Pos(conv_pos_x, conv_pos_y, ref mapBlockData, out conv_pos_x, out conv_pos_y);
        sbyte num1 = (mapBlockData.rot & 1) == 0 ? (sbyte)diffCharData[conv_pos_y] : (sbyte)diffCharData[conv_pos_x];
        sbyte num2 = (sbyte)((sbyte)objGetConvDiff(mapBlockData, (byte)num1) & 15);
        if ((num2 & 8) != 0)
            num2 |= -16;
        if (num2 == -8)
            num2 = 8;
        return num2;
    }

    private static sbyte objGetYDiffData(int pos_x, int pos_y, ushort suf)
    {
        byte[] diffCharData = objGetDiffCharData(pos_x, pos_y, suf);
        int conv_pos_x = pos_x & 7;
        int conv_pos_y = pos_y & 7;
        MP_BLOCK mapBlockData = objGetMapBlockData(pos_x, pos_y, suf);
        objGetConv88Pos(conv_pos_x, conv_pos_y, ref mapBlockData, out conv_pos_x, out conv_pos_y);
        sbyte num1 = (mapBlockData.rot & 1) == 0 ? (sbyte)diffCharData[conv_pos_x] : (sbyte)diffCharData[conv_pos_y];
        sbyte num2 = (sbyte)((sbyte)objGetConvDiff(mapBlockData, (byte)num1) >> 4 & 15);
        if ((num2 & 8) != 0)
            num2 |= -16;
        if (num2 == -8)
            num2 = 8;
        return num2;
    }

    private static ushort objGetDirData(int pos_x, int pos_y, ushort suf)
    {
        int num1 = pos_x >> 3;
        int num2 = pos_y >> 3;
        int num3 = num1 >> 3;
        int num4 = num2 >> 3;
        int pos_x1 = num1 - num3 * 8;
        int pos_y1 = num2 - num4 * 8;
        MP_BLOCK mp_block = _obj_fcol.block_map_datap[suf][_obj_fcol.map_block_num_x * num4 + num3];
        int id = mp_block.id;
        int rot = mp_block.rot;
        int conv_pos_x;
        int conv_pos_y;
        objGetConv88Pos(pos_x1, pos_y1, ref mp_block, out conv_pos_x, out conv_pos_y);
        ushort num5 = (ushort)((uint)_obj_fcol.direc_datap[id].di[conv_pos_y][conv_pos_x] << 8);
        if (mp_block.flip_h != 0)
            num5 = (ushort)-(short)num5;
        if (mp_block.flip_v != 0)
            num5 = (ushort)(-((short)num5 + 16384) - 16384);
        ushort num6 = (ushort)-(short)(ushort)(num5 + (uint)(rot * 16384));
        if ((num6 & -49153 ^ 8192) == 0)
        {
            if ((num6 & 16384) != 0)
                num6 += 256;
            else
                num6 -= 256;
        }
        return num6;
    }

    private static void objGetConv88Pos(
      int pos_x,
      int pos_y,
      ref MP_BLOCK mp_block,
      out int conv_pos_x,
      out int conv_pos_y)
    {
        int num1;
        int num2;
        switch (mp_block.rot)
        {
            case 1:
                num1 = 7 - pos_y;
                num2 = pos_x;
                break;
            case 2:
                num1 = 7 - pos_x;
                num2 = 7 - pos_y;
                break;
            case 3:
                num1 = pos_y;
                num2 = 7 - pos_x;
                break;
            default:
                num1 = pos_x;
                num2 = pos_y;
                break;
        }
        if (mp_block.flip_h != 0)
            num1 = 7 - num1;
        if (mp_block.flip_v != 0)
            num2 = 7 - num2;
        conv_pos_x = num1;
        conv_pos_y = num2;
    }

    private static byte objGetConvDiff(MP_BLOCK mp_block, byte diff)
    {
        byte num1 = (byte)(diff & 15U);
        byte num2 = (byte)(diff >> 4 & 15);
        if (mp_block.flip_h != 0 && (num1 & 7) != 0)
            num1 = (byte)(num1 + 8 & 15);
        if (mp_block.flip_v != 0 && (num2 & 7) != 0)
            num2 = (byte)(num2 + 8 & 15);
        switch (mp_block.rot)
        {
            case 1:
                byte num3 = num1;
                num1 = num2;
                num2 = num3;
                if ((num2 & 7) != 0)
                {
                    num2 = (byte)(num2 + 8 & 15);
                    break;
                }
                break;
            case 2:
                if ((num1 & 7) != 0)
                    num1 = (byte)(num1 + 8 & 15);
                if ((num2 & 7) != 0)
                {
                    num2 = (byte)(num2 + 8 & 15);
                    break;
                }
                break;
            case 3:
                byte num4 = num1;
                num1 = num2;
                num2 = num4;
                if ((num1 & 7) != 0)
                {
                    num1 = (byte)(num1 + 8 & 15);
                    break;
                }
                break;
        }
        return (byte)(num1 & 15 | num2 << 4);
    }

    private static int field_objMapGetDiff(int lCol, sbyte sPix, sbyte sDelta)
    {
        return lCol <= 0 ? (sDelta <= 0 ? lCol + sPix : -(sPix + 1)) : (sDelta <= 0 ? 8 - sPix : lCol - (sPix + 1));
    }

    private static int field_objMapGetForward(sbyte sPix, sbyte sDelta)
    {
        return sDelta <= 0 ? 1 + sPix : 8 - sPix;
    }

    private static int field_objMapGetBack(sbyte sPix, sbyte sDelta)
    {
        return sDelta <= 0 ? sPix - 8 : -(sPix + 1);
    }

    private int objMapGetBackFront(sbyte sPix, sbyte sDelta)
    {
        return sDelta <= 0 ? sPix - 8 : -sPix;
    }

    private static int field_objMapGetForwardRev(sbyte sPix, sbyte sDelta)
    {
        return sDelta <= 0 ? sPix : 8 - (sPix + 1);
    }

    private static int objGetColDataX(
      int lPosX,
      int lPosY,
      ushort ucSuf,
      ushort[] pDir,
      uint[] pAttr)
    {
        sbyte num1 = 0;
        if ((ucSuf & 64) != 0)
        {
            if ((int)(lPosX & 4294967288L) > _obj_fcol.right - 1 || lPosX < _obj_fcol.left - 7)
                num1 = 8;
            else if ((int)((lPosX & 4294967288L) + 8L) > _obj_fcol.right - 1)
            {
                num1 = (sbyte)(_obj_fcol.right - 1 & 7);
                if (num1 == 0)
                    num1 = 8;
            }
            else if ((int)(lPosX & 4294967288L) < _obj_fcol.left)
            {
                num1 = (sbyte)(((_obj_fcol.left & 7) == 0 ? 8 : 8 + (8 - (_obj_fcol.left & 7))) | -16);
                if (num1 == -8)
                    num1 = 8;
            }
        }
        else
        {
            lPosX = MTM_MATH_CLIP(lPosX, _obj_fcol.left, _obj_fcol.right - 1);
            lPosY = MTM_MATH_CLIP(lPosY, _obj_fcol.top, _obj_fcol.bottom - 1);
        }
        sbyte num2 = objGetXDiffData(lPosX, lPosY, (ushort)(ucSuf & 1U));
        byte attrData = objGetAttrData(lPosX, lPosY, (ushort)(ucSuf & 1U));
        if ((ucSuf & 128) != 0 && (attrData & 1) != 0)
            num2 = 0;
        if (Math.Abs(num2) < Math.Abs(num1))
            return num1;
        if (pDir != null && num2 != 0)
            pDir[0] = objGetDirData(lPosX, lPosY, (ushort)(ucSuf & 1U));
        if (pAttr != null && num2 != 0)
            pAttr[0] = attrData;
        return num2;
    }

    private static int objGetColDataY(
      int lPosX,
      int lPosY,
      ushort ucSuf,
      ushort[] pDir,
      uint[] pAttr)
    {
        if ((ucSuf & 64) != 0)
        {
            if ((int)(lPosY & 4294967288L) > _obj_fcol.bottom - 1 || lPosY < _obj_fcol.top - 7)
                return 8;
            if ((int)((lPosY & 4294967288L) + 8L) > _obj_fcol.bottom - 1)
            {
                sbyte num = (sbyte)(_obj_fcol.bottom - 1 & 7);
                if (num == 0)
                    num = 8;
                return num;
            }
            if ((int)(lPosY & 4294967288L) < _obj_fcol.top)
            {
                sbyte num = (sbyte)(((_obj_fcol.top & 7) == 0 ? 8 : 8 + (8 - (_obj_fcol.top & 7))) | -16);
                if (num == -8)
                    num = 8;
                return num;
            }
        }
        else
        {
            lPosX = MTM_MATH_CLIP(lPosX, _obj_fcol.left, _obj_fcol.right - 1);
            lPosY = MTM_MATH_CLIP(lPosY, _obj_fcol.top, _obj_fcol.bottom - 1);
        }
        sbyte num1 = objGetYDiffData(lPosX, lPosY, (ushort)(ucSuf & 1U));
        byte attrData = objGetAttrData(lPosX, lPosY, (ushort)(ucSuf & 1U));
        if ((ucSuf & 128) != 0 && (attrData & 1) != 0)
            num1 = 0;
        if (pDir != null && num1 != 0)
            pDir[0] = objGetDirData(lPosX, lPosY, (ushort)(ucSuf & 1U));
        if (pAttr != null && num1 != 0)
            pAttr[0] = objGetAttrData(lPosX, lPosY, (ushort)(ucSuf & 1U));
        return num1;
    }

    private static byte objGetAttrData(int pos_x, int pos_y, ushort suf)
    {
        int num1 = pos_x >> 3;
        int num2 = pos_y >> 3;
        int num3 = num1 >> 3;
        int num4 = num2 >> 3;
        int pos_x1 = num1 - num3 * 8;
        int pos_y1 = num2 - num4 * 8;
        int index = _obj_fcol.map_block_num_x * num4 + num3;
        if (index < 0)
            index = 0;
        MP_BLOCK mp_block = _obj_fcol.block_map_datap[suf][index];
        int id = mp_block.id;
        int conv_pos_x;
        int conv_pos_y;
        objGetConv88Pos(pos_x1, pos_y1, ref mp_block, out conv_pos_x, out conv_pos_y);
        return _obj_fcol.char_attr_datap[id].at[conv_pos_y][conv_pos_x];
    }

    private ushort ObjGetColDataDir(int lPosY, int lPosX, byte ucSuf)
    {
        return objGetDirData(lPosX, lPosY, ucSuf);
    }

    private static void ObjObjectCollisionSet(
      OBS_OBJECT_WORK pObj,
      OBS_COLLISION_WORK pCol,
      short sOfstX,
      short sOfstY,
      ushort usWidth,
      ushort usHeight)
    {
        if (pCol == null)
        {
            if (pObj.col_work != null)
            {
                pCol = pObj.col_work;
            }
            else
            {
                pCol = new OBS_COLLISION_WORK();
                pObj.flag |= 16777216U;
            }
        }
        pObj.col_work = pCol;
        pCol.obj_col.obj = pObj;
        pCol.obj_col.ofst_x = sOfstX;
        pCol.obj_col.ofst_y = sOfstY;
        pCol.obj_col.width = usWidth;
        pCol.obj_col.height = usHeight;
    }

    private static void ObjObjectCollisionDifSet(
      OBS_OBJECT_WORK pObj,
      string pPath,
      OBS_DATA_WORK pData,
      AMS_AMB_HEADER pArchive)
    {
        if (pObj.col_work == null)
            return;
        pObj.col_work.diff_data_work = pData;
        pObj.col_work.obj_col.diff_data = ObjDataLoad(pData, pPath, pArchive);
    }

    private static void ObjObjectCollisionDirSet(
      OBS_OBJECT_WORK pObj,
      string pPath,
      OBS_DATA_WORK pData,
      AMS_AMB_HEADER pArchive)
    {
        if (pObj.col_work == null)
            return;
        pObj.col_work.dir_data_work = pData;
        pObj.col_work.obj_col.dir_data = ObjDataLoad(pData, pPath, pArchive);
    }

    private static void ObjObjectCollisionAtrSet(
      OBS_OBJECT_WORK pObj,
      string pPath,
      OBS_DATA_WORK pData,
      AMS_AMB_HEADER pArchive)
    {
        if (pObj.col_work == null)
            return;
        pObj.col_work.attr_data_work = pData;
        pObj.col_work.obj_col.attr_data = ObjDataLoad(pData, pPath, pArchive);
    }

    private static void ObjCollisionObjectRegist(OBS_COLLISION_OBJ pObj)
    {
        if (_obj_collision_num_nx >= 144)
        {
            MTM_ASSERT(0);
        }
        else
        {
            if (pObj.obj != null && ((int)pObj.obj.flag & 12) != 0)
                return;
            if (pObj.rider_obj != null && pObj.rider_obj.ride_obj != pObj.obj)
                pObj.rider_obj = null;
            if (pObj.toucher_obj != null && pObj.toucher_obj.touch_obj != pObj.obj)
                pObj.toucher_obj = null;
            _obj_collision_tbl_nx[_obj_collision_num_nx] = pObj;
            ++_obj_collision_num_nx;
            VecFx32 vecFx32 = new VecFx32(pObj.pos);
            if (pObj.obj != null && ((int)pObj.flag & 16) == 0)
            {
                vecFx32.x += pObj.obj.pos.x;
                vecFx32.y += pObj.obj.pos.y;
                vecFx32.z += pObj.obj.pos.z;
            }
            pObj.check_pos = vecFx32;
            pObj.flag &= 1073741823U;
            if (pObj.obj != null)
            {
                if (((int)pObj.obj.disp_flag & 1) != 0)
                    pObj.flag |= 1073741824U;
                if (((int)pObj.obj.disp_flag & 2) != 0)
                    pObj.flag |= 2147483648U;
            }
            else
            {
                if (((int)pObj.flag & 1) != 0)
                    pObj.flag |= 1073741824U;
                if (((int)pObj.flag & 2) != 0)
                    pObj.flag |= 2147483648U;
            }
            objCollsionOffsetSet(pObj, out pObj.check_ofst_x, out pObj.check_ofst_y);
            pObj.left = (pObj.check_pos.x >> 12) + pObj.check_ofst_x;
            pObj.top = (pObj.check_pos.y >> 12) + pObj.check_ofst_y;
            pObj.right = (pObj.check_pos.x >> 12) + pObj.width + pObj.check_ofst_x;
            pObj.bottom = (pObj.check_pos.y >> 12) + pObj.height + pObj.check_ofst_y;
            if (((int)pObj.flag & 64) == 0)
                pObj.check_dir = pObj.dir;
            if (((int)pObj.flag & 32) != 0 || pObj.obj == null)
                return;
            pObj.check_dir += (ushort)(pObj.obj.dir.z + (uint)pObj.obj.dir_fall);
        }
    }

    private static void ObjCollisionObjectClear()
    {
        ushort num;
        for (num = 0; num < _obj_collision_num_nx; ++num)
            _obj_collision_tbl[num] = _obj_collision_tbl_nx[num];
        for (; num < 144; ++num)
            _obj_collision_tbl_nx[num] = null;
        _obj_collision_num = _obj_collision_num_nx;
        _obj_collision_num_nx = 0;
    }

    private static void objCollsionOffsetSet(
      OBS_COLLISION_OBJ pCol,
      out short cOfstX,
      out short cOfstY)
    {
        cOfstX = pCol.ofst_x;
        cOfstY = pCol.ofst_y;
        if (((int)pCol.flag & 1073741824) != 0)
            cOfstX = (short)(-pCol.ofst_x - pCol.width);
        if (((int)pCol.flag & int.MinValue) == 0)
            return;
        cOfstY = (short)(-pCol.ofst_y - pCol.height);
    }

    private static int ObjCollisionObjectFastCheckDet(
      int lPosX,
      int lPosY,
      ushort usFlag,
      ushort usVec,
      ushort[] pDir,
      uint[] pAttr)
    {
        OBS_COL_CHK_DATA pData = GlobalPool<OBS_COL_CHK_DATA>.Alloc();
        pData.pos_x = lPosX;
        pData.pos_y = lPosY;
        pData.dir = pDir;
        pData.attr = pAttr;
        pData.flag = usFlag;
        pData.vec = usVec;
        int num = ObjCollisionObjectFastCheck(pData);
        GlobalPool<OBS_COL_CHK_DATA>.Release(pData);
        return num;
    }

    private static int ObjCollisionObjectFastCheck(OBS_COL_CHK_DATA pData)
    {
        int num = 24;
        int a = 0;
        int posX = pData.pos_x;
        int posY = pData.pos_y;
        if (_obj_collision_num == 0)
            return num;
        OBS_COL_CHK_DATA pData1 = GlobalPool<OBS_COL_CHK_DATA>.Alloc();
        pData1.Assign(pData);
        for (ushort index = 0; index < _obj_collision_num; ++index)
        {
            OBS_COLLISION_OBJ pColObj = _obj_collision_tbl[index];
            if (pColObj.obj != null && ((int)pColObj.flag & 256) == 0)
            {
                pData1.pos_x = pData.pos_x;
                pData1.pos_y = pData.pos_y;
                if (((int)pColObj.flag & 4) != 0 && pColObj.check_dir != 0)
                {
                    int dest_x = (pData1.pos_x << 12) - pColObj.check_pos.x;
                    int dest_y = (pData1.pos_y << 12) - pColObj.check_pos.y;
                    ObjUtilGetRotPosXY(dest_x, dest_y, ref dest_x, ref dest_y, (ushort)-pColObj.check_dir);
                    pData1.pos_x = dest_x + (pColObj.check_pos.x >> 12);
                    pData1.pos_y = dest_y + (pColObj.check_pos.y >> 12);
                }
                if (pColObj.diff_data != null)
                {
                    a = objFastCollisionDiffObject(pColObj, pData1);
                    if (a == 0)
                    {
                        switch (pData.vec)
                        {
                            case 0:
                                if (pColObj.obj.move.x < 0)
                                {
                                    --a;
                                    break;
                                }
                                break;
                            case 1:
                                if (pColObj.obj.move.x > 0)
                                {
                                    --a;
                                    break;
                                }
                                break;
                        }
                    }
                }
                else
                {
                    switch (pData.vec)
                    {
                        case 0:
                            a = pColObj.left - posX;
                            if (a == 0 && pColObj.obj.move.x < 0)
                            {
                                --a;
                                break;
                            }
                            break;
                        case 1:
                            a = posX - pColObj.right;
                            if (a == 0 && pColObj.obj.move.x > 0)
                            {
                                --a;
                                break;
                            }
                            break;
                        case 2:
                            a = pColObj.top - posY;
                            break;
                        case 3:
                            a = posY - pColObj.bottom;
                            break;
                    }
                    a = MTM_MATH_CLIP(a, -31, 31);
                }
                if (num > a)
                    num = a;
            }
        }
        GlobalPool<OBS_COL_CHK_DATA>.Release(pData1);
        return num;
    }

    private static int ObjCollisionObjectCheckDet(
      OBS_OBJECT_WORK pObj,
      int lPosX,
      int lPosY,
      ushort usFlag,
      ushort usVec,
      ushort[] pDir,
      uint[] pAttr)
    {
        OBS_COL_CHK_DATA pData = GlobalPool<OBS_COL_CHK_DATA>.Alloc();
        pData.pos_x = lPosX;
        pData.pos_y = lPosY;
        pData.dir = pDir;
        pData.attr = pAttr;
        pData.flag = usFlag;
        pData.vec = usVec;
        int num = ObjCollisionObjectCheck(pObj, pData);
        GlobalPool<OBS_COL_CHK_DATA>.Release(pData);
        return num;
    }

    private static int ObjCollisionObjectCheck(
      OBS_OBJECT_WORK pObj,
      OBS_COL_CHK_DATA pData)
    {
        int num1 = 24;
        short num2 = 0;
        ushort num3 = 0;
        if (_obj_collision_num == 0)
            return num1;
        OBS_COL_CHK_DATA pData1 = GlobalPool<OBS_COL_CHK_DATA>.Alloc();
        pData1.Assign(pData);
        ushort num4;
        if (((int)pObj.move_flag & 16) == 0)
        {
            num2 = 1;
            num4 = 0;
        }
        else
            num4 = pObj.dir.z;
        switch ((num4 + 8192 & 49152) >> 14)
        {
            case 1:
                if (pData.vec == 1 && (((int)pObj.move_flag & 16) == 0 || pObj.move.x < 0))
                {
                    num3 = 1;
                    break;
                }
                break;
            case 2:
                if (pData.vec == 3 && (((int)pObj.move_flag & 16) == 0 || pObj.move.y < 0))
                {
                    num3 = 1;
                    break;
                }
                break;
            case 3:
                if (pData.vec == 0 && (((int)pObj.move_flag & 16) == 0 || pObj.move.x > 0))
                {
                    num3 = 1;
                    break;
                }
                break;
            default:
                if (pData.vec == 2 && (((int)pObj.move_flag & 16) == 0 || pObj.move.y >= 0))
                {
                    num3 = 1;
                    break;
                }
                break;
        }
        short num5 = !GSM_MAIN_STAGE_IS_SPSTAGE_NOT_RETRY() ? (short)24 : (short)24;
        for (ushort index = 0; index < _obj_collision_num; ++index)
        {
            OBS_COLLISION_OBJ pColObj = _obj_collision_tbl[index];
            if (pColObj.obj != null && pColObj.obj != pObj && ((int)pColObj.flag & 256) == 0)
            {
                pData1.pos_x = pData.pos_x;
                pData1.pos_y = pData.pos_y;
                if (((int)pColObj.flag & 4) != 0 && pColObj.check_dir != 0)
                {
                    int dest_x = (pData1.pos_x << 12) - pColObj.check_pos.x;
                    int dest_y = (pData1.pos_y << 12) - pColObj.check_pos.y;
                    ObjUtilGetRotPosXY(dest_x, dest_y, ref dest_x, ref dest_y, (ushort)-pColObj.check_dir);
                    pData1.pos_x = dest_x + (pColObj.check_pos.x >> 12);
                    pData1.pos_y = dest_y + (pColObj.check_pos.y >> 12);
                }
                if (pData1.pos_x >= (pColObj.check_pos.x >> 12) - num5 + pColObj.check_ofst_x && pData1.pos_x <= (pColObj.check_pos.x >> 12) + pColObj.width + num5 + pColObj.check_ofst_x && (pData1.pos_y >= (pColObj.check_pos.y >> 12) - num5 + pColObj.check_ofst_y && pData1.pos_y <= (pColObj.check_pos.y >> 12) + pColObj.height + num5 + pColObj.check_ofst_y))
                {
                    int num6;
                    if (pColObj.diff_data != null)
                    {
                        num6 = objCollisionDiffObject(pColObj, pData1);
                        if (num6 == 0)
                        {
                            switch (pData.vec)
                            {
                                case 0:
                                    if (pColObj.obj != null && pColObj.obj.move.x < 0)
                                    {
                                        --num6;
                                        break;
                                    }
                                    break;
                                case 1:
                                    if (pColObj.obj != null && pColObj.obj.move.x > 0)
                                    {
                                        --num6;
                                        break;
                                    }
                                    break;
                            }
                        }
                    }
                    else
                    {
                        int a = 24;
                        switch (pData.vec)
                        {
                            case 0:
                                if (pData1.pos_x < pColObj.right && pData1.pos_y > pColObj.top && pData1.pos_y < pColObj.bottom)
                                {
                                    a = pColObj.left - pData1.pos_x;
                                    if (a == 0 && pColObj.obj != null && pColObj.obj.move.x < 0)
                                    {
                                        --a;
                                        break;
                                    }
                                    break;
                                }
                                break;
                            case 1:
                                if (pColObj.left < pData1.pos_x && pData1.pos_y > pColObj.top && pData1.pos_y < pColObj.bottom)
                                {
                                    a = pData1.pos_x - pColObj.right;
                                    if (a == 0 && pColObj.obj != null && pColObj.obj.move.x > 0)
                                    {
                                        --a;
                                        break;
                                    }
                                    break;
                                }
                                break;
                            case 2:
                                if (pData1.pos_y < pColObj.bottom && pData1.pos_x > pColObj.left && pData1.pos_x < pColObj.right)
                                {
                                    a = pColObj.top - pData1.pos_y;
                                    break;
                                }
                                break;
                            case 3:
                                if (pColObj.top < pData1.pos_y && pData1.pos_x > pColObj.left && pData1.pos_x < pColObj.right)
                                {
                                    a = pData1.pos_y - pColObj.bottom;
                                    break;
                                }
                                break;
                        }
                        num6 = MTM_MATH_CLIP(a, -31, 31);
                    }
                    if (num1 > num6)
                    {
                        num1 = num6;
                        if (num1 <= num2)
                        {
                            pObj.touch_obj = pColObj.obj;
                            pColObj.toucher_obj = pObj;
                        }
                        if (num1 <= num2 && num3 != 0)
                        {
                            pObj.ride_obj = pColObj.obj;
                            pColObj.rider_obj = pObj;
                            if (pData.dir != null)
                                pData.dir[0] += pColObj.check_dir;
                            if (pData.attr != null && ((int)pColObj.flag & 128) == 0)
                                pData.attr[0] |= (byte)pColObj.attr;
                        }
                    }
                }
            }
        }
        GlobalPool<OBS_COL_CHK_DATA>.Release(pData1);
        return num1;
    }

    private static OBS_COLLISION_OBJ ObjCollisionDiffObjectGetCollisionObj(byte col_no)
    {
        return col_no < _obj_collision_num ? _obj_collision_tbl[col_no] : null;
    }

    private static int objFastCollisionDiffObject(
      OBS_COLLISION_OBJ pColObj,
      OBS_COL_CHK_DATA pData)
    {
        ushort num1 = 0;
        uint num2 = 0;
        sbyte sDelta = 8;
        if (pData.dir != null)
            num1 = pData.dir[0];
        if (pData.attr != null)
            num2 = pData.attr[0];
        if ((pData.vec & 1) != 0)
            sDelta = -8;
        int lCol;
        sbyte sPix;
        if ((pData.vec & 2) != 0)
        {
            lCol = objGetColDataY(pColObj, pData.pos_x, pData.pos_y, pData.flag, pData.dir, pData.attr);
            sPix = (sbyte)(pData.pos_y - pColObj.top & 7);
        }
        else
        {
            lCol = objGetColDataX(pColObj, pData.pos_x, pData.pos_y, pData.flag, pData.dir, pData.attr);
            sPix = (sbyte)(pData.pos_x - pColObj.left & 7);
        }
        switch (lCol)
        {
            case 0:
                if (pData.dir != null)
                    pData.dir[0] = num1;
                if (pData.attr != null)
                    pData.attr[0] = num2;
                return objMapGetForward(sPix, sDelta);
            case 8:
                return objMapGetBack(sPix, sDelta);
            default:
                return objMapGetDiff(lCol, sPix, sDelta);
        }
    }

    private static int objCollisionDiffObject(
      OBS_COLLISION_OBJ pColObj,
      OBS_COL_CHK_DATA pData)
    {
        int num1 = 0;
        int num2 = 0;
        uint num3 = 0;
        ushort num4 = 0;
        sbyte num5 = 0;
        sbyte num6 = 0;
        if (pData.dir != null)
            num4 = pData.dir[0];
        if (pData.attr != null)
            num3 = pData.attr[0];
        if ((pData.vec & 2) != 0)
        {
            num6 = 8;
            if ((pData.vec & 1) != 0)
                num6 = -8;
        }
        else
        {
            num5 = 8;
            if ((pData.vec & 1) != 0)
                num5 = -8;
        }
        sbyte sPix;
        pFunc_Delegate pFuncDelegate;
        if ((pData.vec & 2) != 0)
        {
            sPix = (sbyte)(pData.pos_y - pColObj.top & 7);
            pFuncDelegate = _objGetColDataY;
        }
        else
        {
            sPix = (sbyte)(pData.pos_x - pColObj.left & 7);
            pFuncDelegate = _objGetColDataX;
        }
        int lCol1 = pFuncDelegate(pColObj, pData.pos_x, pData.pos_y, pData.flag, pData.dir, pData.attr);
        switch (lCol1)
        {
            case 0:
                int num7 = num1 + num5;
                int num8 = num2 + num6;
                int lCol2 = pFuncDelegate(pColObj, pData.pos_x + num7, pData.pos_y + num8, pData.flag, pData.dir, pData.attr);
                switch (lCol2)
                {
                    case 0:
                        int num9 = num7 + num5;
                        int num10 = num8 + num6;
                        int lCol3 = pFuncDelegate(pColObj, pData.pos_x + num9, pData.pos_y + num10, pData.flag, pData.dir, pData.attr);
                        switch (lCol3)
                        {
                            case 0:
                                if (pData.dir != null)
                                    pData.dir[0] = num4;
                                if (pData.attr != null)
                                    pData.attr[0] = num3;
                                return objMapGetForward(sPix, (sbyte)(num5 + num6)) + 16;
                            case 8:
                                return objMapGetBack(sPix, (sbyte)(num5 + num6)) + 16;
                            default:
                                return objMapGetDiff(lCol3, sPix, (sbyte)(num5 + num6)) + 16;
                        }
                    case 8:
                        return objMapGetBack(sPix, (sbyte)(num5 + num6)) + 8;
                    default:
                        return objMapGetDiff(lCol2, sPix, (sbyte)(num5 + num6)) + 8;
                }
            case 8:
                if (pData.dir != null)
                    num4 = pData.dir[0];
                if (pData.attr != null)
                    num3 = pData.attr[0];
                int num11 = num1 - num5;
                int num12 = num2 - num6;
                int lCol4 = pFuncDelegate(pColObj, pData.pos_x + num11, pData.pos_y + num12, pData.flag, pData.dir, pData.attr);
                switch (lCol4)
                {
                    case 0:
                        if (pData.dir != null)
                            pData.dir[0] = num4;
                        if (pData.attr != null)
                            pData.attr[0] = num3;
                        return objMapGetForwardRev(sPix, (sbyte)(num5 + num6)) - 8;
                    case 8:
                        if (pData.dir != null)
                            num4 = pData.dir[0];
                        if (pData.attr != null)
                            num3 = pData.attr[0];
                        int num13 = num11 - num5;
                        int num14 = num12 - num6;
                        int lCol5 = pFuncDelegate(pColObj, pData.pos_x + num13, pData.pos_y + num14, pData.flag, pData.dir, pData.attr);
                        switch (lCol5)
                        {
                            case 0:
                                if (pData.dir != null)
                                    pData.dir[0] = num4;
                                if (pData.attr != null)
                                    pData.attr[0] = num3;
                                return objMapGetForwardRev(sPix, (sbyte)(num5 + num6)) - 16;
                            case 8:
                                return objMapGetBack(sPix, (sbyte)(num5 + num6)) - 16;
                            default:
                                return objMapGetDiff(lCol5, sPix, (sbyte)(num5 + num6)) - 16;
                        }
                    default:
                        return objMapGetDiff(lCol4, sPix, (sbyte)(num5 + num6)) - 8;
                }
            default:
                return objMapGetDiff(lCol1, sPix, (sbyte)(num5 + num6));
        }
    }

    private static int objGetColDataX(
      OBS_COLLISION_OBJ pColObj,
      int lPosX,
      int lPosY,
      ushort ucSuf,
      ushort[] pDir,
      uint[] pAttr)
    {
        sbyte num1 = 0;
        if (lPosX < pColObj.left || lPosX >= pColObj.right || (lPosY < pColObj.top || lPosY >= pColObj.bottom))
            return num1;
        ushort num2 = (ushort)(lPosX - pColObj.left >> 3);
        ushort num3 = (ushort)(lPosY - pColObj.top >> 3);
        if (((int)pColObj.flag & 1073741824) != 0)
            num2 = (ushort)((uint)((pColObj.width >> 3) - 1) - num2);
        if (((int)pColObj.flag & int.MinValue) != 0)
            num3 = (ushort)((uint)((pColObj.height >> 3) - 1) - num3);
        ushort num4 = (ushort)(num2 + num3 * ((uint)pColObj.width >> 3));
        int num5 = lPosY - pColObj.top & 7;
        if (((int)pColObj.flag & int.MinValue) != 0)
            num5 = 7 - num5;
        sbyte num6 = (sbyte)((sbyte)pColObj.diff_data[(num4 << 3) + num5] & 15);
        if ((num6 & 8) != 0)
            num6 |= -16;
        if (num6 == -8)
            num6 = 8;
        if (pColObj.attr_data != null)
        {
            if ((ucSuf & 128) != 0 && ((pColObj.attr_data[num4 >> 3] & 1) != 0 || (pColObj.attr & 1) != 0))
                num6 = 0;
        }
        else if ((ucSuf & 128) != 0 && (pColObj.attr & 1) != 0)
            num6 = 0;
        if (((int)pColObj.flag & 1073741824) != 0 && num6 != 8 && num6 != 0)
        {
            if (num6 > 0)
                num6 -= 8;
            else
                num6 += 8;
        }
        if (pDir != null && num6 != 0)
        {
            ushort num7 = pColObj.dir_data == null ? (ushort)0 : (ushort)((uint)pColObj.dir_data[num4] << 8);
            if (((int)pColObj.flag & 4) != 0)
                num7 += pColObj.check_dir;
            if (((int)pColObj.flag & 8) != 0)
            {
                if (((int)pColObj.flag & 1073741824) != 0)
                    num7 = (ushort)-(short)num7;
                if (((int)pColObj.flag & int.MinValue) != 0)
                    num7 = (ushort)(-((short)num7 + 16384) - 16384);
            }
            pDir[0] = num7;
        }
        if (pAttr != null && num6 != 0)
            pAttr[0] = pColObj.attr_data == null ? pColObj.attr : pColObj.attr_data[num4 >> 3] | (uint)pColObj.attr;
        return num6;
    }

    private static int objGetColDataY(
      OBS_COLLISION_OBJ pColObj,
      int lPosX,
      int lPosY,
      ushort ucSuf,
      ushort[] pDir,
      uint[] pAttr)
    {
        sbyte num1 = 0;
        if (lPosX < pColObj.left || lPosX >= pColObj.right || (lPosY < pColObj.top || lPosY >= pColObj.bottom))
            return num1;
        ushort num2 = (ushort)(lPosX - pColObj.left >> 3);
        ushort num3 = (ushort)(lPosY - pColObj.top >> 3);
        if (((int)pColObj.flag & 1073741824) != 0)
            num2 = (ushort)((uint)((pColObj.width >> 3) - 1) - num2);
        if (((int)pColObj.flag & int.MinValue) != 0)
            num3 = (ushort)((uint)((pColObj.height >> 3) - 1) - num3);
        ushort num4 = (ushort)(num2 + num3 * ((uint)pColObj.width >> 3));
        int num5 = lPosX - pColObj.left & 7;
        if (((int)pColObj.flag & 1073741824) != 0)
            num5 = 7 - num5;
        sbyte num6 = (sbyte)((sbyte)((sbyte)pColObj.diff_data[(num4 << 3) + num5] >> 4) & 15);
        if ((num6 & 8) != 0)
            num6 |= -16;
        if (num6 == -8)
            num6 = 8;
        if (pColObj.attr_data != null)
        {
            if ((ucSuf & 128) != 0 && ((pColObj.attr_data[num4 >> 3] & 1) != 0 || (pColObj.attr & 1) != 0))
                num6 = 0;
        }
        else if ((ucSuf & 128) != 0 && (pColObj.attr & 1) != 0)
            num6 = 0;
        if (((int)pColObj.flag & int.MinValue) != 0 && num6 != 8 && num6 != 0)
        {
            if (num6 > 0)
                num6 -= 8;
            else
                num6 += 8;
        }
        if (pDir != null && num6 != 0)
        {
            ushort num7 = pColObj.dir_data == null ? (ushort)0 : (ushort)((uint)pColObj.dir_data[num4] << 8);
            if (((int)pColObj.flag & 4) != 0)
                num7 += pColObj.check_dir;
            if (((int)pColObj.flag & 8) != 0)
            {
                if (((int)pColObj.flag & 1073741824) != 0)
                    num7 = (ushort)-(short)num7;
                if (((int)pColObj.flag & int.MinValue) != 0)
                    num7 = (ushort)(-((short)num7 + 16384) - 16384);
            }
            pDir[0] = num7;
        }
        if (pAttr != null && num6 != 0)
            pAttr[0] = pColObj.attr_data == null ? pColObj.attr : pColObj.attr_data[num4 >> 3] | (uint)pColObj.attr;
        return num6;
    }

}