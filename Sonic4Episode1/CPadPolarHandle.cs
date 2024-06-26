﻿using System;
using accel;

public partial class AppMain
{
    public class CPadPolarHandle
    {
        public static CPadPolarHandle p_instance = null;
        public static readonly float c_pi = (float)Math.Atan2(0.0, -1.0) * 2f;
        private bool[] m_flag = new bool[1];
        private CArray4<float> m_area;
        private CArray2<float> m_center;
        private int m_focus;
        private CArray2<float> m_prev;
        private int m_around;
        private float m_value;
        private float m_zero_point;

        public static CPadPolarHandle CreateInstance()
        {
            if (p_instance == null)
                p_instance = new CPadPolarHandle();
            return p_instance;
        }

        public bool Create()
        {
            this.m_area.left = 0.0f;
            this.m_area.top = 0.0f;
            this.m_area.right = AMD_SCREEN_2D_WIDTH;
            this.m_area.bottom = 288f;
            this.m_center.x = AMD_SCREEN_2D_WIDTH * 0.5f;
            this.m_center.y = 144f;
            return this.create();
        }

        public bool Create(CArray4<float> area)
        {
            this.m_area = area;
            this.m_center.x = (float)((m_area.left + (double)this.m_area.right) * 0.5);
            this.m_center.y = (float)((m_area.top + (double)this.m_area.bottom) * 0.5);
            return this.create();
        }

        public bool Create(float[] area)
        {
            this.m_area.left = area[0];
            this.m_area.top = area[1];
            this.m_area.right = area[2];
            this.m_area.bottom = area[3];
            this.m_center.x = (float)((m_area.left + (double)this.m_area.right) * 0.5);
            this.m_center.y = (float)((m_area.top + (double)this.m_area.bottom) * 0.5);
            return this.create();
        }

        public bool Create(float area_left, float area_top, float area_right, float area_bottom)
        {
            this.m_area.left = area_left;
            this.m_area.top = area_top;
            this.m_area.right = area_right;
            this.m_area.bottom = area_bottom;
            this.m_center.x = (float)((m_area.left + (double)this.m_area.right) * 0.5);
            this.m_center.y = (float)((m_area.top + (double)this.m_area.bottom) * 0.5);
            return this.create();
        }

        public bool Create(CArray2<float> center)
        {
            this.m_area.left = 0.0f;
            this.m_area.top = 0.0f;
            this.m_area.right = AMD_SCREEN_2D_WIDTH;
            this.m_area.bottom = 288f;
            this.m_center = center;
            return this.create();
        }

        public bool Create(float center_x, float center_y)
        {
            this.m_area.left = 0.0f;
            this.m_area.top = 0.0f;
            this.m_area.right = AMD_SCREEN_2D_WIDTH;
            this.m_area.bottom = 288f;
            this.m_center.x = center_x;
            this.m_center.y = center_y;
            return this.create();
        }

        public bool Create(CArray4<float> area, CArray2<float> center)
        {
            this.m_area = area;
            this.m_center = center;
            return this.create();
        }

        public bool Create(float[] area, float[] center)
        {
            this.m_area.left = area[0];
            this.m_area.top = area[1];
            this.m_area.right = area[2];
            this.m_area.bottom = area[3];
            this.m_center.x = center[0];
            this.m_center.y = center[1];
            return this.create();
        }

        public bool Create(
            float area_left,
            float area_top,
            float area_right,
            float area_bottom,
            float center_x,
            float center_y)
        {
            this.m_area.left = area_left;
            this.m_area.top = area_top;
            this.m_area.right = area_right;
            this.m_area.bottom = area_bottom;
            this.m_center.x = center_x;
            this.m_center.y = center_y;
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
            if (-1 == this.m_focus)
            {
                int pushTpIndex = this.getPushTpIndex();
                if (0 > pushTpIndex)
                    return;
                this.m_focus = pushTpIndex;
                this.m_zero_point += this.getCurrentValue();
            }
            else if (amTpIsTouchOn(this.m_focus))
            {
                this.m_value = this.getCurrentValue() - this.m_zero_point;
            }
            else
            {
                if (!amTpIsTouchPull(this.m_focus))
                    return;
                this.m_zero_point = -this.m_value;
                this.m_focus = -1;
            }
        }

        public float GetFloatValue()
        {
            return this.m_value;
        }

        public int GetAngle32Value()
        {
            return NNM_RADtoA32(this.m_value);
        }

        public bool IsFocus()
        {
            return -1 != this.m_focus;
        }

        public int GetFocusTpIndex()
        {
            return this.m_focus;
        }

        public void SetValue(float value)
        {
            this.m_zero_point = this.getCurrentValue() - value;
            this.m_value = value;
        }

        public void SetValue(int value)
        {
            this.SetValue(NNM_A32toRAD(value));
        }

        private bool create()
        {
            this.Release();
            this.m_value = 0.0f;
            this.m_focus = this.getOnTpIndex();
            this.m_zero_point = 0 > this.m_focus ? 0.0f : this.getCurrentValue();
            this.m_flag[0] = true;
            return true;
        }

        private float getCurrentValue()
        {
            float num;
            if (0 <= this.m_focus && amTpIsTouchOn(this.m_focus))
            {
                ushort[] on = _am_tp_touch[this.m_focus].on;
                if (amTpIsTouchPush(this.m_focus))
                    this.m_around = 0;
                else if (on[0] <= (double)this.m_center.x && m_prev.x <= (double)this.m_center.x)
                {
                    if (m_center.y <= (double)on[1])
                    {
                        if (m_prev.y < (double)this.m_center.y)
                            --this.m_around;
                    }
                    else if (m_center.y <= (double)this.m_prev.y)
                        ++this.m_around;
                }
                this.m_prev = CArray2<float>.initializer(on[0], on[1]);
                CArray2<float> carray2 = _SubrtactArray2(this.m_prev, this.m_center);
                num = (float)Math.Atan2(carray2.y, carray2.x) + c_pi * m_around;
            }
            else
                num = 0.0f;
            return num;
        }

        private int getOnTpIndex()
        {
            int num = -1;
            int index = 0;
            for (int length = _am_tp_touch.Length; index < length; ++index)
            {
                if (amTpIsTouchOn(index) && this.isHit(_am_tp_touch[index].on))
                {
                    num = index;
                    break;
                }
            }
            return num;
        }

        private int getPushTpIndex()
        {
            int num = -1;
            int index = 0;
            for (int length = _am_tp_touch.Length; index < length; ++index)
            {
                if (amTpIsTouchPush(index) && this.isHit(_am_tp_touch[index].push))
                {
                    num = index;
                    break;
                }
            }
            return num;
        }

        private bool isHit(ushort[] point)
        {
            return this.isHit(CArray2<float>.initializer(point[0], point[1]));
        }

        private bool isHit(CArray2<float> pos)
        {
            bool flag = false;
            if (pos.x >= (double)this.m_area.left && m_area.right >= (double)pos.x && (pos.y >= (double)this.m_area.top && m_area.bottom >= (double)pos.y))
                flag = true;
            return flag;
        }

        private class BFlag
        {
            public const int Setup = 0;
            public const int Max = 1;
            public const int None = 2;
        }
    }
}