// Decompiled with JetBrains decompiler
// Type: er.CTrgFlick
// Assembly: Sonic4 ep I, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 093CE2FC-33E2-4332-B0FE-1EA1E44D3AE7
// Assembly location: C:\Users\wamwo\Documents\GitHub\Sonic4Ep1-WP7-Decompilation\XAP\Sonic4 ep I.dll

using accel;

namespace er
{
    public class CTrgFlick : CTrgBase<CTrgStateEx>
    {
        private bool[] m_flag = new bool[1];
        private CArray2<int> m_pos;
        private CArray4<int> m_rect;

        public bool Create(CArray4<int> rect)
        {
            this.Release();
            this.m_rect = rect;
            return this.create();
        }

        public bool Create(CArray2<int> xy1, CArray2<int> xy2)
        {
            this.Release();
            this.m_rect.left = xy1.x;
            this.m_rect.top = xy1.y;
            this.m_rect.right = xy2.x;
            this.m_rect.bottom = xy2.y;
            return this.create();
        }

        public bool Create(int x1, int y1, int x2, int y2)
        {
            this.Release();
            this.m_rect.left = x1;
            this.m_rect.top = y1;
            this.m_rect.right = x2;
            this.m_rect.bottom = y2;
            return this.create();
        }

        public void Release()
        {
            if (!this.m_flag[0])
                return;
            this.m_flag[0] = false;
        }

        public override bool IsValid()
        {
            return this.m_flag[0];
        }

        public bool create()
        {
            this.ResetState();
            this.SetRepeatInterval();
            this.SetDoubleClickTime();
            this.SetMoveThreshold();
            this.m_flag[0] = true;
            return true;
        }

        protected override bool hitTest(IntPair pos, uint index)
        {
            bool flag = false;
            if (this.m_flag[0] && pos.first >= this.m_pos.x + this.m_rect.left && (this.m_pos.x + this.m_rect.right >= pos.first && pos.second >= this.m_pos.y + this.m_rect.top) && this.m_pos.y + this.m_rect.bottom >= pos.second)
                flag = true;
            return flag;
        }

        public enum BFlag
        {
            Setup,
            Max,
            None,
        }
    }
}
