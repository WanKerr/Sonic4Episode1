public partial class AppMain
{
    public class VecFx16
    {
        public short x;
        public short y;
        public short z;

        public VecFx16()
          : this(0, 0, 0)
        {
        }

        public VecFx16(short _x, short _y, short _z)
        {
            this.x = _x;
            this.y = _y;
            this.z = _z;
        }

        public VecFx16(VecFx16 vecFx16)
        {
            this.x = vecFx16.x;
            this.y = vecFx16.y;
            this.z = vecFx16.z;
        }

        public VecFx16 Assign(VecFx16 vecFx16)
        {
            if (this != vecFx16)
            {
                this.x = vecFx16.x;
                this.y = vecFx16.y;
                this.z = vecFx16.z;
            }
            return this;
        }
    }
}
