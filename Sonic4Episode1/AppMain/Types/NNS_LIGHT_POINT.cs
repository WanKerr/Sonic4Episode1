public partial class AppMain
{
    public struct NNS_LIGHT_POINT
    {
        private NNS_LIGHT_TARGET_DIRECTIONAL data_;

        public NNS_LIGHT_POINT(NNS_LIGHT_TARGET_DIRECTIONAL data)
        {
            this.data_ = data;
        }

        public uint User
        {
            get => this.data_.User;
            set => this.data_.User = value;
        }

        public NNS_RGBA Color
        {
            get => this.data_.Color;
            set => this.data_.Color = value;
        }

        public float Intensity
        {
            get => this.data_.Intensity;
            set => this.data_.Intensity = value;
        }

        public NNS_VECTOR Position
        {
            get => this.data_.Position;
            set => this.data_.Position.Assign(value);
        }

        public float FallOffStart
        {
            get => this.data_.Target.x;
            set => this.data_.Target.x = value;
        }

        public float FallOffEnd
        {
            get => this.data_.Target.y;
            set => this.data_.Target.y = value;
        }
    }
}
