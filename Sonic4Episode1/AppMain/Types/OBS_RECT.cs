public partial class AppMain
{
    public class OBS_RECT : IClearable
    {
        public VecFx32 pos = new VecFx32();
        public short left;
        public short top;
        public short back;
        public short right;
        public short bottom;
        public short front;

        public void Clear()
        {
            this.left = this.top = this.back = 0;
            this.right = 0;
            this.width = 0;
            this.height = 0;
            this.bottom = 0;
            this.height = 0;
            this.front = 0;
            this.depth = 0;
        }

        public ushort width
        {
            get => (ushort)this.right;
            set => this.right = (short)value;
        }

        public ushort height
        {
            get => (ushort)this.bottom;
            set => this.bottom = (short)value;
        }

        public short depth
        {
            get => this.front;
            set => this.front = value;
        }
    }
}
