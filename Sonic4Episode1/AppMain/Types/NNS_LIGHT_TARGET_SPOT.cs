using mpp;

public partial class AppMain
{
    public struct NNS_LIGHT_TARGET_SPOT
    {
        private NNS_LIGHT_TARGET_DIRECTIONAL data_;

        public NNS_LIGHT_TARGET_SPOT(NNS_LIGHT_TARGET_DIRECTIONAL data)
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

        public NNS_VECTOR Target
        {
            get => this.data_.Target;
            set => this.data_.Target.Assign(value);
        }

        public int InnerAngle
        {
            get => MppBitConverter.SingleToInt32(this.data_.InnerRange);
            set => this.data_.InnerRange = MppBitConverter.Int32ToSingle(value);
        }

        public int OuterAngle
        {
            get => MppBitConverter.SingleToInt32(this.data_.OuterRange);
            set => this.data_.OuterRange = MppBitConverter.Int32ToSingle(value);
        }

        public float FallOffStart
        {
            get => this.data_.FallOffStart;
            set => this.data_.FallOffStart = value;
        }

        public float FallOffEnd
        {
            get => this.data_.FallOffEnd;
            set => this.data_.FallOffEnd = value;
        }
    }
}
