// Decompiled with JetBrains decompiler
// Type: er.CTrgStateEx
// Assembly: Sonic4 ep I, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 093CE2FC-33E2-4332-B0FE-1EA1E44D3AE7
// Assembly location: C:\Users\wamwo\Documents\GitHub\Sonic4Ep1-WP7-Decompilation\XAP\Sonic4 ep I.dll

using accel;

namespace er
{
    public class CTrgStateEx : CTrgState
    {
        public readonly CCircularBuffer<IntPair> m_pos_history = new CCircularBuffer<IntPair>(6);
        public const int c_pos_history = 6;

        public override void Push(bool is_on, bool is_edge, IntPair move)
        {
            base.Push(is_on, is_edge, move);
            if (is_on || is_edge)
                this.m_pos_history.push_front(move);
            else
                this.m_pos_history.push_front(new IntPair(0, 0));
        }

        public override void ResetState()
        {
            base.ResetState();
            this.m_pos_history.clear();
        }

        public CArray2<float> GetDragSpeed()
        {
            IntPair intPair = new IntPair(0, 0);
            for (int index = 0; index < this.m_pos_history.size(); ++index)
            {
                IntPair at = this.m_pos_history.getAt(index);
                intPair.first += at.first;
                intPair.second += at.second;
            }
            float num = 1f / this.m_pos_history.max_size();
            return new CArray2<float>()
            {
                x = intPair.first * num,
                y = intPair.second * num
            };
        }
    }
}
