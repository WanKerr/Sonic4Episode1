public partial class AppMain
{
    public class VecU32
    {
        public uint x;
        public uint y;
        public uint z;

        public VecU32()
          : this(0U, 0U, 0U)
        {
        }

        public VecU32(uint _x, uint _y, uint _z)
        {
            this.x = _x;
            this.y = _y;
            this.z = _z;
        }

        public VecU32(VecU32 vecU32)
        {
            this.x = vecU32.x;
            this.y = vecU32.y;
            this.z = vecU32.z;
        }

        public VecU32 Assign(VecU32 vecU32)
        {
            this.x = vecU32.x;
            this.y = vecU32.y;
            this.z = vecU32.z;
            return this;
        }
    }
}
