using accel;
using Microsoft.Xna.Framework.Input.Touch;

public class CPadVirtualPad
{
    private bool[] m_flag = new bool[1];
    private readonly bool[] m_focus = new bool[4];
    private CCircularBuffer<ushort> m_on_flag = new CCircularBuffer<ushort>(2);
    private const int c_tp_index_max = 4;
    private static CPadVirtualPad p_instance;
    private CArray4<float> m_area;

    public static CPadVirtualPad CreateInstance()
    {
        if (CPadVirtualPad.p_instance == null)
            CPadVirtualPad.p_instance = new CPadVirtualPad();
        return CPadVirtualPad.p_instance;
    }

    public bool Create()
    {
        this.m_area.left = 0.0f;
        this.m_area.top = 0.0f;
        this.m_area.right = AppMain.AMD_SCREEN_2D_WIDTH;
        this.m_area.bottom = 288f;
        return this.create();
    }

    public bool Create(ref CArray4<float> area)
    {
        this.m_area = area;
        return this.create();
    }

    public bool Create(ref CArray2<float> area_xy1, ref CArray2<float> area_xy2)
    {
        this.m_area.left = area_xy1.x;
        this.m_area.top = area_xy1.y;
        this.m_area.right = area_xy2.x;
        this.m_area.bottom = area_xy2.y;
        return this.create();
    }

    public bool Create(float[] area)
    {
        for (int index = 0; index < 4; ++index)
            this.m_area[index] = area[index];
        return this.create();
    }

    public bool Create(float[] area_xy1, float[] area_xy2)
    {
        this.m_area.left = area_xy1[0];
        this.m_area.top = area_xy1[1];
        this.m_area.right = area_xy2[0];
        this.m_area.bottom = area_xy2[1];
        return this.create();
    }

    public bool Create(float area_left, float area_top, float area_right, float area_bottom)
    {
        this.m_area.left = area_left;
        this.m_area.top = area_top;
        this.m_area.right = area_right;
        this.m_area.bottom = area_bottom;
        return this.create();
    }

    public void Release()
    {
        if (!this.m_flag[0])
            return;
        this.m_flag[0] = false;
    }

    public bool IsValid()
    {
        return this.m_flag[0];
    }

    public void Update()
    {
        if (!this.m_flag[0])
            return;

        int index1 = 0;
        for (int length = this.m_focus.Length; index1 < length; ++index1)
        {
            if (this.m_focus[index1])
            {
                if (!AppMain.amTpIsTouchOn(index1) || !this.isHit(AppMain._am_tp_touch[index1].@on))
                    this.m_focus[index1] = false;
            }
            else if (AppMain.amTpIsTouchOn(index1) && this.isHit(AppMain._am_tp_touch[index1].@on))
                this.m_focus[index1] = true;
        }
        ushort num = 0;
        int index2 = 0;
        for (int length = this.m_focus.Length; index2 < length; ++index2)
        {
            if (this.m_focus[index2])
                num |= this.getOnFlag(AppMain._am_tp_touch[index2].@on);
        }
        if (12 == (12 & (int)num))
            num &= (ushort)65531;
        if (3 == (3 & (int)num))
            num &= (ushort)65534;
        this.m_on_flag.push_front(num);
    }

    public ushort GetValue()
    {
        return this.m_on_flag[0];
    }

    public bool IsFocus(int tp_index)
    {
        return this.m_focus[tp_index];
    }

    public bool create()
    {
        if (!TouchPanel.GetCapabilities().IsConnected)
        {
            // no touchy
            return false;
        }

        this.Release();
        this.m_flag[0] = true;
        return true;
    }

    public bool isHit(ushort[] point)
    {
        return this.isHit(CArray2<float>.initializer((float)point[0], (float)point[1]));
    }

    public bool isHit(CArray2<float> pos)
    {
        bool flag = false;
        if ((double)pos.x >= (double)this.m_area.left && (double)this.m_area.right >= (double)pos.x && ((double)pos.y >= (double)this.m_area.top && (double)this.m_area.bottom >= (double)pos.y))
            flag = true;
        return flag;
    }

    public ushort getOnFlag(ushort[] point)
    {
        return this.getOnFlag(CArray2<float>.initializer((float)point[0], (float)point[1]));
    }

    public ushort getOnFlag(CArray2<float> pos)
    {
        CArray2<float> p1 = CArray2<float>.initializer((float)(((double)this.m_area.left + (double)this.m_area.right) * 0.5), (float)(((double)this.m_area.top + (double)this.m_area.bottom) * 0.5));
        pos.y -= 16f;
        pos.y /= 1.05f;
        ushort num1 = 0;
        float num2 = (float)(((double)this.m_area.bottom - (double)this.m_area.top) * 0.400000005960464 * 0.5);
        CArray2<float> xy1_1 = CArray2<float>.initializer(this.m_area.left, p1.y - num2);
        CArray2<float> xy2_1 = CArray2<float>.initializer(p1.x, (float)((double)p1.y + (double)num2 - 17.0));
        CArray2<float> xy1_2 = CArray2<float>.initializer(p1.x, p1.y - num2);
        CArray2<float> xy2_2 = CArray2<float>.initializer(this.m_area.right, (float)((double)p1.y + (double)num2 - 17.0));
        if (this.isHit(pos, xy1_2, xy2_2))
            num1 = (ushort)8;
        else if (this.isHit(pos, xy1_1, xy2_1))
            num1 = (ushort)4;
        if (num1 == (ushort)0)
        {
            CArray2<float> carray2_1 = CArray2<float>.initializer(this.m_area.left, this.m_area.top);
            CArray2<float> carray2_2 = CArray2<float>.initializer(this.m_area.right, this.m_area.top);
            CArray2<float> carray2_3 = CArray2<float>.initializer(this.m_area.left, this.m_area.bottom);
            CArray2<float> carray2_4 = CArray2<float>.initializer(this.m_area.right, this.m_area.bottom);
            if (this.isHit(pos, p1, carray2_2, carray2_4))
                num1 = (ushort)8;
            else if (this.isHit(pos, p1, carray2_3, carray2_1))
                num1 = (ushort)4;
            else if (this.isHit(pos, p1, carray2_1, carray2_2))
                num1 = (ushort)1;
            else if (this.isHit(pos, p1, carray2_4, carray2_3))
                num1 = (ushort)2;
        }
        return num1;
    }

    public bool isHit(CArray2<float> target, CArray2<float> xy1, CArray2<float> xy2)
    {
        bool flag = false;
        if ((double)target.x >= (double)xy1.x && (double)xy2.x >= (double)target.x && ((double)target.y >= (double)xy1.y && (double)xy2.y >= (double)target.y))
            flag = true;
        return flag;
    }

    public bool isHit(
        CArray2<float> target,
        CArray2<float> p1,
        CArray2<float> p2,
        CArray2<float> p3)
    {
        float num1 = CPadVirtualPad.CLocalLogic.Cross(AppMain._SubrtactArray2(p1, target), AppMain._SubrtactArray2(p2, target));
        float num2 = CPadVirtualPad.CLocalLogic.Cross(AppMain._SubrtactArray2(p2, target), AppMain._SubrtactArray2(p3, target));
        float num3 = CPadVirtualPad.CLocalLogic.Cross(AppMain._SubrtactArray2(p3, target), AppMain._SubrtactArray2(p1, target));
        return 0.0 < (double)num1 * (double)num2 && 0.0 < (double)num1 * (double)num3;
    }

    private class CLocalLogic
    {
        public static float Cross(CArray2<float> p1, CArray2<float> p2)
        {
            return (float)((double)p1.x * (double)p2.y - (double)p1.y * (double)p2.x);
        }
    }

    private class BFlag
    {
        public const int Setup = 0;
        public const int Max = 1;
        public const int None = 2;
    }
}