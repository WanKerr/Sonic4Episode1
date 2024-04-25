using System;

public partial class AppMain
{
    public class AMS_TRAIL_PARTS
    {
        public readonly NNS_VECTOR pos = GlobalPool<NNS_VECTOR>.Alloc();
        public readonly NNS_VECTOR sub_pos = GlobalPool<NNS_VECTOR>.Alloc();
        public readonly NNS_VECTOR dir = GlobalPool<NNS_VECTOR>.Alloc();
        public ushort[] Dummy = new ushort[5];
        public float time;
        public AMS_TRAIL_PARTS pNext;
        public AMS_TRAIL_PARTS pPrev;
        public uint m_Flag;
        public short partsId;

        public void Clear()
        {
            this.pos.Clear();
            this.sub_pos.Clear();
            this.dir.Clear();
            this.time = 0.0f;
            this.pNext = null;
            this.pPrev = null;
            this.m_Flag = 0U;
            this.partsId = 0;
            Array.Clear(Dummy, 0, this.Dummy.Length);
        }
    }
}
