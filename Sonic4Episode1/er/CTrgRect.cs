// Decompiled with JetBrains decompiler
// Type: er.CTrgRect
// Assembly: Sonic4 ep I, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 093CE2FC-33E2-4332-B0FE-1EA1E44D3AE7
// Assembly location: C:\Users\wamwo\Documents\GitHub\Sonic4Ep1-WP7-Decompilation\XAP\Sonic4 ep I.dll

using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;

namespace er
{
    public class CTrgRect : CTrgBase<CTrgState>
    {
        private bool[] m_flag = new bool[1];
        private IntPair m_pos;
        private Rectangle m_rect;

        public bool Create(Rectangle rect)
        {
            this.Release();
            this.m_rect = rect;
            return this.create();
        }

        public bool Create(IntPair xy1, IntPair xy2)
        {
            this.Release();
            this.m_rect.X = xy1.first;
            this.m_rect.Y = xy1.second;
            this.m_rect.Width = xy2.first - xy1.first + 1;
            this.m_rect.Height = xy2.second - xy1.second + 1;
            return this.create();
        }

        public bool Create(int x1, int y1, int x2, int y2)
        {
            this.Release();
            this.m_rect.X = x1;
            this.m_rect.Y = y1;
            this.m_rect.Width = x2 - x1 + 1;
            this.m_rect.Height = y2 - y1 + 1;
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
            this.m_pos = new IntPair();
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
            if (this.m_flag[0] && pos.first >= this.m_pos.first + this.m_rect.Left && (this.m_pos.first + this.m_rect.Right >= pos.first && pos.second >= this.m_pos.second + this.m_rect.Top) && this.m_pos.second + this.m_rect.Bottom >= pos.second)
                flag = true;
            return flag;
        }

        [StructLayout(LayoutKind.Sequential, Size = 1)]
        private struct BFlag
        {
            public const int Setup = 0;
            public const int Max = 1;
            public const int None = 2;
        }
    }
}
