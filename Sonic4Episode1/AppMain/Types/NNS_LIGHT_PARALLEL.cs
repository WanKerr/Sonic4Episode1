public partial class AppMain
{
    public struct NNS_LIGHT_PARALLEL
    {
        private NNS_LIGHT_TARGET_DIRECTIONAL data_;

        public NNS_LIGHT_PARALLEL(NNS_LIGHT_TARGET_DIRECTIONAL data)
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

        public NNS_VECTOR Direction
        {
            get => this.data_.Position;
            set => this.data_.Position.Assign(value);
        }
    }
}
