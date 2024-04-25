using System;

public partial class AppMain
{
    public class AMS_PAD_DATA : IClearable
    {
        public readonly int[] keep_time = new int[0];
        public readonly int[] last_time = new int[0];
        public readonly int[] keep_atime = new int[0];
        public readonly int[] last_atime = new int[0];
        public uint state;
        public ushort direct;
        public ushort stand;
        public ushort release;
        public ushort repeat;
        public short timer_lv;
        public short timer_btn;
        public short alx;
        public short aly;
        public short arx;
        public short ary;
        public short al2;
        public short ar2;
        public ushort adirect;
        public ushort astand;
        public ushort arelease;
        public ushort arepeat;
        public short sensor_x;
        public short sensor_y;
        public short sensor_z;
        public short sensor_g;
        public int rot_x;
        public int rot_y;
        public int rot_z;
        public short point_flag;
        public short point_x;
        public short point_y;
        public float point_z;
        public int vib_flag;

        public void Clear()
        {
            this.state = 0U;
            this.direct = this.stand = this.release = this.repeat = 0;
            this.timer_lv = this.timer_btn = 0;
            this.alx = this.aly = this.arx = this.ary = this.al2 = this.ar2 = 0;
            this.adirect = this.astand = this.arelease = this.arepeat = 0;
            this.sensor_x = this.sensor_y = this.sensor_z = this.sensor_g = this.point_flag = this.point_x = this.point_y = 0;
            this.rot_x = this.rot_y = this.rot_z = this.vib_flag = 0;
            this.point_z = 0.0f;
            Array.Clear(keep_time, 0, this.keep_time.Length);
            Array.Clear(last_time, 0, this.last_time.Length);
            Array.Clear(keep_atime, 0, this.keep_atime.Length);
            Array.Clear(last_atime, 0, this.last_atime.Length);
        }
    }
}
