using System;

public partial class AppMain
{
    public class GMS_EVE_MGR_WORK
    {
        public float[] prev_pos = new float[2];
        public ushort[] map_size = new ushort[2];
        public _sts_proc_ sts_proc;
        public uint flag;

        public void Clear()
        {
            this.sts_proc = null;
            this.flag = 0U;
            Array.Clear(prev_pos, 0, 2);
            Array.Clear(map_size, 0, 2);
        }
    }
}
