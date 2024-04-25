using System;

public partial class AppMain
{
    public class GMS_START_DEMO_DATA : IClearable
    {
        public readonly AOS_TEXTURE[] aos_texture = New<AOS_TEXTURE>(2);
        public readonly object[] demo_amb = new object[2];
        public bool flag_regist;

        public void Clear()
        {
            Array.Clear(demo_amb, 0, this.demo_amb.Length);
            ClearArray(this.aos_texture);
            this.flag_regist = false;
        }
    }
}
