using System;

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

    public static OBS_RECT ObjRectSet(
      OBS_RECT pRec,
      short cLeft,
      short cTop,
      short cRight,
      short cBottom)
    {
        pRec.left = cLeft;
        pRec.top = cTop;
        pRec.right = cRight;
        pRec.bottom = cBottom;
        pRec.back = -16;
        pRec.front = 16;
        if (pRec.right < pRec.left)
            MTM_MATH_SWAP(ref pRec.left, ref pRec.right);
        if (pRec.bottom < pRec.top)
            MTM_MATH_SWAP(ref pRec.top, ref pRec.bottom);
        VEC_Set(ref pRec.pos, 0, 0, 0);
        return pRec;
    }

    public static OBS_RECT ObjRectZSet(
      OBS_RECT pRec,
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
        if (pRec.right < pRec.left)
            MTM_MATH_SWAP(ref pRec.left, ref pRec.right);
        if (pRec.bottom < pRec.top)
            MTM_MATH_SWAP(ref pRec.top, ref pRec.bottom);
        if (pRec.front < pRec.back)
            MTM_MATH_SWAP(ref pRec.back, ref pRec.front);
        VEC_Set(ref pRec.pos, 0, 0, 0);
        return pRec;
    }

    public static OBS_RECT ObjRectAllSet(
      OBS_RECT pRec,
      VecFx32 pos,
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
        if (pRec.right < pRec.left)
            MTM_MATH_SWAP(ref pRec.left, ref pRec.right);
        if (pRec.bottom < pRec.top)
            MTM_MATH_SWAP(ref pRec.top, ref pRec.bottom);
        if (pRec.front < pRec.back)
            MTM_MATH_SWAP(ref pRec.back, ref pRec.front);
        return pRec;
    }

    public static OBS_RECT ObjRectWorkSet(
      OBS_RECT_WORK pRec,
      short cLeft,
      short cTop,
      short cRight,
      short cBottom)
    {
        pRec.flag |= 4U;
        return ObjRectSet(pRec.rect, cLeft, cTop, cRight, cBottom);
    }

    public static OBS_RECT ObjRectWorkZSet(
      OBS_RECT_WORK pRec,
      short cLeft,
      short cTop,
      short cBack,
      short cRight,
      short cBottom,
      short cFront)
    {
        pRec.flag |= 4U;
        return ObjRectZSet(pRec.rect, cLeft, cTop, cBack, cRight, cBottom, cFront);
    }

    public static OBS_RECT ObjRectWorkAllSet(
      OBS_RECT_WORK pRec,
      VecFx32 pos,
      short cLeft,
      short cTop,
      short cBack,
      short cRight,
      short cBottom,
      short cFront)
    {
        pRec.flag |= 4U;
        return ObjRectAllSet(pRec.rect, pos, cLeft, cTop, cBack, cRight, cBottom, cFront);
    }

    public static void ObjRectGroupSet(OBS_RECT_WORK pRec, byte group_no, byte target_g_flag)
    {
        pRec.group_no = group_no;
        pRec.target_g_flag = target_g_flag;
    }

    public static void ObjRectAtkSet(OBS_RECT_WORK pRec, ushort usHitFlag, short sHitPower)
    {
        pRec.flag |= 4U;
        pRec.hit_flag = usHitFlag;
        pRec.hit_power = sHitPower;
        pRec.flag &= 4294966783U;
        pRec.flag &= 4294901759U;
    }

    public static void ObjRectDefSet(OBS_RECT_WORK pRec, ushort usDefFlag, short sDefPower)
    {
        pRec.flag |= 4U;
        pRec.def_flag = usDefFlag;
        pRec.def_power = sDefPower;
        pRec.flag &= 4294967039U;
    }

    public static void ObjRectHitAgain(OBS_RECT_WORK pRec)
    {
        if (((int)pRec.flag & 1024) != 0)
            return;
        pRec.flag &= 4294967039U;
        pRec.flag &= 4294966783U;
    }

    public static void ObjRectCheckInit()
    {
        Array.Clear(_obj_user_resist_nx, 0, _obj_user_resist_nx.Length);
        Array.Clear(_obj_user_resist, 0, _obj_user_resist.Length);
        Array.Clear(_obj_user_resist_num, 0, _obj_user_resist_num.Length);
        Array.Clear(_obj_user_resist_num_nx, 0, _obj_user_resist_num_nx.Length);
        Array.Clear(_obj_user_flag, 0, _obj_user_flag.Length);
        Array.Clear(_obj_user_flag_nx, 0, _obj_user_flag_nx.Length);
        _obj_user_resist_all_num = 0;
        _obj_user_resist_all_num_nx = 0;
        _obj_ulFlagBackA = 0U;
        _obj_ulFlagBackD = 0U;
        _obj_ucNoHit = 0;
    }

    private static void objRectCheckOut()
    {
        Array.Copy(_obj_user_resist_nx, _obj_user_resist, _obj_user_resist.Length);
        Array.Clear(_obj_user_resist_nx, 0, _obj_user_resist_nx.Length);
        Array.Copy(_obj_user_resist_num_nx, _obj_user_resist_num, _obj_user_resist_num.Length);
        Array.Clear(_obj_user_resist_num_nx, 0, _obj_user_resist_num_nx.Length);
        Array.Copy(_obj_user_flag_nx, _obj_user_flag, _obj_user_flag.Length);
        Array.Clear(_obj_user_flag_nx, 0, _obj_user_flag_nx.Length);
        _obj_user_resist_all_num = _obj_user_resist_all_num_nx;
        _obj_user_resist_all_num_nx = 0;
        for (ushort index1 = 0; index1 < _obj_user_resist_all_num - 1; ++index1)
        {
            for (ushort index2 = (ushort)(_obj_user_resist_all_num - 1U); index2 > index1; --index2)
            {
                if (_obj_user_resist[index2].group_no < _obj_user_resist[index2 - 1].group_no)
                {
                    OBS_RECT_WORK obsRectWork = _obj_user_resist[index2 - 1];
                    _obj_user_resist[index2 - 1] = _obj_user_resist[index2];
                    _obj_user_resist[index2] = obsRectWork;
                }
            }
        }
    }

    private static void ObjRectRegist(OBS_RECT_WORK pObj)
    {
        if (((int)pObj.flag & 4) == 0 || pObj.group_no >= 8 || _obj_user_resist_all_num_nx >= 80)
            return;
        _obj_user_resist_nx[_obj_user_resist_all_num_nx] = pObj;
        _obj_user_flag_nx[pObj.group_no] |= pObj.target_g_flag;
        ++_obj_user_resist_num_nx[pObj.group_no];
        ++_obj_user_resist_all_num_nx;
    }

    private static void ObjRectCheckAllGroup()
    {
        if (((int)g_obj.flag & 131072) != 0)
            objRectCheckOut();
        _obj_ulFlagBackA = 0U;
        _obj_ulFlagBackD = 0U;
        _obj_ucNoHit = 0;
        for (ushort index = 0; index < _obj_user_resist_all_num; ++index)
        {
            if (_obj_user_resist[index] != null && ((int)_obj_user_resist[index].flag & 1024) != 0)
                _obj_user_resist[index].flag &= 4294836223U;
        }
        ushort num1 = 0;
        for (ushort index = 0; index < 8; ++index)
        {
            ushort num2 = 0;
            for (byte Index = 0; Index < 8; ++Index)
            {
                if (_obj_user_resist_num[Index] != 0 && (_obj_user_flag[index] & 1 << Index) != 0)
                    objRectCheckGroup(new ArrayPointer<OBS_RECT_WORK>(_obj_user_resist, num1), new ArrayPointer<OBS_RECT_WORK>(_obj_user_resist, num2), _obj_user_resist_num[index], _obj_user_resist_num[Index], Index);
                num2 += _obj_user_resist_num[Index];
            }
            num1 += _obj_user_resist_num[index];
        }
        for (ushort index = 0; index < _obj_user_resist_all_num; ++index)
        {
            if (_obj_user_resist[index] != null)
            {
                if (((int)_obj_user_resist[index].flag & 65536) != 0)
                {
                    _obj_user_resist[index].flag |= 512U;
                    _obj_user_resist[index].flag &= 4294901759U;
                }
                if (((int)_obj_user_resist[index].flag & 1024) != 0 && ((int)_obj_user_resist[index].flag & 131072) == 0)
                    _obj_user_resist[index].flag &= 4294705151U;
            }
        }
        if (((int)g_obj.flag & 131072) != 0)
            return;
        objRectCheckOut();
    }

    private static OBS_RECT_WORK ObjRectRegistGet(byte ucGroup, short sIndex)
    {
        ushort num = 0;
        for (short index = 0; index < 8; ++index)
        {
            if ((ucGroup & 1 << index) != 0)
            {
                if (sIndex < _obj_user_resist_num[index])
                    return _obj_user_resist[num + sIndex];
                sIndex -= _obj_user_resist_num[index];
                num += _obj_user_resist_num[index];
                if (sIndex > 0)
                    ;
            }
            else
                num += _obj_user_resist_num[index];
        }
        return null;
    }

    private static OBS_RECT_WORK ObjRectRegistNxGet(byte ucGroup, short sIndex)
    {
        ushort num = 0;
        for (short index = 0; index < 8; ++index)
        {
            if ((ucGroup & 1 << index) != 0)
            {
                if (sIndex < _obj_user_resist_num_nx[index])
                    return _obj_user_resist_nx[num + sIndex];
                sIndex -= _obj_user_resist_num_nx[index];
                num += _obj_user_resist_num_nx[index];
                if (sIndex > 0)
                    ;
            }
            else
                num += _obj_user_resist_num_nx[index];
        }
        return null;
    }

    private static void objRectCheckGroup(
      ArrayPointer<OBS_RECT_WORK> GroupA,
      ArrayPointer<OBS_RECT_WORK> GroupD,
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
        for (ushort index1 = 0; index1 < GroupNumA; ++index1)
        {
            OBS_RECT_WORK obsRectWork1 = GroupA[index1];
            if (obsRectWork1 != null && ((int)obsRectWork1.flag & 2048) == 0 && (((int)obsRectWork1.flag & 4) != 0 && (obsRectWork1.target_g_flag & 1 << Index) != 0) && (obsRectWork1.parent_obj == null || ((int)obsRectWork1.parent_obj.flag & 6) == 0))
            {
                ObjRectLTBSet(obsRectWork1, ref lLeft1, ref lTop1, ref lBack1);
                ObjRectWHDSet(obsRectWork1, ref usWidth1, ref usHeight1, ref usDepth1);
                for (ushort index2 = 0; index2 < GroupNumD; ++index2)
                {
                    OBS_RECT_WORK obsRectWork2 = GroupD[index2];
                    if (GroupA[index1] != null)
                    {
                        if (obsRectWork2 != null && obsRectWork2 != obsRectWork1 && (obsRectWork2.parent_obj != obsRectWork1.parent_obj || obsRectWork2.parent_obj == null) && ((((int)obsRectWork2.flag | (int)obsRectWork1.flag) & 2048) == 0 && ((int)obsRectWork2.flag & 4) != 0 && (obsRectWork2.parent_obj == null || ((int)obsRectWork2.parent_obj.flag & 6) == 0)))
                        {
                            ObjRectLTBSet(obsRectWork2, ref lLeft2, ref lTop2, ref lBack2);
                            ObjRectWHDSet(obsRectWork2, ref usWidth2, ref usHeight2, ref usDepth2);
                            if ((((int)obsRectWork2.flag | (int)obsRectWork1.flag) & 524288) != 0 || OBM_LINE_AND_LINE(lLeft1, usWidth1, lLeft2, usWidth2) && OBM_LINE_AND_LINE(lTop1, usHeight1, lTop2, usHeight2))
                            {
                                ushort num = objRectCheckFuncCall(obsRectWork1, obsRectWork2);
                                if ((num & 1) != 0)
                                {
                                    if (((int)obsRectWork1.flag & 65536) != 0)
                                    {
                                        obsRectWork1.flag |= 512U;
                                        obsRectWork1.flag &= 4294901759U;
                                    }
                                    GroupA.SetPrimitive(index1, null);
                                }
                                if ((num & 2) != 0)
                                {
                                    if (((int)obsRectWork2.flag & 65536) != 0)
                                    {
                                        obsRectWork2.flag |= 512U;
                                        obsRectWork2.flag &= 4294901759U;
                                    }
                                    GroupD.SetPrimitive(index2, null);
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

    private static void ObjRectPosGet(VecFx32 vPos, OBS_RECT_WORK pRec)
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
      OBS_RECT_WORK pRec,
      ref int lLeft,
      ref int lTop,
      ref int lBack)
    {
        _ObjRectLTBSet(pRec, ref lLeft, ref lTop, ref lBack, true, true, true);
    }

    private static void ObjRectLTBSet(
      OBS_RECT_WORK pRec,
      int? lLeft,
      ref int lTop,
      ref int lBack)
    {
        _ObjRectLTBSet(pRec, ref mppIntNULL, ref lTop, ref lBack, false, true, true);
    }

    private static void ObjRectLTBSet(
      OBS_RECT_WORK pRec,
      ref int lLeft,
      int? lTop,
      ref int lBack)
    {
        _ObjRectLTBSet(pRec, ref lLeft, ref mppIntNULL, ref lBack, true, false, true);
    }

    private static void ObjRectLTBSet(
      OBS_RECT_WORK pRec,
      int? lLeft,
      int? lTop,
      ref int lBack)
    {
        _ObjRectLTBSet(pRec, ref mppIntNULL, ref mppIntNULL, ref lBack, false, false, true);
    }

    private static void ObjRectLTBSet(
      OBS_RECT_WORK pRec,
      ref int lLeft,
      ref int lTop,
      int? lBack)
    {
        _ObjRectLTBSet(pRec, ref lLeft, ref lTop, ref mppIntNULL, true, true, false);
    }

    private static void ObjRectLTBSet(
      OBS_RECT_WORK pRec,
      int? lLeft,
      ref int lTop,
      int? lBack)
    {
        _ObjRectLTBSet(pRec, ref mppIntNULL, ref lTop, ref mppIntNULL, false, true, false);
    }

    private static void ObjRectLTBSet(
      OBS_RECT_WORK pRec,
      ref int lLeft,
      int? lTop,
      int? lBack)
    {
        _ObjRectLTBSet(pRec, ref lLeft, ref mppIntNULL, ref mppIntNULL, true, false, false);
    }

    private static void ObjRectLTBSet(OBS_RECT_WORK pRec, int? lLeft, int? lTop, int? lBack)
    {
        _ObjRectLTBSet(pRec, ref mppIntNULL, ref mppIntNULL, ref mppIntNULL, false, false, false);
    }

    private static void _ObjRectLTBSet(
      OBS_RECT_WORK pRec,
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
                int v1 = ((int)pRec.parent_obj.disp_flag & 1 ^ (int)pRec.flag & 1) == 0 ? pRec.rect.left : -pRec.rect.right;
                if (pRec.parent_obj.scale.x != 4096)
                    v1 = FX_Mul(v1, pRec.parent_obj.scale.x);
                if (_g_obj.draw_scale.x != 4096 && ((int)pRec.parent_obj.disp_flag & 1048576) == 0 && ((int)_g_obj.flag & 4194304) == 0)
                    v1 = FX_Mul(v1, _g_obj.draw_scale.x);
                lLeft = (pRec.parent_obj.pos.x + pRec.rect.pos.x >> 12) + v1;
            }
            if (lTopValid)
            {
                int v1 = ((int)pRec.parent_obj.disp_flag & 2 ^ (int)pRec.flag & 2) == 0 ? pRec.rect.top : -pRec.rect.bottom;
                if (pRec.parent_obj.scale.y != 4096)
                    v1 = FX_Mul(v1, pRec.parent_obj.scale.y);
                if (_g_obj.draw_scale.y != 4096 && ((int)pRec.parent_obj.disp_flag & 1048576) == 0 && ((int)_g_obj.flag & 4194304) == 0)
                    v1 = FX_Mul(v1, _g_obj.draw_scale.y);
                lTop = (pRec.parent_obj.pos.y + pRec.rect.pos.y >> 12) + v1;
            }
            if (!lBackValid)
                return;
            int v1_1 = pRec.rect.back;
            if (pRec.parent_obj.scale.z != 4096)
                v1_1 = FX_Mul(v1_1, pRec.parent_obj.scale.z);
            if (_g_obj.draw_scale.z != 4096 && ((int)pRec.parent_obj.disp_flag & 1048576) == 0 && ((int)g_obj.flag & 4194304) == 0)
                v1_1 = FX_Mul(v1_1, g_obj.draw_scale.z);
            lBack = (pRec.parent_obj.pos.z + pRec.rect.pos.z >> 12) + v1_1;
        }
        else
        {
            if (lLeftValid)
            {
                int num = ((int)pRec.flag & 1) == 0 ? pRec.rect.left : -pRec.rect.right;
                lLeft = (pRec.rect.pos.x >> 12) + num;
            }
            if (lTopValid)
            {
                int num = ((int)pRec.flag & 2) == 0 ? pRec.rect.top : -pRec.rect.bottom;
                lTop = (pRec.rect.pos.y >> 12) + num;
            }
            if (!lBackValid)
                return;
            lBack = (pRec.rect.pos.z >> 12) + pRec.rect.back;
        }
    }

    private static void ObjRectWHDSet(
      OBS_RECT_WORK pRec,
      ref ushort usWidth,
      ref ushort usHeight,
      ref ushort usDepth)
    {
        ObjRectWHDSet(pRec, ref usWidth, ref usHeight, ref usDepth, true, true, true);
    }

    private static void ObjRectWHDSet(
      OBS_RECT_WORK pRec,
      ushort? usWidth,
      ref ushort usHeight,
      ref ushort usDepth)
    {
        ObjRectWHDSet(pRec, ref mppUshortNULL, ref usHeight, ref usDepth, false, true, true);
    }

    private static void ObjRectWHDSet(
      OBS_RECT_WORK pRec,
      ref ushort usWidth,
      ushort? usHeight,
      ref ushort usDepth)
    {
        ObjRectWHDSet(pRec, ref usWidth, ref mppUshortNULL, ref usDepth, true, false, true);
    }

    private static void ObjRectWHDSet(
      OBS_RECT_WORK pRec,
      ushort? usWidth,
      ushort? usHeight,
      ref ushort usDepth)
    {
        ObjRectWHDSet(pRec, ref mppUshortNULL, ref mppUshortNULL, ref usDepth, false, false, true);
    }

    private static void ObjRectWHDSet(
      OBS_RECT_WORK pRec,
      ref ushort usWidth,
      ref ushort usHeight,
      ushort? usDepth)
    {
        ObjRectWHDSet(pRec, ref usWidth, ref usHeight, ref mppUshortNULL, true, true, false);
    }

    private static void ObjRectWHDSet(
      OBS_RECT_WORK pRec,
      ushort? usWidth,
      ref ushort usHeight,
      ushort? usDepth)
    {
        ObjRectWHDSet(pRec, ref mppUshortNULL, ref usHeight, ref mppUshortNULL, false, true, false);
    }

    private static void ObjRectWHDSet(
      OBS_RECT_WORK pRec,
      ref ushort usWidth,
      ushort? usHeight,
      ushort? usDepth)
    {
        ObjRectWHDSet(pRec, ref usWidth, ref mppUshortNULL, ref mppUshortNULL, true, false, false);
    }

    private static void ObjRectWHDSet(
      OBS_RECT_WORK pRec,
      ushort? usWidth,
      ushort? usHeight,
      ushort? usDepth)
    {
        ObjRectWHDSet(pRec, ref mppUshortNULL, ref mppUshortNULL, ref mppUshortNULL, false, false, false);
    }

    private static void ObjRectWHDSet(
      OBS_RECT_WORK pRec,
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
                if (_g_obj.draw_scale.x != 4096 && ((int)pRec.parent_obj.disp_flag & 1048576) == 0 && ((int)_g_obj.flag & 4194304) == 0)
                    num = FX_Mul(num, _g_obj.draw_scale.x);
                if (num != 4096)
                    usWidth = (ushort)FX_Mul(usWidth, num);
            }
            if (usHeightValid)
            {
                usHeight = (ushort)((uint)pRec.rect.bottom - (uint)pRec.rect.top);
                int num = 4096;
                if (pRec.parent_obj.scale.y != 4096)
                    num = pRec.parent_obj.scale.y;
                if (_g_obj.draw_scale.y != 4096 && ((int)pRec.parent_obj.disp_flag & 1048576) == 0 && ((int)_g_obj.flag & 4194304) == 0)
                    num = FX_Mul(num, _g_obj.draw_scale.y);
                if (num != 4096)
                    usHeight = (ushort)FX_Mul(usHeight, num);
            }
            if (!usDepthValid)
                return;
            usDepth = (ushort)((uint)pRec.rect.front - (uint)pRec.rect.back);
            int num1 = 4096;
            if (pRec.parent_obj.scale.z != 4096)
                num1 = pRec.parent_obj.scale.z;
            if (_g_obj.draw_scale.z != 4096 && ((int)pRec.parent_obj.disp_flag & 1048576) == 0 && ((int)_g_obj.flag & 4194304) == 0)
                num1 = FX_Mul(num1, _g_obj.draw_scale.z);
            if (num1 == 4096)
                return;
            usDepth = (ushort)FX_Mul(usDepth, num1);
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

    private static void ObjRectFuncblank(OBS_RECT_WORK pObjA, OBS_RECT_WORK pObjD)
    {
    }

    private static ushort objRectCheckFuncCall(
      OBS_RECT_WORK pObjA,
      OBS_RECT_WORK pObjD)
    {
        ushort num = 0;
        _obj_ulFlagBackA = pObjA.flag;
        _obj_ulFlagBackD = pObjD.flag;
        if (((int)pObjA.flag & 512) != 0 && ((int)pObjD.flag & 256) != 0 || ObjRectFlagCheck(pObjA.hit_flag, pObjD.def_flag, pObjA.hit_power, pObjD.def_power) == 0 || (pObjA.ppCheck != null && pObjA.ppCheck(pObjA, pObjD) == 0U || pObjD.ppCheck != null && pObjD.ppCheck(pObjD, pObjA) == 0U))
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
        if (_obj_ucNoHit != 0)
        {
            _obj_ucNoHit = 0;
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
        if (_obj_ucNoHit != 0)
        {
            _obj_ucNoHit = 0;
            return num;
        }
        if (((int)pObjD.flag & 1024) != 0)
            pObjD.flag |= 131072U;
        if (((int)pObjA.flag & 32) == 0)
            num |= 1;
        if (((int)pObjD.flag & 32) == 0)
            num |= 2;
        return num;
    }

    private static void ObjRectFuncNoHit(OBS_RECT_WORK pMine, OBS_RECT_WORK pDamage)
    {
        pMine.flag = _obj_ulFlagBackA;
        pDamage.flag = _obj_ulFlagBackD;
        _obj_ucNoHit = 1;
    }

    private static ushort ObjRectWorkCheck(OBS_RECT_WORK pObj1, OBS_RECT_WORK pObj2)
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
            ObjRectLTBSet(pObj1, ref lLeft1, ref lTop1, ref lBack1);
            ObjRectLTBSet(pObj2, ref lLeft2, ref lTop2, ref lBack2);
            ObjRectWHDSet(pObj1, ref usWidth1, ref usHeight1, ref usDepth1);
            ObjRectWHDSet(pObj2, ref usWidth2, ref usHeight2, ref usDepth2);
            if (OBM_LINE_AND_LINE(lLeft1, usWidth1, lLeft2, usWidth2) && OBM_LINE_AND_LINE(lTop1, usHeight1, lTop2, usHeight2))
                return 1;
        }
        return 0;
    }

    private static ushort ObjRectCheck(OBS_RECT pObj1, OBS_RECT pObj2)
    {
        int x0_1 = (pObj1.pos.x >> 12) + pObj1.left;
        int x0_2 = (pObj1.pos.y >> 12) + pObj1.top;
        int z1 = pObj1.pos.z;
        int back1 = pObj1.back;
        int x1_1 = (pObj2.pos.x >> 12) + pObj2.left;
        int x1_2 = (pObj2.pos.y >> 12) + pObj2.top;
        int z2 = pObj2.pos.z;
        int back2 = pObj2.back;
        ushort num1 = (ushort)((uint)pObj1.right - (uint)pObj1.left);
        ushort num2 = (ushort)((uint)pObj1.bottom - (uint)pObj1.top);
        ushort num3 = (ushort)((uint)pObj2.right - (uint)pObj2.left);
        ushort num4 = (ushort)((uint)pObj2.bottom - (uint)pObj2.top);
        int front1 = pObj1.front;
        int back3 = pObj1.back;
        int front2 = pObj1.front;
        int back4 = pObj2.back;
        return OBM_LINE_AND_LINE(x0_1, num1, x1_1, num3) && OBM_LINE_AND_LINE(x0_2, num2, x1_2, num4) ? (ushort)1 : (ushort)0;
    }

    private static ushort ObjRectWorkPointCheck(OBS_RECT_WORK pObj, int lX, int lY, int lZ)
    {
        if (((int)pObj.flag & 4) != 0 && ((int)pObj.flag & 2048) == 0)
        {
            int lLeft = 0;
            int lTop = 0;
            int lBack = 0;
            ushort usWidth = 0;
            ushort usHeight = 0;
            ushort usDepth = 0;
            ObjRectLTBSet(pObj, ref lLeft, ref lTop, ref lBack);
            int x1_1 = lX;
            int x1_2 = lY;
            ObjRectWHDSet(pObj, ref usWidth, ref usHeight, ref usDepth);
            if (OBM_POINT_IN_LINE(lLeft, usWidth, x1_1) && OBM_POINT_IN_LINE(lTop, usHeight, x1_2))
                return 1;
        }
        return 0;
    }

    private static ushort ObjRectPointCheck(OBS_RECT pObj, int lX, int lY, int lZ)
    {
        int x0_1 = pObj.left + (pObj.pos.x >> 12);
        int x0_2 = pObj.top + (pObj.pos.y >> 12);
        int back1 = pObj.back;
        int z = pObj.pos.z;
        int x1_1 = lX;
        int x1_2 = lY;
        ushort num1 = (ushort)((uint)pObj.right - (uint)pObj.left);
        ushort num2 = (ushort)((uint)pObj.bottom - (uint)pObj.top);
        int front = pObj.front;
        int back2 = pObj.back;
        return OBM_POINT_IN_LINE(x0_1, num1, x1_1) && OBM_POINT_IN_LINE(x0_2, num2, x1_2) ? (ushort)1 : (ushort)0;
    }

    private static int ObjRectCenterX(OBS_RECT_WORK pWork)
    {
        int num = pWork.rect.pos.x + (pWork.rect.left + pWork.rect.right >> 1 << 12);
        if (pWork.parent_obj != null)
            num += pWork.parent_obj.pos.x;
        return num;
    }

    private static int ObjRectCenterY(OBS_RECT_WORK pWork)
    {
        int num = pWork.rect.pos.y + (pWork.rect.top + pWork.rect.bottom >> 1 << 12);
        if (pWork.parent_obj != null)
            num += pWork.parent_obj.pos.y;
        return num;
    }

    private static int ObjRectCenterZ(OBS_RECT_WORK pWork)
    {
        int num = pWork.rect.pos.z + (pWork.rect.back + pWork.rect.front >> 1 << 12);
        if (pWork.parent_obj != null)
            num += pWork.parent_obj.pos.z;
        return num;
    }

    private static int ObjRectHitCenterX(OBS_RECT_WORK pWork, OBS_RECT_WORK pAttacker)
    {
        int num1 = 0;
        int num2 = 0;
        int[] numArray = new int[4];
        ushort usWidth = 0;
        byte num3 = 0;
        byte num4 = 0;
        ObjRectLTBSet(pWork, ref numArray[0], new int?(), new int?());
        ObjRectWHDSet(pWork, ref usWidth, new ushort?(), new ushort?());
        numArray[1] = numArray[0] + usWidth;
        ObjRectLTBSet(pAttacker, ref numArray[2], new int?(), new int?());
        ObjRectWHDSet(pAttacker, ref usWidth, new ushort?(), new ushort?());
        numArray[3] = numArray[2] + usWidth;
        int num5 = numArray[num3];
        byte num6 = num3;
        byte num7 = (byte)(num3 + 1U);
        if (numArray[num7] > num5)
        {
            num5 = numArray[num7];
            num6 = num7;
        }
        byte num8 = (byte)(num7 + 1U);
        if (numArray[num8] > num5)
        {
            num5 = numArray[num8];
            num6 = num8;
        }
        byte num9 = (byte)(num8 + 1U);
        if (numArray[num9] > num5)
        {
            num1 = numArray[num9];
            num6 = num9;
        }
        byte num10 = (byte)(num9 + 1U);
        byte num11 = 0;
        int num12 = numArray[num11];
        byte num13 = num11;
        byte num14 = (byte)(num11 + 1U);
        if (numArray[num14] < num12)
        {
            num12 = numArray[num14];
            num13 = num14;
        }
        byte num15 = (byte)(num14 + 1U);
        if (numArray[num15] < num12)
        {
            num12 = numArray[num15];
            num13 = num15;
        }
        byte num16 = (byte)(num15 + 1U);
        if (numArray[num16] < num12)
        {
            num2 = numArray[num16];
            num13 = num16;
        }
        num10 = (byte)(num16 + 1U);
        byte num17 = 0;
        while (true)
        {
            if (num17 != num6 && num17 != num13)
            {
                numArray[num4] = numArray[num17];
                if (num4 == 0)
                    ++num4;
                else
                    break;
            }
            ++num17;
        }
        int num18 = Math.Abs(numArray[0] - numArray[1] >> 1);
        return (numArray[0] <= numArray[1] ? num18 + numArray[0] : num18 + numArray[1]) << 12;
    }

    private static int ObjRectHitCenterY(OBS_RECT_WORK pWork, OBS_RECT_WORK pAttacker)
    {
        int num1 = 0;
        int num2 = 0;
        int[] numArray = new int[4];
        ushort usHeight = 0;
        byte num3 = 0;
        byte num4 = 0;
        ObjRectLTBSet(pWork, new int?(), ref numArray[0], new int?());
        ObjRectWHDSet(pWork, new ushort?(), ref usHeight, new ushort?());
        numArray[1] = numArray[0] + usHeight;
        ObjRectLTBSet(pAttacker, new int?(), ref numArray[2], new int?());
        ObjRectWHDSet(pAttacker, new ushort?(), ref usHeight, new ushort?());
        numArray[3] = numArray[2] + usHeight;
        int num5 = numArray[num3];
        byte num6 = num3;
        byte num7 = (byte)(num3 + 1U);
        if (numArray[num7] > num5)
        {
            num5 = numArray[num7];
            num6 = num7;
        }
        byte num8 = (byte)(num7 + 1U);
        if (numArray[num8] > num5)
        {
            num5 = numArray[num8];
            num6 = num8;
        }
        byte num9 = (byte)(num8 + 1U);
        if (numArray[num9] > num5)
        {
            num1 = numArray[num9];
            num6 = num9;
        }
        byte num10 = (byte)(num9 + 1U);
        byte num11 = 0;
        int num12 = numArray[num11];
        byte num13 = num11;
        byte num14 = (byte)(num11 + 1U);
        if (numArray[num14] < num12)
        {
            num12 = numArray[num14];
            num13 = num14;
        }
        byte num15 = (byte)(num14 + 1U);
        if (numArray[num15] < num12)
        {
            num12 = numArray[num15];
            num13 = num15;
        }
        byte num16 = (byte)(num15 + 1U);
        if (numArray[num16] < num12)
        {
            num2 = numArray[num16];
            num13 = num16;
        }
        num10 = (byte)(num16 + 1U);
        byte num17 = 0;
        while (true)
        {
            if (num17 != num6 && num17 != num13)
            {
                numArray[num4] = numArray[num17];
                if (num4 == 0)
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