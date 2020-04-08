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
    public static bool OBM_LINE_AND_LINE(int x0, int w0, int x1, int w1)
    {
        if (x0 <= x1 && x0 + w0 >= x1)
            return true;
        return x0 >= x1 && x1 + w1 >= x0;
    }

    public static bool OBM_POINT_IN_LINE(int x0, int w0, int x1)
    {
        return x0 <= x1 && x0 + w0 >= x1;
    }

    public static AppMain.OBS_RECT ObjRectSet(
      AppMain.OBS_RECT pRec,
      short cLeft,
      short cTop,
      short cRight,
      short cBottom)
    {
        pRec.left = cLeft;
        pRec.top = cTop;
        pRec.right = cRight;
        pRec.bottom = cBottom;
        pRec.back = (short)-16;
        pRec.front = (short)16;
        if ((int)pRec.right < (int)pRec.left)
            AppMain.MTM_MATH_SWAP<short>(ref pRec.left, ref pRec.right);
        if ((int)pRec.bottom < (int)pRec.top)
            AppMain.MTM_MATH_SWAP<short>(ref pRec.top, ref pRec.bottom);
        AppMain.VEC_Set(ref pRec.pos, 0, 0, 0);
        return pRec;
    }

    public static AppMain.OBS_RECT ObjRectZSet(
      AppMain.OBS_RECT pRec,
      short cLeft,
      short cTop,
      short cBack,
      short cRight,
      short cBottom,
      short cFront)
    {
        pRec.left = cLeft;
        pRec.top = cTop;
        pRec.right = cRight;
        pRec.bottom = cBottom;
        pRec.back = cBack;
        pRec.front = cFront;
        if ((int)pRec.right < (int)pRec.left)
            AppMain.MTM_MATH_SWAP<short>(ref pRec.left, ref pRec.right);
        if ((int)pRec.bottom < (int)pRec.top)
            AppMain.MTM_MATH_SWAP<short>(ref pRec.top, ref pRec.bottom);
        if ((int)pRec.front < (int)pRec.back)
            AppMain.MTM_MATH_SWAP<short>(ref pRec.back, ref pRec.front);
        AppMain.VEC_Set(ref pRec.pos, 0, 0, 0);
        return pRec;
    }

    public static AppMain.OBS_RECT ObjRectAllSet(
      AppMain.OBS_RECT pRec,
      AppMain.VecFx32 pos,
      short cLeft,
      short cTop,
      short cBack,
      short cRight,
      short cBottom,
      short cFront)
    {
        pRec.pos.Assign(pos);
        pRec.left = cLeft;
        pRec.top = cTop;
        pRec.right = cRight;
        pRec.bottom = cBottom;
        pRec.back = cBack;
        pRec.front = cFront;
        if ((int)pRec.right < (int)pRec.left)
            AppMain.MTM_MATH_SWAP<short>(ref pRec.left, ref pRec.right);
        if ((int)pRec.bottom < (int)pRec.top)
            AppMain.MTM_MATH_SWAP<short>(ref pRec.top, ref pRec.bottom);
        if ((int)pRec.front < (int)pRec.back)
            AppMain.MTM_MATH_SWAP<short>(ref pRec.back, ref pRec.front);
        return pRec;
    }

    public static AppMain.OBS_RECT ObjRectWorkSet(
      AppMain.OBS_RECT_WORK pRec,
      short cLeft,
      short cTop,
      short cRight,
      short cBottom)
    {
        pRec.flag |= 4U;
        return AppMain.ObjRectSet(pRec.rect, cLeft, cTop, cRight, cBottom);
    }

    public static AppMain.OBS_RECT ObjRectWorkZSet(
      AppMain.OBS_RECT_WORK pRec,
      short cLeft,
      short cTop,
      short cBack,
      short cRight,
      short cBottom,
      short cFront)
    {
        pRec.flag |= 4U;
        return AppMain.ObjRectZSet(pRec.rect, cLeft, cTop, cBack, cRight, cBottom, cFront);
    }

    public static AppMain.OBS_RECT ObjRectWorkAllSet(
      AppMain.OBS_RECT_WORK pRec,
      AppMain.VecFx32 pos,
      short cLeft,
      short cTop,
      short cBack,
      short cRight,
      short cBottom,
      short cFront)
    {
        pRec.flag |= 4U;
        return AppMain.ObjRectAllSet(pRec.rect, pos, cLeft, cTop, cBack, cRight, cBottom, cFront);
    }

    public static void ObjRectGroupSet(AppMain.OBS_RECT_WORK pRec, byte group_no, byte target_g_flag)
    {
        pRec.group_no = group_no;
        pRec.target_g_flag = target_g_flag;
    }

    public static void ObjRectAtkSet(AppMain.OBS_RECT_WORK pRec, ushort usHitFlag, short sHitPower)
    {
        pRec.flag |= 4U;
        pRec.hit_flag = usHitFlag;
        pRec.hit_power = sHitPower;
        pRec.flag &= 4294966783U;
        pRec.flag &= 4294901759U;
    }

    public static void ObjRectDefSet(AppMain.OBS_RECT_WORK pRec, ushort usDefFlag, short sDefPower)
    {
        pRec.flag |= 4U;
        pRec.def_flag = usDefFlag;
        pRec.def_power = sDefPower;
        pRec.flag &= 4294967039U;
    }

    public static void ObjRectHitAgain(AppMain.OBS_RECT_WORK pRec)
    {
        if (((int)pRec.flag & 1024) != 0)
            return;
        pRec.flag &= 4294967039U;
        pRec.flag &= 4294966783U;
    }

    public static void ObjRectCheckInit()
    {
        Array.Clear((Array)AppMain._obj_user_resist_nx, 0, AppMain._obj_user_resist_nx.Length);
        Array.Clear((Array)AppMain._obj_user_resist, 0, AppMain._obj_user_resist.Length);
        Array.Clear((Array)AppMain._obj_user_resist_num, 0, AppMain._obj_user_resist_num.Length);
        Array.Clear((Array)AppMain._obj_user_resist_num_nx, 0, AppMain._obj_user_resist_num_nx.Length);
        Array.Clear((Array)AppMain._obj_user_flag, 0, AppMain._obj_user_flag.Length);
        Array.Clear((Array)AppMain._obj_user_flag_nx, 0, AppMain._obj_user_flag_nx.Length);
        AppMain._obj_user_resist_all_num = (ushort)0;
        AppMain._obj_user_resist_all_num_nx = (ushort)0;
        AppMain._obj_ulFlagBackA = 0U;
        AppMain._obj_ulFlagBackD = 0U;
        AppMain._obj_ucNoHit = (byte)0;
    }

    private static void objRectCheckOut()
    {
        Array.Copy((Array)AppMain._obj_user_resist_nx, (Array)AppMain._obj_user_resist, AppMain._obj_user_resist.Length);
        Array.Clear((Array)AppMain._obj_user_resist_nx, 0, AppMain._obj_user_resist_nx.Length);
        Array.Copy((Array)AppMain._obj_user_resist_num_nx, (Array)AppMain._obj_user_resist_num, AppMain._obj_user_resist_num.Length);
        Array.Clear((Array)AppMain._obj_user_resist_num_nx, 0, AppMain._obj_user_resist_num_nx.Length);
        Array.Copy((Array)AppMain._obj_user_flag_nx, (Array)AppMain._obj_user_flag, AppMain._obj_user_flag.Length);
        Array.Clear((Array)AppMain._obj_user_flag_nx, 0, AppMain._obj_user_flag_nx.Length);
        AppMain._obj_user_resist_all_num = AppMain._obj_user_resist_all_num_nx;
        AppMain._obj_user_resist_all_num_nx = (ushort)0;
        for (ushort index1 = 0; (int)index1 < (int)AppMain._obj_user_resist_all_num - 1; ++index1)
        {
            for (ushort index2 = (ushort)((uint)AppMain._obj_user_resist_all_num - 1U); (int)index2 > (int)index1; --index2)
            {
                if ((int)AppMain._obj_user_resist[(int)index2].group_no < (int)AppMain._obj_user_resist[(int)index2 - 1].group_no)
                {
                    AppMain.OBS_RECT_WORK obsRectWork = AppMain._obj_user_resist[(int)index2 - 1];
                    AppMain._obj_user_resist[(int)index2 - 1] = AppMain._obj_user_resist[(int)index2];
                    AppMain._obj_user_resist[(int)index2] = obsRectWork;
                }
            }
        }
    }

    private static void ObjRectRegist(AppMain.OBS_RECT_WORK pObj)
    {
        if (((int)pObj.flag & 4) == 0 || pObj.group_no >= (byte)8 || AppMain._obj_user_resist_all_num_nx >= (ushort)80)
            return;
        AppMain._obj_user_resist_nx[(int)AppMain._obj_user_resist_all_num_nx] = pObj;
        AppMain._obj_user_flag_nx[(int)pObj.group_no] |= (ushort)pObj.target_g_flag;
        ++AppMain._obj_user_resist_num_nx[(int)pObj.group_no];
        ++AppMain._obj_user_resist_all_num_nx;
    }

    private static void ObjRectCheckAllGroup()
    {
        if (((int)AppMain.g_obj.flag & 131072) != 0)
            AppMain.objRectCheckOut();
        AppMain._obj_ulFlagBackA = 0U;
        AppMain._obj_ulFlagBackD = 0U;
        AppMain._obj_ucNoHit = (byte)0;
        for (ushort index = 0; (int)index < (int)AppMain._obj_user_resist_all_num; ++index)
        {
            if (AppMain._obj_user_resist[(int)index] != null && ((int)AppMain._obj_user_resist[(int)index].flag & 1024) != 0)
                AppMain._obj_user_resist[(int)index].flag &= 4294836223U;
        }
        ushort num1 = 0;
        for (ushort index = 0; index < (ushort)8; ++index)
        {
            ushort num2 = 0;
            for (byte Index = 0; Index < (byte)8; ++Index)
            {
                if (AppMain._obj_user_resist_num[(int)Index] != (byte)0 && ((int)AppMain._obj_user_flag[(int)index] & 1 << (int)Index) != 0)
                    AppMain.objRectCheckGroup(new AppMain.ArrayPointer<AppMain.OBS_RECT_WORK>(AppMain._obj_user_resist, (int)num1), new AppMain.ArrayPointer<AppMain.OBS_RECT_WORK>(AppMain._obj_user_resist, (int)num2), AppMain._obj_user_resist_num[(int)index], AppMain._obj_user_resist_num[(int)Index], Index);
                num2 += (ushort)AppMain._obj_user_resist_num[(int)Index];
            }
            num1 += (ushort)AppMain._obj_user_resist_num[(int)index];
        }
        for (ushort index = 0; (int)index < (int)AppMain._obj_user_resist_all_num; ++index)
        {
            if (AppMain._obj_user_resist[(int)index] != null)
            {
                if (((int)AppMain._obj_user_resist[(int)index].flag & 65536) != 0)
                {
                    AppMain._obj_user_resist[(int)index].flag |= 512U;
                    AppMain._obj_user_resist[(int)index].flag &= 4294901759U;
                }
                if (((int)AppMain._obj_user_resist[(int)index].flag & 1024) != 0 && ((int)AppMain._obj_user_resist[(int)index].flag & 131072) == 0)
                    AppMain._obj_user_resist[(int)index].flag &= 4294705151U;
            }
        }
        if (((int)AppMain.g_obj.flag & 131072) != 0)
            return;
        AppMain.objRectCheckOut();
    }

    private static AppMain.OBS_RECT_WORK ObjRectRegistGet(byte ucGroup, short sIndex)
    {
        ushort num = 0;
        for (short index = 0; index < (short)8; ++index)
        {
            if (((int)ucGroup & 1 << (int)index) != 0)
            {
                if ((int)sIndex < (int)AppMain._obj_user_resist_num[(int)index])
                    return AppMain._obj_user_resist[(int)num + (int)sIndex];
                sIndex -= (short)AppMain._obj_user_resist_num[(int)index];
                num += (ushort)AppMain._obj_user_resist_num[(int)index];
                if (sIndex > (short)0)
                    ;
            }
            else
                num += (ushort)AppMain._obj_user_resist_num[(int)index];
        }
        return (AppMain.OBS_RECT_WORK)null;
    }

    private static AppMain.OBS_RECT_WORK ObjRectRegistNxGet(byte ucGroup, short sIndex)
    {
        ushort num = 0;
        for (short index = 0; index < (short)8; ++index)
        {
            if (((int)ucGroup & 1 << (int)index) != 0)
            {
                if ((int)sIndex < (int)AppMain._obj_user_resist_num_nx[(int)index])
                    return AppMain._obj_user_resist_nx[(int)num + (int)sIndex];
                sIndex -= (short)AppMain._obj_user_resist_num_nx[(int)index];
                num += (ushort)AppMain._obj_user_resist_num_nx[(int)index];
                if (sIndex > (short)0)
                    ;
            }
            else
                num += (ushort)AppMain._obj_user_resist_num_nx[(int)index];
        }
        return (AppMain.OBS_RECT_WORK)null;
    }

    private static void objRectCheckGroup(
      AppMain.ArrayPointer<AppMain.OBS_RECT_WORK> GroupA,
      AppMain.ArrayPointer<AppMain.OBS_RECT_WORK> GroupD,
      byte GroupNumA,
      byte GroupNumD,
      byte Index)
    {
        int lLeft1 = 0;
        int lTop1 = 0;
        int lLeft2 = 0;
        int lTop2 = 0;
        int lBack1 = 0;
        int lBack2 = 0;
        ushort usWidth1 = 0;
        ushort usHeight1 = 0;
        ushort usWidth2 = 0;
        ushort usHeight2 = 0;
        ushort usDepth1 = 0;
        ushort usDepth2 = 0;
        for (ushort index1 = 0; (int)index1 < (int)GroupNumA; ++index1)
        {
            AppMain.OBS_RECT_WORK obsRectWork1 = GroupA[(int)index1];
            if (obsRectWork1 != null && ((int)obsRectWork1.flag & 2048) == 0 && (((int)obsRectWork1.flag & 4) != 0 && ((int)obsRectWork1.target_g_flag & 1 << (int)Index) != 0) && (obsRectWork1.parent_obj == null || ((int)obsRectWork1.parent_obj.flag & 6) == 0))
            {
                AppMain.ObjRectLTBSet(obsRectWork1, ref lLeft1, ref lTop1, ref lBack1);
                AppMain.ObjRectWHDSet(obsRectWork1, ref usWidth1, ref usHeight1, ref usDepth1);
                for (ushort index2 = 0; (int)index2 < (int)GroupNumD; ++index2)
                {
                    AppMain.OBS_RECT_WORK obsRectWork2 = GroupD[(int)index2];
                    if (GroupA[(int)index1] != null)
                    {
                        if (obsRectWork2 != null && obsRectWork2 != obsRectWork1 && (obsRectWork2.parent_obj != obsRectWork1.parent_obj || obsRectWork2.parent_obj == null) && ((((int)obsRectWork2.flag | (int)obsRectWork1.flag) & 2048) == 0 && ((int)obsRectWork2.flag & 4) != 0 && (obsRectWork2.parent_obj == null || ((int)obsRectWork2.parent_obj.flag & 6) == 0)))
                        {
                            AppMain.ObjRectLTBSet(obsRectWork2, ref lLeft2, ref lTop2, ref lBack2);
                            AppMain.ObjRectWHDSet(obsRectWork2, ref usWidth2, ref usHeight2, ref usDepth2);
                            if ((((int)obsRectWork2.flag | (int)obsRectWork1.flag) & 524288) != 0 || AppMain.OBM_LINE_AND_LINE(lLeft1, (int)usWidth1, lLeft2, (int)usWidth2) && AppMain.OBM_LINE_AND_LINE(lTop1, (int)usHeight1, lTop2, (int)usHeight2))
                            {
                                ushort num = AppMain.objRectCheckFuncCall(obsRectWork1, obsRectWork2);
                                if (((int)num & 1) != 0)
                                {
                                    if (((int)obsRectWork1.flag & 65536) != 0)
                                    {
                                        obsRectWork1.flag |= 512U;
                                        obsRectWork1.flag &= 4294901759U;
                                    }
                                    GroupA.SetPrimitive((int)index1, (AppMain.OBS_RECT_WORK)null);
                                }
                                if (((int)num & 2) != 0)
                                {
                                    if (((int)obsRectWork2.flag & 65536) != 0)
                                    {
                                        obsRectWork2.flag |= 512U;
                                        obsRectWork2.flag &= 4294901759U;
                                    }
                                    GroupD.SetPrimitive((int)index2, (AppMain.OBS_RECT_WORK)null);
                                }
                            }
                        }
                    }
                    else
                        break;
                }
            }
        }
    }

    private static void ObjRectPosGet(AppMain.VecFx32 vPos, AppMain.OBS_RECT_WORK pRec)
    {
        if (pRec.parent_obj != null && ((int)pRec.flag & 4096) == 0)
        {
            vPos.x = pRec.parent_obj.pos.x + pRec.rect.pos.x;
            vPos.y = pRec.parent_obj.pos.y + pRec.rect.pos.y;
            vPos.z = pRec.parent_obj.pos.z + pRec.rect.pos.z;
        }
        else
        {
            vPos.x = pRec.rect.pos.x;
            vPos.y = pRec.rect.pos.y;
            vPos.z = pRec.rect.pos.z;
        }
    }

    private static void ObjRectLTBSet(
      AppMain.OBS_RECT_WORK pRec,
      ref int lLeft,
      ref int lTop,
      ref int lBack)
    {
        AppMain._ObjRectLTBSet(pRec, ref lLeft, ref lTop, ref lBack, true, true, true);
    }

    private static void ObjRectLTBSet(
      AppMain.OBS_RECT_WORK pRec,
      int? lLeft,
      ref int lTop,
      ref int lBack)
    {
        AppMain._ObjRectLTBSet(pRec, ref AppMain.mppIntNULL, ref lTop, ref lBack, false, true, true);
    }

    private static void ObjRectLTBSet(
      AppMain.OBS_RECT_WORK pRec,
      ref int lLeft,
      int? lTop,
      ref int lBack)
    {
        AppMain._ObjRectLTBSet(pRec, ref lLeft, ref AppMain.mppIntNULL, ref lBack, true, false, true);
    }

    private static void ObjRectLTBSet(
      AppMain.OBS_RECT_WORK pRec,
      int? lLeft,
      int? lTop,
      ref int lBack)
    {
        AppMain._ObjRectLTBSet(pRec, ref AppMain.mppIntNULL, ref AppMain.mppIntNULL, ref lBack, false, false, true);
    }

    private static void ObjRectLTBSet(
      AppMain.OBS_RECT_WORK pRec,
      ref int lLeft,
      ref int lTop,
      int? lBack)
    {
        AppMain._ObjRectLTBSet(pRec, ref lLeft, ref lTop, ref AppMain.mppIntNULL, true, true, false);
    }

    private static void ObjRectLTBSet(
      AppMain.OBS_RECT_WORK pRec,
      int? lLeft,
      ref int lTop,
      int? lBack)
    {
        AppMain._ObjRectLTBSet(pRec, ref AppMain.mppIntNULL, ref lTop, ref AppMain.mppIntNULL, false, true, false);
    }

    private static void ObjRectLTBSet(
      AppMain.OBS_RECT_WORK pRec,
      ref int lLeft,
      int? lTop,
      int? lBack)
    {
        AppMain._ObjRectLTBSet(pRec, ref lLeft, ref AppMain.mppIntNULL, ref AppMain.mppIntNULL, true, false, false);
    }

    private static void ObjRectLTBSet(AppMain.OBS_RECT_WORK pRec, int? lLeft, int? lTop, int? lBack)
    {
        AppMain._ObjRectLTBSet(pRec, ref AppMain.mppIntNULL, ref AppMain.mppIntNULL, ref AppMain.mppIntNULL, false, false, false);
    }

    private static void _ObjRectLTBSet(
      AppMain.OBS_RECT_WORK pRec,
      ref int lLeft,
      ref int lTop,
      ref int lBack,
      bool lLeftValid,
      bool lTopValid,
      bool lBackValid)
    {
        if (pRec.parent_obj != null && ((int)pRec.flag & 4096) == 0)
        {
            if (lLeftValid)
            {
                int v1 = ((int)pRec.parent_obj.disp_flag & 1 ^ (int)pRec.flag & 1) == 0 ? (int)pRec.rect.left : (int)-pRec.rect.right;
                if (pRec.parent_obj.scale.x != 4096)
                    v1 = AppMain.FX_Mul(v1, pRec.parent_obj.scale.x);
                if (AppMain._g_obj.draw_scale.x != 4096 && ((int)pRec.parent_obj.disp_flag & 1048576) == 0 && ((int)AppMain._g_obj.flag & 4194304) == 0)
                    v1 = AppMain.FX_Mul(v1, AppMain._g_obj.draw_scale.x);
                lLeft = (pRec.parent_obj.pos.x + pRec.rect.pos.x >> 12) + v1;
            }
            if (lTopValid)
            {
                int v1 = ((int)pRec.parent_obj.disp_flag & 2 ^ (int)pRec.flag & 2) == 0 ? (int)pRec.rect.top : (int)-pRec.rect.bottom;
                if (pRec.parent_obj.scale.y != 4096)
                    v1 = AppMain.FX_Mul(v1, pRec.parent_obj.scale.y);
                if (AppMain._g_obj.draw_scale.y != 4096 && ((int)pRec.parent_obj.disp_flag & 1048576) == 0 && ((int)AppMain._g_obj.flag & 4194304) == 0)
                    v1 = AppMain.FX_Mul(v1, AppMain._g_obj.draw_scale.y);
                lTop = (pRec.parent_obj.pos.y + pRec.rect.pos.y >> 12) + v1;
            }
            if (!lBackValid)
                return;
            int v1_1 = (int)pRec.rect.back;
            if (pRec.parent_obj.scale.z != 4096)
                v1_1 = AppMain.FX_Mul(v1_1, pRec.parent_obj.scale.z);
            if (AppMain._g_obj.draw_scale.z != 4096 && ((int)pRec.parent_obj.disp_flag & 1048576) == 0 && ((int)AppMain.g_obj.flag & 4194304) == 0)
                v1_1 = AppMain.FX_Mul(v1_1, AppMain.g_obj.draw_scale.z);
            lBack = (pRec.parent_obj.pos.z + pRec.rect.pos.z >> 12) + v1_1;
        }
        else
        {
            if (lLeftValid)
            {
                int num = ((int)pRec.flag & 1) == 0 ? (int)pRec.rect.left : (int)-pRec.rect.right;
                lLeft = (pRec.rect.pos.x >> 12) + num;
            }
            if (lTopValid)
            {
                int num = ((int)pRec.flag & 2) == 0 ? (int)pRec.rect.top : (int)-pRec.rect.bottom;
                lTop = (pRec.rect.pos.y >> 12) + num;
            }
            if (!lBackValid)
                return;
            lBack = (pRec.rect.pos.z >> 12) + (int)pRec.rect.back;
        }
    }

    private static void ObjRectWHDSet(
      AppMain.OBS_RECT_WORK pRec,
      ref ushort usWidth,
      ref ushort usHeight,
      ref ushort usDepth)
    {
        AppMain.ObjRectWHDSet(pRec, ref usWidth, ref usHeight, ref usDepth, true, true, true);
    }

    private static void ObjRectWHDSet(
      AppMain.OBS_RECT_WORK pRec,
      ushort? usWidth,
      ref ushort usHeight,
      ref ushort usDepth)
    {
        AppMain.ObjRectWHDSet(pRec, ref AppMain.mppUshortNULL, ref usHeight, ref usDepth, false, true, true);
    }

    private static void ObjRectWHDSet(
      AppMain.OBS_RECT_WORK pRec,
      ref ushort usWidth,
      ushort? usHeight,
      ref ushort usDepth)
    {
        AppMain.ObjRectWHDSet(pRec, ref usWidth, ref AppMain.mppUshortNULL, ref usDepth, true, false, true);
    }

    private static void ObjRectWHDSet(
      AppMain.OBS_RECT_WORK pRec,
      ushort? usWidth,
      ushort? usHeight,
      ref ushort usDepth)
    {
        AppMain.ObjRectWHDSet(pRec, ref AppMain.mppUshortNULL, ref AppMain.mppUshortNULL, ref usDepth, false, false, true);
    }

    private static void ObjRectWHDSet(
      AppMain.OBS_RECT_WORK pRec,
      ref ushort usWidth,
      ref ushort usHeight,
      ushort? usDepth)
    {
        AppMain.ObjRectWHDSet(pRec, ref usWidth, ref usHeight, ref AppMain.mppUshortNULL, true, true, false);
    }

    private static void ObjRectWHDSet(
      AppMain.OBS_RECT_WORK pRec,
      ushort? usWidth,
      ref ushort usHeight,
      ushort? usDepth)
    {
        AppMain.ObjRectWHDSet(pRec, ref AppMain.mppUshortNULL, ref usHeight, ref AppMain.mppUshortNULL, false, true, false);
    }

    private static void ObjRectWHDSet(
      AppMain.OBS_RECT_WORK pRec,
      ref ushort usWidth,
      ushort? usHeight,
      ushort? usDepth)
    {
        AppMain.ObjRectWHDSet(pRec, ref usWidth, ref AppMain.mppUshortNULL, ref AppMain.mppUshortNULL, true, false, false);
    }

    private static void ObjRectWHDSet(
      AppMain.OBS_RECT_WORK pRec,
      ushort? usWidth,
      ushort? usHeight,
      ushort? usDepth)
    {
        AppMain.ObjRectWHDSet(pRec, ref AppMain.mppUshortNULL, ref AppMain.mppUshortNULL, ref AppMain.mppUshortNULL, false, false, false);
    }

    private static void ObjRectWHDSet(
      AppMain.OBS_RECT_WORK pRec,
      ref ushort usWidth,
      ref ushort usHeight,
      ref ushort usDepth,
      bool usWidthValid,
      bool usHeightValid,
      bool usDepthValid)
    {
        if (pRec.parent_obj != null && ((int)pRec.flag & 4096) == 0)
        {
            if (usWidthValid)
            {
                usWidth = (ushort)((uint)pRec.rect.right - (uint)pRec.rect.left);
                int num = 4096;
                if (pRec.parent_obj.scale.x != 4096)
                    num = pRec.parent_obj.scale.x;
                if (AppMain._g_obj.draw_scale.x != 4096 && ((int)pRec.parent_obj.disp_flag & 1048576) == 0 && ((int)AppMain._g_obj.flag & 4194304) == 0)
                    num = AppMain.FX_Mul(num, AppMain._g_obj.draw_scale.x);
                if (num != 4096)
                    usWidth = (ushort)AppMain.FX_Mul((int)usWidth, num);
            }
            if (usHeightValid)
            {
                usHeight = (ushort)((uint)pRec.rect.bottom - (uint)pRec.rect.top);
                int num = 4096;
                if (pRec.parent_obj.scale.y != 4096)
                    num = pRec.parent_obj.scale.y;
                if (AppMain._g_obj.draw_scale.y != 4096 && ((int)pRec.parent_obj.disp_flag & 1048576) == 0 && ((int)AppMain._g_obj.flag & 4194304) == 0)
                    num = AppMain.FX_Mul(num, AppMain._g_obj.draw_scale.y);
                if (num != 4096)
                    usHeight = (ushort)AppMain.FX_Mul((int)usHeight, num);
            }
            if (!usDepthValid)
                return;
            usDepth = (ushort)((uint)pRec.rect.front - (uint)pRec.rect.back);
            int num1 = 4096;
            if (pRec.parent_obj.scale.z != 4096)
                num1 = pRec.parent_obj.scale.z;
            if (AppMain._g_obj.draw_scale.z != 4096 && ((int)pRec.parent_obj.disp_flag & 1048576) == 0 && ((int)AppMain._g_obj.flag & 4194304) == 0)
                num1 = AppMain.FX_Mul(num1, AppMain._g_obj.draw_scale.z);
            if (num1 == 4096)
                return;
            usDepth = (ushort)AppMain.FX_Mul((int)usDepth, num1);
        }
        else
        {
            if (usWidthValid)
                usWidth = (ushort)((uint)pRec.rect.right - (uint)pRec.rect.left);
            if (usHeightValid)
                usHeight = (ushort)((uint)pRec.rect.bottom - (uint)pRec.rect.top);
            if (!usDepthValid)
                return;
            usDepth = (ushort)((uint)pRec.rect.front - (uint)pRec.rect.back);
        }
    }

    private static ushort ObjRectFlagCheck(
      uint ulAtkFlag,
      uint usDefFlag,
      int lAtkPower,
      int lDefPower)
    {
        return ((int)ulAtkFlag & ~(int)usDefFlag) != 0 && lAtkPower >= lDefPower ? (ushort)1 : (ushort)0;
    }

    private static void ObjRectFuncblank(AppMain.OBS_RECT_WORK pObjA, AppMain.OBS_RECT_WORK pObjD)
    {
    }

    private static ushort objRectCheckFuncCall(
      AppMain.OBS_RECT_WORK pObjA,
      AppMain.OBS_RECT_WORK pObjD)
    {
        ushort num = 0;
        AppMain._obj_ulFlagBackA = pObjA.flag;
        AppMain._obj_ulFlagBackD = pObjD.flag;
        if (((int)pObjA.flag & 512) != 0 && ((int)pObjD.flag & 256) != 0 || AppMain.ObjRectFlagCheck((uint)pObjA.hit_flag, (uint)pObjD.def_flag, (int)pObjA.hit_power, (int)pObjD.def_power) == (ushort)0 || (pObjA.ppCheck != null && pObjA.ppCheck(pObjA, pObjD) == 0U || pObjD.ppCheck != null && pObjD.ppCheck(pObjD, pObjA) == 0U))
            return num;
        if (((int)pObjD.flag & 1088) == 0 && ((int)pObjA.flag & 1088) == 0)
            pObjA.flag |= 65536U;
        if (((int)pObjD.flag & 1152) == 0 && ((int)pObjA.flag & 1152) == 0)
            pObjD.flag |= 256U;
        if (((int)pObjA.flag & 1024) == 0 || ((int)pObjA.flag & 262144) == 0)
        {
            if (((int)pObjA.flag & 1024) != 0)
                pObjA.flag |= 262144U;
            if (pObjA.ppHit != null)
                pObjA.ppHit(pObjA, pObjD);
        }
        if (AppMain._obj_ucNoHit != (byte)0)
        {
            AppMain._obj_ucNoHit = (byte)0;
            return num;
        }
        if (((int)pObjA.flag & 1024) != 0)
            pObjA.flag |= 131072U;
        if (((int)pObjD.flag & 1024) == 0 || ((int)pObjD.flag & 262144) == 0)
        {
            if (((int)pObjD.flag & 1024) != 0)
                pObjD.flag |= 262144U;
            if (pObjD.ppDef != null)
                pObjD.ppDef(pObjD, pObjA);
        }
        if (AppMain._obj_ucNoHit != (byte)0)
        {
            AppMain._obj_ucNoHit = (byte)0;
            return num;
        }
        if (((int)pObjD.flag & 1024) != 0)
            pObjD.flag |= 131072U;
        if (((int)pObjA.flag & 32) == 0)
            num |= (ushort)1;
        if (((int)pObjD.flag & 32) == 0)
            num |= (ushort)2;
        return num;
    }

    private static void ObjRectFuncNoHit(AppMain.OBS_RECT_WORK pMine, AppMain.OBS_RECT_WORK pDamage)
    {
        pMine.flag = AppMain._obj_ulFlagBackA;
        pDamage.flag = AppMain._obj_ulFlagBackD;
        AppMain._obj_ucNoHit = (byte)1;
    }

    private static ushort ObjRectWorkCheck(AppMain.OBS_RECT_WORK pObj1, AppMain.OBS_RECT_WORK pObj2)
    {
        if (((int)pObj1.flag & 4) != 0 && ((int)pObj2.flag & 4) != 0 && (((int)pObj1.flag & 2048) == 0 && ((int)pObj2.flag & 2048) == 0))
        {
            int lLeft1 = 0;
            int lTop1 = 0;
            int lLeft2 = 0;
            int lTop2 = 0;
            ushort usWidth1 = 0;
            ushort usHeight1 = 0;
            ushort usWidth2 = 0;
            ushort usHeight2 = 0;
            int lBack1 = 0;
            int lBack2 = 0;
            ushort usDepth1 = 0;
            ushort usDepth2 = 0;
            if (pObj1.parent_obj != null && ((int)pObj1.parent_obj.flag & 6) != 0 || pObj2.parent_obj != null && ((int)pObj2.parent_obj.flag & 6) != 0)
                return 0;
            AppMain.ObjRectLTBSet(pObj1, ref lLeft1, ref lTop1, ref lBack1);
            AppMain.ObjRectLTBSet(pObj2, ref lLeft2, ref lTop2, ref lBack2);
            AppMain.ObjRectWHDSet(pObj1, ref usWidth1, ref usHeight1, ref usDepth1);
            AppMain.ObjRectWHDSet(pObj2, ref usWidth2, ref usHeight2, ref usDepth2);
            if (AppMain.OBM_LINE_AND_LINE(lLeft1, (int)usWidth1, lLeft2, (int)usWidth2) && AppMain.OBM_LINE_AND_LINE(lTop1, (int)usHeight1, lTop2, (int)usHeight2))
                return 1;
        }
        return 0;
    }

    private static ushort ObjRectCheck(AppMain.OBS_RECT pObj1, AppMain.OBS_RECT pObj2)
    {
        int x0_1 = (pObj1.pos.x >> 12) + (int)pObj1.left;
        int x0_2 = (pObj1.pos.y >> 12) + (int)pObj1.top;
        int z1 = pObj1.pos.z;
        int back1 = (int)pObj1.back;
        int x1_1 = (pObj2.pos.x >> 12) + (int)pObj2.left;
        int x1_2 = (pObj2.pos.y >> 12) + (int)pObj2.top;
        int z2 = pObj2.pos.z;
        int back2 = (int)pObj2.back;
        ushort num1 = (ushort)((uint)pObj1.right - (uint)pObj1.left);
        ushort num2 = (ushort)((uint)pObj1.bottom - (uint)pObj1.top);
        ushort num3 = (ushort)((uint)pObj2.right - (uint)pObj2.left);
        ushort num4 = (ushort)((uint)pObj2.bottom - (uint)pObj2.top);
        int front1 = (int)pObj1.front;
        int back3 = (int)pObj1.back;
        int front2 = (int)pObj1.front;
        int back4 = (int)pObj2.back;
        return AppMain.OBM_LINE_AND_LINE(x0_1, (int)num1, x1_1, (int)num3) && AppMain.OBM_LINE_AND_LINE(x0_2, (int)num2, x1_2, (int)num4) ? (ushort)1 : (ushort)0;
    }

    private static ushort ObjRectWorkPointCheck(AppMain.OBS_RECT_WORK pObj, int lX, int lY, int lZ)
    {
        if (((int)pObj.flag & 4) != 0 && ((int)pObj.flag & 2048) == 0)
        {
            int lLeft = 0;
            int lTop = 0;
            int lBack = 0;
            ushort usWidth = 0;
            ushort usHeight = 0;
            ushort usDepth = 0;
            AppMain.ObjRectLTBSet(pObj, ref lLeft, ref lTop, ref lBack);
            int x1_1 = lX;
            int x1_2 = lY;
            AppMain.ObjRectWHDSet(pObj, ref usWidth, ref usHeight, ref usDepth);
            if (AppMain.OBM_POINT_IN_LINE(lLeft, (int)usWidth, x1_1) && AppMain.OBM_POINT_IN_LINE(lTop, (int)usHeight, x1_2))
                return 1;
        }
        return 0;
    }

    private static ushort ObjRectPointCheck(AppMain.OBS_RECT pObj, int lX, int lY, int lZ)
    {
        int x0_1 = (int)pObj.left + (pObj.pos.x >> 12);
        int x0_2 = (int)pObj.top + (pObj.pos.y >> 12);
        int back1 = (int)pObj.back;
        int z = pObj.pos.z;
        int x1_1 = lX;
        int x1_2 = lY;
        ushort num1 = (ushort)((uint)pObj.right - (uint)pObj.left);
        ushort num2 = (ushort)((uint)pObj.bottom - (uint)pObj.top);
        int front = (int)pObj.front;
        int back2 = (int)pObj.back;
        return AppMain.OBM_POINT_IN_LINE(x0_1, (int)num1, x1_1) && AppMain.OBM_POINT_IN_LINE(x0_2, (int)num2, x1_2) ? (ushort)1 : (ushort)0;
    }

    private static int ObjRectCenterX(AppMain.OBS_RECT_WORK pWork)
    {
        int num = pWork.rect.pos.x + ((int)pWork.rect.left + (int)pWork.rect.right >> 1 << 12);
        if (pWork.parent_obj != null)
            num += pWork.parent_obj.pos.x;
        return num;
    }

    private static int ObjRectCenterY(AppMain.OBS_RECT_WORK pWork)
    {
        int num = pWork.rect.pos.y + ((int)pWork.rect.top + (int)pWork.rect.bottom >> 1 << 12);
        if (pWork.parent_obj != null)
            num += pWork.parent_obj.pos.y;
        return num;
    }

    private static int ObjRectCenterZ(AppMain.OBS_RECT_WORK pWork)
    {
        int num = pWork.rect.pos.z + ((int)pWork.rect.back + (int)pWork.rect.front >> 1 << 12);
        if (pWork.parent_obj != null)
            num += pWork.parent_obj.pos.z;
        return num;
    }

    private static int ObjRectHitCenterX(AppMain.OBS_RECT_WORK pWork, AppMain.OBS_RECT_WORK pAttacker)
    {
        int num1 = 0;
        int num2 = 0;
        int[] numArray = new int[4];
        ushort usWidth = 0;
        byte num3 = 0;
        byte num4 = 0;
        AppMain.ObjRectLTBSet(pWork, ref numArray[0], new int?(), new int?());
        AppMain.ObjRectWHDSet(pWork, ref usWidth, new ushort?(), new ushort?());
        numArray[1] = numArray[0] + (int)usWidth;
        AppMain.ObjRectLTBSet(pAttacker, ref numArray[2], new int?(), new int?());
        AppMain.ObjRectWHDSet(pAttacker, ref usWidth, new ushort?(), new ushort?());
        numArray[3] = numArray[2] + (int)usWidth;
        int num5 = numArray[(int)num3];
        byte num6 = num3;
        byte num7 = (byte)((uint)num3 + 1U);
        if (numArray[(int)num7] > num5)
        {
            num5 = numArray[(int)num7];
            num6 = num7;
        }
        byte num8 = (byte)((uint)num7 + 1U);
        if (numArray[(int)num8] > num5)
        {
            num5 = numArray[(int)num8];
            num6 = num8;
        }
        byte num9 = (byte)((uint)num8 + 1U);
        if (numArray[(int)num9] > num5)
        {
            num1 = numArray[(int)num9];
            num6 = num9;
        }
        byte num10 = (byte)((uint)num9 + 1U);
        byte num11 = 0;
        int num12 = numArray[(int)num11];
        byte num13 = num11;
        byte num14 = (byte)((uint)num11 + 1U);
        if (numArray[(int)num14] < num12)
        {
            num12 = numArray[(int)num14];
            num13 = num14;
        }
        byte num15 = (byte)((uint)num14 + 1U);
        if (numArray[(int)num15] < num12)
        {
            num12 = numArray[(int)num15];
            num13 = num15;
        }
        byte num16 = (byte)((uint)num15 + 1U);
        if (numArray[(int)num16] < num12)
        {
            num2 = numArray[(int)num16];
            num13 = num16;
        }
        num10 = (byte)((uint)num16 + 1U);
        byte num17 = 0;
        while (true)
        {
            if ((int)num17 != (int)num6 && (int)num17 != (int)num13)
            {
                numArray[(int)num4] = numArray[(int)num17];
                if (num4 == (byte)0)
                    ++num4;
                else
                    break;
            }
            ++num17;
        }
        int num18 = Math.Abs(numArray[0] - numArray[1] >> 1);
        return (numArray[0] <= numArray[1] ? num18 + numArray[0] : num18 + numArray[1]) << 12;
    }

    private static int ObjRectHitCenterY(AppMain.OBS_RECT_WORK pWork, AppMain.OBS_RECT_WORK pAttacker)
    {
        int num1 = 0;
        int num2 = 0;
        int[] numArray = new int[4];
        ushort usHeight = 0;
        byte num3 = 0;
        byte num4 = 0;
        AppMain.ObjRectLTBSet(pWork, new int?(), ref numArray[0], new int?());
        AppMain.ObjRectWHDSet(pWork, new ushort?(), ref usHeight, new ushort?());
        numArray[1] = numArray[0] + (int)usHeight;
        AppMain.ObjRectLTBSet(pAttacker, new int?(), ref numArray[2], new int?());
        AppMain.ObjRectWHDSet(pAttacker, new ushort?(), ref usHeight, new ushort?());
        numArray[3] = numArray[2] + (int)usHeight;
        int num5 = numArray[(int)num3];
        byte num6 = num3;
        byte num7 = (byte)((uint)num3 + 1U);
        if (numArray[(int)num7] > num5)
        {
            num5 = numArray[(int)num7];
            num6 = num7;
        }
        byte num8 = (byte)((uint)num7 + 1U);
        if (numArray[(int)num8] > num5)
        {
            num5 = numArray[(int)num8];
            num6 = num8;
        }
        byte num9 = (byte)((uint)num8 + 1U);
        if (numArray[(int)num9] > num5)
        {
            num1 = numArray[(int)num9];
            num6 = num9;
        }
        byte num10 = (byte)((uint)num9 + 1U);
        byte num11 = 0;
        int num12 = numArray[(int)num11];
        byte num13 = num11;
        byte num14 = (byte)((uint)num11 + 1U);
        if (numArray[(int)num14] < num12)
        {
            num12 = numArray[(int)num14];
            num13 = num14;
        }
        byte num15 = (byte)((uint)num14 + 1U);
        if (numArray[(int)num15] < num12)
        {
            num12 = numArray[(int)num15];
            num13 = num15;
        }
        byte num16 = (byte)((uint)num15 + 1U);
        if (numArray[(int)num16] < num12)
        {
            num2 = numArray[(int)num16];
            num13 = num16;
        }
        num10 = (byte)((uint)num16 + 1U);
        byte num17 = 0;
        while (true)
        {
            if ((int)num17 != (int)num6 && (int)num17 != (int)num13)
            {
                numArray[(int)num4] = numArray[(int)num17];
                if (num4 == (byte)0)
                    ++num4;
                else
                    break;
            }
            ++num17;
        }
        int num18 = Math.Abs(numArray[0] - numArray[1] >> 1);
        return (numArray[0] <= numArray[1] ? num18 + numArray[0] : num18 + numArray[1]) << 12;
    }

    private static void ObjDebugRectActionInit()
    {
    }

}