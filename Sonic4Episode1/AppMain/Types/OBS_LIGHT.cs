public partial class AppMain
{
    public class OBS_LIGHT
    {
        private readonly NNS_LIGHT_TARGET_DIRECTIONAL light_data_ = new NNS_LIGHT_TARGET_DIRECTIONAL();
        public const int ELT_LIGHT_PARALLEL = 0;
        public const int ELT_LIGHT_POINT = 1;
        public const int ELT_LIGHT_TARGET_SPOT = 2;
        public const int ELT_LIGHT_ROTATION_SPOT = 3;
        public uint light_type;

        public NNS_LIGHT_PARALLEL parallel => (NNS_LIGHT_PARALLEL)this.light_data_;

        public NNS_LIGHT_POINT point => (NNS_LIGHT_POINT)this.light_data_;

        public NNS_LIGHT_TARGET_SPOT target_spot => (NNS_LIGHT_TARGET_SPOT)this.light_data_;

        public NNS_LIGHT_ROTATION_SPOT rotation_spot => (NNS_LIGHT_ROTATION_SPOT)this.light_data_;

        public NNS_LIGHT_TARGET_DIRECTIONAL light_param => this.light_data_;
    }
}
