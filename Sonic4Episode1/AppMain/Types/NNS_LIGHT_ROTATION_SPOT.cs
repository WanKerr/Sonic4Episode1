using mpp;

public partial class AppMain
{
    public struct NNS_LIGHT_ROTATION_SPOT
    {
        private NNS_LIGHT_TARGET_DIRECTIONAL data_;

        public NNS_LIGHT_ROTATION_SPOT(NNS_LIGHT_TARGET_DIRECTIONAL data)
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

        public int RotType
        {
            get => MppBitConverter.SingleToInt32(this.data_.Target.x);
            set => this.data_.Target.x = MppBitConverter.Int32ToSingle(value);
        }

        public NNS_ROTATE_A32 Rotation
        {
            get => new NNS_ROTATE_A32()
            {
                x = MppBitConverter.SingleToInt32(this.data_.Target.y),
                y = MppBitConverter.SingleToInt32(this.data_.Target.z),
                z = MppBitConverter.SingleToInt32(this.data_.InnerRange)
            };
            set
            {
                this.data_.Target.y = MppBitConverter.Int32ToSingle(value.x);
                this.data_.Target.z = MppBitConverter.Int32ToSingle(value.y);
                this.data_.InnerRange = MppBitConverter.Int32ToSingle(value.z);
            }
        }

        public int InnerAngle
        {
            get => MppBitConverter.SingleToInt32(this.data_.OuterRange);
            set => this.data_.OuterRange = MppBitConverter.Int32ToSingle(value);
        }

        public int OuterAngle
        {
            get => MppBitConverter.SingleToInt32(this.data_.FallOffStart);
            set => this.data_.FallOffStart = MppBitConverter.Int32ToSingle(value);
        }

        public float FallOffStart
        {
            get => this.data_.FallOffEnd;
            set => this.data_.FallOffEnd = value;
        }

        public float FallOffEnd
        {
            get => this.data_.dummy;
            set => this.data_.dummy = value;
        }
    }
}
