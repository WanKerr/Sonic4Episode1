partial class AppMain
{
    private static void ObjSetBlockCollision(OBS_BLOCK_COLLISION pCol)
    {
        _obj_bcol = pCol;
    }

    private static OBS_BLOCK_COLLISION ObjGetBlockCollision()
    {
        return _obj_bcol;
    }

    private static int ObjBlockCollisionDet(
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
        int num = ObjBlockCollision(pData);
        GlobalPool<OBS_COL_CHK_DATA>.Release(pData);
        return num;
    }

    private static int ObjBlockCollision(OBS_COL_CHK_DATA pData)
    {
        int a = 0;
        short num1 = 0;
        short num2 = 0;
        if (_obj_bcol.pData[0] == null)
        {
            switch (pData.vec)
            {
                case 0:
                    a = _obj_bcol.right - pData.pos_x;
                    break;
                case 1:
                    a = pData.pos_x - _obj_bcol.left;
                    break;
                case 2:
                    a = _obj_bcol.bottom - pData.pos_y;
                    break;
                case 3:
                    a = pData.pos_y - _obj_bcol.top;
                    break;
            }
            return MTM_MATH_CLIP(a, -31, 31);
        }
        if (pData.dir != null)
        {
            int num3 = pData.dir[0];
        }
        if (pData.attr != null)
        {
            int num4 = (int)pData.attr[0];
        }
        int num5 = objGetBlockColData(pData);
        if ((pData.vec & 2) != 0)
        {
            short num6 = (short)(pData.pos_y & 15);
            switch ((pData.vec & 1) == 0 ? num6 + (short)num5 : num6 - (short)num5)
            {
                case 0:
                    num2 = -16;
                    break;
                case 15:
                    num2 = 16;
                    break;
            }
        }
        else
        {
            short num6 = (short)(pData.pos_x & 15);
            switch ((pData.vec & 1) == 0 ? num6 + (short)num5 : num6 - (short)num5)
            {
                case 0:
                    num1 = -16;
                    break;
                case 15:
                    num1 = 16;
                    break;
            }
        }
        if (num1 != 0 || num2 != 0)
        {
            uint num6 = 0;
            ushort num7 = 0;
            if (pData.dir != null)
                num7 = pData.dir[0];
            if (pData.attr != null)
                num6 = pData.attr[0];
            pData.pos_x += num1;
            pData.pos_y += num2;
            int blockColData = objGetBlockColData(pData);
            pData.pos_x -= num1;
            pData.pos_y -= num2;
            if (blockColData >= 0)
            {
                if (pData.dir != null)
                    pData.dir[0] = num7;
                if (pData.attr != null)
                    pData.attr[0] = num6;
            }
            if (num1 < 0)
                ++num1;
            if (num1 > 0)
                --num1;
            if (num2 < 0)
                ++num2;
            if (num2 > 0)
                --num2;
            num5 = (pData.vec & 1) == 0 ? blockColData + (num1 + num2) : blockColData - (num1 + num2);
        }
        return num5;
    }

    private static int objGetBlockColData(OBS_COL_CHK_DATA pData)
    {
        int num1 = 0;
        int num2 = 0;
        if ((pData.flag & 64) != 0)
        {
            int num3 = objBlockColLimit(pData);
            if (num3 < 0)
                return num3;
        }
        else
        {
            num1 = MTM_MATH_CLIP(pData.pos_x, _obj_bcol.left, _obj_bcol.right - 1);
            num2 = MTM_MATH_CLIP(pData.pos_y, _obj_bcol.top, _obj_bcol.bottom - 1);
        }
        uint num4 = (uint)((ulong)(num2 >> 4) * _obj_bcol.width + (ulong)(num1 >> 4));
        ushort num5 = _obj_bcol.pData[pData.flag & 1][(int)num4];
        return _obj_block_collision_func[num5](pData);
    }

    private static int objBlockColLimit(OBS_COL_CHK_DATA pData)
    {
        switch (pData.vec)
        {
            case 0:
                if (_obj_bcol.right - 1 < pData.pos_x)
                    return _obj_bcol.right - 1 - pData.pos_x;
                goto case 1;
            case 1:
                if (_obj_bcol.left > pData.pos_x)
                    return pData.pos_x - _obj_bcol.left;
                break;
            case 2:
                if (_obj_bcol.bottom - 1 < pData.pos_y)
                    return _obj_bcol.bottom - 1 - pData.pos_y;
                goto case 3;
            case 3:
                if (_obj_bcol.top > pData.pos_y)
                    return pData.pos_y - _obj_bcol.top;
                goto case 0;
        }
        return 1;
    }

    private static int objBlockCalcEmpty(OBS_COL_CHK_DATA pData)
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

    private static int objBlockCalcFill(OBS_COL_CHK_DATA pData)
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

    private static int objBlockColEmpty(OBS_COL_CHK_DATA pData)
    {
        if (pData.dir != null)
            pData.dir[0] = 0;
        return objBlockCalcEmpty(pData);
    }

    private static int objBlockColBlockFill(OBS_COL_CHK_DATA pData)
    {
        if (pData.dir != null)
            pData.dir[0] = 0;
        return objBlockCalcFill(pData);
    }

    private static int objBlockColBlockFillThrough(OBS_COL_CHK_DATA pData)
    {
        if (pData.dir != null)
            pData.dir[0] = 0;
        if (pData.attr != null)
            pData.attr[0] |= 2U;
        return (pData.flag & 128) != 0 ? objBlockCalcEmpty(pData) : objBlockCalcFill(pData);
    }

}
