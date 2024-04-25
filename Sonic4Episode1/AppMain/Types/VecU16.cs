public partial class AppMain
{
    public struct VecU16
    {
        public ushort x;
        public ushort y;
        public ushort z;

        public VecU16(ushort _x, ushort _y, ushort _z)
        {
            this.x = _x;
            this.y = _y;
            this.z = _z;
        }

        public VecU16(VecU16 vecU16)
        {
            this.x = vecU16.x;
            this.y = vecU16.y;
            this.z = vecU16.z;
        }

        public void Assign(VecU16 vecU16)
        {
            this.x = vecU16.x;
            this.y = vecU16.y;
            this.z = vecU16.z;
        }

        public void Clear()
        {
            this.x = this.y = this.z = 0;
        }
    }
}
